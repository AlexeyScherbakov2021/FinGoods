using FinGoods.Commands;
using FinGoods.Models;
using FinGoods.Repository;
using FinGoods.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace FinGoods.ViewModels
{
    internal class MainWindowViewModel : Observable
    {
        public string Title {  get; set; }

        private readonly RepositoryMSSQL<Shipment> repoShip = new RepositoryMSSQL<Shipment>();

        private ObservableCollection<Shipment> _listShip;
        public ObservableCollection<Shipment> listShip 
        { 
            get => _listShip; 
            set
            {
                Set(ref _listShip, value);
                _listShipViewSource.Source = value;
                _listShipViewSource.Filter += OnFilterList;
                _listShipViewSource.View.Refresh();

            }
        }

        CollectionViewSource _listShipViewSource = new CollectionViewSource();
        public ICollectionView listShipView => _listShipViewSource?.View;

        public string FindNumber { get;set; }


        private string _Filtr;
        public string Filtr
        {
            get => _Filtr;
            set
            {
                if (_Filtr != value)
                {
                    _Filtr = value;
                    _listShipViewSource.View.Refresh();
                }
            }
        }
        private void OnFilterList(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is Shipment st) || string.IsNullOrEmpty(Filtr)) return;

            if (!st.c_number.ToLower().Contains(Filtr.ToLower()))
                E.Accepted = false;
        }



        private Shipment _SelectShip;
        public Shipment SelectShip { get => _SelectShip; set { Set(ref _SelectShip, value); } }

        public MainWindowViewModel()
        {
#if DEBUG
            Title = "Заказы (Отладочная версия)";
#else
            Title = "Заказы";
#endif

            //App.log.WriteLineLog("Конструктор MainWindowViewModel");
            listShip = new ObservableCollection<Shipment>(repoShip.Items);
            //App.log.WriteLineLog("Получен список listShip");
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
                if (repoShip.Add(Ship))
                {
                    listShip.Add(Ship);
                    repoShip.Save();
                    SelectShip = Ship;
                }
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
        //public ICommand OpenDetailCommand => new LambdaCommand(OnOpenDetailCommandExecuted, CanOpenDetailCommand);
        //private bool CanOpenDetailCommand(object p) => SelectShip != null;
        //private void OnOpenDetailCommandExecuted(object p)
        //{
        //    TypesWindow win = new TypesWindow();
        //    TypesWindowVM vm = new TypesWindowVM(SelectShip.Products, SelectShip.c_number);
        //    win.DataContext = vm;
        //    win.ShowDialog();
        //}


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

        //--------------------------------------------------------------------------------
        // Команда Открыть окно наборов
        //--------------------------------------------------------------------------------
        public ICommand OpenTypesCommand => new LambdaCommand(OnOpenTypesCommandExecuted, CanOpenTypesCommand);
        private bool CanOpenTypesCommand(object p) => true;
        private void OnOpenTypesCommandExecuted(object p)
        {
            TypesWindow win = new TypesWindow();
            win.ShowDialog();
        }

        //--------------------------------------------------------------------------------
        // Команда Открыть окно наборов
        //--------------------------------------------------------------------------------
        public ICommand SearchNumberCommand => new LambdaCommand(OnSearchNumberCommandExecuted, CanSearchNumberCommand);
        private bool CanSearchNumberCommand(object p) => !string.IsNullOrEmpty(FindNumber);
        private void OnSearchNumberCommandExecuted(object p)
        {
            string number;
            RepositoryMSSQL<Product> repoProd = new RepositoryMSSQL<Product>();
            Product prod = repoProd.Items.FirstOrDefault(it => it.g_number == FindNumber);
            if (prod == null)
            {
                RepositoryMSSQL<Module> repoModule = new RepositoryMSSQL<Module>();
                Module mod = repoModule.Items.FirstOrDefault(it => it.m_number == FindNumber);
                if (mod == null)
                    return;

                number = mod.m_number;
                SelectShip = mod.Shipment != null
                    ? mod.Shipment
                    : mod.Product.Shipment == null
                        ? mod.Product.SetterOut.Shipment
                        : mod.Product.Shipment;
            }
            else
            {
                number = prod.g_number;
                SelectShip = prod.Shipment == null ? prod.SetterOut.Shipment : prod.Shipment;
            }
            
            ShipWindowVM vm = new ShipWindowVM(SelectShip, number);
            ShipWindow win = new ShipWindow();
            win.DataContext = vm;
            win.ShowDialog();
        }
        #endregion

    }
}
