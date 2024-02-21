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
using System.Windows.Data;
using System.Windows.Input;

namespace FinGoods.ViewModels
{
    internal class AllSetterWindowVM : Observable
    {
        private ObservableCollection<SetterOut> _listSetter;
        public ObservableCollection<SetterOut> listSetter 
        { 
            get => _listSetter; 
            set
            {
                Set(ref _listSetter, value);
                _listSetViewSource.Source = value;
                _listSetViewSource.Filter += OnFilterList;
                _listSetViewSource.View.Refresh();

            }
        }

        CollectionViewSource _listSetViewSource = new CollectionViewSource();
        public ICollectionView listSetterView => _listSetViewSource?.View;

        private string _Filtr;
        public string Filtr
        {
            get => _Filtr;
            set
            {
                if (_Filtr != value)
                {
                    _Filtr = value;
                    _listSetViewSource.View.Refresh();
                }
            }
        }

        private void OnFilterList(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is SetterOut st) || string.IsNullOrEmpty(Filtr)) return;

            if (!st.s_name.ToLower().Contains(Filtr.ToLower()))
                E.Accepted = false;
        }


        private SetterOut _selectedSetter;
        public SetterOut selectedSetter { get=> _selectedSetter; set { Set(ref _selectedSetter, value); } }

        public Visibility isVisible { get; set; } = Visibility.Collapsed;

        RepositoryMSSQL<SetterOut> repo = new RepositoryMSSQL<SetterOut>();

        public AllSetterWindowVM()
        {
            listSetter = new ObservableCollection<SetterOut>(repo.Items);
        }

        public AllSetterWindowVM(bool s)
        {
            listSetter = new ObservableCollection<SetterOut>(repo.Items);
            listSetter = new ObservableCollection<SetterOut>(listSetter.Where(it => it.idShipment == null));
            isVisible = Visibility.Visible;

        }

        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Добавить 
        //--------------------------------------------------------------------------------
        public ICommand AddSetCommand => new LambdaCommand(OnAddSetCommandExecuted, CanAddSetCommand);
        private bool CanAddSetCommand(object p) => true;
        private void OnAddSetCommandExecuted(object p)
        {
            SetterOut newSet = new SetterOut();
            SetterWindow win = new SetterWindow();
            SetterWindowVM vm = new SetterWindowVM(newSet);
            win.DataContext = vm;
            if (win.ShowDialog() == true)
            {
                bool res = true;
                if (newSet.id == 0)
                    res = repo.Add(newSet, true);

                if (res)
                {
                    listSetter.Add(newSet);
                    selectedSetter = newSet;
                }
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Редактировать 
        //--------------------------------------------------------------------------------
        public ICommand EditSetCommand => new LambdaCommand(OnEditSetCommandExecuted, CanEditSetCommand);
        private bool CanEditSetCommand(object p) => selectedSetter != null;
        private void OnEditSetCommandExecuted(object p)
        {
            SetterWindow win = new SetterWindow();
            SetterWindowVM vm = new SetterWindowVM(selectedSetter);
            win.DataContext = vm;
            if (win.ShowDialog() == true)
            {
                repo.Save();
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Удалить 
        //--------------------------------------------------------------------------------
        public ICommand DelSetCommand => new LambdaCommand(OnDelProdCommandExecuted, CanDelSetCommand);
        private bool CanDelSetCommand(object p) => selectedSetter != null;
        private void OnDelProdCommandExecuted(object p)
        {
            if (MessageBox.Show($"Удалить «{selectedSetter.s_name}»", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                repo.Delete(selectedSetter, true);
                listSetter.Remove(selectedSetter);
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Выбрать
        //--------------------------------------------------------------------------------
        public ICommand SelectModuleCommand => new LambdaCommand(OnSelectModulCommandExecuted, CanSelectModulCommand);
        private bool CanSelectModulCommand(object p) => selectedSetter != null;
        private void OnSelectModulCommandExecuted(object p)
        {
            var win = App.Current.Windows.OfType<AllSetterWindow>().FirstOrDefault();
            win.DialogResult = true;
        }

        //--------------------------------------------------------------------------------
        // Команда Где используется
        //--------------------------------------------------------------------------------
        public ICommand InContentCommand => new LambdaCommand(OnInContentCommandExecuted, CanInContentCommand);
        private bool CanInContentCommand(object p) => selectedSetter?.Shipment != null;
        private void OnInContentCommandExecuted(object p)
        {
            if (selectedSetter?.Shipment != null)
            {
                ShipWindow win = new ShipWindow();
                ShipWindowVM vm = new ShipWindowVM(selectedSetter.Shipment);
                win.DataContext = vm;
                win.ShowDialog();
            }
        }


        #endregion

    }
}
