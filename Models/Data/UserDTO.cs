using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MySocialLife.Models.Data
{
    [Table("tblKullanicilar")]
    public class UserDTO
    {
        [Key]
        public int Id{ get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string EmailAdresi { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
    }
}