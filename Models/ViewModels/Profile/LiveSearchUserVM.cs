using MySocialLife.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialLife.Models.ViewModels.Profile
{
    public class LiveSearchUserVM
    {

        public LiveSearchUserVM()
        {


        }
        public LiveSearchUserVM(UserDTO row)
        {
            UserId = row.Id;
            Adi = row.Adi;
            Soyadi = row.Soyadi;
            KullaniciAdi = row.KullaniciAdi;

        }
        public int UserId{ get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string KullaniciAdi { get; set; }
    }
}