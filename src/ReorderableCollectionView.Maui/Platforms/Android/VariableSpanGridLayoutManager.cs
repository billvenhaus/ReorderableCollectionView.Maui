using System;
using Android.Content;
using AndroidX.RecyclerView.Widget;

namespace ReorderableCollectionView.Maui
{
	internal class VariableSpanGridLayoutManager : GridLayoutManager
	{
		float _currentColumnWidth = -1;
		int _currentItemCount = -1;
		int _currentWidth = -1;

		public float ColumnWidth { get; set; }

		public VariableSpanGridLayoutManager(Context context, int orientation, bool reverseLayout)
			: base(context, 2, orientation, reverseLayout)
		{
		}

		int GetSpanCountFromLookup(VariableSpanGridSizeLookup spanLookup, int max)
		{
			var itemCount = ItemCount;
			var column = 0;
			for (var itemIndex = 0; itemIndex < itemCount; itemIndex++)
			{
				var columnSpan = spanLookup.GetSpanSize(itemIndex, max);
				column = column + columnSpan;
				if (column >= max)
				{
					return max;
				}
			}
			return column;
		}

		public override void OnLayoutChildren(RecyclerView.Recycler recycler, RecyclerView.State state)
		{
			UpdateSpanCount();
			base.OnLayoutChildren(recycler, state);
		}

		void UpdateSpanCount()
		{
			var itemCount = ItemCount;
			if (itemCount == 0)
			{
				return;
			}

			if (ColumnWidth <= 0)
			{
				return;
			}

			var width = Width;
			if (width <= 0)
			{
				return;
			}

			if ((width != _currentWidth) || (ColumnWidth != _currentColumnWidth) || (itemCount != _currentItemCount))
			{
				var totalWidth = width - PaddingRight - PaddingLeft;
				var spanCount = (int)Math.Max(1, Math.Floor(totalWidth / ColumnWidth));
				if (GetSpanSizeLookup() is VariableSpanGridSizeLookup spanLookup)
				{
					spanCount = GetSpanCountFromLookup(spanLookup, spanCount);
					spanLookup.SpanCount = spanCount;
				}
				SpanCount = spanCount;
				_currentWidth = width;
				_currentColumnWidth = ColumnWidth;
				_currentItemCount = itemCount;
			}
		}
	}
}