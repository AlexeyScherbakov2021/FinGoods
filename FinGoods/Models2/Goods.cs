namespace FinGoods.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Goods : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Goods()
        {
            Modules = new ObservableCollection<Module>();
        }

        public int id { get; set; }

        public int g_cardOrderId { get; set; }

        public int g_type { get; set; }

        [StringLength(100)]
        public string g_name { get; set; }

        [StringLength(40)]
        public string g_number { get; set; }

        [StringLength(40)]
        public string g_numberBox { get; set; }


        public virtual CardOrder CardOrder { get; set; }

        public virtual GoodsType GoodsType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Module> Modules { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }

    }
}
