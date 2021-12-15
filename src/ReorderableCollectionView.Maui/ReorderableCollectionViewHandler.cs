using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Controls = Microsoft.Maui.Controls;

namespace ReorderableCollectionView.Maui
{
	public partial class ReorderableCollectionViewHandler
	{
		public ReorderableCollectionViewHandler() : base(ReorderableCollectionViewMapper)
		{

		}
		public ReorderableCollectionViewHandler(PropertyMapper mapper = null) : base(mapper ?? ReorderableCollectionViewMapper)
		{

		}

		public static PropertyMapper<ReorderableCollectionView, ReorderableCollectionViewHandler> ReorderableCollectionViewMapper = new PropertyMapper<ReorderableCollectionView, ReorderableCollectionViewHandler>(ViewMapper)
		{
			[Controls.ItemsView.ItemsSourceProperty.PropertyName] = MapItemsSource,
			[Controls.ItemsView.HorizontalScrollBarVisibilityProperty.PropertyName] = MapHorizontalScrollBarVisibility,
			[Controls.ItemsView.VerticalScrollBarVisibilityProperty.PropertyName] = MapVerticalScrollBarVisibility,
			[Controls.ItemsView.ItemTemplateProperty.PropertyName] = MapItemTemplate,
			[Controls.ItemsView.EmptyViewProperty.PropertyName] = MapEmptyView,
			[Controls.ItemsView.EmptyViewTemplateProperty.PropertyName] = MapEmptyViewTemplate,
			[Controls.ItemsView.FlowDirectionProperty.PropertyName] = MapFlowDirection,
			[Controls.ItemsView.IsVisibleProperty.PropertyName] = MapIsVisible,
			[Controls.ItemsView.ItemsUpdatingScrollModeProperty.PropertyName] = MapItemsUpdatingScrollMode,
			[StructuredItemsView.HeaderTemplateProperty.PropertyName] = MapHeaderTemplate,
			[StructuredItemsView.FooterTemplateProperty.PropertyName] = MapFooterTemplate,
			[StructuredItemsView.ItemsLayoutProperty.PropertyName] = MapItemsLayout,
			[StructuredItemsView.ItemSizingStrategyProperty.PropertyName] = MapItemSizingStrategy,
			[ReorderableCollectionView.CanReorderItemsProperty.PropertyName] = MapCanReorderItems,
			//[SelectableItemsView.SelectedItemProperty.PropertyName] = MapSelectedItem,
			//[SelectableItemsView.SelectedItemsProperty.PropertyName] = MapSelectedItems,
			//[SelectableItemsView.SelectionModeProperty.PropertyName] = MapSelectionMode,
			//[GroupableItemsView.IsGroupedProperty.PropertyName] = MapIsGrouped
		};
	}
}