namespace ExportData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SetterOut")]
    public partial class SetterOut : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SetterOut()
        {
            Product = new HashSet<Product>();
        }

        public int id { get; set; }

        public int? idShipment { get; set; }

        [StringLength(150)]
        public string s_name { get; set; }

        [StringLength(150)]
        public string s_orderNum { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Product { get; set; }

        public virtual Shipment Shipment { get; set; }
    }
}
