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


        public void Copy(Shipment ship)
        {

            this.c_schet = ship.c_schet;
            this.c_questList = ship.c_questList;
            this.c_number = ship.c_number;
            this.c_cardOrder = ship.c_cardOrder;
            this.c_customer = ship.c_customer;
            this.c_dateOut = ship.c_dateOut;
            this.c_objectInstall = ship.c_objectInstall;

            if (SetterOuts == null)
                SetterOuts = new ObservableCollection<SetterOut>();
            SetterOuts.Clear();
            foreach (SetterOut st in ship.SetterOuts)
                SetterOuts.Add(st);
            //this.SetterOuts = ship.SetterOuts;

            if (Products == null)
                Products = new ObservableCollection<Product>();
            Products.Clear();
            foreach (var item in ship.Products)
                Products.Add(item);
            //this.Products = ship.Products;

            if (Modules == null)
                Modules= new ObservableCollection<Module>();
            Modules.Clear();
            foreach (var module in ship.Modules)
                Modules.Add(module);
            //this.Modules = ship.Modules;

        }


        public Shipment(Shipment ship)
        {
            Copy(ship);
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
        public string c_questList { get => _c_questList; set { Set(ref _c_questList, value); } }
        private string _c_questList;

        [StringLength(180)]
        public string c_schet { get => _c_schet; set { Set(ref _c_schet, value); } }
        private string _c_schet;

        [StringLength(80)]
        public string c_cardOrder { get => _c_cardOrder; set { Set(ref _c_cardOrder, value); } }
        private string _c_cardOrder;

        [StringLength(80)]
        public string c_numberUPD { get => _c_numberUPD; set { Set(ref _c_numberUPD, value); } }
        private string _c_numberUPD;



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Module> Modules { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Product> Products { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<SetterOut> SetterOuts { get; set; }
    }
}
