using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using ObjCRuntime;
using UIKit;
using Microsoft.Maui.Controls.Handlers.Items;

// Cloned from the main .NET MAUI repository.
// Added case for when the itemsSource is already an IList. Use it, don't create a new one. Reorder only works with original list.
// Removed CreateForCarouselView. We don't need that for this library.
// https://github.com/dotnet/maui/tree/main/src/Controls/src/Core/Handlers/Items/iOS/ItemsSourceFactory.cs
namespace ReorderableCollectionView.Maui
{
	internal static class ItemsSourceFactory
	{
		public static IItemsViewSource Create(IEnumerable itemsSource, UICollectionViewController collectionViewController)
		{
			if (itemsSource == null)
			{
				return new EmptySource();
			}

			switch (itemsSource)
			{
				case IList _ when itemsSource is INotifyCollectionChanged:
					return new ObservableItemsSource(itemsSource as IList, collectionViewController);
				case IEnumerable _ when itemsSource is INotifyCollectionChanged:
					return new ObservableItemsSource(itemsSource as IEnumerable, collectionViewController);
				case IList list:
					return new ListSource(list);
				case IEnumerable<object> generic:
					return new ListSource(generic);
			}

			return new ListSource(itemsSource);
		}

		public static IItemsViewSource CreateGrouped(IEnumerable itemsSource, UICollectionViewController collectionViewController)
		{
			if (itemsSource == null)
			{
				return new EmptySource();
			}

			return new ObservableGroupedSource(itemsSource, collectionViewController);
		}
	}
}