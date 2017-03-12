// Copyright (c) 2016-2017 meil
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreTweet;
using Newtonsoft.Json;
using Twichirp.Core.DataObjects;
using Twichirp.Core.Events;
using Twichirp.Core.Extensions;
using Twichirp.Core.Services;
using CStatus = CoreTweet.Status;

namespace Twichirp.Core.Models {

    class StatusModel : BaseModel {

        public event EventHandler<EventArgs> StatusChanged;
        public event EventHandler<EventArgs<string>> ErrorMessageCreated;

        private SemaphoreSlim slim = new SemaphoreSlim(1, 1);
        private CStatus status;
        private ITwitterEventService twitterEventService;

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

        public StatusModel(ITwitterEventService twitterEventService, CStatus status) {
            this.twitterEventService = twitterEventService;
            SetStatus(status);
        }

        public void SetStatus(CStatus status) {
            this.status = status;

            if (User == null) {
                User = new UserModel(status.User);
            } else {
                User.SetUser(status.User);
            }
            RaisePropertyChanged(nameof(User));

            SetProperty(this, x => x.Id, status.Id);
            {
                var extended = status.GetExtendedTweetElements();
                SetProperty(this, x => x.Text, extended.TweetText);
                SetProperty(this, x => x.HiddenPrefix, extended.HiddenPrefix);
                SetProperty(this, x => x.HiddenSuffix, extended.HiddenSuffix);
            }

            SetProperty(this, x => x.Source, status.ParseSource().Name);
            SetProperty(this, x => x.CreatedAt, status.CreatedAt);
            SetProperty(this, x => x.CurrentUserRetweet, status.CurrentUserRetweet);
            SetProperty(this, x => x.UserMention, status.Entities?.UserMentions ?? Enumerable.Empty<UserMentionEntity>());
            SetProperty(this, x => x.Hashtag, status.Entities?.HashTags ?? Enumerable.Empty<HashtagEntity>());
            SetProperty(this, x => x.Symbol, status.Entities?.Symbols ?? Enumerable.Empty<CashtagEntity>());
            SetProperty(this, x => x.Url, status.Entities?.Urls ?? Enumerable.Empty<UrlEntity>());
            SetProperty(this, x => x.Media, status.ExtendedEntities?.Media ?? Enumerable.Empty<MediaEntity>());
            SetProperty(this, x => x.FavoriteCount, status.FavoriteCount ?? 0);
            SetProperty(this, x => x.IsFavorited, status.IsFavorited ?? false);
            SetProperty(this, x => x.InReplyToScreenName, status.InReplyToScreenName);
            SetProperty(this, x => x.InReplyToStatusId, status.InReplyToStatusId);
            SetProperty(this, x => x.InReplyToUserId, status.InReplyToUserId);

            if (status.QuotedStatus != null && QuotedStatus == null) {
                QuotedStatus = new StatusModel(twitterEventService, status.QuotedStatus);
            } else if (status.QuotedStatus != null) {
                QuotedStatus.SetStatus(status.QuotedStatus);
            } else {
                QuotedStatus = null;
            }
            RaisePropertyChanged(nameof(QuotedStatus));

            SetProperty(this, x => x.RetweetCount, status.RetweetCount ?? 0);
            SetProperty(this, x => x.IsRetweeted, status.IsRetweeted ?? false);

            if (status.RetweetedStatus != null && RetweetedStatus == null) {
                RetweetedStatus = new StatusModel(twitterEventService, status.RetweetedStatus);
            } else if (status.RetweetedStatus != null) {
                RetweetedStatus.SetStatus(status.RetweetedStatus);
            } else {
                RetweetedStatus = null;
            }
            RaisePropertyChanged(nameof(RetweetedStatus));

            StatusChanged?.Invoke(this, new EventArgs());
        }

        public StatusModel ToContentStatus() => RetweetedStatus == null ? this : RetweetedStatus;

        public string ExportJson() => JsonConvert.SerializeObject(status);

        public IEnumerable<StatusModel> DeploymentStatus() {
            yield return this;
            if (RetweetedStatus != null) {
                yield return RetweetedStatus;
            }
            if (RetweetedStatus?.QuotedStatus != null) {
                yield return RetweetedStatus.QuotedStatus;
            }
            if (QuotedStatus != null) {
                yield return QuotedStatus;
            }
        }

        public async Task RetweetAsync(ImmutableAccount account) {
            await slim.WaitAsync();
            try {
                CStatus status = await account.CoreTweetToken.Statuses.RetweetAsync(id: Id, include_ext_alt_text: true, tweet_mode: TweetMode.extended);
                status.CheckValid();
                foreach (var s in status.DeploymentStatus()) {
                    twitterEventService.UpdateStatus(account, s);
                }
            } catch (Exception e) {
                ErrorMessageCreated?.Invoke(this, new EventArgs<string>(e.Message));
            } finally {
                slim.Release();
            }
        }

        public async Task UnRetweetAsync(ImmutableAccount account) {
            await slim.WaitAsync();
            try {
                CStatus status = await account.CoreTweetToken.Statuses.UnretweetAsync(id: Id, include_ext_alt_text: true, tweet_mode: TweetMode.extended);
                status.CheckValid();
                if (status.IsRetweeted ?? false) {
                    //返り値に反映されてない
                    status.IsRetweeted = false;
                    status.RetweetCount -= 1;
                }
                foreach (var s in status.DeploymentStatus()) {
                    twitterEventService.UpdateStatus(account, s);
                }
            } catch (Exception e) {
                ErrorMessageCreated?.Invoke(this, new EventArgs<string>(e.Message));
            } finally {
                slim.Release();
            }
        }

        public async Task FavoriteAsync(ImmutableAccount account) {
            await slim.WaitAsync();
            try {
                CStatus status = await account.CoreTweetToken.Favorites.CreateAsync(id: Id, include_ext_alt_text: true, tweet_mode: TweetMode.extended);
                status.CheckValid();
                foreach (var s in status.DeploymentStatus()) {
                    twitterEventService.UpdateStatus(account, s);
                }
            } catch (Exception e) {
                ErrorMessageCreated?.Invoke(this, new EventArgs<string>(e.Message));
            } finally {
                slim.Release();
            }
        }

        public async Task UnFavoriteAsync(ImmutableAccount account) {
            await slim.WaitAsync();
            try {
                CStatus status = await account.CoreTweetToken.Favorites.DestroyAsync(id: Id, include_ext_alt_text: true, tweet_mode: TweetMode.extended);
                status.CheckValid();
                foreach (var s in status.DeploymentStatus()) {
                    twitterEventService.UpdateStatus(account, s);
                }
            } catch (Exception e) {
                ErrorMessageCreated?.Invoke(this, new EventArgs<string>(e.Message));
            } finally {
                slim.Release();
            }
        }
    }
}
