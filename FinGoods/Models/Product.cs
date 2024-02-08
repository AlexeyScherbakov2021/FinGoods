namespace FinGoods.Models
{
    using FinGoods.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product : Observable, IEntity
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

        private string _g_name;
        [StringLength(100)]
        public string g_name { get => _g_name; set { Set(ref _g_name, value); } }

        private string _g_number;
        [StringLength(40)]
        public string g_number { get => _g_number; set { Set(ref _g_number, value); } }

        private string _g_numberBox;
        [StringLength(40)]
        public string g_numberBox { get => _g_numberBox; set { Set(ref _g_numberBox, value); } }

        [StringLength(40)]
        public string g_numberBI { get; set;}

        [StringLength(40)]
        public string g_numberUSIKP { get; set;}

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


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Module> Modules { get; set; }

        private ProductType _ProductType;
        public virtual ProductType ProductType { get => _ProductType; set { Set(ref _ProductType, value); } }

        public virtual SetterOut SetterOut { get; set; }

        public virtual Shipment Shipment { get; set; }
    }
}
