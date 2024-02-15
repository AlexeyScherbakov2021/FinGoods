namespace ExportData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Shipment")]
    public partial class Shipment : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shipment()
        {
            Modules = new HashSet<Modules>();
            Product = new HashSet<Product>();
            SetterOut = new HashSet<SetterOut>();
        }

        public int id { get; set; }

        [StringLength(150)]
        public string c_number { get; set; }

        public string c_objectInstall { get; set; }

        public DateTime? c_dateOut { get; set; }

        [StringLength(150)]
        public string c_customer { get; set; }

        [StringLength(180)]
        public string c_questList { get; set; }

        [StringLength(180)]
        public string c_schet { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Modules> Modules { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Product { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SetterOut> SetterOut { get; set; }
    }
}
