using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MySocialLife.Models.Data
{
    [Table("tblArkadaslar")]
    public class FriendDTO
    {
        [Key]
        public int Id { get; set; }
        public int Kullanici1{ get; set; }
        public int Kullanici2{ get; set; }
        public bool Aktif{ get; set; }

        [ForeignKey("Kullanici1")]
        public virtual UserDTO Kullanicilar1 { get; set; }

        [ForeignKey("Kullanici2")]
        public virtual UserDTO Kullanicilar2 { get; set; }

    }
}