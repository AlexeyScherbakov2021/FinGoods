using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.Models
{
    public class Users : IEntity

    {
        [Key]
        [Column("UserId")]
        public int id { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string UserPass { get; set; }

        [StringLength(100)]
        public string UserFullName { get; set; }

        //public DateTime? DTDel { get; set; }
    }
}
