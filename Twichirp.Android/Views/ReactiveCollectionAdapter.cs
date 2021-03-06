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
using System.Collections.Specialized;
using Android.Support.V7.Widget;
using Android.Views;
using Reactive.Bindings;

namespace Twichirp.Android.Views {

    public class ReactiveCollectionAdapter<T> : RecyclerView.Adapter, IDisposable where T : class {

        public event EventHandler<EventArgs> LastItemShowed;

        private List<RecyclerView> recyclerViews = new List<RecyclerView>();
        private INotifyCollectionChanged notify;
        private IList<T> list;
        private Func<T, int, int> viewTypeSelector;
        private Func<ViewGroup, int, RecyclerView.ViewHolder> viewCreator;

        public ReactiveCollectionAdapter(
            INotifyCollectionChanged list,
            Func<T, int, int> viewTypeSelector,
            Func<ViewGroup, int, RecyclerView.ViewHolder> viewCreator) : base() {
            this.list = list as IList<T> ?? throw new ArgumentNullException(nameof(list));
            if (this.list == null) {
                throw new ArgumentException(nameof(list));
            }
            this.notify = list;
            list.CollectionChanged += collectionChanged;

            this.viewTypeSelector = viewTypeSelector;
            this.viewCreator = viewCreator;
        }

        public override int ItemCount => list.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
            if (holder is IBindable<T> bindable) {
                bindable.OnBind(list[position], position);
            }
            if (position == list.Count - 1) {
                LastItemShowed?.Invoke(this, new EventArgs());
            }
        }

        public override int GetItemViewType(int position) => viewTypeSelector(list[position], position);

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) => viewCreator(parent, viewType);

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            if (disposing) {
                notify.CollectionChanged -= collectionChanged;
            }
        }

        public override void OnAttachedToRecyclerView(RecyclerView recyclerView) {
            base.OnAttachedToRecyclerView(recyclerView);
            recyclerViews.Add(recyclerView);
        }

        public override void OnDetachedFromRecyclerView(RecyclerView recyclerView) {
            base.OnDetachedFromRecyclerView(recyclerView);
            recyclerViews.Remove(recyclerView);
        }

        private bool isHolderRecyclable(int startIndex, int count) {
            bool isRecyclable = false;
            foreach (var recyclerView in recyclerViews) {
                for (int i = startIndex; i < startIndex + count; i++) {
                    isRecyclable |= recyclerView.FindViewHolderForAdapterPosition(i)?.IsRecyclable ?? false;
                    if (isRecyclable) {
                        return isRecyclable;
                    }
                }
            }
            return isRecyclable;
        }

        private void collectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            // うーーんこの https://code.google.com/p/android/issues/detail?id=193069
            if (e.Action == NotifyCollectionChangedAction.Add) {
                if (e.NewItems.Count == 1) {
                    this.NotifyItemInserted(e.NewStartingIndex);
                } else {
                    this.NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems.Count);
                }
            } else if (e.Action == NotifyCollectionChangedAction.Move) {
                this.NotifyItemMoved(e.OldStartingIndex, e.NewStartingIndex);
            } else if (e.Action == NotifyCollectionChangedAction.Remove) {
                if (e.OldItems.Count == 1) {
                    this.NotifyItemRemoved(e.OldStartingIndex);
                } else {
                    this.NotifyItemRangeRemoved(e.OldStartingIndex, e.OldItems.Count);
                }
                this.NotifyDataSetChanged();
            } else if (e.Action == NotifyCollectionChangedAction.Replace) {
                bool isRecylable = isHolderRecyclable(e.NewStartingIndex, e.NewItems.Count);
                if (isRecylable) {
                    if (e.NewItems.Count == 1) {
                        this.NotifyItemChanged(e.NewStartingIndex);
                    } else {
                        this.NotifyItemRangeChanged(e.NewStartingIndex, e.NewItems.Count);
                    }
                } else {
                    this.NotifyDataSetChanged();
                }
            } else if (e.Action == NotifyCollectionChangedAction.Reset) {
                this.NotifyDataSetChanged();
            }
        }

        public override void OnViewRecycled(Java.Lang.Object holder) {
            base.OnViewRecycled(holder);

            if (holder is IRecyclable recyclable) {
                recyclable.OnRecycled();
            }
        }
    }

    public interface IBindable<T> {
        void OnBind(T item, int position);
    }

    public interface IRecyclable {
        void OnRecycled();
    }

    public static class RecyclerAdapterExtensions {

        public static ReactiveCollectionAdapter<T> ToAdapter<T>(
            this ReadOnlyReactiveCollection<T> collection,
            Func<T, int, int> viewTypeSelector,
            Func<ViewGroup, int, RecyclerView.ViewHolder> viewCreator)
            where T : class
            => new ReactiveCollectionAdapter<T>(collection, viewTypeSelector, viewCreator);

        public static ReactiveCollectionAdapter<T> ToAdapter<T>(
           this ReactiveCollection<T> collection,
           Func<T, int, int> viewTypeSelector,
           Func<ViewGroup, int, RecyclerView.ViewHolder> viewCreator)
            where T : class
           => new ReactiveCollectionAdapter<T>(collection, viewTypeSelector, viewCreator);
    }
}