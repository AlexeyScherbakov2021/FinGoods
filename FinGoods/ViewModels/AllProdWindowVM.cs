using FinGoods.Commands;
using FinGoods.Repository;
using FinGoods.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using FinGoods.Models;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections;

namespace FinGoods.ViewModels
{
    internal class AllProdWindowVM : Observable
    {
        private ObservableCollection<Product> _listProduct;
        public ObservableCollection<Product> listProduct 
        { 
            get => _listProduct; 
            set
            {
                Set(ref _listProduct, value);
                _listProdViewSource.Source = value;
                _listProdViewSource.Filter += OnFilterList;
                _listProdViewSource.View.Refresh();

            }
        }

        CollectionViewSource _listProdViewSource = new CollectionViewSource();
        public ICollectionView listProductView => _listProdViewSource?.View;

        private string _Filtr;
        public string Filtr
        {
            get => _Filtr;
            set
            {
                if (_Filtr != value)
                {
                    _Filtr = value;
                    _listProdViewSource.View.Refresh();
                }
            }
        }

        private void OnFilterList(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is Product pd) || string.IsNullOrEmpty(Filtr)) return;

            if (!pd.g_number.ToLower().Contains(Filtr.ToLower()))
                E.Accepted = false;
        }

        public IList SelectedItems { get; set; }

        private Product _selectedProduct;
        public Product selectedProduct { get => _selectedProduct; set { Set(ref _selectedProduct, value); } }

        public Visibility isVisible { get; set; } = Visibility.Collapsed;

        RepositoryMSSQL<Product> repo = new RepositoryMSSQL<Product>();
        //RepositoryMSSQL<Module> repoModul = new RepositoryMSSQL<Module>();

        public AllProdWindowVM()
        {
            listProduct = new ObservableCollection<Product>(repo.Items);
        }

        //public AllProdWindowVM(bool isFiltr)
        //{
        //    listProduct = new ObservableCollection<Product>(repo.Items
        //        .Where(it => it.idSetter == null && it.idShipment == null));

        //    isVisible = Visibility.Visible;

        //}

        public AllProdWindowVM(IEnumerable<Product> listExclProd)
        {
            listProduct = new ObservableCollection<Product>(repo.Items
                .Where(it => it.idSetter == null && it.idShipment == null));

            foreach (var module in listExclProd)
                listProduct.Remove(module);

            isVisible = Visibility.Visible;

        }


        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Добавить 
        //--------------------------------------------------------------------------------
        public ICommand AddProdCommand => new LambdaCommand(OnAddProdCommandExecuted, CanAddProdCommand);
        private bool CanAddProdCommand(object p) => true;
        private void OnAddProdCommandExecuted(object p)
        {
            bool res = true;

            //Product newProd = new Product();
            ProdWindow win = new ProdWindow();
            ProdWindowVM vm = new ProdWindowVM(this, win);
            win.DataContext = vm;
            if (win.ShowDialog() == true)
            {
                Product newProd = new Product();
                newProd.Copy(vm.product);

                if (vm.product.id == 0 )
                    res = repo.Add(newProd, true);

                if (res)
                {
                    listProduct.Add(newProd);
                    selectedProduct = newProd;
                }
            }
        }

        public int AddNewProduct(Product prod)
        {
            bool res = true;
            Product newProd = new Product();
            newProd.Copy(prod);

            if (newProd.id == 0)
                res = repo.Add(newProd, true);

            if (res)
            {
                listProduct.Add(newProd);
                selectedProduct = newProd;
            }
            return newProd.id;
        }


        //--------------------------------------------------------------------------------
        // Команда Редактировать 
        //--------------------------------------------------------------------------------
        public ICommand EditProdCommand => new LambdaCommand(OnEditProdCommandExecuted, CanEditProdCommand);
        private bool CanEditProdCommand(object p) => selectedProduct != null;
        private void OnEditProdCommandExecuted(object p)
        {
            ProdWindow win = new ProdWindow();
            ProdWindowVM vm = new ProdWindowVM(selectedProduct);
            win.DataContext = vm;
            if (win.ShowDialog() == true)
            {
                selectedProduct.Copy(vm.product);
                //repoModul.Save();
                repo.Save();
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Удалить 
        //--------------------------------------------------------------------------------
        public ICommand DelProdCommand => new LambdaCommand(OnDelProdCommandExecuted, CanDelProdCommand);
        private bool CanDelProdCommand(object p) => selectedProduct != null 
            && selectedProduct.idShipment == null
            && selectedProduct.idSetter == null;
        private void OnDelProdCommandExecuted(object p)
        {
            if (MessageBox.Show($"Удалить «{selectedProduct.g_name}»", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                repo.Delete(selectedProduct, true);
                listProduct.Remove(selectedProduct);
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Выбрать
        //--------------------------------------------------------------------------------
        public ICommand SelectModuleCommand => new LambdaCommand(OnSelectModulCommandExecuted, CanSelectModulCommand);
        private bool CanSelectModulCommand(object p) => selectedProduct != null;
        private void OnSelectModulCommandExecuted(object p)
        {
            var win = App.Current.Windows.OfType<AllProdWindow>().FirstOrDefault();
            win.DialogResult = true;
        }

        //--------------------------------------------------------------------------------
        // Команда Где используется
        //--------------------------------------------------------------------------------
        public ICommand InContentCommand => new LambdaCommand(OnInContentCommandExecuted, CanInContentCommand);
        private bool CanInContentCommand(object p) => 
            selectedProduct?.SetterOut != null 
            || selectedProduct?.Shipment != null;
        private void OnInContentCommandExecuted(object p)
        {
            if(selectedProduct?.SetterOut != null)
            {
                SetterWindow win = new SetterWindow();
                SetterWindowVM vm = new SetterWindowVM(selectedProduct.SetterOut);
                win.DataContext = vm;
                win.ShowDialog();
            }

            if(selectedProduct?.Shipment != null)
            {
                ShipWindow win = new ShipWindow();
                ShipWindowVM vm = new ShipWindowVM(selectedProduct.Shipment);
                win.DataContext = vm;
                win.ShowDialog();
            }
        }

        #endregion

    }
}
