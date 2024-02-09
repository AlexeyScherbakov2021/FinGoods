using FinGoods.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FinGoods.View
{
    /// <summary>
    /// Логика взаимодействия для SetterWindow.xaml
    /// </summary>
    public partial class SetterWindow : Window
    {
        public SetterWindow()
        {
            InitializeComponent();
        }

        private void contract_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SetterWindowVM vm = (DataContext as SetterWindowVM);
            ContractsUCVM vmContr = contract.DataContext as ContractsUCVM;
            e.Source = vmContr.selectContract;
            vm.SelectContractCommand.Execute(e);
            popup.IsOpen = false;

        }
    }
}
