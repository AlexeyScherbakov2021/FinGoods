using FinGoods.Commands;
using FinGoods.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace FinGoods.ViewModels
{
    internal class ContractsUCVM : Observable
    {
        private ObservableCollection<string> _listContract;
        public ObservableCollection<string> listContract
        {
            get => _listContract;
            set
            {
                Set(ref _listContract, value);
                _listContractViewSource.Source = value;
                _listContractViewSource.Filter += OnFilterList;
                _listContractViewSource.View.Refresh();
                //OnPropertyChanged(nameof(listContractView));
                //listMK.CollectionChanged += ListMK_CollectionChanged;

            }
        }

        public string selectContract { get; set; }

        private string _Filtr;
        public string Filtr 
        {
            get => _Filtr;
            set
            {
                if (_Filtr != value)
                {
                    _Filtr = value;
                    _listContractViewSource.View.Refresh();
                }
            }
        }

        CollectionViewSource _listContractViewSource = new CollectionViewSource();
        public ICollectionView listContractView => _listContractViewSource?.View;


        public ContractsUCVM()
        {
            RepositoryFP repositoryFP = new RepositoryFP();
            listContract = new ObservableCollection<string>(repositoryFP.GetListContract());
        }

        //--------------------------------------------------------------------------------
        // Фильтрация 
        //--------------------------------------------------------------------------------
        private void OnFilterList(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is string mk) || string.IsNullOrEmpty(Filtr)) return;

            if (!mk.ToLower().Contains(Filtr.ToLower()))
                E.Accepted = false;
        }



        #region Команды
        //--------------------------------------------------------------------------------
        // Команда Выбрано
        //--------------------------------------------------------------------------------
        public ICommand SelectContractCommand => new LambdaCommand(OnSelectContractCommandExecuted, CanSelectContractCommand);
        private bool CanSelectContractCommand(object p) => true;
        private void OnSelectContractCommandExecuted(object p)
        {

        }



        #endregion

    }
}
