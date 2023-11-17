using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dictionary.Models;

namespace Dictionary.Areas.Admin.Controllers
{
    public class WordController : Controller
    {
        private DictionaryEntities db = new DictionaryEntities();

        // GET: Admin/Word
        public ActionResult Index()
        {
            var tblWords = db.tblWords.Include(t => t.tblLanguage).Include(t => t.tblLanguage1).Include(t => t.tblUser).Include(t => t.tblWord_type);
            return View(tblWords.ToList());
        }

        // GET: Admin/Word/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblWord tblWord = db.tblWords.Find(id);
            if (tblWord == null)
            {
                return HttpNotFound();
            }
            return View(tblWord);
        }

        // GET: Admin/Word/Create
        public ActionResult Create()
        {
            ViewBag.Id_Language = new SelectList(db.tblLanguages, "Id", "sLanguage");
            ViewBag.Id_Language_trans = new SelectList(db.tblLanguages, "Id", "sLanguage");
            ViewBag.Id_user = new SelectList(db.tblUsers, "Id", "sEmail");
            ViewBag.Id_wordtype = new SelectList(db.tblWord_type, "Id", "sWordtype");
            return View();
        }

        // POST: Admin/Word/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Language,Id_Language_trans,Id_wordtype,Id_user,sWord,sExample,sDefinition")] tblWord tblWord)
        {
            if (ModelState.IsValid)
            {
                db.tblWords.Add(tblWord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Language = new SelectList(db.tblLanguages, "Id", "sLanguage", tblWord.Id_Language);
            ViewBag.Id_Language_trans = new SelectList(db.tblLanguages, "Id", "sLanguage", tblWord.Id_Language_trans);
            ViewBag.Id_user = new SelectList(db.tblUsers, "Id", "sEmail", tblWord.Id_user);
            ViewBag.Id_wordtype = new SelectList(db.tblWord_type, "Id", "sWordtype", tblWord.Id_wordtype);
            return View(tblWord);
        }

        // GET: Admin/Word/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblWord tblWord = db.tblWords.Find(id);
            if (tblWord == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Language = new SelectList(db.tblLanguages, "Id", "sLanguage", tblWord.Id_Language);
            ViewBag.Id_Language_trans = new SelectList(db.tblLanguages, "Id", "sLanguage", tblWord.Id_Language_trans);
            ViewBag.Id_user = new SelectList(db.tblUsers, "Id", "sEmail", tblWord.Id_user);
            ViewBag.Id_wordtype = new SelectList(db.tblWord_type, "Id", "sWordtype", tblWord.Id_wordtype);
            return View(tblWord);
        }

        // POST: Admin/Word/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_Language,Id_Language_trans,Id_wordtype,Id_user,sWord,sExample,sDefinition")] tblWord tblWord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblWord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Language = new SelectList(db.tblLanguages, "Id", "sLanguage", tblWord.Id_Language);
            ViewBag.Id_Language_trans = new SelectList(db.tblLanguages, "Id", "sLanguage", tblWord.Id_Language_trans);
            ViewBag.Id_user = new SelectList(db.tblUsers, "Id", "sEmail", tblWord.Id_user);
            ViewBag.Id_wordtype = new SelectList(db.tblWord_type, "Id", "sWordtype", tblWord.Id_wordtype);
            return View(tblWord);
        }

        // GET: Admin/Word/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblWord tblWord = db.tblWords.Find(id);
            if (tblWord == null)
            {
                return HttpNotFound();
            }
            return View(tblWord);
        }

        // POST: Admin/Word/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblWord tblWord = db.tblWords.Find(id);
            db.tblWords.Remove(tblWord);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
