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
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using AndroidSlideLayout;
using CoreTweet;
using FFImageLoading;
using FFImageLoading.Views;
using Reactive.Bindings.Extensions;
using Twichirp.Android.App.Extensions;
using Twichirp.Android.App.View;
using Twichirp.Android.App.ViewModel;

namespace Twichirp.Android.App.ViewController {

    public class ImageViewerViewController : BaseViewController<IImageViewerView,ImageViewerViewModel> {

        public ImageViewerViewController(IImageViewerView view,ImageViewerViewModel viewModel) : base(view,viewModel) {
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.OnCreateEventHandler += x,x => view.OnCreateEventHandler -= x)
                .Subscribe(x => onCreate(x.Sender,x.EventArgs))
                .AddTo(Disposable);
            Observable.FromEventPattern<LifeCycleEventArgs>(x => view.OnDestroyEventHandler += x,x => view.OnDestroyEventHandler -= x)
                .Subscribe(x => onDestroy(x.Sender,x.EventArgs))
                .AddTo(Disposable);
        }

        private void onCreate(object sender,LifeCycleEventArgs args) {
            Observable.FromEventPattern<ViewReleasedEventArgs>(x => View.SlideLayout.ViewReleased += x,x => View.SlideLayout.ViewReleased -= x)
                .Subscribe(x => viewReleased(x.Sender,x.EventArgs))
                .AddTo(Disposable);
            ViewModel.Media.Subscribe(x => setMedia(x)).AddTo(Disposable);
        }

        private void onDestroy(object sender,LifeCycleEventArgs args) {
            releaseImages();
        }

        private void setMedia(IEnumerable<MediaEntity> medias) {
            if(View.PageLayout.PageCount > 0) {
                releaseImages();
                View.PageLayout.ClearPage();
            }
            for(int i = 0;i < medias.Count();i++) {
                var imageViewAsync = new ImageViewAsync(View.PageLayout.Context);
                var asyncImage = ImageService.Instance.LoadUrl(medias.ElementAt(i).MediaUrl).Retry(2);
                if(i == ViewModel.DefaultPage) {
                    ViewCompat.SetTransitionName(imageViewAsync,ImageViewerViewModel.TransitionName);
                    View.Activity.SupportPostponeEnterTransition();
                    asyncImage.Success(() => View.Activity.SupportStartPostponedEnterTransition());
                }
                View.PageLayout.AddPageView(imageViewAsync);
                asyncImage.Into(imageViewAsync);
            }
            View.PageLayout.DefaultPage = ViewModel.DefaultPage;
        }

        private void releaseImages() {
            for(int i = 0;i < View.PageLayout.PageCount;i++) {
                var page = View.PageLayout.GetPage(i);
                if(page is ImageView) {
                    (page as ImageView).ReleaseImage();
                }
            }
        }

        private void viewReleased(object sender,ViewReleasedEventArgs args) {
            var slideLayout = sender as SlideLayout;
            int distance = Math.Abs(slideLayout.CurrentDragChildViewLayoutedTop - slideLayout.CurrentDragChildViewDraggedTop);
            int finishDistance = convertDensityIndependentPixelToPixel(150);
            if(distance > finishDistance) {
                args.Handled = true;
                if(View.PageLayout.CurrentFirstVisiblePage == ViewModel.DefaultPage) {
                    ActivityCompat.FinishAfterTransition(View.Activity);
                } else {
                    View.Activity.Finish();
                }
            }
        }

        private int convertDensityIndependentPixelToPixel(float dp) {
            var metrics = View.ApplicationContext.Resources.DisplayMetrics;
            return (int)(dp * ((int)metrics.DensityDpi / 160f));
        }

    }
}