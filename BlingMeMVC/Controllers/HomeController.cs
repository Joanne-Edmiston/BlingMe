namespace BlingMeMVC.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using BlingMeMVC.Models.ViewModels;
    using BlingMe.Domain.EF;
    using BlingMe.Domain.Entities;


    public class HomeController : Controller
    {
        private UnitOfWork uow = new UnitOfWork();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {

            var repo = uow.GetRepository<Bracelet>();
            var loggedOnUserId = GetLoggedOnUserId();

            var modelBracelet = repo.Get(filter: b => b.Owner == loggedOnUserId
                && b.Type == BraceletType.Person).Single();


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

            // Dirty hack - investigate this
            var charms = charmsRepo.Get(filter: c => c.ParentID == modelBracelet.ID);
            modelBracelet.Charms = charms.ToList();


            return View(new BraceletView(modelBracelet, GetLoggedOnUserId()));
        }

        public ActionResult _Search()
        {
            return PartialView(from b in uow.GetRepository<Bracelet>().Get().OrderBy(b => b.Name) select b);
        }

        public ActionResult Search(string query)
        {
            return RedirectToAction("Bracelet", new { id = Convert.ToInt32(query) });
        }


        private string GetLoggedOnUserId()
        {
            return User.Identity.Name.Replace("AVELO\\", string.Empty);
        }
    }
}
