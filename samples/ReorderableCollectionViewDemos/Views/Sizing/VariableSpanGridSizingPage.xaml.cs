using Microsoft.Maui.Controls;
using ReorderableCollectionViewDemos.ViewModels;
using ReorderableCollectionView.Maui;
using ReorderableCollectionViewDemos.Models;

namespace ReorderableCollectionViewDemos.Views
{
	public partial class VariableSpanGridSizingPage : ContentPage, IItemSpanLookup
	{
		public VariableSpanGridSizingPage()
		{
			InitializeComponent();
			VariableSpanLayout.ItemSpanLookup = this;
			BindingContext = new MonkeysViewModel();
		}

		public int GetColumnSpan(object item)
		{
			if (item is Monkey monkey && monkey.Name == "Blue Monkey")
			{
				return 2;
			}
			return 1;
		}
	}
}
