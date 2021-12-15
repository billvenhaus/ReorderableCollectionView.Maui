using ReorderableCollectionViewDemos.ViewModels;
using Microsoft.Maui.Controls;

namespace ReorderableCollectionViewDemos.Views
{
    public partial class VariableSpanGridSpacingPage : ContentPage
    {
        public VariableSpanGridSpacingPage()
        {
            InitializeComponent();
            BindingContext = new MonkeysViewModel();
        }
    }
}
