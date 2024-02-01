using FinGoods.Commands;
using FinGoods.Infrastructure;
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
    internal class TypesWindowVM : Observable
    {
        //public string Title { get; set; }
        public ObservableCollection<ProductType> listProd { get; set; }
        public ObservableCollection<ModuleType> listModul { get; set; }

        private ProductType _SelectedProd;
        public ProductType SelectedProd { get => _SelectedProd; set { Set(ref _SelectedProd, value); } }

        private ModuleType _SelectedModule;
        public ModuleType SelectedModule { get => _SelectedModule; set { Set(ref _SelectedModule, value); } }

        private ModuleType _EditableModule;
        public ModuleType EditableModule { get => _EditableModule; set { Set(ref _EditableModule, value); } }


        private readonly RepositoryMSSQL<ProductType> repoProd = new RepositoryMSSQL<ProductType>();
        private readonly RepositoryMSSQL<ModuleType> repoModul = new RepositoryMSSQL<ModuleType>();

        public TypesWindowVM()
        {
            listProd = new ObservableCollection<ProductType>(repoProd.Items);
            listModul = new ObservableCollection<ModuleType>(repoModul.Items.Where(it => it.idParent == null));
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
                ////SelectCard = null;
                //SelectedProd = null;
                //SelectedModule = null;

                //if (e.NewValue is Product g)
                //{
                //    SelectedProd = g;
                //}
                if (e.NewValue is ModuleType m)
                {
                    SelectedModule = m;
                }
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Добавить оборудование
        //--------------------------------------------------------------------------------
        public ICommand AddProdCommand => new LambdaCommand(OnAddProdCommandExecuted, CanAddProdCommand);
        private bool CanAddProdCommand(object p) => true;
        private void OnAddProdCommandExecuted(object p)
        {
            QueryNameWindow win = new QueryNameWindow();
            QueryNameWindowVM vm = win.DataContext as QueryNameWindowVM;
            vm.Title = "Наименование типа изделия";

            if (win.ShowDialog() == true && !string.IsNullOrEmpty(vm.Name))
            {
                ProductType newProdType = new ProductType();
                newProdType.gt_name = vm.Name;
                repoProd.Add(newProdType, true);
                listProd.Add(newProdType);
                SelectedProd = listProd.Last();
            }

        }


        //--------------------------------------------------------------------------------
        // Команда Редактировать оборудование
        //--------------------------------------------------------------------------------
        //public ICommand EditGoodsCommand => new LambdaCommand(OnEditGoodsCommandExecuted, CanEditGoodsCommand);
        //private bool CanEditGoodsCommand(object p) => SelectedProd != null;
        //private void OnEditGoodsCommandExecuted(object p)
        //{
        //    ProdWindow win = new ProdWindow();
        //}

        //--------------------------------------------------------------------------------
        // Команда Удалить оборудование
        //--------------------------------------------------------------------------------
        public ICommand DelProdCommand => new LambdaCommand(OnDelProdCommandExecuted, CanDelProdCommand);
        private bool CanDelProdCommand(object p) => SelectedProd != null;
        private void OnDelProdCommandExecuted(object p)
        {
            if (MessageBox.Show($"Удалить «{SelectedProd.gt_name}»", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    repoProd.Delete(SelectedProd, true);
                    listProd.Remove(SelectedProd);
                }
                catch
                {
                    repoProd.Undelete(SelectedProd);
                    MessageBox.Show($"Тип используется, удалить невозможно", "Предупреждение", MessageBoxButton.OK);
                }
            }
        }


        //--------------------------------------------------------------------------------
        // Команда Добавить модуль
        //--------------------------------------------------------------------------------
        public ICommand AddModulCommand => new LambdaCommand(OnAddModulCommandExecuted, CanAddModulCommand);
        private bool CanAddModulCommand(object p) => true;
        private void OnAddModulCommandExecuted(object p)
        {
            string param = p.ToString();

            QueryNameWindow win = new QueryNameWindow();
            QueryNameWindowVM vm = win.DataContext as QueryNameWindowVM;
            vm.Title = "Наименование типа модуля";

            if(win.ShowDialog() == true && !string.IsNullOrEmpty(vm.Name))
            {
                ModuleType newMod = new ModuleType();
                newMod.mt_name = vm.Name;
                newMod.mt_number = vm.Number;

                if (SelectedModule != null && param == "1")
                {
                    SelectedModule.ChildModuleType.Add(newMod);
                    repoModul.Save();
                }
                else if(param == "0")
                {
                    repoModul.Add(newMod, true);
                    listModul.Add(newMod);
                    _SelectedModule = listModul.Last();
                }
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Редактировать модуль
        //--------------------------------------------------------------------------------
        public ICommand EditModuleCommand => new LambdaCommand(OnEditModuleCommandExecuted, CanEditModuleCommand);
        private bool CanEditModuleCommand(object p) => SelectedModule != null;
        private void OnEditModuleCommandExecuted(object p)
        {
            QueryNameWindow win = new QueryNameWindow();
            QueryNameWindowVM vm = win.DataContext as QueryNameWindowVM;
            vm.Title = "Наименование типа модуля";
            vm.Name = SelectedModule.mt_name;
            vm.Number = SelectedModule.mt_number;
            if (win.ShowDialog() == true && !string.IsNullOrEmpty(vm.Name))
            {
                SelectedModule.mt_name = vm.Name;
                SelectedModule.mt_number = vm.Number;
                repoModul.Save();
            }

            //if (p == null)
            //    EditableModule = null;
            //else
            //    EditableModule = SelectedModule;
        }

        //--------------------------------------------------------------------------------
        // Команда Удалить модуль
        //--------------------------------------------------------------------------------
        public ICommand DelModulCommand => new LambdaCommand(OnDelModulCommandExecuted, CanDelModulCommand);
        private bool CanDelModulCommand(object p) => SelectedModule != null;
        private void OnDelModulCommandExecuted(object p)
        {
            if (MessageBox.Show($"Удалить «{SelectedModule.mt_name}»", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    bool isRoot = SelectedModule.ParentModuleType == null;

                    repoModul.Delete(SelectedModule);
                    if (isRoot)
                        listModul.Remove(SelectedModule);
                    repoModul.Save();
                }
                catch
                {
                    repoModul.Undelete(SelectedModule);
                    MessageBox.Show($"Тип используется, удалить невозможно", "Предупреждение", MessageBoxButton.OK);
                }
            }
        }
    }
}
