namespace BlingMeMVC.Models.ViewModels
{
    using System.Collections.Generic;

    using BlingMe.Domain.Entities;

    public class CharmView
    {
        public CharmView(Charm charm)
        {
            ID = charm.ID;
            Name = charm.Name;
            Owner = charm.Owner;

            Parent = charm.Parent;
            Children = charm.Children;
        }

        public int ID { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }

        public virtual Bracelet Parent { get; set; }
        public ICollection<Bracelet> Children { get; set; }
    }
}