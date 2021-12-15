using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers.Items;
using Microsoft.Maui.Graphics;

namespace ReorderableCollectionView.Maui
{
	public partial class ReorderableCollectionViewHandler : GroupableItemsViewHandler<ReorderableCollectionView>
	{
		protected override ItemsViewController<ReorderableCollectionView> CreateController(ReorderableCollectionView itemsView, ItemsViewLayout layout)
			=> new ReorderableCollectionViewController<ReorderableCollectionView>(itemsView, layout);

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			if (Controller.Layout is VariableSpanGridViewLayout variableSpanLayout)
			{
				var maxWidth = Math.Min(Device.Info.ScaledScreenSize.Width, widthConstraint);
				var maxHeight = Math.Min(Device.Info.ScaledScreenSize.Height, heightConstraint);
				var fitSize = variableSpanLayout.GetSizeThatFits(maxWidth, maxHeight, Controller.ItemsSource);
				return fitSize;
			}
			return base.GetDesiredSize(widthConstraint, heightConstraint);
		}

		protected override ItemsViewLayout SelectLayout()
		{
			var itemsLayout = ItemsView.ItemsLayout;

			if (itemsLayout is VariableSpanGridItemsLayout variableSpanItemsLayout)
			{
				return new VariableSpanGridViewLayout(variableSpanItemsLayout);
			}
			else
			{
				return base.SelectLayout();
			}
		}

		public static void MapCanReorderItems(ReorderableCollectionViewHandler handler, ReorderableCollectionView itemsView)
		{
			(handler.Controller as ReorderableCollectionViewController<ReorderableCollectionView>)?.UpdateCanReorderItems();
		}
	}
}