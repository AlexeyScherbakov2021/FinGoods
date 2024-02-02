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

        //public ObservableCollection<string> listContract { get; set; }

        public INode SelectedNode;

        public ShipWindowVM()
        {
        }


        private void addNodeSetter(SetterOut setter)
        {
            NodeSetter adding = new NodeSetter(setter.s_name, setter);
            listComposite.Add(adding);

            foreach (var item2 in setter.Products)
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

        private void addNodeProd(Product prod)
        {
            NodeProd adding = new NodeProd(prod.g_name, prod);
            listComposite.Add(adding);

            foreach (var item2 in prod.Modules)
            {
                NodeModul adding2 = new NodeModul(item2.m_name, item2);
                adding.Children.Add(adding2);
            }
        }



        public ShipWindowVM(Shipment co)
        {
            Ship = co;
            foreach(var item in Ship.SetterOuts)
            {
                addNodeSetter(item);

                //NodeSetter adding = new NodeSetter(item.s_name, item);
                //listComposite.Add(adding);

                //foreach (var item2 in item.Products)
                //{
                //    NodeProd adding2 = new NodeProd(item2.g_name, item2);
                //    adding.Children.Add(adding2);

                //    foreach (var item3 in item2.Modules)
                //    {
                //        NodeModul adding3 = new NodeModul(item3.m_name, item3);
                //        adding2.Children.Add(adding3);
                //    }
                //}
            }

            foreach (var item in Ship.Products)
            {
                addNodeProd(item);
                //NodeProd adding = new NodeProd(item.g_name, item);
                //listComposite.Add(adding);

                //foreach (var item2 in item.Modules)
                //{
                //    NodeModul adding2 = new NodeModul(item2.m_name, item2);
                //    adding.Children.Add(adding2);
                //}

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
                foreach(var item in vm.selectedSetter.Products)
                {
                    item.g_number = ProdWindowVM.CreateSerialNumber(item, Ship.c_number.Substring(0, 6));
                }

                Ship.SetterOuts.Add(vm.selectedSetter);
                RepositoryMSSQL<Shipment> repo = new RepositoryMSSQL<Shipment>();
                repo.Save();

                addNodeSetter(vm.selectedSetter);
                //NodeSetter node = new NodeSetter(vm.selectedSetter.s_name, vm.selectedSetter);
                //listComposite.Add(node);
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
                //NodeProd node = new NodeProd(vm.selectedProduct.g_name, vm.selectedProduct);
                //listComposite.Add(node);
                vm.selectedProduct.g_number = 
                    ProdWindowVM.CreateSerialNumber(vm.selectedProduct, Ship.c_number.Substring(0, 6));
                Ship.Products.Add(vm.selectedProduct);
                RepositoryMSSQL<Shipment> repo = new RepositoryMSSQL<Shipment>();
                repo.Save();

                addNodeProd(vm.selectedProduct);

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
                Ship.Modules.Add(vm.selectedModule);
                RepositoryMSSQL<Shipment> repo = new RepositoryMSSQL<Shipment>();
                repo.Save();

                NodeModul node = new NodeModul(vm.selectedModule.m_name, vm.selectedModule);
                listComposite.Add(node);
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

        //--------------------------------------------------------------------------------
        // Команда Список заказов из ФП
        //--------------------------------------------------------------------------------
        public ICommand SelectContractCommand => new LambdaCommand(OnSelectContractCommandExecuted, CanSelectContractCommand);
        private bool CanSelectContractCommand(object p) => true;
        private void OnSelectContractCommandExecuted(object p)
        {
            Ship.c_number = (p as MouseButtonEventArgs).Source.ToString();

            foreach (var item in Ship.Products)
            {
                item.g_number =
                        ProdWindowVM.CreateSerialNumber(item, Ship.c_number.Substring(0, 6));
            }

            foreach (var ship in Ship.SetterOuts)
            {
                foreach (var item in ship.Products)
                {
                    item.g_number =
                            ProdWindowVM.CreateSerialNumber(item, Ship.c_number.Substring(0, 6));
                }
            }
        }



        #endregion
    }
}
