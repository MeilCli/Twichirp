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
        public string Json {
            get {
                return StatusModel.ExportJson();
            }
        }
        public string QuotedJson {
            get {
                return StatusModel.QuotedStatus?.ExportJson();
            }
        }

        public ReactiveProperty<ISpannableString> SpannableText { get; private set; } = new ReactiveProperty<ISpannableString>();
        public ReactiveProperty<string> Source { get; private set; } = new ReactiveProperty<string>();
        public ReadOnlyReactiveProperty<string> DateTime { get; private set; }
        public ReactiveProperty<int> RetweetCount { get; private set; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> FavoriteCount { get; private set; } = new ReactiveProperty<int>();
        public ReactiveProperty<bool> IsRetweeted { get; private set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> IsFavorited { get; private set; } = new ReactiveProperty<bool>();
        /// <summary>
        /// Nullable
        /// </summary>
        public ReactiveProperty<string> ReplyToUser { get; private set; } = new ReactiveProperty<string>();
        public ReactiveProperty<IEnumerable<MediaEntity>> Media { get; private set; } = new ReactiveProperty<IEnumerable<MediaEntity>>();

        public ReactiveProperty<string> IconUrl { get; private set; } = new ReactiveProperty<string>();
        public ReactiveProperty<ISpannableString> SpannableName { get; private set; } = new ReactiveProperty<ISpannableString>();
        public ReactiveProperty<bool> IsProtected { get; private set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> IsVerified { get; private set; } = new ReactiveProperty<bool>();

        /// <summary>
        /// Nullable
        /// </summary>
        public ReactiveProperty<string> RetweetingUser { get; private set; } = new ReactiveProperty<string>();

        /*
         * Quoted Status is Nullable
         * */
        public bool IsQuoting { get; private set; }
        public ReactiveProperty<ISpannableString> QuotedSpannableName { get; private set; } = new ReactiveProperty<ISpannableString>();
        public ReactiveProperty<ISpannableString> QuotedSpannableText { get; private set; } = new ReactiveProperty<ISpannableString>();
        public ReactiveProperty<IEnumerable<MediaEntity>> QuotedMedia { get; private set; } = new ReactiveProperty<IEnumerable<MediaEntity>>();

        public ReactiveCommand UpdateDateTimeCommand { get; } = new ReactiveCommand();
        public ReactiveCommand RetweetCommand { get; } = new ReactiveCommand();
        public ReactiveCommand FavoriteCommand { get; } = new ReactiveCommand();
        public ReactiveCommand<int> StartMediaViewerPageCommand { get; } = new ReactiveCommand<int>();

        public StatusViewModel(ITwichirpApplication application,Status status,Account account) : base(application) {
            Account = account;
            StatusModel = new StatusModel(Application,status);

            DateTime = StatusModel.ToContentStatus().ObserveProperty(x => x.CreatedAt)
                .CombineLatest(UpdateDateTimeCommand,(x,y) => relativeDateTime(x))
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
                    if(IsRetweeted.Value) {
                        StatusModel.ToContentStatus().UnRetweet(account);
                    } else {
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

        private void initStaticValue() {
            IResource stringResource = Application.Resource;
            var contentStatus = StatusModel.ToContentStatus();

            Id = StatusModel.Id;
            SpannableText.Value = spannableText(
                contentStatus.Text,
                contentStatus.HiddenPrefix,
                contentStatus.HiddenSuffix,
                contentStatus.QuotedStatus == null && contentStatus.Media.Count() == 0
                );
            Source.Value = contentStatus.Source;
            RetweetCount.Value = contentStatus.RetweetCount;
            FavoriteCount.Value = contentStatus.FavoriteCount;
            IsRetweeted.Value = contentStatus.IsRetweeted;
            IsFavorited.Value = contentStatus.IsFavorited;
            if(contentStatus.InReplyToScreenName != null) {
                ReplyToUser.Value = string.Format(Application.Resource.StatusReplyToUser.Value,$"@{contentStatus.InReplyToScreenName}");
            }
            Media.Value = contentStatus.Media;

            IconUrl.Value = contentStatus.User.ProfileImageUrl;
            SpannableName.Value = spannableName(contentStatus.User.Name,contentStatus.User.ScreenName);
            IsProtected.Value = contentStatus.User.IsProtected;
            IsVerified.Value = contentStatus.User.IsVerified;

            if(StatusModel.RetweetedStatus != null) {
                RetweetingUser.Value = string.Format(Application.Resource.StatusRetweetingUser.Value,$"@{StatusModel.User.ScreenName}");
            }

            if(contentStatus.QuotedStatus != null) {
                var quotedStatus = contentStatus.QuotedStatus;

                IsQuoting = true;
                QuotedSpannableName.Value = spannableName(quotedStatus.User.Name,quotedStatus.User.ScreenName);
                QuotedSpannableText.Value = spannableText(quotedStatus.Text,quotedStatus.HiddenPrefix,quotedStatus.HiddenSuffix,quotedStatus.Media.Count() == 0);
                QuotedMedia.Value = quotedStatus.Media;
            }
        }

        private string relativeDateTime(DateTimeOffset dateTimeOffset) {
            IResource stringResource = Application.Resource;
            TimeSpan span = DateTimeOffset.Now - dateTimeOffset;
            if(span.TotalMinutes < 1) {
                if(span.Seconds == 1) {
                    return string.Format(Application.Resource.TimeSecoundAgo.Value,span.Seconds);
                }
                return string.Format(Application.Resource.TimeSecoundsAgo.Value,span.Seconds);
            }
            if(span.TotalHours < 1) {
                if(span.Minutes == 1) {
                    return string.Format(Application.Resource.TimeMinuteAgo.Value,span.Minutes);
                }
                return string.Format(Application.Resource.TimeMinutesAgo.Value,span.Minutes);
            }
            if(span.TotalDays < 1) {
                if(span.Hours == 1) {
                    return string.Format(Application.Resource.TimeHourAgo.Value,span.Hours);
                }
                return string.Format(Application.Resource.TimeHoursAgo.Value,span.Hours);
            }
            if(span.TotalDays < 31) {
                if(span.Days == 1) {
                    return string.Format(Application.Resource.TimeDayAgo.Value,span.Days);
                }
                return string.Format(Application.Resource.TimeDaysAgo.Value,span.Days);
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

        private ISpannableString spannableName(string name,string screenName) {
            var spans = new List<Span>();

            spans.Add(new Span() { Text = name });
            spans.Add(new Span() { Text = $" @{screenName}",FontSize = FontSize.Small });

            return CrossCrossFormattedText.Current.Format(new FormattedString() { Spans = spans });
        }

        public int ToStatusType() {
            if(IsQuoting && QuotedMedia.Value.Count() > 0 && Media.Value.Count() > 0) {
                return QuotedInnerAndOuterMediaTweet;
            }
            if(IsQuoting && QuotedMedia.Value.Count() > 0) {
                return QuotedInnerMediaTweet;
            }
            if(IsQuoting && Media.Value.Count() > 0) {
                return QuotedOuterMediaTweet;
            }
            if(IsQuoting) {
                return QuotedTweet;
            }
            if(Media.Value.Count() > 0) {
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
