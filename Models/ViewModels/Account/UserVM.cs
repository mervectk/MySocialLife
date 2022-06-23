using MySocialLife.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MySocialLife.Models.ViewModels.Account
{
    public class UserVM
    {
        public UserVM()
        {


        }
        public UserVM(UserDTO row)
        {
            Id = row.Id; //satır
            Adi = row.Adi;
            Soyadi = row.Soyadi;
            EmailAdresi = row.EmailAdresi;
            KullaniciAdi = row.KullaniciAdi;
            Sifre = row.Sifre;
        }
        public int Id { get; set; }
        [Required]//boş geçilmemesi için tanımladık
        public string Adi { get; set; }
        [Required]
        public string Soyadi { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]//Email adresini yazarken formata uyum için ekledim
        public string EmailAdresi { get; set; }
        [Required]
        public string KullaniciAdi { get; set; }
        [Required]
        public string Sifre { get; set; }
    }
}
