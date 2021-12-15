using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers.Items;

namespace ReorderableCollectionView.Maui
{
	public interface IReorderableMauiRecyclerView<TItemsView> : IMauiRecyclerView<TItemsView>
		where TItemsView : ItemsView
	{
		void UpdateCanReorderItems();
	}
}