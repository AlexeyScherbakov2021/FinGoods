using FinGoods.Commands;
using FinGoods.Models;
using FinGoods.Repository;
using FinGoods.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FinGoods.ViewModels
{
    internal class ModulWindowVM : Observable
    {
        public Module module { get; set; }
        private readonly RepositoryMSSQL<ModuleType> repoMdulType = new RepositoryMSSQL<ModuleType>();
        public List<ModuleType> listModuleType { get; set; }
        private ModuleType selectedModule;

        private bool _isOpenPopup;
        public bool isOpenPopup { get => _isOpenPopup; set { Set(ref _isOpenPopup, value); } }

        public ModulWindowVM()
        {
        }

        public ModulWindowVM(Module m)
        {
            module = m;
            listModuleType = new List<ModuleType>(repoMdulType.Items.Where(it => it.idParent == null));
        }

        #region Команды

        //--------------------------------------------------------------------------------
        // Команда открыть всплываюшее окно
        //--------------------------------------------------------------------------------
        public ICommand OpenPopupCommand => new LambdaCommand(OnOpenPopupCommandExecuted, CanOpenPopupCommand);
        private bool CanOpenPopupCommand(object p) => true;
        private void OnOpenPopupCommandExecuted(object p)
        {
            isOpenPopup = false;
            module.ModuleType = selectedModule;
        }

        //--------------------------------------------------------------------------------
        // Команда открыть всплываюшее окно
        //--------------------------------------------------------------------------------
        public ICommand ToggleCommand => new LambdaCommand(OnToggleCommandExecuted, CanToggleCommand);
        private bool CanToggleCommand(object p) => true;
        private void OnToggleCommandExecuted(object p)
        {
            if(p is RoutedPropertyChangedEventArgs<object> e)
            {
                selectedModule = (ModuleType)e.NewValue;
            }
        }
        #endregion
    }
}
