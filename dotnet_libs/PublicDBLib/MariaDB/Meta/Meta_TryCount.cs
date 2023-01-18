using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PublicDBLib.MariaDB.Meta
{
    [Table("Meta_TryCount")]
    [Index(nameof(Name))]
    public partial class Meta_TryCount
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int Count { get; set; }
        public DateTime TryDate { get; set; }
    }
}
