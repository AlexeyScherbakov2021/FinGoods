namespace FinGoods.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Module : IEntity
    {
        public int id { get; set; }

        public int m_goodsId { get; set; }

        public int m_modTypeId { get; set; }

        [StringLength(100)]
        public string m_name { get; set; }

        [StringLength(40)]
        public string m_number { get; set; }

        [StringLength(40)]
        public string m_numberFW { get; set; }

        public DateTime? m_dateEnd { get; set; }

        public virtual Goods Goods { get; set; }

        public virtual ModuleType ModuleType { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }

    }
}
