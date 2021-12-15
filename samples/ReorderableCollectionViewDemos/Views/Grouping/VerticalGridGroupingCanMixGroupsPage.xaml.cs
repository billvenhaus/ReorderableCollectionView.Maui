using ReorderableCollectionViewDemos.ViewModels;
using Microsoft.Maui.Controls;

namespace ReorderableCollectionViewDemos.Views
{
    public partial class VerticalGridGroupingCanMixGroupsPage : ContentPage
    {
        public VerticalGridGroupingCanMixGroupsPage()
        {
            InitializeComponent();
            BindingContext = new GroupedAnimalsViewModel();
        }
	}
}
