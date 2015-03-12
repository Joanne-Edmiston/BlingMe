namespace BlingMeMVC.Controllers
{
    using System.Data.Entity;
    using System.Web.Mvc;

    using BlingMe.Domain.EF;
    using BlingMe.Domain.Entities;

    using BlingMeMVC.Models.ViewModels;

    public class BraceletController : Controller
    {
        private readonly EFDbContext db = new EFDbContext();

        public ActionResult Details(int id)
        {
            return RedirectToAction("Bracelet", "Home", new { id });
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Bracelet bracelet)
        {
            if (ModelState.IsValid)
            {
                db.Bracelets.Add(bracelet);
                db.SaveChanges();

                return RedirectToAction("Bracelet", "Home", new { id = bracelet.ID });
            }
            return View(bracelet);
        }

        public ActionResult ChangeAvatar(int id)
        {
            Bracelet bracelet = db.Bracelets.Find(id);
            if (bracelet == null)
            {
                return HttpNotFound();
            }

            var model = new Avatar
            {
                ID = id,
                Bracelet = bracelet
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAvatar(Avatar avatar)
        {
            if (ModelState.IsValid)
            {
                var newPic = new byte[avatar.File.InputStream.Length];
                avatar.File.InputStream.Read(newPic, 0, newPic.Length);

                var bracelet = db.Bracelets.Find(avatar.ID);
                bracelet.Avatar = newPic;
                db.Entry(bracelet).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Bracelet", "Home", new { id = avatar.ID });
            }
            return View(avatar);
        }

        public ActionResult Edit(int id)
        {
            Bracelet bracelet = db.Bracelets.Find(id);
            if (bracelet == null)
            {
                return HttpNotFound();
            }
            return View(bracelet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Bracelet bracelet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bracelet).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Bracelet","Home", new { id = bracelet.ID});
            }
            return View(bracelet);
        }

        public ActionResult Delete(int id)
        {
            Bracelet bracelet = db.Bracelets.Find(id);
            if (bracelet == null)
            {
                return HttpNotFound();
            }
            return View(bracelet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var bracelet = db.Bracelets.Find(id);
            db.Bracelets.Remove(bracelet);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
