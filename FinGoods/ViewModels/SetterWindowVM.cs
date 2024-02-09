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

        //--------------------------------------------------------------------------------
        // Команда Открыть Изделие
        //--------------------------------------------------------------------------------
        public ICommand OpenProdCommand => new LambdaCommand(OnOpenProdCommandExecuted, CanOpenProdCommand);
        private bool CanOpenProdCommand(object p) => true;
        private void OnOpenProdCommandExecuted(object p)
        {
            ProdWindow win = new ProdWindow();
            ProdWindowVM vm = new ProdWindowVM(selectProd);
            win.DataContext = vm;
            win.ShowDialog();
        }


        //--------------------------------------------------------------------------------
        // Команда Список заказов из ФП
        //--------------------------------------------------------------------------------
        public ICommand SelectContractCommand => new LambdaCommand(OnSelectContractCommandExecuted, CanSelectContractCommand);
        private bool CanSelectContractCommand(object p) => true;
        private void OnSelectContractCommandExecuted(object p)
        {
            var order = (p as MouseButtonEventArgs).Source as OrderFP;
            if (order == null)
                throw new Exception("Ошибка выбора заказа");

            setter.s_orderNum = order.doc_name;
            //Ship.c_customer = order.cli_name;
            //Ship.c_schet = order.PactNo;

            foreach (var prod in setter.Products)
            {
                prod.g_number =
                    ProdWindowVM.CreateSerialNumber(prod, setter.s_orderNum.Substring(0, 6));
            }
        }


        #endregion

    }
}
