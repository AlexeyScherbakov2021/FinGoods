namespace ExportData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SerialNumber")]
    public partial class SerialNumber
    {
        public int id { get; set; }

        public int? kind_number { get; set; }

        public int? year_number { get; set; }

        public int? gen_number { get; set; }
    }
}
