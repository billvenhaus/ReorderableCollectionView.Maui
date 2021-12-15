namespace ReorderableCollectionView.Maui
{
	public interface IItemSpanLookup
	{
		int GetColumnSpan(object item);
	}
}