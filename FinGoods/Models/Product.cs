namespace FinGoods.Models
{
    using FinGoods.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Windows.Media.TextFormatting;

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
        public string g_numberBI { get => _g_numberBI; set { Set(ref _g_numberBI, value); } }
        private string _g_numberBI;

        [StringLength(40)]
        public string g_numberUSIKP { get => _g_numberUSIKP; set { Set(ref _g_numberUSIKP, value); } }
        private string _g_numberUSIKP;

        [StringLength(40)]
        public string g_shunt { get; set; }


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
        public bool g_zip { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Module> Modules { get; set; }

        private ProductType _ProductType;
        public virtual ProductType ProductType { get => _ProductType; set { Set(ref _ProductType, value); } }

        public virtual SetterOut SetterOut { get; set; }

        public virtual Shipment Shipment { get; set; }

        public void Copy(Product prod)
        {
            this.idShipment = prod.idShipment;
            this.Shipment = prod.Shipment;
            this.g_number = prod.g_number;
            this.g_akb = prod.g_akb;
            this.g_avr = prod.g_avr;
            this.g_cooler = prod.g_cooler;
            this.g_numberBox = prod.g_numberBox;
            this.g_questList = prod.g_questList;
            this.g_dateRegister = prod.g_dateRegister;
            this.g_generatedNumber = prod.g_generatedNumber;
            this.g_name = prod.g_name;
            this.g_numberBI = prod.g_numberBI;
            this.g_numberUSIKP = prod.g_numberUSIKP;
            this.g_redaction1 = prod.g_redaction1;
            this.g_redaction2 = prod.g_redaction2;
            this.g_redactionPS = prod.g_redactionPS;
            this.g_shunt = prod.g_shunt;
            this.g_skm = prod.g_skm;
            this.g_zip = prod.g_zip;
            this.idSetter = prod.idSetter;
            this.SetterOut = prod.SetterOut;
            this.ProductType = prod.ProductType;
            this.g_ProductTypeId = prod.g_ProductTypeId;

            if (this.Modules == null)
                this.Modules = new ObservableCollection<Module>();
            this.Modules.Clear();
            foreach(var module in prod.Modules)
                this.Modules.Add(module);
        }
    }
}
