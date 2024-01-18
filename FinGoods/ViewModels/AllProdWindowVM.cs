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

namespace FinGoods.ViewModels
{
    internal class AllProdWindowVM
    {
        public ObservableCollection<Product> listProduct { get; set; }
        public Product selectedProduct { get; set; }

        RepositoryMSSQL<Product> repo = new RepositoryMSSQL<Product>();

        public AllProdWindowVM()
        {
            listProduct = new ObservableCollection<Product>(repo.Items);

        }

        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Добавить 
        //--------------------------------------------------------------------------------
        public ICommand AddProdCommand => new LambdaCommand(OnAddProdCommandExecuted, CanAddProdCommand);
        private bool CanAddProdCommand(object p) => true;
        private void OnAddProdCommandExecuted(object p)
        {
            Product newMod = new Product();
            ProdWindow win = new ProdWindow();
            ProdWindowVM vm = new ProdWindowVM(newMod);
            win.DataContext = vm;
            if (win.ShowDialog() == true)
            {
                //RepositoryMSSQL<Module> repo = new RepositoryMSSQL<Module>();
                repo.Add(newMod, true);
                listProduct.Add(newMod);
            }
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
                repo.Save();
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Удалить 
        //--------------------------------------------------------------------------------
        public ICommand DelProdCommand => new LambdaCommand(OnDelProdCommandExecuted, CanDelProdCommand);
        private bool CanDelProdCommand(object p) => selectedProduct != null
            && selectedProduct.idShipment == null;
        private void OnDelProdCommandExecuted(object p)
        {
            if (MessageBox.Show($"Удалить «{selectedProduct.g_name}»", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                repo.Delete(selectedProduct, true);
                listProduct.Remove(selectedProduct);
            }
        }
        #endregion

    }
}
