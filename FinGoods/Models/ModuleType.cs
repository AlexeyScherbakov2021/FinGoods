namespace FinGoods.Models
{
    using FinGoods.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ModuleType")]
    public partial class ModuleType : Observable, IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ModuleType()
        {
            Modules = new HashSet<Module>();
            ChildModuleType = new ObservableCollection<ModuleType>();
        }

        public int id { get; set; }

        public int? idParent { get; set; }

        private string _mt_name;
        [StringLength(100)]
        public string mt_name { get => _mt_name; set { Set(ref _mt_name, value); } }

        private int? _mt_number;
        public int? mt_number { get => _mt_number; set { Set(ref _mt_number, value); } }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Module> Modules { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<ModuleType> ChildModuleType { get; set; }

        public virtual ModuleType ParentModuleType { get; set; }
    }
}
