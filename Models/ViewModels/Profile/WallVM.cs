using MySocialLife.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialLife.Models.ViewModels.Profile
{
    public class WallVM
    {
        public WallVM()
        {
        }

        public WallVM(WallDTO row)
        {
            Id = row.Id;
            Mesaj = row.Mesaj;
            DuzenlenmeTarihi = row.DuzenlenmeTarihi;
        }

        public int Id { get; set; }
        public string Mesaj { get; set; }
        public DateTime DuzenlenmeTarihi { get; set; }
    }
}