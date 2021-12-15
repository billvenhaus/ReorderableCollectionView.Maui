using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ReorderableCollectionViewDemos.Models
{
    public class AnimalGroup : ObservableCollection<Animal>
    {
        public string Name { get; private set; }

        public AnimalGroup(string name, List<Animal> animals) : base(animals)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
