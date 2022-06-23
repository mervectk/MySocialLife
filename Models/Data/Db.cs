using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MySocialLife.Models.Data
{
    public class Db:DbContext
    {
        public DbSet<UserDTO> Kullanicilar { get; set; }
        public DbSet<FriendDTO> Arkadaslar { get; set; }
        public DbSet<MessageDTO> Mesajlar { get; set; }
        public DbSet<WallDTO> Wall { get; set; }
        public DbSet<OnlineDTO> Online { get; set; }

    }
}