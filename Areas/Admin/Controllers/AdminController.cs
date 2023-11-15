using Dictionary.DTO;
using Dictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Dictionary.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // Hàm tạo salt ngẫu nhiên
        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        // Hàm băm mật khẩu với salt
        private byte[] HashPassword(string password, byte[] salt)
        {
            using (var sha256 = new SHA256Managed())
            {
                var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
                var saltedPassword = salt.Concat(passwordBytes).ToArray();
                return sha256.ComputeHash(saltedPassword);
            }
        }
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(UserDTO model)
        {


            DictionaryEntities db = new DictionaryEntities();
            var existingUser = db.tblUsers.FirstOrDefault(x => x.sEmail == model.sEmail);
            if (existingUser != null)
            {
                //Tài khoản đã tồn tại
                TempData["Error"] = "Tài khoản đã tồn tại";
                return View(model);

            }
            else
            {
                // Tạo salt ngẫu nhiên
                byte[] salt = GenerateSalt();
                //Băm mật khẩu với salt
                byte[] passwordHash = HashPassword(model.sPassword, salt);
                var admin = new tblUser
                {
                    sEmail = model.sEmail,
                    sPasswordHash = Convert.ToBase64String(passwordHash),
                    sSalt = Convert.ToBase64String(salt),
                    sRole = "Admin"

                };
                db.tblUsers.Add(admin);
                db.SaveChanges();
                TempData["Success"] = "Đăng ký tài khoản thành công";
                return View(model);

            }


        }
    }
}