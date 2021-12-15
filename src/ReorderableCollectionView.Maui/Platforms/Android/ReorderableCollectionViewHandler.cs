using System;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Controls.Handlers.Items;

namespace ReorderableCollectionView.Maui
{
	public partial class ReorderableCollectionViewHandler : GroupableItemsViewHandler<ReorderableCollectionView>
	{
		new protected ReorderableCollectionViewAdapter<ReorderableCollectionView, IGroupableItemsViewSource> CreateAdapter() => new(VirtualView);

		protected override RecyclerView CreateNativeView() =>
			new ReorderableMauiRecyclerView<ReorderableCollectionView, ReorderableCollectionViewAdapter<ReorderableCollectionView, IGroupableItemsViewSource>, IGroupableItemsViewSource>(Context, GetItemsLayout, CreateAdapter);

		public static void MapCanReorderItems(ReorderableCollectionViewHandler handler, ReorderableCollectionView itemsView)
		{
			(handler.NativeView as IReorderableMauiRecyclerView<ReorderableCollectionView>)?.UpdateCanReorderItems();
		}
	}
}