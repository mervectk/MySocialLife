using MySocialLife.Models.Data;
using MySocialLife.Models.ViewModels.Account;
using MySocialLife.Models.ViewModels.Profile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MySocialLife.Controllers
{
    public class AccountController : Controller
    {
        // GET: /
        public ActionResult Index()
            
        {
            //kullanıcının oturum açmadığını onayla
            string kullaniciadi = User.Identity.Name;
            if (!string.IsNullOrEmpty(kullaniciadi))
                return Redirect("~/" + kullaniciadi);//home page yönlendirme

           //return View
            return View();
        }
        // Post: Account/CreateAccount
        [HttpPost]
        public ActionResult CreateAccount(UserVM model,HttpPostedFileBase file)


        {
            //Init db
            Db db = new Db();
            //Check model state
            if(!ModelState.IsValid)
            {
                return View("Index", model);
            }
            //Make sure username is unique
            if (db.Kullanicilar.Any(x => x.KullaniciAdi.Equals(model.KullaniciAdi))){
                ModelState.AddModelError("", "KullaniciAdi" + model.KullaniciAdi + "sistemde mevcuttur.");
                model.KullaniciAdi = "";
                return View("Index", model);

            }

            //Create User DTO
            UserDTO userDTO = new UserDTO()
            {
                Adi = model.Adi,
                Soyadi = model.Soyadi,
                EmailAdresi=model.EmailAdresi,
                KullaniciAdi = model.KullaniciAdi,
                Sifre=model.Sifre

            };
            //Add to DTO
            db.Kullanicilar.Add(userDTO);
            //Save
            db.SaveChanges();
            //Get inserted ID
            int userId = userDTO.Id;
            //login user
            FormsAuthentication.SetAuthCookie(model.KullaniciAdi, false);
            //set uploads directory
            var uploadsDir = new DirectoryInfo(string.Format("{0}Uploads", Server.MapPath(@"\")));
            //check if a file was uploaded
            if (file != null && file.ContentLength > 0)
                {
                //get extension
                string ext = file.ContentType.ToLower();
            //verify extension
            if(     ext !="image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png" )
                {
                    ModelState.AddModelError("", "Dosya yüklenmedi-yanlış fotograf dosyası uzantısı!");
                    
                    return View("Index", model);

                }


                //set image name
                string ImageName = userId + ".jpg";
                //set image path
                var path = string.Format("{0}\\{1}", uploadsDir, ImageName);
                //save image
                file.SaveAs(path);
            }

            // Add to wall

            WallDTO wall = new WallDTO();

            wall.Id = userId;
            wall.Mesaj = "";
            wall.DuzenlenmeTarihi = DateTime.Now;

            db.Wall.Add(wall);
            db.SaveChanges();

           

            //redirect
            return Redirect("~/" + model.KullaniciAdi);


            
        }

      //GET:/{kullaniciadi}
      [Authorize]
        public ActionResult KullaniciAdi(string kullaniciadi= "")
        {
            //INIT DB
            Db db = new Db();
            //Check if user exists
            if(! db.Kullanicilar.Any(x=>x.KullaniciAdi.Equals(kullaniciadi)))
            {
                return Redirect("~/");
            }
            //viewbag username-controllerdaki bilgiler view taşırım!!
            ViewBag.KullaniciAdi = kullaniciadi;
           
            // Get logged in user's username
            string kullanici = User.Identity.Name;

            // Viewbag user's full name
            UserDTO userDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(kullaniciadi)).FirstOrDefault();
            ViewBag.FullName = userDTO.Adi + " " + userDTO.Soyadi;

            //get user's id
            int userId = userDTO.Id;

            //viewbag user id
            ViewBag.UserId = userId;


            // Get viewing full name
            UserDTO userDTO2 = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(kullaniciadi)).FirstOrDefault();
            ViewBag.ViewingFullName = userDTO2.Adi + " " + userDTO2.Soyadi;
            //Get username's image
            ViewBag.KullaniciAdiImage = userDTO2.Id + ".jpg";
            //viewbag user type
            string userType = "guest";
            if (kullaniciadi.Equals(kullanici))
                userType = "owner"
;
            ViewBag.UserType = userType;//önce viewbag yaz

     
            // Check if they are friends

            if (userType == "guest")
            {
                UserDTO u1 = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(kullanici)).FirstOrDefault();
                int id1 = u1.Id;

                UserDTO u2 = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(kullaniciadi)).FirstOrDefault();
                int id2 = u2.Id;

                FriendDTO f1 = db.Arkadaslar.Where(x => x.Kullanici1 == id1 && x.Kullanici2 == id2).FirstOrDefault();
                FriendDTO f2 = db.Arkadaslar.Where(x => x.Kullanici2 == id1 && x.Kullanici1 == id2).FirstOrDefault();

                if (f1 == null && f2 == null)
                {
                    ViewBag.NotFriends = "True";
                }

                if (f1 != null)
                {
                    if (!f1.Aktif)
                    {
                        ViewBag.NotFriends = "Pending";
                    }
                }

                if (f2 != null)
                {
                    if (!f2.Aktif)
                    {
                        ViewBag.NotFriends = "Pending";
                    }
                }
            }
            //viewbag friend request count
            var friendCount = db.Arkadaslar.Count(x => x.Kullanici2 == userId && x.Aktif == false);
            if (friendCount > 0)
            {
                ViewBag.FRCount = friendCount;
            }
            //viewbag friend count
            UserDTO uDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(kullaniciadi)).FirstOrDefault();
            int usernameId = uDTO.Id;

            var friendCount2 = db.Arkadaslar.Count(x => x.Kullanici2 ==usernameId && x.Aktif == true 
                                                      || x.Kullanici1 == usernameId && x.Aktif == true);

            ViewBag.FCount = friendCount2;

            //VIEWBAG MESSAGE COUNT
            var messageCount = db.Mesajlar.Count(x => x.To == userId && x.Okundu == false);

            ViewBag.MsgCount = messageCount;

            // Viewbag user wall
            WallDTO wall = new WallDTO();
            ViewBag.WallMessage = db.Wall.Where(x => x.Id == userId).Select(x => x.Mesaj).FirstOrDefault();

            //VİEWBAG FRIENDS WALL
            List<int> friendIds1 = db.Arkadaslar.Where(x => x.Kullanici1 == userId && x.Aktif== true).ToArray().Select(x => x.Kullanici2).ToList();

            List<int> friendIds2 = db.Arkadaslar.Where(x => x.Kullanici2 == userId && x.Aktif == true).ToArray().Select(x => x.Kullanici1).ToList();

            List<int> allFriendsIds = friendIds1.Concat(friendIds2).ToList();

            List<WallVM> walls = db.Wall.Where(x => allFriendsIds.Contains(x.Id)).ToArray().OrderByDescending(x => x.DuzenlenmeTarihi).Select(x => new WallVM(x)).ToList();

              
            ViewBag.Walls = walls;

            return View();
        }
        //GET:account/Logout
        [Authorize]
        public ActionResult Logout()
        {
            //sign out
            FormsAuthentication.SignOut();

            //redirect
            return Redirect("~/");
        }
         public ActionResult LoginPartial()
        {
            return PartialView();
        }

        //POST:account/Login
         [HttpPost]
        public string Login(string kullaniciadi,string sifre)
        {
            //Init DB
            Db db = new Db();
            //check if user exists
            if(db.Kullanicilar.Any(x=>x.KullaniciAdi.Equals(kullaniciadi)&& x.Sifre.Equals(sifre)))
            {
                //log in
                FormsAuthentication.SetAuthCookie(kullaniciadi, false);
                return "ok";
            }
            else
            {
                return "problem";
            }
        }


    }
}