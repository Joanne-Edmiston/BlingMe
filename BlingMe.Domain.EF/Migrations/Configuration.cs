namespace BlingMe.Domain.EF.Migrations
{
    using BlingMe.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BlingMe.Domain.EF.EFDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BlingMe.Domain.EF.EFDbContext context)
        {
            var bracelets = new List<Bracelet>
            {
                new Bracelet { Type = BraceletType.Person, Name = "Joanne Edmiston", Description = "Developer", Owner = "edmistj", Email = "Joanne.Edmiston@iress.co.uk" },
                new Bracelet { Type = BraceletType.Person, Name = "James Ashwin", Description = "Team Awesome Team Leader", Owner = "ashwinj", Email = "James.Ashwin@iress.co.uk" },
                new Bracelet { Type = BraceletType.Person, Name = "Lauren Gore", Description = "Developer", Owner = "gorel", Email = "Lauren.Gore@iress.co.uk" },
                new Bracelet { Type = BraceletType.Person, Name = "Hanna Williams", Description = "Developer", Owner = "williah", Email = "Hannah.Williams@iress.co.uk" },
                new Bracelet { Type = BraceletType.Person, Name = "Andy Fryer", Description = "Dev Team Manager", Owner = "fryera", Email = "Andy.Fryer@iress.co.uk" },
                new Bracelet { Type = BraceletType.Person, Name = "Justin Healey", Description = "Dev Manager", Owner = "healeyj", Email = "Justin.Healey@iress.co.uk" },
                
                new Bracelet { Type = BraceletType.Location, Name = "Poulton", Description = "Soon to be ex-office", Owner = "ashwinj" },
                new Bracelet { Type = BraceletType.Location, Name = "Tomlinson", Description = "Meeting Room, U4D, Poulton", Owner = "ashwinj", Email = "Tomlinson.MeetingRoom@iress.co.uk" },
                new Bracelet { Type = BraceletType.Location, Name = "Nansen", Description = "Meeting Room, U4U, Poulton", Owner = "ashwinj", Email = "Nansen.MeetingRoom@iress.co.uk" },
                new Bracelet { Type = BraceletType.Location, Name = "U4D - Poulton", Description = "U4D, Poulton", Owner = "ashwinj" },

                new Bracelet { Type = BraceletType.Group, Name = "Team Awesome", Description = "Team Awesome", Owner = "ashwinj" },
                new Bracelet { Type = BraceletType.Interest, Name = "AWESOME Potatoes", Description = "Group of people who like Jacket Potatoes", Owner = "ashwinj" },
            };

            bracelets.ForEach(b =>
                context.Bracelets.AddOrUpdate(p => p.Name, b));
            context.SaveChanges();


            var charms = new List<Charm> 
            {
                
                
                new Charm { Name = "Andy's Team", ParentID = context.Bracelets.Where(b => b.Type == BraceletType.Person && b.Name == "Andy Fryer").Single().ID, 
                   Children = new List<Bracelet> { context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "James Ashwin").Single(),
                    }
                },
                
                new Charm { Name = "Justin's Team", ParentID = context.Bracelets.Where(b => b.Type == BraceletType.Person && b.Name == "Justin Healey").Single().ID, 
                   Children = new List<Bracelet> { context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "Andy Fryer").Single(),
                    }
                },
                
                new Charm { Name = "Poulton Locations", ParentID = context.Bracelets.Where(b => b.Type == BraceletType.Location && b.Name == "Poulton").Single().ID, 
                   Children = new List<Bracelet> { context.Bracelets.Where( b => b.Type == BraceletType.Location && b.Name == "U4D - Poulton").Single(),
                        context.Bracelets.Where( b => b.Type == BraceletType.Location && b.Name == "Tomlinson").Single(),
                        context.Bracelets.Where( b => b.Type == BraceletType.Location && b.Name == "Nansen").Single(),
                    }
                },
                
                new Charm { Name = "U4D People", ParentID = context.Bracelets.Where(b => b.Type == BraceletType.Location && b.Name == "U4D - Poulton").Single().ID, 
                    Children = new List<Bracelet> { context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "Joanne Edmiston").Single(),
                        context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "James Ashwin").Single(),
                        context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "Lauren Gore").Single(),
                        context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "Hanna Williams").Single() }
                },

                new Charm { Name = "Team Awesome Members", ParentID = context.Bracelets.Where(b => b.Type == BraceletType.Group && b.Name == "Team Awesome").Single().ID, 
                    Children = new List<Bracelet> { context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "Joanne Edmiston").Single(),
                        context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "James Ashwin").Single(),
                        context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "Lauren Gore").Single(),
                        context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "Hanna Williams").Single() }
                },
                
                new Charm { Name = "AWESOME Potatoes", ParentID = context.Bracelets.Where(b => b.Type == BraceletType.Interest && b.Name == "AWESOME Potatoes").Single().ID, 
                    Children = new List<Bracelet> { context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "Joanne Edmiston").Single(),
                        context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "James Ashwin").Single(),
                        context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "Lauren Gore").Single(),
                        context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "Hanna Williams").Single() }
                },
                 
                new Charm { Name = "James's Team", ParentID = context.Bracelets.Where(b => b.Type == BraceletType.Person && b.Name == "James Ashwin").Single().ID, 
                   Children = new List<Bracelet> { context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "Lauren Gore").Single(),
                                                    context.Bracelets.Where( b => b.Type == BraceletType.Person && b.Name == "Hanna Williams").Single() },
                },
            };

            charms.ForEach(c =>
                context.Charms.AddOrUpdate(p => p.Name, c));

            context.SaveChanges();
        }
    }
}
