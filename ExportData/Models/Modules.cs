namespace ExportData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Modules : IEntity
    {
        public int id { get; set; }

        public int? idShipment { get; set; }

        public int? idProduct { get; set; }

        public int m_modTypeId { get; set; }

        [StringLength(100)]
        public string m_name { get; set; }

        [StringLength(40)]
        public string m_number { get; set; }

        [StringLength(40)]
        public string m_numberFW { get; set; }

        public DateTime? m_dateEnd { get; set; }

        public DateTime? m_dateCreate { get; set; }

        public int m_generatedNumber { get; set; }
        public bool m_zip { get; set; }

        public virtual ModuleType ModuleType { get; set; }

        public virtual Product Product { get; set; }

        public virtual Shipment Shipment { get; set; }
    }
}
