using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers.Items;

namespace ReorderableCollectionView.Maui
{
	public interface IItemsViewAdapter
	{
		IItemsViewSource ItemsSource { get; }

		ItemsView ItemsView { get; }
	}
}