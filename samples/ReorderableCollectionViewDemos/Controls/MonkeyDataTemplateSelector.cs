using ReorderableCollectionViewDemos.Models;
using Microsoft.Maui.Controls;

namespace ReorderableCollectionViewDemos.Controls
{
    public class MonkeyDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AmericanMonkey { get; set; }
        public DataTemplate OtherMonkey { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((Monkey)item).Location.Contains("America") ? AmericanMonkey : OtherMonkey;
        }
    }
}
