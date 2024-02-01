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
        private readonly RepositoryMSSQL<Module> repoModul = new RepositoryMSSQL<Module>();
        public List<ProductType> listProdType { get; set; }
        public Module selectModul { get; set; }

        //private int generated_number = 0;

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
                vm.selectedModule.Product = product;
                repoModul.Save();
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
                selectModul.Product = null;
                product.Modules.Remove(selectModul);
                repoModul.Save();
            }

        }

        //--------------------------------------------------------------------------------
        // Команда Генерировать номер
        //--------------------------------------------------------------------------------
        public ICommand GenNumCommand => new LambdaCommand(OnGenNumCommandExecuted, CanGenNumCommand);
        private bool CanGenNumCommand(object p) => product?.g_dateRegister != null;
        private void OnGenNumCommandExecuted(object p)
        {

            RepositoryMSSQL<SerialNumber> repoGen = new RepositoryMSSQL<SerialNumber>();
            var serialLine = repoGen.Items.Where(it => it.kind_number == KindNumber.Production
                    && it.year_number == product.g_dateRegister.Value.Year).FirstOrDefault();

            if (serialLine == null)
            {
                serialLine = new SerialNumber()
                {
                    gen_number = 0,
                    kind_number = KindNumber.Production,
                    year_number = product.g_dateRegister.Value.Year
                };

                repoGen.Add(serialLine);
            }

            if (product.g_generatedNumber == 0)
            {
                serialLine.gen_number++;
                product.g_generatedNumber = serialLine.gen_number;
            }

            product.g_number =
                //product.ProductType.gt_number.ToString()
                //+ product.g_dateRegister?.ToString("yy")
                //+ "xxxxx"
                //+ generated_number.ToString();

            CreateSerialNumber(product, "xx-xxx");
        }


        public static string CreateSerialNumber(Product product, string NumberKZ)
        {
            return
                product.ProductType.gt_number.ToString()
                + product.g_dateRegister?.ToString("yy")
                + NumberKZ.Substring(0, 2) + NumberKZ.Substring(3,3)
                + product.g_generatedNumber.ToString();
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
