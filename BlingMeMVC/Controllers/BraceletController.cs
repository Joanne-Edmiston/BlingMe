namespace BlingMeMVC.Controllers
{
    using System.Web.Mvc;

    using BlingMe.Domain.EF;

    public class BraceletController : Controller
    {
        private readonly EFDbContext db = new EFDbContext();

        public ActionResult Index()
        {
            return View(db.Bracelets);
        }

        public ActionResult Details(int id)
        {
            return RedirectToAction("Bracelet", "Home", new { id });
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
