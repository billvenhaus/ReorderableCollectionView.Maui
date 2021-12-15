using System;

// Cloned from the main .NET MAUI repository.
// No changes were made.
// https://github.com/dotnet/maui/tree/main/src/Controls/src/Core/Handlers/Items/iOS/ItemComparer.cs
namespace ReorderableCollectionView.Maui
{
	public static class ItemComparer
	{
        public static bool AreEquals(object valueA, object valueB)
        {
            bool result;
            IComparable selfValueComparer;

            selfValueComparer = valueA as IComparable;

            if (valueA == null && valueB != null || valueA != null && valueB == null)
                result = false;
            else if (selfValueComparer != null && selfValueComparer.CompareTo(valueB) != 0)
                result = false;
            else if (!object.Equals(valueA, valueB))
                result = false;
            else
                result = true; 

            return result;
        }
    }
}