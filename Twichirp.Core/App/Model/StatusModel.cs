// Copyright (c) 2016 meil
//
// This file is part of Twichirp.
// 
// Twichirp is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Twichirp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with Twichirp.  If not, see <http://www.gnu.org/licenses/>.
using CoreTweet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twichirp.Core.Model;
using System.Threading;
using Twichirp.Core.Extensions;

namespace Twichirp.Core.App.Model {
    class StatusModel : BaseModel {

        private SemaphoreSlim slim = new SemaphoreSlim(1,1);

        private string _errorMessage;
        public string ErrorMessage {
            get {
                return _errorMessage;
            }
            private set {
                _errorMessage = value;
                RaisePropertyChanged(nameof(ErrorMessage));
            }
        }

        // Statusを反映した時刻
        private DateTime _pushTime;
        public DateTime PushTime {
            get {
                return _pushTime;
            }
            private set {
                _pushTime = value;
                RaisePropertyChanged(nameof(PushTime));
            }
        }

        private Status status;

        public UserModel User { get; private set; }

        public long Id { get; private set; }

        public IEnumerable<TextPart> Text { get; private set; }

        public IEnumerable<UserMentionEntity> HiddenPrefix { get; private set; }

        public IEnumerable<UrlEntity> HiddenSuffix { get; private set; }

        public string Source { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public long? CurrentUserRetweet { get; private set; }

        public IEnumerable<UserMentionEntity> UserMention { get; private set; }

        public IEnumerable<HashtagEntity> Hashtag { get; private set; }

        public IEnumerable<CashtagEntity> Symbol { get; private set; }

        public IEnumerable<UrlEntity> Url { get; private set; }

        public IEnumerable<MediaEntity> Media { get; private set; }

        public int FavoriteCount { get; private set; }

        public bool IsFavorited { get; private set; }

        public string InReplyToScreenName { get; private set; }

        public long? InReplyToStatusId { get; private set; }

        public long? InReplyToUserId { get; private set; }

        public StatusModel QuotedStatus { get; private set; }

        public int RetweetCount { get; private set; }

        public bool IsRetweeted { get; private set; }

        public StatusModel RetweetedStatus { get; private set; }

        public StatusModel(ITwichirpApplication application,Status status) : base(application) {
            SetStatus(status);
        }

        public void SetStatus(Status status) {
            this.status = status;

            if(User == null) {
                User = new UserModel(Application,status.User);
            } else {
                User.SetUser(status.User);
            }
            RaisePropertyChanged(nameof(User));

            Id = status.Id;
            RaisePropertyChanged(nameof(Id));

            var extended = status.GetExtendedTweetElements();

            Text = extended.TweetText;
            RaisePropertyChanged(nameof(Text));

            HiddenPrefix = extended.HiddenPrefix;
            RaisePropertyChanged(nameof(HiddenPrefix));

            HiddenSuffix = extended.HiddenSuffix;
            RaisePropertyChanged(nameof(HiddenSuffix));

            Source = status.ParseSource().Name;
            RaisePropertyChanged(nameof(Source));

            CreatedAt = status.CreatedAt;
            RaisePropertyChanged(nameof(CreatedAt));

            CurrentUserRetweet = status.CurrentUserRetweet;
            RaisePropertyChanged(nameof(CurrentUserRetweet));

            UserMention = status.Entities?.UserMentions ?? Enumerable.Empty<UserMentionEntity>();
            RaisePropertyChanged(nameof(UserMention));

            Hashtag = status.Entities?.HashTags ?? Enumerable.Empty<HashtagEntity>();
            RaisePropertyChanged(nameof(Hashtag));

            Symbol = status.Entities?.Symbols ?? Enumerable.Empty<CashtagEntity>();
            RaisePropertyChanged(nameof(Symbol));

            Url = status.Entities?.Urls ?? Enumerable.Empty<UrlEntity>();
            RaisePropertyChanged(nameof(Url));

            Media = status.ExtendedEntities?.Media ?? Enumerable.Empty<MediaEntity>();
            RaisePropertyChanged(nameof(Media));

            FavoriteCount = status.FavoriteCount ?? 0;
            RaisePropertyChanged(nameof(FavoriteCount));

            IsFavorited = status.IsFavorited ?? false;
            RaisePropertyChanged(nameof(IsFavorited));

            InReplyToScreenName = status.InReplyToScreenName;
            RaisePropertyChanged(nameof(InReplyToScreenName));

            InReplyToStatusId = status.InReplyToStatusId;
            RaisePropertyChanged(nameof(InReplyToStatusId));

            InReplyToUserId = status.InReplyToUserId;
            RaisePropertyChanged(nameof(InReplyToUserId));

            if(status.QuotedStatus != null && QuotedStatus == null) {
                QuotedStatus = new StatusModel(Application,status.QuotedStatus);
            } else if(status.QuotedStatus != null) {
                QuotedStatus.SetStatus(status.QuotedStatus);
            } else {
                QuotedStatus = null;
            }
            RaisePropertyChanged(nameof(QuotedStatus));

            RetweetCount = status.RetweetCount ?? 0;
            RaisePropertyChanged(nameof(RetweetCount));

            IsRetweeted = status.IsRetweeted ?? false;
            RaisePropertyChanged(nameof(IsRetweeted));

            if(status.RetweetedStatus != null && RetweetedStatus == null) {
                RetweetedStatus = new StatusModel(Application,status.RetweetedStatus);
            } else if(status.RetweetedStatus != null) {
                RetweetedStatus.SetStatus(status.RetweetedStatus);
            } else {
                RetweetedStatus = null;
            }
            RaisePropertyChanged(nameof(RetweetedStatus));

            PushTime = DateTime.Now;
        }

        public StatusModel ToContentStatus() => RetweetedStatus == null ? this : RetweetedStatus;

        public string ExportJson() => JsonConvert.SerializeObject(status);

        public IEnumerable<StatusModel> DeploymentStatus() {
            yield return this;
            if(RetweetedStatus != null) {
                yield return RetweetedStatus;
            }
            if(RetweetedStatus?.QuotedStatus != null) {
                yield return RetweetedStatus.QuotedStatus;
            }
            if(QuotedStatus != null) {
                yield return QuotedStatus;
            }
        }

        public async void Retweet(Account account) {
            ErrorMessage = null;
            await slim.WaitAsync();
            try {
                Status status = await account.Token.Statuses.RetweetAsync(id: Id,include_ext_alt_text: true,tweet_mode: TweetMode.extended);
                foreach(var s in status.DeploymentStatus()) {
                    Application.TwitterEvent.UpdateStatus = Tuple.Create(account,s);
                }
            } catch(Exception e) {
                ErrorMessage = e.Message;
            } finally {
                slim.Release();
            }
        }

        public async void UnRetweet(Account account) {
            ErrorMessage = null;
            await slim.WaitAsync();
            try {
                Status status = await account.Token.Statuses.UnretweetAsync(id: Id,include_ext_alt_text: true,tweet_mode: TweetMode.extended);
                if(status.IsRetweeted ?? false) {
                    //返り値に反映されてない
                    status.IsRetweeted = false;
                    status.RetweetCount -= 1;
                }
                foreach(var s in status.DeploymentStatus()) {
                    Application.TwitterEvent.UpdateStatus = Tuple.Create(account,s);
                }
            } catch(Exception e) {
                ErrorMessage = e.Message;
            } finally {
                slim.Release();
            }
        }

        public async void Favorite(Account account) {
            ErrorMessage = null;
            await slim.WaitAsync();
            try {
                Status status = await account.Token.Favorites.CreateAsync(id: Id,include_ext_alt_text: true,tweet_mode: TweetMode.extended);
                foreach(var s in status.DeploymentStatus()) {
                    Application.TwitterEvent.UpdateStatus = Tuple.Create(account,s);
                }
            } catch(Exception e) {
                ErrorMessage = e.Message;
            } finally {
                slim.Release();
            }
        }

        public async void UnFavorite(Account account) {
            ErrorMessage = null;
            await slim.WaitAsync();
            try {
                Status status = await account.Token.Favorites.DestroyAsync(id: Id,include_ext_alt_text: true,tweet_mode: TweetMode.extended);
                foreach(var s in status.DeploymentStatus()) {
                    Application.TwitterEvent.UpdateStatus = Tuple.Create(account,s);
                }
            } catch(Exception e) {
                ErrorMessage = e.Message;
            } finally {
                slim.Release();
            }
        }
    }
}
