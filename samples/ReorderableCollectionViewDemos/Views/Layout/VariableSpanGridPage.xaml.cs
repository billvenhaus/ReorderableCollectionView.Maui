using Microsoft.Maui.Controls;
using ReorderableCollectionViewDemos.ViewModels;
using ReorderableCollectionView.Maui;
using ReorderableCollectionViewDemos.Models;

namespace ReorderableCollectionViewDemos.Views
{
    public partial class VariableSpanGridPage : ContentPage
    {
        public VariableSpanGridPage()
        {
            InitializeComponent();
            BindingContext = new MonkeysViewModel();
        }
    }
}
