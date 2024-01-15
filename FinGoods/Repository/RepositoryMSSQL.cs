using FinGoods.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.Repository
{
    internal class RepositoryMSSQL<T> where T : class, IEntity, new()
    {
        public static readonly ModelBase BaseFG = ModelBase.GetBase();
        protected readonly DbSet<T> _Set;
        public virtual IQueryable<T> Items => _Set;

        public RepositoryMSSQL()
        {
            _Set = BaseFG.Set<T>();
        }

        public T Get(int id)
        {
            return Items.SingleOrDefault(it => it.id == id);
        }

        public void Save()
        {
            BaseFG.SaveChanges();
        }


        public T Add(T item, bool Autosave = false)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _Set.Add(item);

            if (Autosave)
                Save();
            return item;
        }

        public void Delete(int id, bool Autosave = false)
        {
            if (id < 1)
                return;

            //T item = new T();
            //item.id = id;

            var item = _Set.Local.FirstOrDefault(i => i.id == id) ?? new T { id = id };
            Delete(item, Autosave);

        }

        public void Delete(T item, bool Autosave = false) 
        {
            if (item is null || item.id <= 0)
                return;

            BaseFG.Entry(item).State = EntityState.Deleted;
            if (Autosave)
                Save();
        }


        public void Remove(T item, bool Autosave = false)
        {
            BaseFG.Set<T>().Remove(item);
            if (Autosave)
                Save();
        }

    }
}
