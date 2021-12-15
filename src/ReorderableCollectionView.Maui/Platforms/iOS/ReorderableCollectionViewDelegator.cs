using CoreGraphics;
using Foundation;
using UIKit;
using Microsoft.Maui.Controls.Handlers.Items;

namespace ReorderableCollectionView.Maui
{
	public class ReorderableCollectionViewDelegator<TItemsView, TViewController> : GroupableItemsViewDelegator<TItemsView, TViewController>
		where TItemsView : ReorderableCollectionView
		where TViewController : ReorderableCollectionViewController<TItemsView>
	{
		public ReorderableCollectionViewDelegator(ItemsViewLayout itemsViewLayout, TViewController itemsViewController)
			: base(itemsViewLayout, itemsViewController)
		{
		}

		public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
		{
			if (ItemsViewLayout is VariableSpanGridViewLayout variableSpanLayout)
			{
				var itemsLayout = variableSpanLayout.ItemsLayout;
				var columnSpan = itemsLayout.ItemSpanLookup?.GetColumnSpan(ViewController.ItemsSource[indexPath]) ?? 1;
				if (columnSpan > 1)
				{
					var itemWidth = (itemsLayout.ItemWidth * columnSpan) + itemsLayout.HorizontalItemSpacing * (columnSpan - 1);
					return new CGSize(itemWidth, itemsLayout.ItemHeight);
				}
				else
				{
					return variableSpanLayout.ItemSize;
				}
			}
			else
			{
				return base.GetSizeForItem(collectionView, layout, indexPath);
			}
		}

		public override NSIndexPath GetTargetIndexPathForMove(UICollectionView collectionView, NSIndexPath originalIndexPath, NSIndexPath proposedIndexPath)
		{
			NSIndexPath targetIndexPath;

			var itemsView = ViewController.ItemsView;
			if (itemsView?.IsGrouped == true)
			{
				if (originalIndexPath.Section == proposedIndexPath.Section || itemsView.CanMixGroups)
				{
					targetIndexPath = proposedIndexPath;
				}
				else
				{
					targetIndexPath = originalIndexPath;
				}
			}
			else
			{
				targetIndexPath = proposedIndexPath;
			}

			return targetIndexPath;
		}
	}
}