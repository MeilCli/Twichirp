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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Twichirp.Android.App.Extensions;
using Twichirp.Core.App.DataHolder;
using Twichirp.Core.App.ViewModel;
using Twichirp.Core.Extensions;

namespace Twichirp.Android.App.DataHolder {
    public class StatusDataHolder:BaseDataHolder {

        public ViewStates VisiblePrefixText { get; private set; }
        public string PrefixText { get; private set; }
        public ViewStates VisibleText { get; private set; }
        public string Text { get; private set; }
        public ViewStates VisibleSuffixText { get; private set; }
        public string SuffixText { get; private set; }

        public ViewStates VisibleRetweetingUser { get; private set; }

        public ViewStates VisibleReplyToUser { get; private set; }

        public ViewStates VisibleLockIcon { get; private set; }
        public ViewStates VisibleVerifyIcon { get; private set; }

        public StatusDataHolder(StatusViewModel viewModel) {
            VisiblePrefixText = viewModel.HiddenPrefix.Map(x => x.Count() > 0 ? ViewStates.Visible : ViewStates.Gone);
            PrefixText = viewModel.HiddenPrefix.Map(x => $"To: {string.Join(" ",x.Select(y => $"@{y.ScreenName}"))}");
            VisibleText = viewModel.Text.Map(x => x.Count() > 0 ? ViewStates.Visible : ViewStates.Gone);
            Text = viewModel.Text.Map(x => x.ToText());
            VisibleSuffixText = viewModel.HiddenSuffix.Map(x => x.Count() > 0 ? ViewStates.Visible : ViewStates.Gone);
            SuffixText = viewModel.HiddenSuffix.Map(x => $"Attach: {string.Join(" ",x.Select(y => y.DisplayUrl))}");

            VisibleRetweetingUser = viewModel.IsRetweeting.Map(x => x == true ? ViewStates.Visible : ViewStates.Gone);

            VisibleReplyToUser = viewModel.InReplyToScreenName.Map(x => x == null ? ViewStates.Gone : ViewStates.Visible);

            VisibleLockIcon = viewModel.IsProtected.Map(x => x == true ? ViewStates.Visible : ViewStates.Gone);
            VisibleVerifyIcon = viewModel.IsVerified.Map(x => x == true ? ViewStates.Visible : ViewStates.Gone);
        }
    }
}