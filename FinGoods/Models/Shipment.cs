namespace FinGoods.Models
{
    using FinGoods.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Shipment")]
    public partial class Shipment : Observable, IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shipment()
        {
            Modules = new ObservableCollection<Module>();
            Products = new ObservableCollection<Product>();
            SetterOuts = new ObservableCollection<SetterOut>();
        }

        public int id { get; set; }

        private string _c_number;
        [StringLength(150)]
        public string c_number { get => _c_number; set { Set(ref _c_number, value); } }

        public string c_objectInstall { get; set; }

        public DateTime? c_dateOut { get; set; }

        [StringLength(150)]
        public string c_customer { get => _c_customer; set { Set(ref _c_customer, value); } }
        private string _c_customer;

        [StringLength(180)]
        public string c_questList { get; set; }

        [StringLength(180)]
        public string c_schet { get => _c_schet; set { Set(ref _c_schet, value); } }
        private string _c_schet;

        [StringLength(80)]
        public string c_cardOrder { get => _c_cardOrder; set { Set(ref _c_cardOrder, value); } }
        private string _c_cardOrder;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Module> Modules { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Product> Products { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<SetterOut> SetterOuts { get; set; }
    }
}
