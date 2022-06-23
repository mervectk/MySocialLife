using MySocialLife.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialLife.Models.ViewModels.Profile
{
    public class MessageVM
    {

        public MessageVM()
        {
        }

        public MessageVM(MessageDTO row)
        {
            Id = row.Id;
            From = row.From;
            To = row.To;
            Mesaj = row.Mesaj;
            GonderimTarihi = row.GonderimTarihi;
            Okundu = row.Okundu;
            FromId = row.FromUsers.Id;
            FromUsername = row.FromUsers.KullaniciAdi;
            FromFirstName = row.FromUsers.Adi;
            FromLastName = row.FromUsers.Soyadi;
        }

        public int Id { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string Mesaj { get; set; }
        public DateTime GonderimTarihi { get; set; }
        public bool Okundu { get; set; }

        public int FromId { get; set; }
        public string FromUsername { get; set; }
        public string FromFirstName { get; set; }
        public string FromLastName { get; set; }
    }
}