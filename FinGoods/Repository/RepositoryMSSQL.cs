using FinGoods.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinGoods.Repository
{
    internal class RepositoryMSSQL<T> where T : class, IEntity, new()
    {
        public static readonly ModelBase BaseFG = ModelBase.GetBase();
        protected readonly DbSet<T> _Set;
        public virtual IQueryable<T> Items => _Set;

        public RepositoryMSSQL()
        {
            //App.log.WriteLineLog("Конструктор RepositoryMSSQL");
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


        public bool Add(T item, bool Autosave = false)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            try
            {
                _Set.Add(item);

                if (Autosave)
                    Save();
            }
            catch (Exception e)
            {
                MessageBox.Show("Произошла ошибка записи в базу данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
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


        public void Undelete(T item)
        {
            if (item is null || item.id <= 0)
                return;
            
            BaseFG.Entry(item).State = EntityState.Unchanged;
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
