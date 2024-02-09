namespace FinGoods.Models
{
    using FinGoods.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SetterOut")]
    public partial class SetterOut : Observable, IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SetterOut()
        {
            Products = new ObservableCollection<Product>();
        }

        public int id { get; set; }

        public int? idShipment { get; set; }

        private string _s_name;
        [StringLength(150)]
        public string s_name { get => _s_name; set { Set(ref _s_name, value); } }

        private string _s_orderNum;
        [StringLength(150)]
        public string s_orderNum { get => _s_orderNum; set { Set(ref _s_orderNum, value); } }

        

        private ObservableCollection<Product> _Products;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Product> Products { get => _Products; set { Set(ref _Products, value); } }

        public virtual Shipment Shipment { get; set; }
    }
}
