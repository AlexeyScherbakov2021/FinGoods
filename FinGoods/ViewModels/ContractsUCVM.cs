using FinGoods.Commands;
using FinGoods.Models;
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
    public enum KindContract : int { Contract, Invoice };

    internal class ContractsUCVM : Observable
    {
        public KindContract kind { get;set; }
        public string kind2 { get; set; }


        private ObservableCollection<OrderFP> _listContract;
        public ObservableCollection<OrderFP> listContract
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

        public OrderFP selectContract { get; set; }

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

            if(kind == KindContract.Invoice)
            {
                listContract = new ObservableCollection<OrderFP>(repositoryFP.GetListInvoice());
            }
            else
                listContract = new ObservableCollection<OrderFP>(repositoryFP.GetListContract());
        }

        //--------------------------------------------------------------------------------
        // Фильтрация 
        //--------------------------------------------------------------------------------
        private void OnFilterList(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is OrderFP mk) || string.IsNullOrEmpty(Filtr)) return;

            if (!mk.doc_name.ToLower().Contains(Filtr.ToLower()))
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
