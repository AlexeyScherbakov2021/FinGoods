using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum KindNumber : int { Module = 1, Production };

namespace FinGoods.Models
{
    [Table("SerialNumber")]
    public partial class SerialNumber : IEntity
    {
        public int id { get; set; }
        
        public KindNumber kind_number { get; set; }
        public int year_number { get; set; }
        public int gen_number { get; set; }
    }
}
