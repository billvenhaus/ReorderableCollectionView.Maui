using System.Collections.Specialized;
using Microsoft.Maui.Controls.Handlers.Items;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using WThickness = Microsoft.UI.Xaml.Thickness;
using WSetter = Microsoft.UI.Xaml.Setter;
using WStyle = Microsoft.UI.Xaml.Style;

namespace ReorderableCollectionView.Maui
{
	public partial class ReorderableCollectionViewHandler : GroupableItemsViewHandler<ReorderableCollectionView>
	{
		bool _trackerAllowDrop;

		protected override void ConnectHandler(ListViewBase nativeView)
		{
			base.ConnectHandler(nativeView);

			nativeView.SetValue(FormsListViewAttachedProperties.ItemsViewProperty, Element);
			nativeView.DragItemsStarting += HandleDragItemsStarting;
			nativeView.DragItemsCompleted += HandleDragItemsCompleted;
		}

		protected override void DisconnectHandler(ListViewBase nativeView)
		{
			nativeView.ClearValue(FormsListViewAttachedProperties.ItemsViewProperty);
			nativeView.DragItemsStarting -= HandleDragItemsStarting;
			nativeView.DragItemsCompleted -= HandleDragItemsCompleted;

			base.DisconnectHandler(nativeView);
		}

		protected override CollectionViewSource CreateCollectionViewSource()
		{
			if (Element.IsGrouped)
			{
				return base.CreateCollectionViewSource();
			}

			var itemsSource = Element.ItemsSource;

			return new CollectionViewSource
			{
				Source = itemsSource,
				IsSourceGrouped = false
			};
		}

		/*
		// The Maui implementation of this method is not virtual so we can't override.
		// Some of the ScrollTo functions may not work properly.
		protected override object FindBoundItem(ScrollToRequestEventArgs args)
		{
			if (Element.IsGrouped)
			{
				return base.FindBoundItem(args);
			}

			if (args.Mode == ScrollToMode.Position)
			{
				if (args.Index >= ItemCount)
				{
					return null;
				}

				return GetItem(args.Index);
			}

			return args.Item;
		}*/

		void HandleDragItemsStarting(object sender, DragItemsStartingEventArgs e)
		{
			// Built in reordering only supports ungrouped sources & observable collections.
			var supportsReorder = Element != null && !Element.IsGrouped && Element.ItemsSource is INotifyCollectionChanged;
			if (supportsReorder)
			{
				// The AllowDrop property needs to be enabled when we start the drag operation.
				// We can't simply enable it when we set CanReorderItems because the VisualElementTracker also updates this property.
				// That means the tracker can overwrite any set we do in UpdateCanReorderItems.
				// To avoid that possibility, let's force it to true when the user begins to drag an item.
				// Reset it back to what it was when finished.
				_trackerAllowDrop = ListViewBase.AllowDrop;
				ListViewBase.AllowDrop = true;
			}
			else
			{
				e.Cancel = true;
			}
		}

		void HandleDragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
		{
			ListViewBase.AllowDrop = _trackerAllowDrop;

			Element?.SendReorderCompleted();
		}

		/*  
		// The default Maui Windows handlers do not currently respond to Layout property changes.
		// So we won't worry about the VariableSpanGridItemsLayout property changes at this time.
		// We'll include them once they are implemented for the LinearItemsLayout & GridItemsLayouts.
		protected override void HandleLayoutPropertyChanged(PropertyChangedEventArgs property)
		{
			base.HandleLayoutPropertyChanged(property);

			if (Layout is VariableSpanGridItemsLayout variableSpanLayout && ListViewBase is FormsVariableSpanGridView variableSpanGridView)
			{
				if (property.PropertyName == VariableSpanGridItemsLayout.ItemWidthProperty.PropertyName || property.PropertyName == VariableSpanGridItemsLayout.ItemHeightProperty.PropertyName)
				{
					UpdateVariableSpanGridItemSize(variableSpanGridView, variableSpanLayout);
				}
				else if (property.PropertyName == VariableSpanGridItemsLayout.HorizontalItemSpacingProperty.PropertyName || property.PropertyName == VariableSpanGridItemsLayout.VerticalItemSpacingProperty.PropertyName)
				{
					UpdateVariableSpanGridSpacing(variableSpanGridView, variableSpanLayout);
					UpdateVariableSpanGridItemSize(variableSpanGridView, variableSpanLayout);
				}
				else if (property.PropertyName == VariableSpanGridItemsLayout.ItemSpanSizeLookupProperty.PropertyName)
				{
					UpdateVariableSpanGridLookup(variableSpanGridView, variableSpanLayout);
					UpdateVariableSpanGridItemSize(variableSpanGridView, variableSpanLayout);
				}
			}
		}*/

		public static void MapCanReorderItems(ReorderableCollectionViewHandler handler, ReorderableCollectionView itemsView)
		{
			handler.UpdateCanReorderItems();
		}

		protected override ListViewBase SelectListViewBase()
		{
			switch (Layout)
			{
				case VariableSpanGridItemsLayout variableSpanLayout:
					var gridView = new FormsVariableSpanGridView() { VerticalContentAlignment = VerticalAlignment.Stretch };
					UpdateVariableSpanGridLookup(gridView, variableSpanLayout);
					UpdateVariableSpanGridItemSize(gridView, variableSpanLayout);
					UpdateVariableSpanGridSpacing(gridView, variableSpanLayout);
					return gridView;
				default:
					return base.SelectListViewBase();
			}
		}

		void UpdateCanReorderItems()
		{
			if (Element == null || ListViewBase == null)
			{
				return;
			}

			if (Element.CanReorderItems)
			{
				ListViewBase.CanDragItems = true;
				ListViewBase.CanReorderItems = true;
				ListViewBase.IsSwipeEnabled = true; // Needed so user can reorder with touch (according to docs).
			}
			else
			{
				ListViewBase.CanDragItems = false;
				ListViewBase.CanReorderItems = false;
				ListViewBase.IsSwipeEnabled = false;
			}
		}

		protected override void UpdateItemTemplate()
		{
			if (Element != null && Element.IsGrouped)
			{
				base.UpdateItemTemplate();
				return;
			}

			if (Element == null || ListViewBase == null)
			{
				return;
			}

			ListViewBase.ItemTemplate = Element.ItemTemplate == null ? null : ReorderableCollectionViewTemplates.GetItemTemplate();

			UpdateItemsSource();
		}

		void UpdateVariableSpanGridItemSize(FormsVariableSpanGridView gridView, VariableSpanGridItemsLayout itemsLayout)
		{
			var itemWidth = itemsLayout.ItemWidth + itemsLayout.HorizontalItemSpacing;
			var itemHeight = itemsLayout.ItemHeight + itemsLayout.VerticalItemSpacing;
			gridView.ItemsPanel = itemsLayout.ItemSpanLookup != null ? ReorderableCollectionViewTemplates.GetVariableSizedWrapGrid(itemWidth, itemHeight) : ReorderableCollectionViewTemplates.GetItemsWrapGrid(itemWidth, itemHeight);
		}

		void UpdateVariableSpanGridLookup(FormsVariableSpanGridView gridView, VariableSpanGridItemsLayout itemsLayout)
		{
			gridView.ItemSpanLookup = itemsLayout.ItemSpanLookup;
		}

		void UpdateVariableSpanGridSpacing(FormsVariableSpanGridView gridView, VariableSpanGridItemsLayout itemsLayout)
		{
			var horizontalPadding = -itemsLayout.HorizontalItemSpacing / 2.0;
			var verticalPadding = -itemsLayout.VerticalItemSpacing / 2.0;
			var padding = new WThickness(horizontalPadding, verticalPadding, horizontalPadding, verticalPadding);

			var itemHorizontalMargin = itemsLayout.HorizontalItemSpacing / 2.0;
			var itemVerticalMargin = itemsLayout.VerticalItemSpacing / 2.0;
			var itemMargin = new WThickness(itemHorizontalMargin, itemVerticalMargin, itemHorizontalMargin, itemVerticalMargin);
			var itemContainerStyle = new WStyle(typeof(GridViewItem));
			itemContainerStyle.Setters.Add(new WSetter(FrameworkElement.MarginProperty, itemMargin));

			gridView.Padding = padding;
			gridView.ItemContainerStyle = itemContainerStyle;
		}
	}
}