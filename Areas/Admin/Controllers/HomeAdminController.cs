using Dictionary.DTO;
using Dictionary.Models;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Dictionary.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {

        private DictionaryEntities db = new DictionaryEntities();

        // GET: Admin/Word/Index
        public ActionResult Index()
        {
            var result = db.tblWords.ToList();
            return View(result);
        }

        // GET: Admin/Word/Create
        public ActionResult Create()
        {
            // Load data for dropdown lists (Id_Language, Id_wordtype, Id_Language_trans) here

            return PartialView("_Create");
        }

        // POST: Admin/Word/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblWord word)
        {
            if (ModelState.IsValid)
            {
                db.tblWords.Add(word);
                db.SaveChanges();
                return Json(new { success = true });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors = errors });
        }

        // GET: Admin/Word/Edit/5
        public ActionResult Edit(int id)
        {
            var word = db.tblWords.Find(id);
            if (word == null)
            {
                return HttpNotFound();
            }

            // Load data for dropdown lists (Id_Language, Id_wordtype, Id_Language_trans) here

            return PartialView("_Edit", word);
        }

        // POST: Admin/Word/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblWord word)
        {
            if (ModelState.IsValid)
            {
                db.Entry(word).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors = errors });
        }

        // GET: Admin/Word/Delete/5
        public ActionResult Delete(int id)
        {
            var word = db.tblWords.Find(id);
            if (word == null)
            {
                return HttpNotFound();
            }

            return PartialView("_Delete", word);
        }

        // POST: Admin/Word/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var word = db.tblWords.Find(id);
            if (word == null)
            {
                return HttpNotFound();
            }

            db.tblWords.Remove(word);
            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}