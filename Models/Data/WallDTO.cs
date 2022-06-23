using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MySocialLife.Models.Data
{
    [Table("tblWall")]
    public class WallDTO
    {
        [Key]
        public int Id { get; set; }
        public string Mesaj { get; set; }
        public DateTime DuzenlenmeTarihi { get; set; }

        [ForeignKey("Id")]
        public virtual UserDTO Users { get; set; }
    }

}