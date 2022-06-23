using MySocialLife.Models.Data;
using MySocialLife.Models.ViewModels.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MySocialLife.Controllers
{
    public class ProfileController : Controller
    {
        // GET: /
        public ActionResult Index()
        {
            return View();
        }
        //POST:Profile/LiveSearch
        [HttpPost]
        public JsonResult LiveSearch(string searchVal)
        {
            //INIT DB

            Db db = new Db();
            //CREATE LIST
            List<LiveSearchUserVM> kullaniciadis = db.Kullanicilar.Where
          (x => x.KullaniciAdi.Contains(searchVal) && x.KullaniciAdi != User.Identity.Name).ToArray().Select(x => new LiveSearchUserVM(x)).ToList();


            //RETURN JSON
            return Json(kullaniciadis);

        }
        //POST:Profile/AddFriend
        [HttpPost]
        public ActionResult AddFriend(string friend)
        {
            // Init db
            Db db = new Db();

            // Get user's id
            UserDTO userDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(User.Identity.Name)).FirstOrDefault();
            int userId = userDTO.Id;

            // Get friend to be id
            UserDTO userDTO2 = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(friend)).FirstOrDefault();
            int friendId = userDTO2.Id;

            // Add DTO

            FriendDTO friendDTO = new FriendDTO();

            friendDTO.Kullanici1 = userId;
            friendDTO.Kullanici2 = friendId;
            friendDTO.Aktif = false;

            db.Arkadaslar.Add(friendDTO);

            db.SaveChanges();
            return View();
        }

        // POST: Profile/DisplayFriendRequests
        [HttpPost]
        public JsonResult DisplayFriendRequests()
        {
            // Init db
            Db db = new Db();

            // Get user id
            UserDTO userDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(User.Identity.Name)).FirstOrDefault();
            int userId = userDTO.Id;

            // Create list of fr
            List<FriendRequestVM> list = db.Arkadaslar.Where(x => x.Kullanici2 == userId && x.Aktif == false).ToArray().Select(x => new FriendRequestVM(x)).ToList();

            // Init list of users

            List<UserDTO> users = new List<UserDTO>();

            foreach (var item in list)
            {
                var user = db.Kullanicilar.Where(x => x.Id == item.Kullanici1).FirstOrDefault();
                users.Add(user);
            }

            // Return json
            return Json(users);
        }

        // POST: Profile/AcceptFriendRequest
        [HttpPost]
        public void AcceptFriendRequest(int friendId)
        {
            // Init db
            Db db = new Db();

            // Get user id
            UserDTO userDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(User.Identity.Name)).FirstOrDefault();
            int userId = userDTO.Id;

            // Make friends

            FriendDTO friendDTO = db.Arkadaslar.Where(x => x.Kullanici1 == friendId && x.Kullanici2 == userId).FirstOrDefault();

            friendDTO.Aktif = true;

            db.SaveChanges();
        }

        // POST: Profile/DeclineFriendRequest
        [HttpPost]
        public void DeclineFriendRequest(int friendId)
        {
            // Init db
            Db db = new Db();

            // Get user id
            UserDTO userDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(User.Identity.Name)).FirstOrDefault();
            int userId = userDTO.Id;

            // Delete friend request

            FriendDTO friendDTO = db.Arkadaslar.Where(x => x.Kullanici1 == friendId && x.Kullanici2== userId).FirstOrDefault();

            db.Arkadaslar.Remove(friendDTO);

            db.SaveChanges();
        }
        // POST: Profile/SendMessage
        [HttpPost]
        public void SendMessage(string friend, string message)
        {
            // Init db
            Db db = new Db();

            // Get user id
            UserDTO userDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(User.Identity.Name)).FirstOrDefault();
            int userId = userDTO.Id;

            // Get friend id

            UserDTO userDTO2=db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(friend)).FirstOrDefault();
            int userId2 = userDTO2.Id;


            // Save message

            MessageDTO dto = new MessageDTO();

            dto.From = userId;
            dto.To = userId2;
            dto.Mesaj = message;
            dto.GonderimTarihi = DateTime.Now;
            dto.Okundu = false;

            db.Mesajlar.Add(dto);
            db.SaveChanges();
        }
        // POST: Profile/DisplayUnreadMessages
        [HttpPost]
        public JsonResult DisplayUnreadMessages()
        {
            // Init db
            Db db = new Db();

            // Get user id
            UserDTO userDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(User.Identity.Name)).FirstOrDefault();
            int userId = userDTO.Id;

            // Create a list of unread messages
            List<MessageVM> list = db.Mesajlar.Where(x => x.To == userId && x.Okundu == false).ToArray().Select(x => new MessageVM(x)).ToList();

            // Make unread read
            db.Mesajlar.Where(x => x.To == userId && x.Okundu == false).ToList().ForEach(x => x.Okundu = true);
            db.SaveChanges();

            // Return json
            return Json(list);
        }

        // POST: Profile/UpdateWallMessage
        [HttpPost]
        public void UpdateWallMessage(int id, string message)
        {
            // Init db
            Db db = new Db();

            // Update wall
            WallDTO wall = db.Wall.Find(id);

            wall.Mesaj = message;
            wall.DuzenlenmeTarihi = DateTime.Now;

            db.SaveChanges();
        }






    }
}
    
