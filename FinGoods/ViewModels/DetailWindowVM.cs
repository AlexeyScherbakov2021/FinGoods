using FinGoods.Commands;
using FinGoods.Models;
using FinGoods.Repository;
using FinGoods.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinGoods.ViewModels
{
    internal class DetailWindowVM : Observable
    {

        public string Title { get; set; }
        public ObservableCollection<Product> listProd { get; set; }
        public Product SelectedProd { get; set; }
        public Module SelectedModule { get; set; }

        public DetailWindowVM()
        {
        }

        public DetailWindowVM(ObservableCollection<Product> list, string name)
        {
            listProd = list;
            Title = name;
        }


        //--------------------------------------------------------------------------------
        // Команда 
        //--------------------------------------------------------------------------------
        public ICommand SelectItemCommand => new LambdaCommand(OnSelectItemCommandExecuted, CanSelectItemCommand);
        private bool CanSelectItemCommand(object p) => true;
        private void OnSelectItemCommandExecuted(object p)
        {
            if (p is RoutedPropertyChangedEventArgs<object> e)
            {
                //SelectCard = null;
                SelectedProd = null;
                SelectedModule = null;

                //if (e.NewValue is CardOrder co)
                //{
                //    SelectCard = co;
                //}
                if (e.NewValue is Product g)
                {
                    SelectedProd = g;
                }
                if (e.NewValue is Module m)
                {
                    SelectedModule = m;
                }
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Добавить оборудование
        //--------------------------------------------------------------------------------
        public ICommand AddGoodsCommand => new LambdaCommand(OnAddGoodsCommandExecuted, CanAddGoodsCommand);
        private bool CanAddGoodsCommand(object p) => true;
        private void OnAddGoodsCommandExecuted(object p)
        {
            Product newProd = new Product();

            //newGoods.CardOrder = SelectCard;
            ProdWindow win = new ProdWindow();
            ProdWindowVM vm = new ProdWindowVM(newProd);
            win.DataContext = vm;
            if (win.ShowDialog() == true)
            {
                listProd.Add(newProd);
                RepositoryMSSQL<Product> repo = new Repository.RepositoryMSSQL<Product>();
                repo.Add(newProd, true);
                //repo.Save();
            }
        }


        //--------------------------------------------------------------------------------
        // Команда Редактировать оборудование
        //--------------------------------------------------------------------------------
        public ICommand EditGoodsCommand => new LambdaCommand(OnEditGoodsCommandExecuted, CanEditGoodsCommand);
        private bool CanEditGoodsCommand(object p) => SelectedProd != null;
        private void OnEditGoodsCommandExecuted(object p)
        {
            ProdWindow win = new ProdWindow();
            ProdWindowVM vm = new ProdWindowVM(SelectedProd);
            win.DataContext = vm;
            if (win.ShowDialog() == true)
            {
                RepositoryMSSQL<Product> repo = new Repository.RepositoryMSSQL<Product>();
                repo.Save();
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Удалить оборудование
        //--------------------------------------------------------------------------------
        public ICommand DelGoodsCommand => new LambdaCommand(OnDelGoodsCommandExecuted, CanDelGoodsCommand);
        private bool CanDelGoodsCommand(object p) => SelectedProd != null;
        private void OnDelGoodsCommandExecuted(object p)
        {
            if (MessageBox.Show($"Удалить «{SelectedProd.g_name}»", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                RepositoryMSSQL<Module> repoMod = new RepositoryMSSQL<Module>();
                RepositoryMSSQL<Product> repoGoods = new RepositoryMSSQL<Product>();

                while (SelectedProd.Modules.Count > 0)
                    repoMod.Remove(SelectedProd.Modules[0]);
                
                repoGoods.Delete(SelectedProd);
                repoGoods.Save();
            }
        }


        //--------------------------------------------------------------------------------
        // Команда Добавить модуль
        //--------------------------------------------------------------------------------
        public ICommand AddModulCommand => new LambdaCommand(OnAddModulCommandExecuted, CanAddModulCommand);
        private bool CanAddModulCommand(object p) => SelectedProd != null;
        private void OnAddModulCommandExecuted(object p)
        {
            Module newMod = new Module();
            //newMod.Goods = SelectedGoods;

            ModulWindow win = new ModulWindow();
            ModulWindowVM vm = new ModulWindowVM(newMod);
            win.DataContext = vm;
            if (win.ShowDialog() == true)
            {
                SelectedProd.Modules.Add(newMod);
                RepositoryMSSQL<Product> repoProd = new RepositoryMSSQL<Product>();
                repoProd.Save();
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Редактировать модуль
        //--------------------------------------------------------------------------------
        public ICommand EditModuleCommand => new LambdaCommand(OnEditModuleCommandExecuted, CanEditModuleCommand);
        private bool CanEditModuleCommand(object p) => SelectedModule != null;
        private void OnEditModuleCommandExecuted(object p)
        {
            ModulWindow win = new ModulWindow();
            ModulWindowVM vm = new ModulWindowVM(SelectedModule);
            win.DataContext = vm;
            if (win.ShowDialog() == true)
            {
                RepositoryMSSQL<Product> repoProd = new RepositoryMSSQL<Product>();
                repoProd.Save();
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Удалить модуль
        //--------------------------------------------------------------------------------
        public ICommand DelModulCommand => new LambdaCommand(OnDelModulCommandExecuted, CanDelModulCommand);
        private bool CanDelModulCommand(object p) => SelectedModule != null;
        private void OnDelModulCommandExecuted(object p)
        {
            if (MessageBox.Show($"Удалить «{SelectedModule.m_name}»", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                RepositoryMSSQL<Module> repoMod = new RepositoryMSSQL<Module>();

                repoMod.Delete(SelectedModule.id, true);
                //repoMod.Save();
            //    SelectModule.Goods.Modules.Remove(SelectModule);
            }
        }


    }
}
