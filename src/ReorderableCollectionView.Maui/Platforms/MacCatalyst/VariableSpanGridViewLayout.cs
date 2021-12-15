using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers.Items;
using Microsoft.Maui.Graphics;

namespace ReorderableCollectionView.Maui
{
	internal class VariableSpanGridViewLayout : ItemsViewLayout
	{
		internal VariableSpanGridItemsLayout ItemsLayout { get; }

		public VariableSpanGridViewLayout(VariableSpanGridItemsLayout itemsLayout) : base(itemsLayout, ItemSizingStrategy.MeasureFirstItem)
		{
			ItemsLayout = itemsLayout;
			ItemSize = new CGSize(itemsLayout.ItemWidth, itemsLayout.ItemHeight);
			EstimatedItemSize = CGSize.Empty;
		}

		protected override void HandlePropertyChanged(PropertyChangedEventArgs propertyChanged)
		{
			base.HandlePropertyChanged(propertyChanged);

			if (propertyChanged.PropertyName == VariableSpanGridItemsLayout.ItemWidthProperty.PropertyName || propertyChanged.PropertyName == VariableSpanGridItemsLayout.ItemHeightProperty.PropertyName)
			{
				ItemSize = new CGSize(ItemsLayout.ItemWidth, ItemsLayout.ItemHeight);
				InvalidateLayout();
			}
			else if (propertyChanged.PropertyName == VariableSpanGridItemsLayout.HorizontalItemSpacingProperty.PropertyName || propertyChanged.PropertyName == VariableSpanGridItemsLayout.VerticalItemSpacingProperty.PropertyName)
			{
				UpdateItemSpacing();
			}
			else if (propertyChanged.PropertyName == VariableSpanGridItemsLayout.ItemSpanSizeLookupProperty.PropertyName)
			{
				InvalidateLayout();
			}
		}

		public override void ConstrainTo(CGSize size)
		{
			ConstrainedDimension = size.Width;
		}

		public override nfloat GetMinimumInteritemSpacingForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
		{
			return (nfloat)ItemsLayout.HorizontalItemSpacing;
		}

		public override nfloat GetMinimumLineSpacingForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
		{
			return (nfloat)ItemsLayout.VerticalItemSpacing;
		}

		int GetNumberOfColumnsForWidth(double width, IItemsViewSource itemsSource)
		{
			var max = (int)((width + ItemsLayout.HorizontalItemSpacing) / (ItemsLayout.ItemWidth + ItemsLayout.HorizontalItemSpacing));

			var groupCount = itemsSource.GroupCount;
			if (groupCount > 1)
			{
				return max;
			}
			else
			{
				int column = 0;
				var itemCount = itemsSource.ItemCountInGroup(0);
				for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
				{
					var item = itemsSource[NSIndexPath.FromItemSection(itemIndex, 0)];
					var columnSpan = ItemsLayout.ItemSpanLookup?.GetColumnSpan(item) ?? 1;
					column = column + columnSpan;
					if (column >= max)
					{
						return max;
					}
				}
				return column;
			}
		}

		public Size GetSizeThatFits(double width, double height, IItemsViewSource itemsSource)
		{
			if (itemsSource.ItemCount == 0)
			{
				return Size.Zero;
			}

			var numberOfColumns = GetNumberOfColumnsForWidth(width, itemsSource);
			var fitWidth = (numberOfColumns * ItemsLayout.ItemWidth) + ((numberOfColumns - 1) * ItemsLayout.HorizontalItemSpacing);

			if (itemsSource.GroupCount == 1)
			{
				var fitHeight = 0.0d;
				var currentColumn = numberOfColumns;

				var itemCount = itemsSource.ItemCountInGroup(0);
				for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
				{
					var item = itemsSource[NSIndexPath.FromItemSection(itemIndex, 0)];
					var itemSpan = ItemsLayout.ItemSpanLookup?.GetColumnSpan(item) ?? 1;
					currentColumn = currentColumn + itemSpan;
					if (currentColumn > numberOfColumns)
					{
						currentColumn = itemSpan;
						fitHeight = fitHeight + ItemsLayout.ItemHeight + ItemsLayout.VerticalItemSpacing;
					}
				}

				if (fitHeight > 0)
				{
					fitHeight = fitHeight - ItemsLayout.VerticalItemSpacing;
				}

				return new Size(fitWidth, fitHeight);
			}
			else
			{
				return new Size(fitWidth, height);
			}
		}
	}
}