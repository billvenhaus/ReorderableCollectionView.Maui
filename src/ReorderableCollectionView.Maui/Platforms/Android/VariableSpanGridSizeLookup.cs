using AndroidX.RecyclerView.Widget;

namespace ReorderableCollectionView.Maui
{
	internal class VariableSpanGridSizeLookup : GridLayoutManager.SpanSizeLookup
	{
		readonly RecyclerView _recyclerView;

		public IItemSpanLookup Lookup { get; set; }
		public int SpanCount { get; set; }

		public VariableSpanGridSizeLookup(IItemSpanLookup lookup, RecyclerView recyclerView)
		{
			Lookup = lookup;
			_recyclerView = recyclerView;
		}

		public override int GetSpanSize(int position)
		{
			return GetSpanSize(position, SpanCount);
		}

		public int GetSpanSize(int position, int maxSpanSize)
		{
			var adapter = _recyclerView.GetAdapter();
			var itemViewType = adapter.GetItemViewType(position);

			if (itemViewType == ItemViewType.Header || itemViewType == ItemViewType.Footer
				|| itemViewType == ItemViewType.GroupHeader || itemViewType == ItemViewType.GroupFooter)
			{
				return maxSpanSize;
			}

			if (Lookup != null && adapter is IItemsViewAdapter itemsViewAdapter)
			{
				return Lookup.GetColumnSpan(itemsViewAdapter.ItemsSource?.GetItem(position));
			}

			return 1;
		}
	}
}