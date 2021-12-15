using ReorderableCollectionViewDemos.ViewModels;
using Microsoft.Maui.Controls;

namespace ReorderableCollectionViewDemos.Views
{
    public partial class VerticalListGroupingPage : ContentPage
    {
        public VerticalListGroupingPage()
        {
            InitializeComponent();
            BindingContext = new GroupedAnimalsViewModel();
        }
    }
}
