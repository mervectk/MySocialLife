using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MySocialLife.Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MySocialLife
{
    [HubName("echo")]
    public class EchoHub : Hub
    {

        public void Hello(string message)
        {
            //Clients.All.hello();
            Trace.WriteLine(message);
            //Set clients
            var clients = Clients.All;
            //Call js funtion
            clients.Test("Bu bir testtir!!");
        }
        public void Notify(string friend)
        {
            // Init db
            Db db = new Db();

            // Get friend's id
            UserDTO userDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(friend)).FirstOrDefault();
            int friendId = userDTO.Id;

            // Get fr count
            var frCount = db.Arkadaslar.Count(x => x.Kullanici2 == friendId && x.Aktif == false);


            //Set clients
            var clients = Clients.Others;
            //Call js funtion
            clients.frnotify(friend, frCount);
        }
        public void GetFrcount()
        {
            // Init db
            Db db = new Db();

            // Get user id
            UserDTO userDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(Context.User.Identity.Name)).FirstOrDefault();
            int userId = userDTO.Id;

            // Get fr count
            var friendReqCount = db.Arkadaslar.Count(x => x.Kullanici2 == userId && x.Aktif == false);

            // Set clients
            var clients = Clients.Caller;

            // Call js function
            clients.frcount(Context.User.Identity.Name, friendReqCount);
        }
        public void GetFcount(int friendId)
        {
            // Init db
            Db db = new Db();

            // Get user id
            UserDTO userDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(Context.User.Identity.Name)).FirstOrDefault();
            int userId = userDTO.Id;

            // Get friend count for user
            var friendCount1 = db.Arkadaslar.Count(x => x.Kullanici2 == userId && x.Aktif == true
                                                        || x.Kullanici1 == userId && x.Aktif == true);

            // Get user2 username
            UserDTO userDTO2 = db.Kullanicilar.Where(x => x.Id == friendId).FirstOrDefault();
            string username = userDTO2.KullaniciAdi;

            // Get friend count for user2
            var friendCount2 = db.Arkadaslar.Count(x => x.Kullanici2 == friendId && x.Aktif == true
                                                  || x.Kullanici1 == friendId && x.Aktif == true);

            //UPDATE CHAT
            UpdateChat();

            // Set clients
            var clients = Clients.All;

            // Call js function
            clients.fcount(Context.User.Identity.Name, username, friendCount1, friendCount2);


        }
        public void NotifyOfMessage(string friend)
        {
            // Init db
            Db db = new Db();

            // Get friend id
            UserDTO userDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(friend)).FirstOrDefault();
            int friendId = userDTO.Id;

            // Get message count
            var messageCount = db.Mesajlar.Count(x => x.To == friendId && x.Okundu == false);

            // Set clients
            var clients = Clients.Others;

            // Call js function
            clients.msgcount(friend, messageCount);
        }
        public void NotifyOfMessageOwner()
        {
            // Init db
            Db db = new Db();

            // Get user id
            UserDTO userDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(Context.User.Identity.Name)).FirstOrDefault();
            int userId = userDTO.Id;

            // Get message count
            var messageCount = db.Mesajlar.Count(x => x.To == userId && x.Okundu == false);

            // Set clients
            var clients = Clients.Caller;

            // Call js function
            clients.msgcount(Context.User.Identity.Name, messageCount);
        }
        public override Task OnConnected()//SİSTEMDE MEVCUT ONCONNECTED()EZME YAPIYORUM OVERRİDE İLE
        {
            // Log user conn
            Trace.WriteLine("Here I am " + Context.ConnectionId);


            //// Init db
            Db db = new Db();

            //// Get user id
            UserDTO userDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(Context.User.Identity.Name)).FirstOrDefault();
            int userId = userDTO.Id;

            //// Get conn id
            string connId = Context.ConnectionId;

            //// Add onlineDTO
            if (!db.Online.Any(x => x.Id == userId))
            {
                OnlineDTO online = new OnlineDTO();

                online.Id = userId;
                online.ConnId = connId;

                db.Online.Add(online);

                db.SaveChanges();
            }

            /// Get all online ids
            List<int> onlineIds = db.Online.ToArray().Select(x => x.Id).ToList();
            // Get friend ids
            List<int> friendIds1 = db.Arkadaslar.Where(x => x.Kullanici1 == userId && x.Aktif == true).ToArray().Select
                (x => x.Kullanici2).ToList();

            List<int> friendIds2 = db.Arkadaslar.Where(x => x.Kullanici2 == userId && x.Aktif == true).ToArray().Select
                (x => x.Kullanici1).ToList();

            List<int> allFriendsIds = friendIds1.Concat(friendIds2).ToList();

            // Get final set of ids
            List<int> resultList = onlineIds.Where((i) => allFriendsIds.Contains(i)).ToList();
            // Create a dict of friend ids and usernames

            Dictionary<int, string> dictFriends = new Dictionary<int, string>();

            foreach (var id in resultList)
            {
                var users = db.Kullanicilar.Find(id);
                string friend = users.KullaniciAdi;

                if (!dictFriends.ContainsKey(id))
                {
                    dictFriends.Add(id, friend);
                }
            }

            var transformed = from key in dictFriends.Keys
                              select new { id = key, friend = dictFriends[key] };
            string json = JsonConvert.SerializeObject(transformed);
            // Set clients
            var clients = Clients.Caller;
            // Call js function
            clients.getonlinefriends(Context.User.Identity.Name, json);

            UpdateChat();
            // Return
            return base.OnConnected();







        }
        public override Task OnDisconnected(bool stopCalled)
        {
            // Log
            Trace.WriteLine("gone - " + Context.ConnectionId + " " + Context.User.Identity.Name);

            // Init db
            Db db = new Db();

            // Get user id
            UserDTO userDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(Context.User.Identity.Name)).FirstOrDefault();
            int userId = userDTO.Id;

            // Remove from db
            if (db.Online.Any(x => x.Id == userId))
            {
                OnlineDTO online = db.Online.Find(userId);
                db.Online.Remove(online);
                db.SaveChanges();
            }

            // Update chat-fonksiyon sonrası aşağıda metot oluşturdum
            UpdateChat();

            return base.OnDisconnected(stopCalled);

        }
        public void UpdateChat()
        {
            // Init db
            Db db = new Db();

            // Get all online ids
            List<int> onlineIds = db.Online.ToArray().Select(x => x.Id).ToList();

            // Loop thru onlineids and get friends
            foreach (var userId in onlineIds)
            {
                // Get username
                UserDTO user = db.Kullanicilar.Find(userId);
                string username = user.KullaniciAdi;
                // Get all friend ids

                List<int> friendIds1 = db.Arkadaslar.Where(x => x.Kullanici1 == userId && x.Aktif == true).ToArray().
                    Select(x => x.Kullanici2).ToList();

                List<int> friendIds2 = db.Arkadaslar.Where(x => x.Kullanici2 == userId && x.Aktif == true).ToArray().
                    Select(x => x.Kullanici1).ToList();

                List<int> allFriendsIds = friendIds1.Concat(friendIds2).ToList();
                // Get final set of ids
                List<int> resultList = onlineIds.Where((i) => allFriendsIds.Contains(i)).ToList();

                // Create a dict of friend ids and usernames

                Dictionary<int, string> dictFriends = new Dictionary<int, string>();

                foreach (var id in resultList)
                {
                    var users = db.Kullanicilar.Find(id);
                    string friend = users.KullaniciAdi;

                    if (!dictFriends.ContainsKey(id))
                    {
                        dictFriends.Add(id, friend);
                    }
                }

                var transformed = from key in dictFriends.Keys
                                  select new { id = key, friend = dictFriends[key] };

                string json = JsonConvert.SerializeObject(transformed);

                // Set clients
                var clients = Clients.All;

                // Call js function
                clients.updatechat(username, json);
            }


        }
        public void SendChat(int friendId, string friendUsername, string message)
        {
            // Init db
            Db db = new Db();

            // Get user id
            UserDTO userDTO = db.Kullanicilar.Where(x => x.KullaniciAdi.Equals(Context.User.Identity.Name)).FirstOrDefault();
            int userId = userDTO.Id;

            // Set clients
            var clients = Clients.All;

            // Call js function
            clients.sendchat(userId, Context.User.Identity.Name, friendId, friendUsername, message);
        }



    }
}
