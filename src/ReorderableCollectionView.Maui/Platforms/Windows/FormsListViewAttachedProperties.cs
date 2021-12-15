using Microsoft.Maui.Controls;
using Microsoft.UI.Xaml;

namespace ReorderableCollectionView.Maui
{
	internal class FormsListViewAttachedProperties : DependencyObject
	{
		public static readonly DependencyProperty ItemsViewProperty = DependencyProperty.RegisterAttached("ItemsView", typeof(ItemsView), typeof(FormsListViewAttachedProperties), new PropertyMetadata(null));
	}
}