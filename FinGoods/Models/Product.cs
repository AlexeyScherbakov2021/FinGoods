namespace FinGoods.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Modules = new ObservableCollection<Module>();
        }

        public int id { get; set; }

        public int? idShipment { get; set; }

        public int? idSetter { get; set; }

        public int g_ProductTypeId { get; set; }

        [StringLength(100)]
        public string g_name { get; set; }

        [StringLength(40)]
        public string g_number { get; set; }

        [StringLength(40)]
        public string g_numberBox { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Module> Modules { get; set; }

        public virtual ProductType ProductType { get; set; }

        public virtual SetterOut SetterOut { get; set; }

        public virtual Shipment Shipment { get; set; }
    }
}
