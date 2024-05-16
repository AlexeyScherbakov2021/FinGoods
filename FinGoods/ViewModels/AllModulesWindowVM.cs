using FinGoods.Commands;
using FinGoods.Models;
using FinGoods.Repository;
using FinGoods.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace FinGoods.ViewModels
{
    internal class AllModulesWindowVM : Observable
    {
        //private bool IsFromProd = false;
        private ObservableCollection<Module> _listModules;
        public ObservableCollection<Module> listModules 
        { 
            get => _listModules;
            set
            {
                Set(ref _listModules, value);
                _listModulesViewSource.Source = value;
                _listModulesViewSource.Filter += OnFilterList;
                _listModulesViewSource.View.Refresh();
            }
        }

        CollectionViewSource _listModulesViewSource = new CollectionViewSource();
        public ICollectionView listModuleView => _listModulesViewSource?.View;

        private Module _selectedModule;
        public Module selectedModule { get => _selectedModule; set { Set(ref _selectedModule, value); } }

        public Visibility isVisible { get; set; }  = Visibility.Collapsed;

        RepositoryMSSQL<Module> repo = new RepositoryMSSQL<Module>();

        private string _Filtr;
        public string Filtr
        {
            get => _Filtr;
            set
            {
                if (_Filtr != value)
                {
                    _Filtr = value;
                    _listModulesViewSource.View.Refresh();
                }
            }
        }

        private void OnFilterList(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is Module mod) || string.IsNullOrEmpty(Filtr)) return;

            if (!mod.m_number.ToLower().Contains(Filtr.ToLower()))
                E.Accepted = false;
        }



        public AllModulesWindowVM()
        {
            listModules = new ObservableCollection<Module>(repo.Items);
        }

        //public AllModulesWindowVM(bool prod) 
        //{
        //    IsFromProd = prod;
        //    listModules = new ObservableCollection<Module>(repo.Items
        //        .Where(it => it.idProduct == null && it.idShipment == null));
        //    isVisible = Visibility.Visible;
        //}

        public AllModulesWindowVM(IEnumerable<Module> listExclModule) 
        {
            //IsFromProd = true;
            listModules = new ObservableCollection<Module>(repo.Items
                .Where(it => it.idProduct == null && it.idShipment == null));

            foreach (var item in listExclModule)
                listModules.Remove(item);

            isVisible = Visibility.Visible;
        }



        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Добавить модуль
        //--------------------------------------------------------------------------------
        public ICommand AddModulCommand => new LambdaCommand(OnAddModulCommandExecuted, CanAddModulCommand);
        private bool CanAddModulCommand(object p) => true;
        private void OnAddModulCommandExecuted(object p)
        {
            Module newMod = null; // = new Module();
            ModulWindow win = new ModulWindow();
            ModulWindowVM vm = new ModulWindowVM(newMod);
            win.DataContext = vm;
            if (win.ShowDialog() == true)
            {
                foreach( var item in vm.listAddModules)
                {
                    newMod = new Module();
                    newMod.Copy(item);
                    if (repo.Add(newMod, true))
                    {
                        listModules.Add(newMod);
                        selectedModule = newMod;
                    }
                }

                if (!string.IsNullOrEmpty( vm.module.m_number))
                {
                    newMod = new Module();
                    newMod.Copy(vm.module);
                    if (repo.Add(newMod, true))
                    {
                        listModules.Add(newMod);
                        selectedModule = newMod;
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Редактировать модуль
        //--------------------------------------------------------------------------------
        public ICommand EditModuleCommand => new LambdaCommand(OnEditModuleCommandExecuted, CanEditModuleCommand);
        private bool CanEditModuleCommand(object p) => selectedModule != null;
        private void OnEditModuleCommandExecuted(object p)
        {
            ModulWindow win = new ModulWindow();
            ModulWindowVM vm = new ModulWindowVM(selectedModule);
            win.DataContext = vm;
            if (win.ShowDialog() == true)
            {
                selectedModule.Copy(vm.module);
                try
                {
                    repo.Save();
                }
                catch
                {
                    MessageBox.Show($"Ошибка записи в базу данных. Возможно было дублирование номера {selectedModule.m_number} модуля.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Удалить модуль
        //--------------------------------------------------------------------------------
        public ICommand DelModulCommand => new LambdaCommand(OnDelModulCommandExecuted, CanDelModulCommand);
        private bool CanDelModulCommand(object p) => selectedModule != null 
            && selectedModule.idProduct == null
            && selectedModule.idShipment == null;
        private void OnDelModulCommandExecuted(object p)
        {
            if (MessageBox.Show($"Удалить «{selectedModule.m_name}»", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                repo.Delete(selectedModule, true);
                listModules.Remove(selectedModule);
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Выбрать
        //--------------------------------------------------------------------------------
        public ICommand SelectModuleCommand => new LambdaCommand(OnSelectModulCommandExecuted, CanSelectModulCommand);
        private bool CanSelectModulCommand(object p) => selectedModule != null;
        private void OnSelectModulCommandExecuted(object p)
        {
            var win = App.Current.Windows.OfType<AllModulesWindow>().FirstOrDefault();
            win.DialogResult = true;
        }

        //--------------------------------------------------------------------------------
        // Команда Где используется
        //--------------------------------------------------------------------------------
        public ICommand InContentCommand => new LambdaCommand(OnInContentCommandExecuted, CanInContentCommand);
        private bool CanInContentCommand(object p) =>
            selectedModule?.Product != null
            || selectedModule?.Shipment != null;
        private void OnInContentCommandExecuted(object p)
        {
            if (selectedModule?.Product != null)
            {
                ProdWindow win = new ProdWindow();
                ProdWindowVM vm = new ProdWindowVM(selectedModule.Product);
                win.DataContext = vm;
                win.ShowDialog();
            }

            if (selectedModule?.Shipment != null)
            {
                ShipWindow win = new ShipWindow();
                ShipWindowVM vm = new ShipWindowVM(selectedModule.Shipment);
                win.DataContext = vm;
                win.ShowDialog();
                
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Перенести в другой заказ
        //--------------------------------------------------------------------------------
        //public ICommand MoveCommand => new LambdaCommand(OnMoveCommandExecuted, CanMoveCommand);
        //private bool CanMoveCommand(object p) => selectedModule != null;
        //private void OnMoveCommandExecuted(object p)
        //{

        //}

        #endregion

    }
}
