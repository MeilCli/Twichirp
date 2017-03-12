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

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidSlideLayout;
using Reactive.Bindings.Extensions;
using Twichirp.Android.Extensions;
using Twichirp.Android.App.View;
using Twichirp.Android.Events;
using Twichirp.Core.ViewModels;

namespace Twichirp.Android.App.ViewController {

    public class VideoViewerViewController : BaseViewController<IVideoViewerView,StatusViewModel> {

        private bool isAnimatedGif;

        public VideoViewerViewController(IVideoViewerView view,StatusViewModel viewModel) : base(view,viewModel) {
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.Created += x,x => view.Created -= x)
                .Subscribe(x => onCreate(x.Sender,x.EventArgs))
                .AddTo(Disposable);
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.Resumed += x,x => view.Resumed -= x)
                .Subscribe(x => onResume(x.Sender,x.EventArgs))
                .AddTo(Disposable);
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.Paused += x,x => view.Paused -= x)
                .Subscribe(x => onPause(x.Sender,x.EventArgs))
                .AddTo(Disposable);
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.Destroyed += x,x => view.Destroyed -= x)
                .Subscribe(x => onDestroy(x.Sender,x.EventArgs))
                .AddTo(Disposable);
        }

        private void onCreate(object sender,LifeCycleEventArgs args) {
            Observable.FromEventPattern<ViewReleasedEventArgs>(x => View.SlideLayout.ViewReleased += x,x => View.SlideLayout.ViewReleased -= x)
                .Subscribe(x => viewReleased(x.Sender,x.EventArgs))
                .AddTo(Disposable);

            var media = ViewModel.Media.Value.ElementAt(0);
            var variants = media.VideoInfo.Variants;
            isAnimatedGif = media.Type == "animated_gif";
            var url = variants.Where(x => x.ContentType.Contains("mp4")).Select(x => x.Url).FirstOrDefault();
            if(url == null) {
                // こんなことにはならないはず
                Toast.MakeText(View.ApplicationContext,"Not support Video",ToastLength.Short).Show();
                return;
            }
            View.VideoView.SetVideoURI(global::Android.Net.Uri.Parse(url));
            View.VideoView.SetMediaController(new MediaController(View.Activity));
            View.VideoView.RequestFocus();
            View.VideoView.Prepared += prepared;
            View.VideoView.Completion += completion;
        }

        private void onResume(object sender,LifeCycleEventArgs args) {
            View.VideoView.Resume();
        }

        private void onPause(object sender,LifeCycleEventArgs args) {
            if(View.VideoView.CanPause()) {
                View.VideoView.Pause();
            }
        }

        private void onDestroy(object sender,LifeCycleEventArgs args) {
            View.VideoView.StopPlayback();
        }

        private void prepared(object sender,EventArgs args) {
            View.VideoView.Start();
        }

        private void completion(object sender,EventArgs args) {
            if(isAnimatedGif == false) {
                return;
            }
            View.VideoView.SeekTo(0);
            View.VideoView.Start();
        }

        private void viewReleased(object sender,ViewReleasedEventArgs args) {
            var slideLayout = sender as SlideLayout;
            int distance = Math.Abs(slideLayout.CurrentDragChildViewLayoutedTop - slideLayout.CurrentDragChildViewDraggedTop);
            int finishDistance = View.ApplicationContext.ConvertDensityIndependentPixelToPixel(150);
            if(distance > finishDistance) {
                args.Handled = true;
                View.Activity.Finish();
            }
        }

    }
}