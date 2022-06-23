using MySocialLife.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialLife.Models.ViewModels.Profile
{
    public class FriendRequestVM
    {

        public FriendRequestVM()
        {

        }
        public FriendRequestVM(FriendDTO row)
        {
            Kullanici1 = row.Kullanici1;
            Kullanici2 = row.Kullanici2;
            Aktif = row.Aktif;
        }
        public int Kullanici1 { get; set; }
        public int Kullanici2 { get; set; }
        public bool Aktif { get; set; }
    }
}