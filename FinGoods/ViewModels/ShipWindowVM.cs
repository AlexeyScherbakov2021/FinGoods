using FinGoods.Commands;
using FinGoods.Infrastructure;
using FinGoods.Models;
using FinGoods.Repository;
using FinGoods.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FinGoods.ViewModels
{

    internal class ShipWindowVM
    {

        public Shipment Ship { get; set; }
        public ObservableCollection<INode> listComposite { get; set; } = new ObservableCollection<INode>();

        public INode SelectedNode;

        public ShipWindowVM()
        {
        }

        public ShipWindowVM(Shipment co)
        {
            Ship = co;
            foreach(var item in Ship.SetterOuts)
            {
                NodeSetter adding = new NodeSetter(item.s_name, item);
                listComposite.Add(adding);

                foreach (var item2 in item.Products)
                {
                    NodeProd adding2 = new NodeProd(item2.g_name, item2);
                    adding.Children.Add(adding2);

                    foreach (var item3 in item2.Modules)
                    {
                        NodeModul adding3 = new NodeModul(item3.m_name, item3);
                        adding2.Children.Add(adding3);
                    }
                }
            }

            foreach (var item in Ship.Products)
            {
                NodeProd adding = new NodeProd(item.g_name, item);
                listComposite.Add(adding);

                foreach (var item2 in item.Modules)
                {
                    NodeModul adding2 = new NodeModul(item2.m_name, item2);
                    adding.Children.Add(adding2);
                }

            }

            foreach (var item in Ship.Modules)
            {
                NodeModul adding = new NodeModul(item.m_name, item);
                listComposite.Add(adding);
            }
        }

        #region команды

        //--------------------------------------------------------------------------------
        // Команда Добавить отгрузку
        //--------------------------------------------------------------------------------
        public ICommand AddSetCommand => new LambdaCommand(OnAddSetCommandExecuted, CanAddSetCommand);
        private bool CanAddSetCommand(object p) => true;
        private void OnAddSetCommandExecuted(object p)
        {
            AllSetterWindow win = new AllSetterWindow();
            AllSetterWindowVM vm = new AllSetterWindowVM(true);
            win.DataContext = vm;

            if (win.ShowDialog() == true)
            {
                NodeSetter node = new NodeSetter(vm.selectedSetter.s_name, vm.selectedSetter);
                listComposite.Add(node);
                Ship.SetterOuts.Add(vm.selectedSetter);
                RepositoryMSSQL<Shipment> repo = new RepositoryMSSQL<Shipment>();
                repo.Save();
            }
        }

        public ICommand AddProdCommand => new LambdaCommand(OnAddProdCommandExecuted, CanAddProdCommand);
        private bool CanAddProdCommand(object p) => true;
        private void OnAddProdCommandExecuted(object p)
        {
            AllProdWindow win = new AllProdWindow();
            AllProdWindowVM vm = new AllProdWindowVM(true);
            win.DataContext = vm;

            if(win.ShowDialog() == true)
            {
                NodeProd node = new NodeProd(vm.selectedProduct.g_name, vm.selectedProduct);
                listComposite.Add(node);
                Ship.Products.Add(vm.selectedProduct);
                RepositoryMSSQL<Shipment> repo = new RepositoryMSSQL<Shipment>();
                repo.Save();
            }
        }

        public ICommand AddModulCommand => new LambdaCommand(OnAddModulCommandExecuted, CanAddModulCommand);
        private bool CanAddModulCommand(object p) => true; // SelectedNode?.Item is Product;
        private void OnAddModulCommandExecuted(object p)
        {
            AllModulesWindow win = new AllModulesWindow();
            AllModulesWindowVM vm = new AllModulesWindowVM(true);
            win.DataContext = vm;

            if(win.ShowDialog() == true)
            {
                NodeModul node = new NodeModul(vm.selectedModule.m_name, vm.selectedModule);
                listComposite.Add(node);
                Ship.Modules.Add(vm.selectedModule);
                RepositoryMSSQL<Shipment> repo = new RepositoryMSSQL<Shipment>();
                repo.Save();
            }
        }


        public ICommand DeleteCommand => new LambdaCommand(OnDeleteCommandExecuted, CanDeleteCommand);
        private bool CanDeleteCommand(object p) => SelectedNode != null && SelectedNode.isRoot;
        private void OnDeleteCommandExecuted(object p)
        {
            if (MessageBox.Show($"Удалить из списка «{SelectedNode.Name}»", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (SelectedNode.Item is SetterOut so)
                    Ship.SetterOuts.Remove(so);
                if (SelectedNode.Item is Product pd)
                    Ship.Products.Remove(pd);
                if (SelectedNode.Item is Module md)
                    Ship.Modules.Remove(md);

                RepositoryMSSQL<Shipment> repo = new RepositoryMSSQL<Shipment>();
                repo.Save();

                listComposite.Remove(SelectedNode);
            }
        }


        public ICommand SelectItemCommand => new LambdaCommand(OnSelectItemCommandExecuted, CanSelectItemCommand);
        private bool CanSelectItemCommand(object p) => true;
        private void OnSelectItemCommandExecuted(object p)
        {
            if(p is RoutedPropertyChangedEventArgs<object> e)
            {
                SelectedNode = e.NewValue as INode;
            }
        }


        #endregion
    }
}
