using System;
using Foundation;
using Microsoft.Maui.Controls.Handlers.Items;

// Cloned from the main .NET MAUI repository.
// No changes were made.
// https://github.com/dotnet/maui/tree/main/src/Controls/src/Core/Handlers/Items/iOS/EmptySource.cs
namespace ReorderableCollectionView.Maui
{
	internal class EmptySource : ILoopItemsViewSource
	{
		public int GroupCount => 0;

		public int ItemCount => 0;

		public bool Loop { get; set; }

		public int LoopCount => 0;

		public object this[NSIndexPath indexPath] => throw new IndexOutOfRangeException("IItemsViewSource is empty");

		public int ItemCountInGroup(nint group) => 0;

		public object Group(NSIndexPath indexPath)
		{
			throw new IndexOutOfRangeException("IItemsViewSource is empty");
		}

		public NSIndexPath GetIndexForItem(object item)
		{
			throw new IndexOutOfRangeException("IItemsViewSource is empty");
		}

		public void Dispose()
		{
		}
	}
}