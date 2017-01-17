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
using Plugin.CrossFormattedText.Abstractions;
using Plugin.CrossFormattedText;

namespace Twichirp.Core.App.ViewModel {
    public class StatusViewModel : BaseViewModel {

        public const int NormalTweet = 1;
        public const int MediaTweet = 2;
        public const int QuotedTweet = 3;
        public const int QuotedInnerMediaTweet = 4;
        public const int QuotedOuterMediaTweet = 5;
        public const int QuotedInnerAndOuterMediaTweet = 6;

        public IDisposable DataHolder { get; set; }
        public Account Account { get; }

        internal StatusModel StatusModel;
        public long Id { get; private set; }
        public ReactiveProperty<ISpannableString> SpannableText { get; private set; } = new ReactiveProperty<ISpannableString>();
        public string Source { get; private set; }
        public ReadOnlyReactiveProperty<string> DateTime { get; private set; }
        public int RetweetCount { get; private set; }
        public string RetweetCountText { get; private set; }
        public int FavoriteCount { get; private set; }
        public string FavoriteCountText { get; private set; }
        public bool IsRetweeted { get; private set; }
        public bool IsFavorited { get; private set; }
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

        /*
         * Quoted Status is Nullable
         * */
        public bool IsQuoting { get; private set; }
        public ReactiveProperty<ISpannableString> QuotedSpannableText { get; private set; } = new ReactiveProperty<ISpannableString>();
        public string QuotedName { get; private set; }
        public string QuotedScreenName { get; private set; }
        public IEnumerable<MediaEntity> QuotedMedia { get; private set; }

        public ReactiveCommand ShowStatusCommand { get; } = new ReactiveCommand();
        public ReactiveCommand RetweetCommand { get; } = new ReactiveCommand();
        public ReactiveCommand FavoriteCommand { get; } = new ReactiveCommand();

        public StatusViewModel(ITwichirpApplication application,Status status,Account account) : base(application) {
            Account = account;
            StatusModel = new StatusModel(Application,status);

            DateTime = StatusModel.ToContentStatus().ObserveProperty(x => x.CreatedAt)
                .CombineLatest(ShowStatusCommand,(x,y) => relativeDateTime(x))
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposable);

            initStaticValue();
            Observable.FromEventPattern<EventArgs>(x => StatusModel.StatusChanged += x,x => StatusModel.StatusChanged -= x)
                .Subscribe(x => initStaticValue())
                .AddTo(Disposable);
            if(StatusModel.RetweetedStatus != null) {
                Observable.FromEventPattern<EventArgs>(x => StatusModel.RetweetedStatus.StatusChanged += x,x => StatusModel.RetweetedStatus.StatusChanged -= x)
                    .Subscribe(x => initStaticValue())
                    .AddTo(Disposable);
            }
            if(StatusModel.QuotedStatus != null) {
                Observable.FromEventPattern<EventArgs>(x => StatusModel.QuotedStatus.StatusChanged += x,x => StatusModel.QuotedStatus.StatusChanged -= x)
                    .Subscribe(x => initStaticValue())
                    .AddTo(Disposable);
            }

            RetweetCommand
                .Subscribe(x => {
                    if(IsRetweeted) {
                        StatusModel.ToContentStatus().UnRetweet(account);
                    } else {
                        StatusModel.ToContentStatus().Retweet(account);
                    }
                })
                .AddTo(Disposable);
            FavoriteCommand
                .Subscribe(x => {
                    if(IsFavorited) {
                        StatusModel.ToContentStatus().UnFavorite(account);
                    } else {
                        StatusModel.ToContentStatus().Favorite(account);
                    }
                })
                .AddTo(Disposable);
        }

        private void initStaticValue() {
            IStringResource stringResource = Application.StringResource;
            var contentStatus = StatusModel.ToContentStatus();

            SpannableText.Value = spannableText(
                contentStatus.Text,
                contentStatus.HiddenPrefix,
                contentStatus.HiddenSuffix,
                contentStatus.QuotedStatus == null && contentStatus.Media.Count() == 0
                );

            if(StatusModel.ToContentStatus().QuotedStatus != null) {
                var quotedStatus = contentStatus.QuotedStatus;

                QuotedSpannableText.Value = spannableText(quotedStatus.Text,quotedStatus.HiddenPrefix,quotedStatus.HiddenSuffix,quotedStatus.Media.Count() == 0);
            }

            Id = StatusModel.Id;

            Source = StatusModel.ToContentStatus().Source;

            RetweetCount = StatusModel.ToContentStatus().RetweetCount;
            RetweetCountText = RetweetCount.ToString();
            FavoriteCount = StatusModel.ToContentStatus().FavoriteCount;
            FavoriteCountText = FavoriteCount.ToString();
            IsRetweeted = StatusModel.ToContentStatus().IsRetweeted;
            IsFavorited = StatusModel.ToContentStatus().IsFavorited;
            InReplyToScreenName = StatusModel.ToContentStatus().InReplyToScreenName.Map(x => x == null ? null : $"@{x}");
            ReplyToUser = InReplyToScreenName?.Map(x => string.Format(Application.GetLocalizedString(stringResource.StatusReplyToUser),x));
            Media = StatusModel.ToContentStatus().Media;

            IconUrl = StatusModel.ToContentStatus().User.ProfileImageUrl;
            Name = StatusModel.ToContentStatus().User.Name;
            ScreenName = StatusModel.ToContentStatus().User.ScreenName.Map(x => $"@{x}");
            IsProtected = StatusModel.ToContentStatus().User.IsProtected;
            IsVerified = StatusModel.ToContentStatus().User.IsVerified;

            IsRetweeting = StatusModel.RetweetedStatus.Map(x => x != null);
            RetweetingUser = StatusModel.User.ScreenName.Map(x => string.Format(Application.GetLocalizedString(stringResource.StatusRetweetingUser),x));

            IsQuoting = StatusModel.ToContentStatus().QuotedStatus.Map(x => x != null);
            QuotedName = StatusModel.ToContentStatus().QuotedStatus?.User.Name;
            QuotedScreenName = StatusModel.ToContentStatus().QuotedStatus?.User.ScreenName.Map(x => $"@{x}");
            QuotedMedia = StatusModel.ToContentStatus().QuotedStatus?.Media;
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

        private ISpannableString spannableText(IEnumerable<TextPart> text,IEnumerable<UserMentionEntity> hiddenPrefix,IEnumerable<UrlEntity> hiddenSuffix,bool containsSuffix) {
            var blueColor = new SpanColor(60,90,170);

            var spans = new List<Span>();
            foreach(var mention in hiddenPrefix) {
                spans.Add(new Span() { Text = $"@{mention.ScreenName} ",ForegroundColor = blueColor });
            }
            foreach(var textPart in text) {
                switch(textPart.Type) {
                    case TextPartType.Plain: {
                            spans.Add(new Span { Text = textPart.Text });
                            break;
                        }
                    case TextPartType.Hashtag: {
                            spans.Add(new Span { Text = textPart.Text,ForegroundColor = blueColor });
                            break;
                        }
                    case TextPartType.Cashtag: {
                            spans.Add(new Span { Text = textPart.Text,ForegroundColor = blueColor });
                            break;
                        }
                    case TextPartType.Url: {
                            spans.Add(new Span { Text = textPart.Text,ForegroundColor = blueColor });
                            break;
                        }
                    case TextPartType.UserMention: {
                            spans.Add(new Span { Text = textPart.Text,ForegroundColor = blueColor });
                            break;
                        }
                }
            }
            if(containsSuffix) {
                foreach(var url in hiddenSuffix) {
                    spans.Add(new Span { Text = $" {url.DisplayUrl}",ForegroundColor = blueColor });
                }
            }
            return CrossCrossFormattedText.Current.Format(new FormattedString() { Spans = spans });
        }

        public int ToStatusType() {
            if(IsQuoting && QuotedMedia.Count() > 0 && Media.Count() > 0) {
                return QuotedInnerAndOuterMediaTweet;
            }
            if(IsQuoting && QuotedMedia.Count() > 0) {
                return QuotedInnerMediaTweet;
            }
            if(IsQuoting && Media.Count() > 0) {
                return QuotedOuterMediaTweet;
            }
            if(IsQuoting) {
                return QuotedTweet;
            }
            if(Media.Count() > 0) {
                return MediaTweet;
            }
            return NormalTweet;
        }

        public override void Dispose() {
            base.Dispose();
            DataHolder?.Dispose();
        }
    }
}
