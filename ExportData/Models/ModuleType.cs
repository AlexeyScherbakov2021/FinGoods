namespace ExportData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ModuleType")]
    public partial class ModuleType : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ModuleType()
        {
            Modules = new HashSet<Modules>();
            ModuleType1 = new HashSet<ModuleType>();
        }

        public int id { get; set; }

        public int? idParent { get; set; }

        [StringLength(100)]
        public string mt_name { get; set; }

        public int? mt_number { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Modules> Modules { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ModuleType> ModuleType1 { get; set; }

        public virtual ModuleType ModuleType2 { get; set; }
    }
}
