using Dictionary.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dictionary.DTO;

namespace Dictionary.Areas.Admin.Controllers
{
    public class tblWordController : Controller
    {
        // GET: Admin/tblWord
        public ActionResult Index()
        {
            return View();
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
        [HttpDelete]
        public JsonResult DeleteWord(int Id)
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
        [HttpPut]
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
    }
    
}