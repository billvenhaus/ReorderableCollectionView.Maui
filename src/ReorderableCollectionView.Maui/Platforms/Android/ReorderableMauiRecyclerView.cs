using System;
using System.ComponentModel;
using Android.Content;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers.Items;

namespace ReorderableCollectionView.Maui
{
	public class ReorderableMauiRecyclerView<TItemsView, TAdapter, TItemsViewSource> : MauiRecyclerView<TItemsView, TAdapter, TItemsViewSource>, IReorderableMauiRecyclerView<TItemsView>
		where TItemsView : ItemsView
		where TAdapter : ItemsViewAdapter<TItemsView, TItemsViewSource>
		where TItemsViewSource : IItemsViewSource
	{
		bool _disposed;
		ItemTouchHelper _itemTouchHelper;
		SimpleItemTouchHelperCallback _itemTouchHelperCallback;

		public ReorderableMauiRecyclerView(Context context, Func<IItemsLayout> getItemsLayout, Func<TAdapter> getAdapter) : base(context, getItemsLayout, getAdapter)
		{
		}

		protected override ItemDecoration CreateSpacingDecoration(IItemsLayout itemsLayout)
		{
			return new EqualSpacingItemDecoration(Context, itemsLayout);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && !_disposed)
			{
				if (_itemTouchHelper != null)
				{
					_itemTouchHelper.AttachToRecyclerView(null);
					_itemTouchHelper.Dispose();
					_itemTouchHelper = null;
				}

				if (_itemTouchHelperCallback != null)
				{
					_itemTouchHelperCallback.Dispose();
					_itemTouchHelperCallback = null;
				}

				_disposed = true;
			}

			base.Dispose(disposing);
		}

		protected override void LayoutPropertyChanged(object sender, PropertyChangedEventArgs propertyChanged)
		{
			base.LayoutPropertyChanged(sender, propertyChanged);

			if (ItemsLayout is VariableSpanGridItemsLayout variableLayout)
			{
				if (propertyChanged.PropertyName == VariableSpanGridItemsLayout.ItemWidthProperty.PropertyName || propertyChanged.PropertyName == VariableSpanGridItemsLayout.ItemHeightProperty.PropertyName)
				{
					if (GetLayoutManager() is VariableSpanGridLayoutManager gridLayoutManager)
					{
						UpdateVariableSpanGridColumnWidth(gridLayoutManager, variableLayout);
						ItemsViewAdapter?.NotifyDataSetChanged();
					}
				}
				else if (propertyChanged.PropertyName == VariableSpanGridItemsLayout.HorizontalItemSpacingProperty.PropertyName || propertyChanged.PropertyName == VariableSpanGridItemsLayout.VerticalItemSpacingProperty.PropertyName)
				{
					if (GetLayoutManager() is VariableSpanGridLayoutManager gridLayoutManager)
					{
						UpdateVariableSpanGridColumnWidth(gridLayoutManager, variableLayout);
					}
					UpdateItemSpacing();
				}
				else if (propertyChanged.PropertyName == VariableSpanGridItemsLayout.ItemSpanSizeLookupProperty.PropertyName)
				{
					if (GetLayoutManager() is VariableSpanGridLayoutManager gridLayoutManager && gridLayoutManager.GetSpanSizeLookup() is VariableSpanGridSizeLookup spanLookup)
					{
						spanLookup.Lookup = variableLayout.ItemSpanLookup;
						ItemsViewAdapter?.NotifyDataSetChanged();
					}
				}
			}
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

			if (GetLayoutManager() is VariableSpanGridLayoutManager gridLayoutManager)
			{
				// The default measurement stretches the entire width when using headers & footers.
				var fitWidth = (gridLayoutManager.SpanCount * (int)gridLayoutManager.ColumnWidth) + PaddingRight + PaddingLeft;
				var newMeasuredWidth = Math.Min(fitWidth, MeasuredWidth);
				SetMeasuredDimension(newMeasuredWidth, MeasuredHeight);
			}
		}

		protected override LayoutManager SelectLayoutManager(IItemsLayout layoutSpecification)
		{
			switch (layoutSpecification)
			{
				case VariableSpanGridItemsLayout gridItemsLayout:
					var layoutManager = new VariableSpanGridLayoutManager(Context, LinearLayoutManager.Vertical, false);
					UpdateVariableSpanGridColumnWidth(layoutManager, gridItemsLayout);
					layoutManager.SetSpanSizeLookup(new VariableSpanGridSizeLookup(gridItemsLayout.ItemSpanLookup, this));
					return layoutManager;
				default:
					return base.SelectLayoutManager(layoutSpecification);
			}
		}

		public override void UpdateAdapter()
		{
			base.UpdateAdapter();

			_itemTouchHelperCallback?.SetAdapter(ItemsViewAdapter as IItemTouchHelperAdapter);
		}

		public void UpdateCanReorderItems()
		{
			var canReorderItems = (ItemsView as ReorderableCollectionView)?.CanReorderItems == true;

			if (canReorderItems)
			{
				if (_itemTouchHelperCallback == null)
				{
					_itemTouchHelperCallback = new SimpleItemTouchHelperCallback();
				}
				if (_itemTouchHelper == null)
				{
					_itemTouchHelper = new ItemTouchHelper(_itemTouchHelperCallback);
					_itemTouchHelper.AttachToRecyclerView(this);
				}
				_itemTouchHelperCallback?.SetAdapter(ItemsViewAdapter as IItemTouchHelperAdapter);
			}
			else
			{
				if (_itemTouchHelper != null)
				{
					_itemTouchHelper.AttachToRecyclerView(null);
					_itemTouchHelper.Dispose();
					_itemTouchHelper = null;
				}
				if (_itemTouchHelperCallback != null)
				{
					_itemTouchHelperCallback.Dispose();
					_itemTouchHelperCallback = null;
				}
			}
		}

		protected override void UpdateItemSpacing()
		{
			base.UpdateItemSpacing();

			for (int i = 0; i < ItemDecorationCount; i++)
			{
				var itemDecoration = GetItemDecorationAt(i);
				if (itemDecoration is EqualSpacingItemDecoration spacingDecoration)
				{
					// EqualSpacingItemDecoration applies spacing to all items & all 4 sides of the items.
					// We need to adjust the padding on the RecyclerView so this spacing isn't visible around the outer edge of our control.
					// Horizontal & vertical spacing should only exist between items.
					var horizontalPadding = -spacingDecoration.HorizontalOffset;
					var verticalPadding = -spacingDecoration.VerticalOffset;
					SetPadding(horizontalPadding, verticalPadding, horizontalPadding, verticalPadding);
				}
			}
		}

		void UpdateVariableSpanGridColumnWidth(VariableSpanGridLayoutManager layoutManager, VariableSpanGridItemsLayout itemsLayout)
		{
			layoutManager.ColumnWidth = Context.ToPixels(itemsLayout.ItemWidth + itemsLayout.HorizontalItemSpacing);
		}
	}
}