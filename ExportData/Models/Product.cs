namespace ExportData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Modules = new HashSet<Modules>();
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

        public DateTime? g_dateRegister { get; set; }

        public int g_generatedNumber { get; set; }

        [StringLength(20)]
        public string g_redaction1 { get; set; }

        [StringLength(20)]
        public string g_redaction2 { get; set; }

        [StringLength(20)]
        public string g_redactionPS { get; set; }

        public string g_questList { get; set; }

        public bool? g_avr { get; set; }

        public bool? g_akb { get; set; }

        public bool? g_cooler { get; set; }

        public bool? g_skm { get; set; }

        [StringLength(40)]
        public string g_numberBI { get; set; }

        [StringLength(40)]
        public string g_numberUSIKP { get; set; }

        [StringLength(40)]
        public string g_shunt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Modules> Modules { get; set; }

        public virtual ProductType ProductType { get; set; }

        public virtual SetterOut SetterOut { get; set; }

        public virtual Shipment Shipment { get; set; }
    }
}
