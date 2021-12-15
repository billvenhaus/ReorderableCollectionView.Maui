using Microsoft.Maui.Controls;

namespace ReorderableCollectionView.Maui
{
	public class VariableSpanGridItemsLayout : ItemsLayout
	{
		public static readonly BindableProperty ItemWidthProperty =
			BindableProperty.Create(nameof(ItemWidth), typeof(double), typeof(VariableSpanGridItemsLayout), 100.0d);

		public double ItemWidth
		{
			get => (double)GetValue(ItemWidthProperty);
			set => SetValue(ItemWidthProperty, value);
		}

		public static readonly BindableProperty ItemHeightProperty =
			BindableProperty.Create(nameof(ItemHeight), typeof(double), typeof(VariableSpanGridItemsLayout), 100.0d);

		public double ItemHeight
		{
			get => (double)GetValue(ItemHeightProperty);
			set => SetValue(ItemHeightProperty, value);
		}

		public static readonly BindableProperty VerticalItemSpacingProperty =
			BindableProperty.Create(nameof(VerticalItemSpacing), typeof(double), typeof(VariableSpanGridItemsLayout), default(double),
				validateValue: (bindable, value) => (double)value >= 0);

		public double VerticalItemSpacing
		{
			get => (double)GetValue(VerticalItemSpacingProperty);
			set => SetValue(VerticalItemSpacingProperty, value);
		}

		public static readonly BindableProperty HorizontalItemSpacingProperty =
			BindableProperty.Create(nameof(HorizontalItemSpacing), typeof(double), typeof(VariableSpanGridItemsLayout), default(double),
				validateValue: (bindable, value) => (double)value >= 0);

		public double HorizontalItemSpacing
		{
			get => (double)GetValue(HorizontalItemSpacingProperty);
			set => SetValue(HorizontalItemSpacingProperty, value);
		}

		public static readonly BindableProperty ItemSpanSizeLookupProperty =
			BindableProperty.Create(nameof(ItemSpanLookup), typeof(IItemSpanLookup), typeof(VariableSpanGridItemsLayout), null);

		public IItemSpanLookup ItemSpanLookup
		{
			get => (IItemSpanLookup)GetValue(ItemSpanSizeLookupProperty);
			set => SetValue(ItemSpanSizeLookupProperty, value);
		}

		public VariableSpanGridItemsLayout() : base(ItemsLayoutOrientation.Vertical)
		{
		}
	}
}