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
        private readonly RepositoryMSSQL<CardOrder> repoCO = new RepositoryMSSQL<CardOrder>();
        public ObservableCollection<CardOrder> listCardOrder { get; set; }

        public CardOrder SelectCard { get; set; }
        //public Goods SelectGoods { get; set; }
        //public Module SelectModule { get; set; }


        public MainWindowViewModel()
        {
            listCardOrder = new ObservableCollection<CardOrder>(repoCO.Items);
        }

        #region команды

        //--------------------------------------------------------------------------------
        // Команда Добавить карту заказа
        //--------------------------------------------------------------------------------
        public ICommand AddCardOrderCommand => new LambdaCommand(OnAddCardOrderCommandExecuted, CanAddCardOrderCommand);
        private bool CanAddCardOrderCommand(object p) => true;
        private void OnAddCardOrderCommandExecuted(object p)
        {
            CardOrder newCard = new CardOrder();

            CardsOrderWindow win = new CardsOrderWindow();
            CardsOrderWindowVM vm = new CardsOrderWindowVM(newCard);
            win.DataContext = vm;

            if(win.ShowDialog() == true)
            {
                listCardOrder.Add(newCard);
                repoCO.Add(newCard);
                repoCO.Save();
            }

        }

        //--------------------------------------------------------------------------------
        // Команда Редактировать карту заказа
        //--------------------------------------------------------------------------------
        public ICommand EditCardOrderCommand => new LambdaCommand(OnEditCardOrderCommandExecuted, CanEditCardOrderCommand);
        private bool CanEditCardOrderCommand(object p) => SelectCard != null;
        private void OnEditCardOrderCommandExecuted(object p)
        {
            CardsOrderWindow win = new CardsOrderWindow();
            CardsOrderWindowVM vm = new CardsOrderWindowVM(SelectCard);
            win.DataContext = vm;

            if(win.ShowDialog() == true)
            {
                repoCO.Save();
            }

        }

        //--------------------------------------------------------------------------------
        // Команда Удалить карту заказа
        //--------------------------------------------------------------------------------
        public ICommand DelCardOrderCommand => new LambdaCommand(OnDelCardOrderCommandExecuted, CanDelCardOrderCommand);
        private bool CanDelCardOrderCommand(object p) => SelectCard != null;
        private void OnDelCardOrderCommandExecuted(object p)
        {

            if(MessageBox.Show($"Удалить «{SelectCard.c_number}»","Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                repoCO.Delete(SelectCard.id);
                repoCO.Save();
                listCardOrder.Remove(SelectCard);
            }

        }

        //--------------------------------------------------------------------------------
        // Команда Открыть окно детализации
        //--------------------------------------------------------------------------------
        public ICommand OpenDetailCommand => new LambdaCommand(OnOpenDetailCommandExecuted, CanOpenDetailCommand);
        private bool CanOpenDetailCommand(object p) => SelectCard != null;
        private void OnOpenDetailCommandExecuted(object p)
        {
            DetailWindow win = new DetailWindow();
            DetailWindowVM vm = new DetailWindowVM(SelectCard.Goods, SelectCard.c_number);
            win.DataContext = vm;
            win.ShowDialog();
        }




        #endregion

    }
}
