using System;
using System.Collections;
using Android.Content;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers.Items;
using Microsoft.Maui.Graphics;

namespace ReorderableCollectionView.Maui
{
	public class ReorderableCollectionViewAdapter<TItemsView, TItemsViewSource> : GroupableItemsViewAdapter<TItemsView, TItemsViewSource>, IItemsViewAdapter, IItemTouchHelperAdapter
		where TItemsView : ReorderableCollectionView
		where TItemsViewSource : IGroupableItemsViewSource
	{
		IItemsViewSource IItemsViewAdapter.ItemsSource => ItemsSource;
		ItemsView IItemsViewAdapter.ItemsView => ItemsView;

		public ReorderableCollectionViewAdapter(TItemsView reorderableCollectionView,
			Func<View, Context, ItemContentView> createView = null) : base(reorderableCollectionView, createView)
		{
		}

		protected override void BindTemplatedItemViewHolder(TemplatedItemViewHolder templatedItemViewHolder, object context)
		{
			if (ItemsView.ItemsLayout is VariableSpanGridItemsLayout variableSpanLayout)
			{
				var itemViewType = templatedItemViewHolder.ItemViewType;
				if (itemViewType == ItemViewType.TemplatedItem || itemViewType == ItemViewType.TextItem)
				{
					var androidContext = templatedItemViewHolder.ItemView.Context;
					var columnSpan = variableSpanLayout.ItemSpanLookup?.GetColumnSpan(context) ?? 1;
					var itemWidth = (columnSpan * variableSpanLayout.ItemWidth) + variableSpanLayout.HorizontalItemSpacing * (columnSpan - 1);
					templatedItemViewHolder.Bind(context, ItemsView, null, new Size(androidContext.ToPixels(itemWidth), androidContext.ToPixels(variableSpanLayout.ItemHeight)));
					return;
				}
			}

			base.BindTemplatedItemViewHolder(templatedItemViewHolder, context);
		}

		protected override TItemsViewSource CreateItemsSource()
		{
			return (TItemsViewSource)ItemsSourceFactory.Create(ItemsView, this);
		}

		public bool OnItemMove(int fromPosition, int toPosition)
		{
			var itemsSource = ItemsSource;
			var itemsView = ItemsView;

			if (itemsSource == null || itemsView == null)
			{
				return false;
			}

			if (itemsSource is IGroupedItemsViewSource groupedSource)
			{
				var (fromGroupIndex, fromIndex) = groupedSource.GetGroupAndIndex(fromPosition);
				var fromList = groupedSource.GetGroup(fromGroupIndex) as IList;
				var fromItemsSource = groupedSource.GetGroupItemsViewSource(fromGroupIndex);
				var fromItemIndex = fromIndex - (fromItemsSource?.HasHeader == true ? 1 : 0);

				var (toGroupIndex, toIndex) = groupedSource.GetGroupAndIndex(toPosition);
				var toList = groupedSource.GetGroup(toGroupIndex) as IList;
				var toItemsSource = groupedSource.GetGroupItemsViewSource(toGroupIndex);
				var toItemIndex = toIndex - (toItemsSource?.HasHeader == true ? 1 : 0);

				if (toGroupIndex > fromGroupIndex)
				{
					toItemIndex = toItemIndex + 1;
				}

				if (toGroupIndex != fromGroupIndex && !itemsView.CanMixGroups)
				{
					return false;
				}

				if (fromList != null && toList != null)
				{
					var fromItem = fromList[fromItemIndex];
					SetObserveChanges(fromItemsSource, false);
					SetObserveChanges(toItemsSource, false);
					fromList.RemoveAt(fromItemIndex);
					toList.Insert(toItemIndex, fromItem);
					NotifyItemMoved(fromPosition, toPosition);
					SetObserveChanges(fromItemsSource, true);
					SetObserveChanges(toItemsSource, true);
					itemsView.SendReorderCompleted();
					return true;
				}
			}
			else if (itemsView.ItemsSource is IList list)
			{
				var fromItem = list[fromPosition];
				SetObserveChanges(itemsSource, false);
				list.RemoveAt(fromPosition);
				list.Insert(toPosition, fromItem);
				NotifyItemMoved(fromPosition, toPosition);
				SetObserveChanges(itemsSource, true);
				itemsView.SendReorderCompleted();
				return true;
			}
			return false;
		}

		void SetObserveChanges(IItemsViewSource itemsSource, bool enable)
		{
			if (itemsSource is IObservableItemsViewSource observableSource)
			{
				observableSource.ObserveChanges = enable;
			}
		}
	}
}