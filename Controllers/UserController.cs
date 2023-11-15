using Dictionary.DTO;
using Dictionary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

using System.Web;
using System.Web.Mvc;

namespace Dictionary.Controllers
{
    public class UserController : Controller
    {
        
        // GET: User
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
                var user = new tblUser
                {
                    sEmail = model.sEmail,
                    sPasswordHash = Convert.ToBase64String(passwordHash),
                    sSalt = Convert.ToBase64String(salt),
                    sRole = "User"

                };
                db.tblUsers.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");

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
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserDTO model) 
        {
            DictionaryEntities db = new DictionaryEntities();
            var existingUser = db.tblUsers.FirstOrDefault(x => x.sEmail == model.sEmail);
            if (existingUser != null && VerifyPassword(model.sPassword, existingUser.sPasswordHash, existingUser.sSalt))
            {
                TempData["response"] = "Đăng nhập thành công";
                //Lưu người dùng vào cookies
                UserResponseDTO userResponse = new UserResponseDTO();
                userResponse.sEmail = existingUser.sEmail;
                userResponse.sRole = existingUser.sRole;
                HttpCookie cookie = new HttpCookie("LoggedUser");
                cookie.Value = JsonConvert.SerializeObject(userResponse);
                //Cookie có thời hạn 7 ngày sau khi đăng nhập
                cookie.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Home");
                

            }
            else if (existingUser != null && !VerifyPassword(model.sPassword, existingUser.sPasswordHash, existingUser.sSalt) && existingUser.sRole.Equals("User"))
            {
                TempData["response"] = "Sai mật khẩu";
                return View(model);


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