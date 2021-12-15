using Foundation;
using Microsoft.Maui.Controls.Handlers.Items;

namespace ReorderableCollectionView.Maui
{
	public interface IGroupedItemsViewSource : IItemsViewSource
	{
		IItemsViewSource GroupItemsViewSource(NSIndexPath indexPath);
	}
}