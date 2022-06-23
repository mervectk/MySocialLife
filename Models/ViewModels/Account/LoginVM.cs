using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MySocialLife.Views.Account
{
    public class LoginVM
    {
        [Required]
        public string KullaniciAdi { get; set; }
        [Required]
        public string Sifre{ get; set; }
    }
}
