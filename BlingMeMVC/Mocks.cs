namespace BlingMeMVC
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using BlingMe.Domain.Entities;

    public class Mocks
    {
        public Mocks()
        {
            Bracelets = new List<Bracelet>();
            Bracelets.Add(new Bracelet { Owner = "smithf", Name = "Fred Smith", ID = 1, Type = BraceletType.Person });
            Bracelets.Add(new Bracelet { Owner = "jonesd", Name = "Dave Jones", ID = 3, Type = BraceletType.Person });
            Bracelets.Add(new Bracelet { Owner = "fryera", Name = "Andy Fryer", ID = 4, Type = BraceletType.Person, Email = "fryera@iress.co.uk" });
            Bracelets.Add(new Bracelet { Owner = "gorel", Name = "Lauren Gore", ID = 5, Type = BraceletType.Person, Email = "gorel@iress.co.uk" });
            Bracelets.Add(new Bracelet { Owner = "edmistj", Name = "Joanne Edmiston", ID = 6, Type = BraceletType.Person, Email = "edmistj@iress.co.uk" });
            Bracelets.Add(new Bracelet { Owner = "ashwinj", Name = "James Ashwin", ID = 7, Type = BraceletType.Person, Email = "ashwinj@iress.co.uk" });
            Bracelets.Add(new Bracelet { Owner = "willihan", Name = "Hannah Williams", ID = 8, Type = BraceletType.Person, Email = "willihan@iress.co.uk" });
            Bracelets.Add(new Bracelet { Owner = "edwards", Name = "Sarah Edwards", ID = 9, Type = BraceletType.Person, Email = "edwards@iress.co.uk" });
            Bracelets.Add(new Bracelet { Owner = "U4D", Name = "U4D", ID = 14, Type = BraceletType.Location });
            Bracelets.Add(new Bracelet { Owner = "Nansen", Name = "Nansen", ID = 15, Type = BraceletType.Location });
            Bracelets.Add(new Bracelet { Owner = "Tomlinson", Name = "Tomlinson", Description = "A delightful meeting room", ID = 16, Type = BraceletType.Location });
            Bracelets.Add(new Bracelet { Owner = "Shackleton", Name = "Shackleton", ID = 17, Type = BraceletType.Location });
            Bracelets.Add(new Bracelet { Owner = "Poulton", Name = "Poulton", ID = 10, Type = BraceletType.Location });
            Bracelets.Add(new Bracelet { Owner = "Australia", Name = "Australia", ID = 25, Type = BraceletType.Location });
            Bracelets.Add(new Bracelet { Owner = "R15", Name = "R15", ID = 11, Type = BraceletType.Project });
            Bracelets.Add(new Bracelet { Owner = "reception", Name = "Jacket Potatoes", ID = 21, Type = BraceletType.Interest });

            Charms = new List<Charm>();
            Charms.Add(addCharm(1, 3));
            Charms.Add(addCharm(7, 4));
            Charms.Add(addCharm(8, 7));
            Charms.Add(addCharm(9, 7));
            Charms.Add(addCharm(5, 7));
            Charms.Add(addCharm(5, 11));
            Charms.Add(addCharm(6, 7));
            Charms.Add(addCharm(15, 10));
            Charms.Add(addCharm(16, 10));
            Charms.Add(addCharm(17, 10));
            Charms.Add(addCharm(5, 14));
            Charms.Add(addCharm(5, 21));
        }

        public List<Charm> Charms { get; set; }

        public List<Bracelet> Bracelets { get; set; }

        public Charm addCharm(int child, int parent)
        {
            var charm = new Charm();
            var parentBracelet = (from b in Bracelets where b.ID == parent select b).FirstOrDefault();
            var childBracelet = (from b in Bracelets where b.ID == child select b).FirstOrDefault();
            charm.Parent = parentBracelet;
            charm.Children = new List<Bracelet>();
            charm.Children.Add(childBracelet);

            if (null == parentBracelet.Charms)
            {
                parentBracelet.Charms = new List<Charm>();
            }
            if (null == childBracelet.Charms)
            {
                childBracelet.Charms = new List<Charm>();
            }
            parentBracelet.Charms.Add(charm);
            childBracelet.Charms.Add(charm);
            return charm;
        }
    }
}