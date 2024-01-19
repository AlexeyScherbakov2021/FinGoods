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
    internal class MainWindowViewModel : Observable
    {
        private readonly RepositoryMSSQL<Shipment> repoShip = new RepositoryMSSQL<Shipment>();
        public ObservableCollection<Shipment> listShip { get; set; }

        public Shipment SelectShip { get; set; }

        public MainWindowViewModel()
        {
            listShip = new ObservableCollection<Shipment>(repoShip.Items);
        }

        #region команды

        //--------------------------------------------------------------------------------
        // Команда Добавить отгрузку
        //--------------------------------------------------------------------------------
        public ICommand AddShipCommand => new LambdaCommand(OnAddShipCommandExecuted, CanAddShipCommand);
        private bool CanAddShipCommand(object p) => true;
        private void OnAddShipCommandExecuted(object p)
        {
            Shipment Ship = new Shipment();

            ShipWindow win = new ShipWindow();
            ShipWindowVM vm = new ShipWindowVM(Ship);
            win.DataContext = vm;

            if(win.ShowDialog() == true)
            {
                listShip.Add(Ship);
                //repoCO.Add(Ship);
                repoShip.Save();
            }

        }

        //--------------------------------------------------------------------------------
        // Команда Редактировать отгрузку
        //--------------------------------------------------------------------------------
        public ICommand EditShipCommand => new LambdaCommand(OnEditShipCommandExecuted, CanShipOrderCommand);
        private bool CanShipOrderCommand(object p) => SelectShip != null;
        private void OnEditShipCommandExecuted(object p)
        {
            ShipWindow win = new ShipWindow();
            ShipWindowVM vm = new ShipWindowVM(SelectShip);
            win.DataContext = vm;

            if(win.ShowDialog() == true)
            {
                repoShip.Save();
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Удалить отгрузку
        //--------------------------------------------------------------------------------
        public ICommand DelShipCommand => new LambdaCommand(OnDelShipCommandExecuted, CanDelShipCommand);
        private bool CanDelShipCommand(object p) => SelectShip != null;
        private void OnDelShipCommandExecuted(object p)
        {

            if(MessageBox.Show($"Удалить «{SelectShip.c_number}»","Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                repoShip.Delete(SelectShip.id);
                repoShip.Save();
                listShip.Remove(SelectShip);
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Открыть окно детализации
        //--------------------------------------------------------------------------------
        public ICommand OpenDetailCommand => new LambdaCommand(OnOpenDetailCommandExecuted, CanOpenDetailCommand);
        private bool CanOpenDetailCommand(object p) => SelectShip != null;
        private void OnOpenDetailCommandExecuted(object p)
        {
            DetailWindow win = new DetailWindow();
            DetailWindowVM vm = new DetailWindowVM(SelectShip.Products, SelectShip.c_number);
            win.DataContext = vm;
            win.ShowDialog();
        }


        //--------------------------------------------------------------------------------
        // Команда 
        //--------------------------------------------------------------------------------
        public ICommand FromFPCommand => new LambdaCommand(OnFromFPCommandExecuted, CanFromFPCommand);
        private bool CanFromFPCommand(object p) => SelectShip != null;
        private void OnFromFPCommandExecuted(object p)
        {
            RepositoryFP repo = new RepositoryFP();
            repo.Load();
        }

        //--------------------------------------------------------------------------------
        // Команда Открыть окно модулей
        //--------------------------------------------------------------------------------
        public ICommand OpenModulesCommand => new LambdaCommand(OnOpenModulesCommandExecuted, CanOpenModulesCommand);
        private bool CanOpenModulesCommand(object p) => true;
        private void OnOpenModulesCommandExecuted(object p)
        {
            AllModulesWindow win = new AllModulesWindow();
            win.ShowDialog();
        }

        //--------------------------------------------------------------------------------
        // Команда Открыть окно готовых продуктов
        //--------------------------------------------------------------------------------
        public ICommand OpenProdCommand => new LambdaCommand(OnOpenProdCommandExecuted, CanOpenProdCommand);
        private bool CanOpenProdCommand(object p) => true;
        private void OnOpenProdCommandExecuted(object p)
        {
            AllProdWindow win = new AllProdWindow();
            win.ShowDialog();
        }

        //--------------------------------------------------------------------------------
        // Команда Открыть окно наборов
        //--------------------------------------------------------------------------------
        public ICommand OpenSetterCommand => new LambdaCommand(OnOpenSetterCommandExecuted, CanOpenSetterCommand);
        private bool CanOpenSetterCommand(object p) => true;
        private void OnOpenSetterCommandExecuted(object p)
        {
            AllSetterWindow win = new AllSetterWindow();
            win.ShowDialog();
        }


        #endregion

    }
}
