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
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twichirp.Core.App.Model;
using System.Reactive.Linq;
using Twichirp.Core.Extensions;
using Twichirp.Core.Model;

namespace Twichirp.Core.App.ViewModel {
    public class StatusViewModel : BaseViewModel {

        public const int NormalTweet = 1;
        public const int RetweetedNormalTweet = 2;

        public IDisposable DataHolder { get; set; }
        public Account Account { get; }

        internal StatusModel StatusModel;
        public long Id { get; private set; }
        public IEnumerable<TextPart> Text { get; private set; }
        public IEnumerable<UserMentionEntity> HiddenPrefix { get; private set; }
        public IEnumerable<UrlEntity> HiddenSuffix { get; private set; }
        public string Source { get; private set; } 
        public ReadOnlyReactiveProperty<string> DateTime { get; private set; }
        public ReadOnlyReactiveProperty<int> RetweetCount { get; private set; }
        public ReadOnlyReactiveProperty<string> RetweetCountText { get; private set; }
        public ReadOnlyReactiveProperty<int> FavoriteCount { get; private set; }
        public ReadOnlyReactiveProperty<string> FavoriteCountText { get; private set; }
        public ReadOnlyReactiveProperty<bool> IsRetweeted { get; private set; }
        public ReadOnlyReactiveProperty<bool> IsFavorited { get; private set; }
        public string InReplyToScreenName { get; private set; }
        public string ReplyToUser { get; private set; }
        public IEnumerable<MediaEntity> Media { get; private set; }

        public string IconUrl { get; private set; }
        public string Name { get; private set; }
        public string ScreenName { get; private set; }
        public bool IsProtected { get; private set; }
        public bool IsVerified { get; private set; }

        public bool IsRetweeting { get; private set; }
        public string RetweetingUser { get; private set; }

        public ReactiveCommand ShowStatusCommand { get; } = new ReactiveCommand();
        public ReactiveCommand RetweetCommand { get; } = new ReactiveCommand();
        public ReactiveCommand FavoriteCommand { get; } = new ReactiveCommand();

        public StatusViewModel(ITwichirpApplication application,Status status,Account account) : base(application) {
            Account = account;

            IStringResource stringResource = application.StringResource;

            StatusModel = new StatusModel(Application,status);
            Id = StatusModel.Id;
            Text = StatusModel.ToContentStatus().Text;
            HiddenPrefix = StatusModel.ToContentStatus().HiddenPrefix;
            HiddenSuffix = StatusModel.ToContentStatus().HiddenSuffix;
            Source = StatusModel.ToContentStatus().Source;
            DateTime = StatusModel.ToContentStatus().ObserveProperty(x => x.CreatedAt)
                .CombineLatest(ShowStatusCommand,(x,y)=> relativeDateTime(x))
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposable);
            RetweetCount = StatusModel.ToContentStatus().ObserveProperty(x => x.RetweetCount).ToReadOnlyReactiveProperty().AddTo(Disposable);
            RetweetCountText = RetweetCount
                .Select(x => x.ToString())
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposable);
            FavoriteCount = StatusModel.ToContentStatus().ObserveProperty(x => x.FavoriteCount).ToReadOnlyReactiveProperty().AddTo(Disposable);
            FavoriteCountText = FavoriteCount
                .Select(x => x.ToString())
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposable);
            IsRetweeted = StatusModel.ToContentStatus().ObserveProperty(x => x.IsRetweeted).ToReadOnlyReactiveProperty().AddTo(Disposable);
            IsFavorited = StatusModel.ToContentStatus().ObserveProperty(x => x.IsFavorited).ToReadOnlyReactiveProperty().AddTo(Disposable);
            InReplyToScreenName = StatusModel.ToContentStatus().InReplyToScreenName.Map(x => x==null?null:$"@{x}");
            ReplyToUser = InReplyToScreenName?.Map(x => string.Format(Application.GetLocalizedString(stringResource.StatusReplyToUser),x));
            Media = StatusModel.ToContentStatus().Media;

            IconUrl = StatusModel.ToContentStatus().User.ProfileImageUrl;
            Name = StatusModel.ToContentStatus().User.Name;
            ScreenName = StatusModel.ToContentStatus().User.ScreenName.Map(x => $"@{x}");
            IsProtected = StatusModel.ToContentStatus().User.IsProtected;
            IsVerified = StatusModel.ToContentStatus().User.IsVerified;

            IsRetweeting = StatusModel.RetweetedStatus.Map(x => x != null);
            RetweetingUser = StatusModel.User.ScreenName.Map(x => string.Format(Application.GetLocalizedString(stringResource.StatusRetweetingUser),x));

            RetweetCommand
                .Subscribe(x=> {
                    if(IsRetweeted.Value) {
                        StatusModel.ToContentStatus().UnRetweet(account);
                    }else {
                        StatusModel.ToContentStatus().Retweet(account);
                    }
                })
                .AddTo(Disposable);
            FavoriteCommand
                .Subscribe(x => {
                    if(IsFavorited.Value) {
                        StatusModel.ToContentStatus().UnFavorite(account);
                    } else {
                        StatusModel.ToContentStatus().Favorite(account);
                    }
                })
                .AddTo(Disposable);
        }

        private string relativeDateTime(DateTimeOffset dateTimeOffset) {
            IStringResource stringResource = Application.StringResource;
            TimeSpan span = DateTimeOffset.Now - dateTimeOffset;
            if(span.TotalMinutes < 1) {
                if(span.Seconds == 1) {
                    return string.Format(Application.GetLocalizedString(stringResource.TimeSecoundAgo),span.Seconds);
                }
                return string.Format(Application.GetLocalizedString(stringResource.TimeSecoundsAgo),span.Seconds);
            }
            if(span.TotalHours < 1) {
                if(span.Minutes == 1) {
                    return string.Format(Application.GetLocalizedString(stringResource.TimeMinuteAgo),span.Minutes);
                }
                return string.Format(Application.GetLocalizedString(stringResource.TimeMinutesAgo),span.Minutes);
            }
            if(span.TotalDays < 1) {
                if(span.Hours == 1) {
                    return string.Format(Application.GetLocalizedString(stringResource.TimeHourAgo),span.Hours);
                }
                return string.Format(Application.GetLocalizedString(stringResource.TimeHoursAgo),span.Hours);
            }
            if(span.TotalDays < 31) {
                if(span.Days == 1) {
                    return string.Format(Application.GetLocalizedString(stringResource.TimeDayAgo),span.Days);
                }
                return string.Format(Application.GetLocalizedString(stringResource.TimeDaysAgo),span.Days);
            }
            return dateTimeOffset.ToLocalTime().ToString("G");
        }

        public int ToStatusType() {
            if(StatusModel.RetweetedStatus != null) {
                return RetweetedNormalTweet;
            }
            return NormalTweet;
        }

        public override void Dispose() {
            base.Dispose();
            DataHolder?.Dispose();
        }
    }
}
