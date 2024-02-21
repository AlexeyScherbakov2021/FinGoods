namespace FinGoods.Models
{
    using FinGoods.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.SqlTypes;

    public partial class Module : Observable, IEntity
    {
        public int id { get; set; }

        public int? idShipment { get; set; }

        public int? idProduct { get; set; }

        public int m_modTypeId { get; set; }

        private string _m_name;
        [StringLength(100)]
        public string m_name { get => _m_name; set { Set(ref _m_name, value); } }

        private string _m_number;
        [StringLength(40)]
        public string m_number { get => _m_number; set { Set(ref _m_number, value); } }

        private string _m_numberFW;
        [StringLength(40)]
        public string m_numberFW { get => _m_numberFW; set { Set(ref _m_numberFW, value); } }

        public DateTime? m_dateEnd { get; set; }
        public DateTime? m_dateCreate { get; set; }

        public int m_generatedNumber { get; set; }

        public bool m_zip { get; set; }

        private ModuleType _ModuleType;
        public virtual ModuleType ModuleType { get => _ModuleType; set { Set(ref _ModuleType, value); } }



        public virtual Product Product { get; set; }

        public virtual Shipment Shipment { get; set; }

    }
}
