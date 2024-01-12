using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace FinGoods.Models
{
    public partial class ModelBase : DbContext
    {
        public ModelBase()
            : base("name=ModelBase")
        {
        }

        public virtual DbSet<CardOrder> CardOrders { get; set; }
        public virtual DbSet<Goods> Goods { get; set; }
        public virtual DbSet<GoodsType> GoodsTypes { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<ModuleType> ModuleTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardOrder>()
                .Property(e => e.c_number)
                .IsUnicode(false);

            modelBuilder.Entity<CardOrder>()
                .HasMany(e => e.Goods)
                .WithRequired(e => e.CardOrder)
                .HasForeignKey(e => e.g_cardOrderId);

            modelBuilder.Entity<Goods>()
                .Property(e => e.g_name)
                .IsUnicode(false);

            modelBuilder.Entity<Goods>()
                .Property(e => e.g_number)
                .IsUnicode(false);

            modelBuilder.Entity<Goods>()
                .Property(e => e.g_numberBox)
                .IsUnicode(false);

            modelBuilder.Entity<CardOrder>()
                .Property(e => e.c_customer)
                .IsUnicode(false);

            modelBuilder.Entity<CardOrder>()
                .Property(e => e.c_questList)
                .IsUnicode(false);

            modelBuilder.Entity<CardOrder>()
                .Property(e => e.c_objectInstall)
                .IsUnicode(false);

            modelBuilder.Entity<Goods>()
                .HasMany(e => e.Modules)
                .WithRequired(e => e.Goods)
                .HasForeignKey(e => e.m_goodsId);

            modelBuilder.Entity<GoodsType>()
                .Property(e => e.gt_name)
                .IsUnicode(false);

            modelBuilder.Entity<GoodsType>()
                .HasMany(e => e.Goods)
                .WithRequired(e => e.GoodsType)
                .HasForeignKey(e => e.g_type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Module>()
                .Property(e => e.m_name)
                .IsUnicode(false);

            modelBuilder.Entity<Module>()
                .Property(e => e.m_number)
                .IsUnicode(false);

            modelBuilder.Entity<Module>()
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
        }
    }
}
