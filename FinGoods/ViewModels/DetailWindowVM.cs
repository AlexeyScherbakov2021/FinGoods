using FinGoods.Commands;
using FinGoods.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FinGoods.ViewModels
{
    internal class DetailWindowVM : Observable
    {
        public string Title { get; set; }
        public ObservableCollection<Goods> listGoods { get; set; }
        public Goods SelectedGoods { get; set; }
        public Module SelectedModule { get; set; }

        public DetailWindowVM()
        {
        }

        public DetailWindowVM(ObservableCollection<Goods> list, string name)
        {
            listGoods = list;
            Title = name;
        }


        //--------------------------------------------------------------------------------
        // Команда 
        //--------------------------------------------------------------------------------
        public ICommand SelectItemCommand => new LambdaCommand(OnSelectItemCommandExecuted, CanSelectItemCommand);
        private bool CanSelectItemCommand(object p) => true;
        private void OnSelectItemCommandExecuted(object p)
        {
            if (p is RoutedPropertyChangedEventArgs<object> e)
            {
                //SelectCard = null;
                SelectedGoods = null;
                SelectedModule = null;

                //if (e.NewValue is CardOrder co)
                //{
                //    SelectCard = co;
                //}
                if (e.NewValue is Goods g)
                {
                    SelectedGoods = g;
                }
                if (e.NewValue is Module m)
                {
                    SelectedModule = m;
                }
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Добавить оборудование
        //--------------------------------------------------------------------------------
        public ICommand AddGoodsCommand => new LambdaCommand(OnAddGoodsCommandExecuted, CanAddGoodsCommand);
        private bool CanAddGoodsCommand(object p) => true;
        private void OnAddGoodsCommandExecuted(object p)
        {
            //Goods newGoods = new Goods();

            //newGoods.CardOrder = SelectCard;
            //GoodsWindow win = new GoodsWindow();
            //GoodsWindowVM vm = new GoodsWindowVM(newGoods);
            //win.DataContext = vm;
            //if (win.ShowDialog() == true)
            //{
            //    SelectCard.Goods.Add(newGoods);
            //    repoCO.Save();
            //}

        }


        //--------------------------------------------------------------------------------
        // Команда Редактировать оборудование
        //--------------------------------------------------------------------------------
        public ICommand EditGoodsCommand => new LambdaCommand(OnEditGoodsCommandExecuted, CanEditGoodsCommand);
        private bool CanEditGoodsCommand(object p) => SelectedGoods != null;
        private void OnEditGoodsCommandExecuted(object p)
        {

            //GoodsWindow win = new GoodsWindow();
            //GoodsWindowVM vm = new GoodsWindowVM(SelectGoods);
            //win.DataContext = vm;
            //if (win.ShowDialog() == true)
            //{
            //    repoCO.Save();
            //}
        }

        //--------------------------------------------------------------------------------
        // Команда Удалить оборудование
        //--------------------------------------------------------------------------------
        public ICommand DelGoodsCommand => new LambdaCommand(OnDelGoodsCommandExecuted, CanDelGoodsCommand);
        private bool CanDelGoodsCommand(object p) => SelectedGoods != null;
        private void OnDelGoodsCommandExecuted(object p)
        {
            //if (MessageBox.Show($"Удалить «{SelectGoods.g_name}»", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //{
            //    RepositoryMSSQL<Goods> repoGoods = new RepositoryMSSQL<Goods>();

            //    //repoGoods.Remove()
            //    repoGoods.Delete(SelectGoods, true);
            //    //repoGoods.Save();
            //    //SelectGoods = null;

            //    //OnPropertyChanged(nameof( listCardOrder));

            //    repoCO.Save();
            //    SelectGoods.CardOrder.Goods.Remove(SelectGoods);
            //    repoCO.Save();
            //}
        }


        //--------------------------------------------------------------------------------
        // Команда Добавить модуль
        //--------------------------------------------------------------------------------
        public ICommand AddModulCommand => new LambdaCommand(OnAddModulCommandExecuted, CanAddModulCommand);
        private bool CanAddModulCommand(object p) => SelectedGoods != null;
        private void OnAddModulCommandExecuted(object p)
        {
            //Module newMod = new Module();
            //newMod.Goods = SelectGoods;

            //ModulWindow win = new ModulWindow();
            //ModulWindowVM vm = new ModulWindowVM(newMod);
            //win.DataContext = vm;
            //if (win.ShowDialog() == true)
            //{
            //    SelectGoods.Modules.Add(newMod);
            //    repoCO.Save();
            //}
        }

        //--------------------------------------------------------------------------------
        // Команда Редактировать модуль
        //--------------------------------------------------------------------------------
        public ICommand EditModuleCommand => new LambdaCommand(OnEditModuleCommandExecuted, CanEditModuleCommand);
        private bool CanEditModuleCommand(object p) => SelectedModule != null;
        private void OnEditModuleCommandExecuted(object p)
        {
            //ModulWindow win = new ModulWindow();
            //ModulWindowVM vm = new ModulWindowVM(SelectModule);
            //win.DataContext = vm;
            //if (win.ShowDialog() == true)
            //{
            //    repoCO.Save();
            //}

        }

        //--------------------------------------------------------------------------------
        // Команда Удалить модуль
        //--------------------------------------------------------------------------------
        public ICommand DelModulCommand => new LambdaCommand(OnDelModulCommandExecuted, CanDelModulCommand);
        private bool CanDelModulCommand(object p) => SelectedModule != null;
        private void OnDelModulCommandExecuted(object p)
        {
            //if (MessageBox.Show($"Удалить «{SelectModule.m_name}»", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //{
            //    RepositoryMSSQL<Module> repoMod = new RepositoryMSSQL<Module>();

            //    repoMod.Delete(SelectModule.id, true);
            //    //repoMod.Save();
            //    SelectModule.Goods.Modules.Remove(SelectModule);
            //}
        }


    }
}
