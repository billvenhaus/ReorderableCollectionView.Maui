using AndroidX.RecyclerView.Widget;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Handlers.Items;

// Cloned from the main .NET MAUI repository.
// No changes were made.
// https://github.com/dotnet/maui/tree/main/src/Controls/src/Core/Handlers/Items/Android/Adapters/AdapterNotifier.cs
namespace ReorderableCollectionView.Maui
{
	internal class AdapterNotifier : ICollectionChangedNotifier
	{
		readonly RecyclerView.Adapter _adapter;

		public AdapterNotifier(RecyclerView.Adapter adapter)
		{
			_adapter = adapter;
		}

		public void NotifyDataSetChanged()
		{
			if (IsValidAdapter())
				_adapter.NotifyDataSetChanged();
		}

		public void NotifyItemChanged(IItemsViewSource source, int startIndex)
		{
			if (IsValidAdapter())
				_adapter.NotifyItemChanged(startIndex);
		}

		public void NotifyItemInserted(IItemsViewSource source, int startIndex)
		{
			if (IsValidAdapter())
			{
				_adapter.NotifyItemInserted(startIndex);

				var changedCount = _adapter.ItemCount - startIndex;
				_adapter.NotifyItemRangeChanged(startIndex, changedCount);
			}
		}

		public void NotifyItemMoved(IItemsViewSource source, int fromPosition, int toPosition)
		{
			if (IsValidAdapter())
			{
				_adapter.NotifyItemMoved(fromPosition, toPosition);

				var minPosition = System.Math.Min(fromPosition, toPosition);
				var changedCount = _adapter.ItemCount - minPosition;
				_adapter.NotifyItemRangeChanged(minPosition, changedCount);
			}
		}

		public void NotifyItemRangeChanged(IItemsViewSource source, int start, int end)
		{
			if (IsValidAdapter())
				_adapter.NotifyItemRangeChanged(start, end);
		}

		public void NotifyItemRangeInserted(IItemsViewSource source, int startIndex, int count)
		{
			if (IsValidAdapter())
			{
				_adapter.NotifyItemRangeInserted(startIndex, count);

				var changedCount = _adapter.ItemCount - startIndex;
				_adapter.NotifyItemRangeChanged(startIndex, changedCount);
			}
		}

		public void NotifyItemRangeRemoved(IItemsViewSource source, int startIndex, int count)
		{
			if (IsValidAdapter())
			{
				_adapter.NotifyItemRangeRemoved(startIndex, count);

				var changedCount = _adapter.ItemCount - startIndex;
				_adapter.NotifyItemRangeChanged(startIndex, changedCount);
			}
		}

		public void NotifyItemRemoved(IItemsViewSource source, int startIndex)
		{
			if (IsValidAdapter())
			{
				_adapter.NotifyItemRemoved(startIndex);

				var changedCount = _adapter.ItemCount - startIndex;
				_adapter.NotifyItemRangeChanged(startIndex, changedCount);
			}
		}

		internal bool IsValidAdapter()
		{
			if (_adapter == null || _adapter.IsDisposed())
				return false;

			return true;
		}
	}
}