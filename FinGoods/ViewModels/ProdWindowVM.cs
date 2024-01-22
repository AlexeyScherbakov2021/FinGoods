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
    internal class ProdWindowVM
    {
        public Product product { get; set; }
        private readonly RepositoryMSSQL<ProductType> repoGT = new RepositoryMSSQL<ProductType>();
        public List<ProductType> listProdType { get; set; }
        public Module selectModul { get; set; }

        public ProdWindowVM()
        {
        }

        public ProdWindowVM(Product g)
        {
            listProdType = new List<ProductType>(repoGT.Items);
            product = g;

        }


        #region  Команды

        //--------------------------------------------------------------------------------
        // Команда 
        //--------------------------------------------------------------------------------
        public ICommand AddModulCommand => new LambdaCommand(OnAddModulCommandExecuted, CanAddModulCommand);
        private bool CanAddModulCommand(object p) => true;
        private void OnAddModulCommandExecuted(object p)
        {
            AllModulesWindow win = new AllModulesWindow();
            AllModulesWindowVM vm = new AllModulesWindowVM(true);
            win.DataContext = vm;
            if (win.ShowDialog() == true && vm.selectedModule != null)
            {
                product.Modules.Add(vm.selectedModule);
            }
        }

        //--------------------------------------------------------------------------------
        // Команда 
        //--------------------------------------------------------------------------------
        public ICommand DelModulCommand => new LambdaCommand(OnDelModulCommandExecuted, CanDelModulCommand);
        private bool CanDelModulCommand(object p) => selectModul != null;
        private void OnDelModulCommandExecuted(object p)
        {
            if (MessageBox.Show($"Удалить «{selectModul.m_name}»", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                product.Modules.Remove(selectModul);
            }

        }

        //--------------------------------------------------------------------------------
        // Команда Добавить модули
        //--------------------------------------------------------------------------------
        //public ICommand SelectModulesCommand => new LambdaCommand(OnSelectModulesCommandExecuted, CanSelectModulesCommand);
        //private bool CanSelectModulesCommand(object p) => true;
        //private void OnSelectModulesCommandExecuted(object p)
        //{
        //}

        #endregion

    }
}
