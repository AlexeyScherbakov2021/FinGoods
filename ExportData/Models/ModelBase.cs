using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Configuration;
using System.Linq;

namespace ExportData.Models
{
    public partial class ModelBase : DbContext
    {
        private static ModelBase currentBase = null;


        public static ModelBase GetBase()
        {
            string ConnectString = null;
#if DEBUG
            ConnectString = ConfigurationManager.ConnectionStrings["LocalBase"].ConnectionString;
            //#endif

#else
            ConnectString = ConfigurationManager.ConnectionStrings["ModelBase"].ConnectionString;
            ConnectString += ";user id=fpLoginName;password=ctcnhjt,s";
#endif

            //App.log.WriteLineLog("Получение базы ModelBase");
            if (currentBase == null)
                new ModelBase(ConnectString);
            return currentBase;
        }

        public ModelBase(string stringBase)  : base(stringBase)
        {
            currentBase = this;

        }

        public virtual DbSet<Modules> Modules { get; set; }
        public virtual DbSet<ModuleType> ModuleType { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<SerialNumber> SerialNumber { get; set; }
        public virtual DbSet<SetterOut> SetterOut { get; set; }
        public virtual DbSet<Shipment> Shipment { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Modules>()
                .Property(e => e.m_name)
                .IsUnicode(false);

            modelBuilder.Entity<Modules>()
                .Property(e => e.m_number)
                .IsUnicode(false);

            modelBuilder.Entity<Modules>()
                .Property(e => e.m_numberFW)
                .IsUnicode(false);

            modelBuilder.Entity<ModuleType>()
                .Property(e => e.mt_name)
                .IsUnicode(false);

            modelBuilder.Entity<ModuleType>()
                .HasMany(e => e.Modules)
                .WithRequired(e => e.ModuleType)
                .HasForeignKey(e => e.m_modTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ModuleType>()
                .HasMany(e => e.ModuleType1)
                .WithOptional(e => e.ModuleType2)
                .HasForeignKey(e => e.idParent);

            modelBuilder.Entity<Product>()
                .Property(e => e.g_name)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.g_number)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.g_numberBox)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.g_redaction1)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.g_redaction2)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.g_redactionPS)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.g_questList)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.g_numberBI)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.g_numberUSIKP)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Modules)
                .WithOptional(e => e.Product)
                .HasForeignKey(e => e.idProduct);

            modelBuilder.Entity<ProductType>()
                .Property(e => e.gt_name)
                .IsUnicode(false);

            modelBuilder.Entity<ProductType>()
                .HasMany(e => e.Product)
                .WithRequired(e => e.ProductType)
                .HasForeignKey(e => e.g_ProductTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SetterOut>()
                .Property(e => e.s_name)
                .IsUnicode(false);

            modelBuilder.Entity<SetterOut>()
                .Property(e => e.s_orderNum)
                .IsUnicode(false);

            modelBuilder.Entity<SetterOut>()
                .HasMany(e => e.Product)
                .WithOptional(e => e.SetterOut)
                .HasForeignKey(e => e.idSetter);

            modelBuilder.Entity<Shipment>()
                .Property(e => e.c_number)
                .IsUnicode(false);

            modelBuilder.Entity<Shipment>()
                .Property(e => e.c_objectInstall)
                .IsUnicode(false);

            modelBuilder.Entity<Shipment>()
                .Property(e => e.c_customer)
                .IsUnicode(false);

            modelBuilder.Entity<Shipment>()
                .Property(e => e.c_questList)
                .IsUnicode(false);

            modelBuilder.Entity<Shipment>()
                .Property(e => e.c_schet)
                .IsUnicode(false);

            modelBuilder.Entity<Shipment>()
                .HasMany(e => e.Modules)
                .WithOptional(e => e.Shipment)
                .HasForeignKey(e => e.idShipment);

            modelBuilder.Entity<Shipment>()
                .HasMany(e => e.Product)
                .WithOptional(e => e.Shipment)
                .HasForeignKey(e => e.idShipment);

            modelBuilder.Entity<Shipment>()
                .HasMany(e => e.SetterOut)
                .WithOptional(e => e.Shipment)
                .HasForeignKey(e => e.idShipment);
        }
    }
}
