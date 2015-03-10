using BlingMe.Domain.EF;
using BlingMe.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingMe.Test
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void GetAllBraceletsOrderedDescending()
        {
            var uow = new UnitOfWork();
            var repo = uow.GetRepository<Bracelet>();

            var braceletes = repo.Get(orderByField: "Name", ascending: false).ToList();

            Assert.IsTrue(braceletes.Count() > 0);
            Assert.IsTrue(braceletes.Any(b => b.Name == "U4D - Poulton"));
            Assert.IsTrue(braceletes.First().Name == "U4D - Poulton");
            
        }


        [TestMethod]
        public void GetBraceletByName()
        {
            var uow = new UnitOfWork();
            var repo = uow.GetRepository<Bracelet>();

            //Note - to retireve the "Charms" with the entity you need to include it in the include properties
            var braceletes = repo.Get(filter: b => b.Type == BraceletType.Person && b.Name == "James Ashwin",
                includeProperties: "Charms").ToList();

            Assert.IsTrue(braceletes.Count() == 1);
            Assert.IsTrue(braceletes[0].Name == "James Ashwin");
            Assert.IsTrue(braceletes[0].Charms != null && braceletes[0].Charms.Count > 0);

        }

        [TestMethod]
        public void AddAndDeleteBracelet()
        {
            int id = 0;
            var uow = new UnitOfWork();
            var repo = uow.GetRepository<Bracelet>();
            var bracelet = new Bracelet { Name = "AddBracelet", Description = "AddBracelet", Email = "AddBracelet@bla.com", Owner = "Me" };

            repo.Insert(bracelet);
            uow.Save();

            id = bracelet.ID;

            VerifyBraceletSaved(id);

            DeleteBracelets(id);

            VerifyBraceletDeleted(id);

        }

        [TestMethod]
        public void AddandRemoveCharmsBracelet()
        {
            var uow = new UnitOfWork();
            var braceletRepo = uow.GetRepository<Bracelet>();
            var charmRepo = uow.GetRepository<Charm>();

            // Add parent bracelet
            var parentBracelet = new Bracelet { Name = "AddCharmsBraceletParent", Description = "AddCharmsBraceletParent", Email = "AddCharmsBraceletParent@bla.com", Owner = "Me" };

            // Add some childs
            var child1Bracelet = new Bracelet { Name = "child1Bracelet", Description = "child1Bracelet", Email = "child1Bracelet@bla.com", Owner = "Me" };
            var child2Bracelet = new Bracelet { Name = "child2Bracelet", Description = "child2Bracelet", Email = "child2Bracelet@bla.com", Owner = "Me" };
            var child3Bracelet = new Bracelet { Name = "child2Bracelet", Description = "child2Bracelet", Email = "child2Bracelet@bla.com", Owner = "Me" };

            braceletRepo.Insert(parentBracelet);
            braceletRepo.Insert(child1Bracelet);
            braceletRepo.Insert(child2Bracelet);
            braceletRepo.Insert(child3Bracelet);

            // Save bracelets so we can get some IDs
            uow.Save();

            // Add a Charm for the parentBracelete and 2 childs
            var charm = new Charm
            {
                Name = "AddCharmsBracelet",
                ParentID = parentBracelet.ID,
                Children = new List<Bracelet> { child1Bracelet, child2Bracelet }
            };

            charmRepo.Insert(charm);
            uow.Save();

            VerifyCharmSaved(charm.ID, new List<int> { child1Bracelet.ID, child2Bracelet.ID });
            VerifyParentCharmChildren(parentBracelet.ID, charm.ID, new List<int> { child1Bracelet.ID, child2Bracelet.ID});

            // Add another child to the charm
            charm.Children.Add(child3Bracelet);
            uow.Save();

            VerifyParentCharmChildren(parentBracelet.ID, charm.ID, new List<int> { child1Bracelet.ID, child2Bracelet.ID, child3Bracelet.ID });

            // Remove child from charm
            charm.Children.Remove(child3Bracelet);
            uow.Save();

            VerifyParentCharmChildren(parentBracelet.ID, charm.ID, new List<int> { child1Bracelet.ID, child2Bracelet.ID });


            // Tidy up
            DeleteCharms(charm.ID);
            DeleteBracelets(parentBracelet.ID, child1Bracelet.ID, child2Bracelet.ID, child3Bracelet.ID);

        }

        private void VerifyBraceletSaved(int id)
        {
            var uow = new UnitOfWork();
            var repo = uow.GetRepository<Bracelet>();

            Assert.IsNotNull(repo.GetByID(id));
            
        }

        private void VerifyBraceletDeleted(int id)
        {
            var uow = new UnitOfWork();
            var repo = uow.GetRepository<Bracelet>();

            Assert.IsTrue(repo.Get(filter: b => b.ID == id).Count() == 0);
        }

        private void DeleteBracelets(params int[] ids)
        {
            var uow = new UnitOfWork();
            var repo = uow.GetRepository<Bracelet>();

            ids.ToList().ForEach(id => repo.Delete(id));
            uow.Save();
        }

        private void VerifyCharmSaved(int id, List<int> braceletIDs)
        {
            var uow = new UnitOfWork();
            var repo = uow.GetRepository<Charm>();

            var charm = repo.Get(filter: c => c.ID == id, includeProperties: "Children").Single();

            Assert.IsNotNull(charm.Children != null);
            braceletIDs.ForEach(i => charm.Children.Any(c => c.ID == i));
        }


        private void VerifyParentCharmChildren(int parentBraceletId, int charmId, List<int> childIds)
        {
            var uow = new UnitOfWork();
            var braceletRepo = uow.GetRepository<Bracelet>();
            var charmRepo = uow.GetRepository<Charm>();

            // Get the parent bracelete
            var parent = braceletRepo.Get(filter: b => b.ID == parentBraceletId, includeProperties: "Charms").Single();
            Assert.IsNotNull(parent.Charms);

            // Get the charm and it's child braceletes
            var charm = charmRepo.Get(filter: c => c.ID == charmId, includeProperties: "Children").Single();
            Assert.IsTrue(charm.Children != null && charm.Children.Count() == childIds.Count());

            childIds.ForEach(i => charm.Children.Any(c => c.ID == i));
            
        }

        private void DeleteCharms(params int[] ids)
        {
            var uow = new UnitOfWork();
            var repo = uow.GetRepository<Charm>();

            ids.ToList().ForEach(id => {
                // First remove the children
                var charm = repo.Get(filter: c => c.ID == id, includeProperties: "Children").Single();
                charm.Children.Clear();
                
                // Remove the relationship from the db
                uow.Save();

                repo.Delete(charm);
            });

            uow.Save();
        }

    }
}
