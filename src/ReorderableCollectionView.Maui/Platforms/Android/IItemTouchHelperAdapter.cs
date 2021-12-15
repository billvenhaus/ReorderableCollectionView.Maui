namespace ReorderableCollectionView.Maui
{
	public interface IItemTouchHelperAdapter
	{
		bool OnItemMove(int fromPosition, int toPosition);
	}
}