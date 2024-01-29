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
using System.Windows.Input;

namespace FinGoods.ViewModels
{
    internal class AllSetterWindowVM
    {
        public ObservableCollection<SetterOut> listSetter { get; set; }
        public SetterOut selectedSetter { get; set; }
        public Visibility isVisible { get; set; } = Visibility.Collapsed;

        RepositoryMSSQL<SetterOut> repo = new RepositoryMSSQL<SetterOut>();

        public AllSetterWindowVM()
        {
            listSetter = new ObservableCollection<SetterOut>(repo.Items);
        }

        public AllSetterWindowVM(bool s)
        {
            //ModelBase BaseFG = ModelBase.GetBase();
            //listSetter = new ObservableCollection<SetterOut>(BaseFG.Set<SetterOut>().Where(it => it.idShipment == null));
            //listSetter = BaseFG.Set<SetterOut>().Local;

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
                //RepositoryMSSQL<Module> repo = new RepositoryMSSQL<Module>();
                repo.Add(newSet, true);
                listSetter.Add(newSet);
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


        #endregion

    }
}
