namespace BlingMeMVC.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using BlingMeMVC.Models.ViewModels;
    using BlingMe.Domain.EF;
    using BlingMe.Domain.Entities;
using System.Collections.Generic;


    public class HomeController : Controller
    {
        private UnitOfWork uow = new UnitOfWork();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {

            var repo = uow.GetRepository<Bracelet>();

            Bracelet modelBracelet;

            try
            {
                modelBracelet = repo.Get(filter: b => b.Owner == loggedOnUserId
                   && b.Type == BraceletType.Person).Single();
            }
            catch (Exception e)
            {
                if (e.Message == "Sequence contains no elements")
                {
                    return RedirectToAction("Create", "Bracelet");
                }
                return HttpNotFound();
            }


            return RedirectToRoute("Bracelet", new { id = modelBracelet.ID });
            
            /*
            var mock = new Mocks();
            var braceletId = (from b in mock.Bracelets
                              where b.Owner == User.Identity.Name.Replace("AVELO\\", string.Empty)
                              select b.ID).FirstOrDefault();

            // get the user's own bracelet number from EF and redirect to it
            // User.Identity.Name gives you their Avelo name so this is easy enough
            return RedirectToRoute("Bracelet", new { id = braceletId });
             */
        }

        [AllowAnonymous]
        public ActionResult Bracelet(int id)
        {
            
            var repo = uow.GetRepository<Bracelet>();
            var charmsRepo = uow.GetRepository<Charm>();

            var modelBracelet = repo.Get(filter: b => b.ID == id).FirstOrDefault();

            var charms = charmsRepo.Get(filter: c => c.ParentID == modelBracelet.ID);
            modelBracelet.Charms = charms.ToList();


            return View(new BraceletView(modelBracelet, GetLoggedOnUserBracelet()));
        }

        public ActionResult _Search()
        {
            return PartialView(from b in uow.GetRepository<Bracelet>().Get().OrderBy(b => b.Name) select b);
        }

        public ActionResult Search(string query)
        {
            return RedirectToAction("Bracelet", new { id = Convert.ToInt32(query) });
        }

        [HttpPost]
        public ActionResult AddUser(int parentBraceletId, int userBraceletId)
        {
            var braceletRepo = uow.GetRepository<Bracelet>();
            var charmRepo = uow.GetRepository<Charm>();

            // load the userBracelet
            var userBracelet = braceletRepo.GetByID(userBraceletId);

            var charm = new Charm
            {
                ParentID = parentBraceletId,
                Children = new List<Bracelet> { userBracelet }
            };

            charmRepo.Insert(charm);
            uow.Save();

            return RedirectToAction("Bracelet", new { id = parentBraceletId});
        }

        [HttpPost]
        public ActionResult RemoveUser(int parentBraceletId, int userBraceletId )
        {
            var braceletRepo = uow.GetRepository<Bracelet>();
            var charmRepo = uow.GetRepository<Charm>();

            // load the userBracelet
            var userBracelet = braceletRepo.GetByID(userBraceletId);

            // Find charms where the user is a child 
            var charms = charmRepo.Get(filter: c => c.ParentID == parentBraceletId && c.Children.Any( i => i.ID == userBraceletId),
                includeProperties: "Children").ToList();

            charms.ForEach(c => 
                {
                    c.Children.Remove(userBracelet);
                    if (c.Children.Count == 0)
                    {
                        charmRepo.Delete(c);
                    }
                    else
                    {
                        charmRepo.Update(c);
                    }
                });

            uow.Save();

            return RedirectToAction("Bracelet", new { id = parentBraceletId });
        }

        private Bracelet GetLoggedOnUserBracelet()
        {
            var loggedOnUserId = User.Identity.Name.Replace("AVELO\\", string.Empty);
            return uow.GetRepository<Bracelet>().Get(filter: b => b.Owner == loggedOnUserId
                && b.Type == BraceletType.Person).FirstOrDefault();
        }
    }
}
