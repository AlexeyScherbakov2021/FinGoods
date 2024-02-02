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

        //--------------------------------------------------------------------------------
        // Команда Генерировать номер
        //--------------------------------------------------------------------------------
        public ICommand GenNumCommand => new LambdaCommand(OnGenNumCommandExecuted, CanGenNumCommand);
        private bool CanGenNumCommand(object p) => module?.m_dateCreate != null;
        private void OnGenNumCommandExecuted(object p)
        {

            RepositoryMSSQL<SerialNumber> repoGen = new RepositoryMSSQL<SerialNumber>();
            var serialLine = repoGen.Items.Where(it => it.kind_number == KindNumber.Module
                    && it.year_number == module.m_dateCreate.Value.Year).FirstOrDefault();

            if (serialLine == null)
            {
                serialLine = new SerialNumber() { 
                    gen_number = 0, kind_number = KindNumber.Module, year_number = module.m_dateCreate.Value.Year
                };

                repoGen.Add(serialLine);
            }

            if(module.m_generatedNumber == 0)
            {
                serialLine.gen_number++;
                module.m_generatedNumber = serialLine.gen_number;
            }

            module.m_number =
                module.ModuleType.mt_number.ToString()
                + module.m_dateCreate?.ToString("yy")
                + module.m_dateCreate.Value.Month.ToString()
                + module.m_generatedNumber.ToString("00000");
        }

        #endregion
    }
}
