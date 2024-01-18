namespace FinGoods.Models
{
    using FinGoods.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Module : Observable, IEntity
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

        private ModuleType _ModuleType;
        public virtual ModuleType ModuleType { get => _ModuleType; set { Set(ref _ModuleType, value); } }

        public virtual Product Product { get; set; }

        public virtual Shipment Shipment { get; set; }

    }
}
