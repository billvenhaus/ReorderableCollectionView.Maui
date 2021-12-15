using ReorderableCollectionViewDemos.ViewModels;
using Microsoft.Maui.Controls;

namespace ReorderableCollectionViewDemos.Views
{
    public partial class VariableSpanGridGroupingPage : ContentPage
    {
        public VariableSpanGridGroupingPage()
        {
            InitializeComponent();
            BindingContext = new GroupedAnimalsViewModel();
        }
    }
}
