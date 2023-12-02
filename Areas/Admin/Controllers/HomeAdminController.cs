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
    public class HomeAdminController : Controller
    {
        // GET: Admin/Home
      
        public ActionResult Index()
        { 
            return View();
        }

        public ActionResult SignUpAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUpAdmin(UserDTO model)
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
                return RedirectToAction("LoginAdmin");

            }


        }
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
        public ActionResult LoginAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAdmin(UserDTO model)
        {
            DictionaryEntities db = new DictionaryEntities();
            var existingUser = db.tblUsers.FirstOrDefault(x => x.sEmail == model.sEmail);

            if (existingUser != null)
            {
                if (VerifyPassword(model.sPassword, existingUser.sPasswordHash, existingUser.sSalt))
                {
                    if (existingUser.sRole.Equals("Admin"))
                    {
                        TempData["response"] = "Đăng nhập thành công";
                        //Lưu người dùng vào session
                        Session["LoggedAdmin"] = existingUser.Id;
                        return RedirectToAction("Index", "HomeAdmin");
                    }
                    else
                    {
                        TempData["response"] = "Tài khoản "+ model.sEmail + " không có quyền truy cập vào trang Admin";
                        return View(model);
                    }
                }
                else
                {
                    TempData["response"] = "Sai mật khẩu";
                    return View(model);
                }
            }
            else
            {
                TempData["response"] = "Tài khoản không tồn tại";
                return View(model);
            }
        }

        // Hàm kiểm tra xác thực mật khẩu
        private bool VerifyPassword(string enteredPassword, string passwordHash, string salt)
        {
            using (var sha256 = new SHA256Managed())
            {
                var passwordBytes = System.Text.Encoding.UTF8.GetBytes(enteredPassword);
                byte[] storedHash = Convert.FromBase64String(passwordHash);
                byte[] storedSalt = Convert.FromBase64String(salt);
                var hashBytes = sha256.ComputeHash(storedSalt.Concat(passwordBytes).ToArray());

                return hashBytes.SequenceEqual(storedHash);
            }
        }
    }

}