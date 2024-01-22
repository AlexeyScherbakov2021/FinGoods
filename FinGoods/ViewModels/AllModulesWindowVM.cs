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

namespace FinGoods.ViewModels
{
    internal class AllModulesWindowVM
    {
        public ObservableCollection<Module> listModules { get; set; } 
        public Module selectedModule { get; set; }
        public Visibility isVisible { get; set; }  = Visibility.Collapsed;

        RepositoryMSSQL<Module> repo = new RepositoryMSSQL<Module>();


        public AllModulesWindowVM()
        {
            listModules = new ObservableCollection<Module>(repo.Items);
        }

        public AllModulesWindowVM(bool prod) 
        {
            listModules = new ObservableCollection<Module>(repo.Items
                .Where(it => it.idProduct == null && it.idShipment == null));
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
            Module newMod = new Module();
            ModulWindow win = new ModulWindow();
            ModulWindowVM vm = new ModulWindowVM(newMod);
            win.DataContext = vm;
            if (win.ShowDialog() == true)
            {
                //RepositoryMSSQL<Module> repo = new RepositoryMSSQL<Module>();
                repo.Add(newMod, true);
                listModules.Add(newMod);
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
                //RepositoryMSSQL<Product> repoProd = new RepositoryMSSQL<Product>();
                repo.Save();
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
        // Команда Выбрать модули
        //--------------------------------------------------------------------------------
        //public ICommand SelectModuleCommand => new LambdaCommand(OnDelModulCommandExecuted, CanDelModulCommand);
        //private bool CanDelModulCommand(object p) => selectedModule != null
        //    && selectedModule.idProduct == null
        //    && selectedModule.idShipment == null;
        //private void OnDelModulCommandExecuted(object p)
        //{

        //}

        #endregion

    }
}
