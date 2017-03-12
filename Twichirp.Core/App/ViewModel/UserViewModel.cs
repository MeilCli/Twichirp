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
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;
using Plugin.CrossFormattedText;
using Plugin.CrossFormattedText.Abstractions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Core.App.Model;
using Twichirp.Core.Constants;
using Twichirp.Core.DataObjects;
using CUser = CoreTweet.User;

namespace Twichirp.Core.App.ViewModel {

    public class UserViewModel : BaseViewModel {

        public ImmutableAccount Account { get; }
        private UserModel userModel;

        public ReactiveProperty<string> IconUrl { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Name { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ScreenName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> IsProtected { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> IsVerified { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<ISpannableString> Description { get; } = new ReactiveProperty<ISpannableString>();
        public ReactiveProperty<string> Location { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<ISpannableString> Url { get; } = new ReactiveProperty<ISpannableString>();

        public UserViewModel(CUser user,ImmutableAccount account) : this(new UserModel(user),account) { }

        public UserViewModel(UserModel userModel,ImmutableAccount account) {
            Account = account;
            this.userModel = userModel;

            initStaticValue();
            Observable.FromEventPattern<EventArgs>(x => userModel.UserChanged += x,x => userModel.UserChanged -= x)
                .Subscribe(x => initStaticValue())
                .AddTo(Disposable);
        }

        private void initStaticValue() {
            IconUrl.Value = userModel.ProfileImageUrl.Replace("_normal","_bigger");
            Name.Value = userModel.Name;
            ScreenName.Value = $"@{userModel.ScreenName}";
            IsProtected.Value = userModel.IsProtected;
            IsVerified.Value = userModel.IsVerified;
            Description.Value = toSpannable(userModel.Description);
            Location.Value = userModel.Location;
            Url.Value = toSpannable(userModel.Url);
        }

        private ISpannableString toSpannable(IEnumerable<TextPart> text) {
            var spans = new List<Span>();
            foreach(var textPart in text) {
                switch(textPart.Type) {
                    case TextPartType.Plain: {
                            spans.Add(new Span { Text = textPart.Text });
                            break;
                        }
                    case TextPartType.Hashtag: {
                            spans.Add(new Span { Text = textPart.Text,ForegroundColor = SpanConstant.BlueColor });
                            break;
                        }
                    case TextPartType.Cashtag: {
                            spans.Add(new Span { Text = textPart.Text,ForegroundColor = SpanConstant.BlueColor });
                            break;
                        }
                    case TextPartType.Url: {
                            spans.Add(new Span { Text = textPart.Text,ForegroundColor = SpanConstant.BlueColor });
                            break;
                        }
                    case TextPartType.UserMention: {
                            spans.Add(new Span { Text = textPart.Text,ForegroundColor = SpanConstant.BlueColor });
                            break;
                        }
                }
            }
            return CrossCrossFormattedText.Current.Format(new FormattedString() { Spans = spans });
        }
    }
}
