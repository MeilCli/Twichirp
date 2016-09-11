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

namespace Twichirp.Core.App.ViewModel {
    public class StatusViewModel : BaseViewModel {

        public const int NormalTweet = 1;
        public const int RetweetedNormalTweet = 2;

        private StatusModel statusModel;
        public ReadOnlyReactiveProperty<long> Id { get; private set; }
        public ReadOnlyReactiveProperty<IEnumerable<TextPart>> Text { get; private set; }
        public ReadOnlyReactiveProperty<IEnumerable<UserMentionEntity>> HiddenPrefix { get; private set; }
        public ReadOnlyReactiveProperty<IEnumerable<UrlEntity>> HiddenSuffix { get; private set; }
        public ReadOnlyReactiveProperty<string> Source { get; private set; } 
        public ReadOnlyReactiveProperty<string> DateTime { get; private set; }
        public ReadOnlyReactiveProperty<int> RetweetCount { get; private set; }
        public ReadOnlyReactiveProperty<int> FavoriteCount { get; private set; }
        public ReadOnlyReactiveProperty<bool> IsRetweeted { get; private set; }
        public ReadOnlyReactiveProperty<bool> IsFavorited { get; private set; }
        public ReadOnlyReactiveProperty<string> InReplyToScreenName { get; private set; }
        public ReadOnlyReactiveProperty<string> ReplyToUser { get; private set; }

        public ReadOnlyReactiveProperty<string> IconUrl { get; private set; }
        public ReadOnlyReactiveProperty<string> Name { get; private set; }
        public ReadOnlyReactiveProperty<string> ScreenName { get; private set; }

        public ReadOnlyReactiveProperty<string> RetweetingUser { get; private set; }

        public ReactiveCommand ShowStatusCommand { get; } = new ReactiveCommand();

        public StatusViewModel(ITwichirpApplication application,Status status) : base(application) {
            IStringResource stringResource = application.StringResource;

            statusModel = new StatusModel(Application,status);
            Id = statusModel.ObserveProperty(x => x.Id).ToReadOnlyReactiveProperty().AddTo(Disposable);
            Text = statusModel.ObserveProperty(x => x.Text).ToReadOnlyReactiveProperty().AddTo(Disposable);
            HiddenPrefix = statusModel.ObserveProperty(x => x.HiddenPrefix).ToReadOnlyReactiveProperty().AddTo(Disposable);
            HiddenSuffix = statusModel.ObserveProperty(x => x.HiddenSuffix).ToReadOnlyReactiveProperty().AddTo(Disposable);
            Source = statusModel.ObserveProperty(x => x.Source).ToReadOnlyReactiveProperty().AddTo(Disposable);
            DateTime = statusModel.ObserveProperty(x => x.CreatedAt)
                .CombineLatest(ShowStatusCommand,(x,y)=> relativeDateTime(x))
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposable);
            RetweetCount = statusModel.ObserveProperty(x => x.RetweetCount).ToReadOnlyReactiveProperty().AddTo(Disposable);
            FavoriteCount = statusModel.ObserveProperty(x => x.FavoriteCount).ToReadOnlyReactiveProperty().AddTo(Disposable);
            IsRetweeted = statusModel.ObserveProperty(x => x.IsRetweeted).ToReadOnlyReactiveProperty().AddTo(Disposable);
            IsFavorited = statusModel.ObserveProperty(x => x.IsFavorited).ToReadOnlyReactiveProperty().AddTo(Disposable);
            InReplyToScreenName = statusModel.ObserveProperty(x => x.InReplyToScreenName)
                .Select(x => x==null?null:$"@{x}")
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposable);
            ReplyToUser = InReplyToScreenName
                .Select(x=>string.Format(Application.GetLocalizedString(stringResource.StatusReplyToUser),x))
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposable);

            IconUrl = statusModel.User.ObserveProperty(x => x.ProfileImageUrl).ToReadOnlyReactiveProperty().AddTo(Disposable);
            Name = statusModel.User.ObserveProperty(x => x.Name).ToReadOnlyReactiveProperty().AddTo(Disposable);
            ScreenName = statusModel.User.ObserveProperty(x => x.ScreenName)
                .Select(x => $"@{x}")
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposable);

            RetweetingUser = ScreenName
                .Select(x => string.Format(Application.GetLocalizedString(stringResource.StatusRetweetingUser),x))
                .ToReadOnlyReactiveProperty()
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
            if(statusModel.RetweetedStatus != null) {
                return RetweetedNormalTweet;
            }
            return NormalTweet;
        }
    }
}
