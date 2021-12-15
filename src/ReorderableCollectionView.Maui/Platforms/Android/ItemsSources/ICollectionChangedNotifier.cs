using Microsoft.Maui.Controls.Handlers.Items;

// Cloned from the main .NET MAUI repository.
// No changes were made.
// https://github.com/dotnet/maui/tree/main/src/Controls/src/Core/Handlers/Items/Android/ItemsSources/ICollectionChangedNotifier.cs
namespace ReorderableCollectionView.Maui
{
	// Lets observable items sources notify observers about dataset changes
	internal interface ICollectionChangedNotifier
	{
		void NotifyDataSetChanged();
		void NotifyItemChanged(IItemsViewSource source, int startIndex);
		void NotifyItemInserted(IItemsViewSource source, int startIndex);
		void NotifyItemMoved(IItemsViewSource source, int fromPosition, int toPosition);
		void NotifyItemRangeChanged(IItemsViewSource source, int start, int end);
		void NotifyItemRangeInserted(IItemsViewSource source, int startIndex, int count);
		void NotifyItemRangeRemoved(IItemsViewSource source, int startIndex, int count);
		void NotifyItemRemoved(IItemsViewSource source, int startIndex);
	}
}