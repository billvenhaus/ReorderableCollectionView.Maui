using ReorderableCollectionViewDemos.ViewModels;
using Microsoft.Maui.Controls;

namespace ReorderableCollectionViewDemos.Views
{
    public partial class VerticalGridGroupingPage : ContentPage
    {
        public VerticalGridGroupingPage()
        {
            InitializeComponent();
            BindingContext = new GroupedAnimalsViewModel();
        }
    }
}
