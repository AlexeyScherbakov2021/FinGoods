namespace FinGoods.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Windows.Controls;

    public partial class CardOrder : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CardOrder()
        {
            Goods = new ObservableCollection<Goods>();
        }

        public int id { get; set; }

        [StringLength(150)]
        public string c_number { get; set; }

        [StringLength(150)]
        public string c_customer { get; set; }

        [StringLength(180)]
        public string c_questList { get; set; }

        public DateTime? c_dateOut { get; set; }

        public string c_objectInstall { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Goods> Goods { get; set; }


        [NotMapped]
        public bool IsSelected { get; set; }

    }
}
