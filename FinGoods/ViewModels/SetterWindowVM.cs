using FinGoods.Commands;
using FinGoods.Models;
using FinGoods.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace FinGoods.ViewModels
{
    internal class SetterWindowVM
    {
        public SetterOut setter { get; set; }
        public Product selectProd { get; set; }

        public SetterWindowVM()
        {
        }

        public SetterWindowVM(SetterOut st)
        {
            setter = st;
        }

        #region  Команды

        //--------------------------------------------------------------------------------
        // Команда 
        //--------------------------------------------------------------------------------
        public ICommand AddProdCommand => new LambdaCommand(OnAddProdCommandExecuted, CanAddProdCommand);
        private bool CanAddProdCommand(object p) => true;
        private void OnAddProdCommandExecuted(object p)
        {
            AllProdWindow win = new AllProdWindow();
            AllProdWindowVM vm = new AllProdWindowVM(true);
            win.DataContext = vm;
            if (win.ShowDialog() == true && vm.selectedProduct != null)
            {
                setter.Products.Add(vm.selectedProduct);
            }
        }

        //--------------------------------------------------------------------------------
        // Команда 
        //--------------------------------------------------------------------------------
        public ICommand DelProdCommand => new LambdaCommand(OnDelProdCommandExecuted, CanDelProdCommand);
        private bool CanDelProdCommand(object p) => selectProd != null;
        private void OnDelProdCommandExecuted(object p)
        {
            if (MessageBox.Show($"Удалить «{selectProd.g_name}»", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                setter.Products.Remove(selectProd);
            }
        }


        #endregion

    }
}
