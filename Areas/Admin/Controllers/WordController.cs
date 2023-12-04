using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dictionary.DTO;
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

            if (tblWord.Id_Language == null)
            {
                ModelState.AddModelError("Id_Language", "Vui lòng chọn ngôn ngữ.");
            }
            else if (tblWord.Id_wordtype == null)
            {
                ModelState.AddModelError("Id_wordtype", "Vui lòng chọn loại từ.");
            }
            else if (string.IsNullOrEmpty(tblWord.sWord))
            {
                ModelState.AddModelError("sWord", "Vui lòng nhập từ.");
            }
            else if (db.tblWords.Any(w => w.sWord == tblWord.sWord))
            {
                ModelState.AddModelError("sWord", "Từ đã tồn tại.");
            }
            else if (string.IsNullOrEmpty(tblWord.sExample))
            {
                ModelState.AddModelError("sExample", "Vui lòng nhập ví dụ.");
            }
            else if (tblWord.Id_Language_trans == null)
            {
                ModelState.AddModelError("Id_Language_trans", "Vui lòng chọn ngôn ngữ dịch.");
            }
            else if (string.IsNullOrEmpty(tblWord.sDefinition))
            {
                ModelState.AddModelError("Definition", "Vui lòng nhập mô tả cho ngôn ngữ dịch.");
            }

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





        public ActionResult SelectWords()
        {
            DictionaryEntities db = new DictionaryEntities();
            List<tblWord> words = db.tblWords.ToList();
            return View(words);
        }

        [HttpPost]
        public ActionResult InsertWord(tblWord model)
        {
            DictionaryEntities db = new DictionaryEntities();
            db.tblWords.Add(model);
            db.SaveChanges();
            return RedirectToAction("SelectWords");
        }
        [HttpGet]
        public ActionResult InsertWord()
        {
            return View();
        }
        public JsonResult GetLanguage()
        {
            DictionaryEntities db = new DictionaryEntities();
            List<tblLanguage> languages = db.tblLanguages.ToList();
            var languageOptions = languages.Select(language => new
            {
                value = language.Id,
                text = language.sLanguage.ToString()
            });
            return Json(languageOptions, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUsers()
        {
            DictionaryEntities db = new DictionaryEntities();
            List<tblUser> users = db.tblUsers.ToList();
            var userOptions = users.Select(user => new
            {
                value = user.Id,
                text = user.sEmail.ToString()
            });
            return Json(userOptions, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWordTypes()
        {
            DictionaryEntities db = new DictionaryEntities();
            List<tblWord_type> types = db.tblWord_type.ToList();
            var typesOptions = types.Select(type => new
            {
                value = type.Id,
                text = type.sWordtype.ToString()
            });
            return Json(typesOptions, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteWord(int? Id)
        {
            DictionaryEntities db = new DictionaryEntities();
            tblWord word = db.tblWords.Where(row => row.Id == Id).FirstOrDefault();
            if (word != null)
            {
                db.tblWords.Remove(word);
                db.SaveChanges();
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return Json(new { message = "Xoá thành công" });

            }
            else
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Json(new { error = "Không tồn tại từ này" });
            }
        }

        public ActionResult UpdateWord(int id)
        {
            DictionaryEntities db = new DictionaryEntities();
            tblWord word = db.tblWords.Where(row => row.Id == id).FirstOrDefault();

            return View(word);
        }
        [HttpPost]
        public JsonResult UpdateWord(WordDTO request)
        {
            try
            {
                DictionaryEntities db = new DictionaryEntities();
                tblWord word = db.tblWords.Where(row => row.Id == request.Id).FirstOrDefault();
                if (word != null)
                {

                    word.Id_Language = request.Id_Language;
                    word.Id_Language_trans = request.Id_Language_trans;
                    word.Id_wordtype = request.Id_wordtype;
                    word.Id_user = request.Id_user;
                    word.sWord = request.sWord;
                    word.sExample = request.sExample;
                    word.sDefinition = request.sDefinition;

                    db.SaveChanges();
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { response = "Sucess" });
                }
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { response = "Không tìm thấy" });

            }
            catch (DbEntityValidationException ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Console.WriteLine("Validation Errors:");

                foreach (var entityValidationError in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationError.ValidationErrors)
                    {
                        Console.WriteLine($"Entity: {entityValidationError.Entry.Entity.GetType().Name}, Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                    }
                }

                return Json(new { response = "Lỗi ngoại lệ" });
            }

        }
        public ActionResult filterWordType(int wordTypeID)
        {
            DictionaryEntities db = new DictionaryEntities();
            List<tblWord> words = db.tblWords.Where(w => w.Id_wordtype == wordTypeID).ToList();
            return PartialView("_patialWord", words);
        }
    }

}
