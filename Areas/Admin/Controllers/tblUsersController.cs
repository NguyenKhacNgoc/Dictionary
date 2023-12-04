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
    public class tblUsersController : Controller
    {
        private DictionaryEntities db = new DictionaryEntities();

        // GET: Admin/tblUsers
        public ActionResult Index()
        {
            return View(db.tblUsers.ToList());
        }

        // GET: Admin/tblUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = db.tblUsers.Find(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
        }

        // GET: Admin/tblUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/tblUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,sEmail,sPasswordHash,sSalt,sRole")] tblUser tblUser)
        {
            if (ModelState.IsValid)
            {
                db.tblUsers.Add(tblUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblUser);
        }

        // GET: Admin/tblUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = db.tblUsers.Find(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }

            // Chuyển đổi từ tblUser sang EditUserRoleViewModel
            var viewModel = new EditUserRoleViewModel
            {
                Id = tblUser.Id,
                sRole = tblUser.sRole
            };

            return View(viewModel);
        }

        // POST: Admin/tblUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserRoleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Lấy lại tblUser từ Database
                var tblUser = db.tblUsers.Find(viewModel.Id);

                // Cập nhật chỉ trường sRole
                tblUser.sRole = viewModel.sRole;

                db.Entry(tblUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Admin/tblUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = db.tblUsers.Find(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
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

        // POST: Admin/tblUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblUser tblUser = db.tblUsers.Find(id);
            db.tblUsers.Remove(tblUser);
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
