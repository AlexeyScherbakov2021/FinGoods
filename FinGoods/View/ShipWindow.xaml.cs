using FinGoods.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для CardsOrderWindow.xaml
    /// </summary>
    public partial class ShipWindow : Window
    {
        public ShipWindow()
        {
            InitializeComponent();
        }

        private void ContractsUC_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShipWindowVM vm = (DataContext as ShipWindowVM);
            ContractsUCVM vmContr = contract.DataContext as ContractsUCVM;
            e.Source = vmContr.selectContract;
            vm.SelectContractCommand.Execute(e);
            popup.IsOpen = false;
        }

        private void TreeView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            ShipWindowVM vm = (DataContext as ShipWindowVM);
            ContractsUCVM vmContr = contract.DataContext as ContractsUCVM;
            e.Source = vmContr.selectContract;
            vm.DblClickCommand.Execute(e);
        }

        private void contractKZ_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
            ShipWindowVM vm = (DataContext as ShipWindowVM);
            GrafikUCVM vmContr = contractKZ.DataContext as GrafikUCVM;
            e.Source = vmContr.selectGrafik;
            vm.SelectGrafikCommand.Execute(e);
            popupKZ.IsOpen = false;
        }
    }
}
