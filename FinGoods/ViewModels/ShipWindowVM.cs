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
        public string SearchNumber { get; set; }

        public Shipment Ship { get; set; }
        public ObservableCollection<Node> listComposite { get; set; } = new ObservableCollection<Node>();

        //public ObservableCollection<string> listContract { get; set; }

        public Node SelectedNode;

        public ShipWindowVM()
        {
        }

        public ShipWindowVM(Shipment co, string num) : this(co)
        {
            SearchNumber = num;
            OnSearchCommandExecuted(null);
        }


        private void addNodeSetter(SetterOut setter)
        {
            NodeSetter adding = new NodeSetter(setter.s_name, "", setter);
            listComposite.Add(adding);

            foreach (var item2 in setter.Products)
            {
                NodeProd adding2 = new NodeProd(item2.g_name, item2.g_number, item2);
                adding2.Parent = adding;
                adding.Children.Add(adding2);

                foreach (var item3 in item2.Modules)
                {
                    NodeModul adding3 = new NodeModul(item3.m_name, item3.m_number, item3);
                    adding3.Parent = adding2;
                    adding2.Children.Add(adding3);
                }
            }
            SelectedNode = adding;
        }

        private void addNodeProd(Product prod)
        {
            NodeProd adding = new NodeProd(prod.g_name, prod.g_number, prod);
            listComposite.Add(adding);

            foreach (var item2 in prod.Modules)
            {
                NodeModul adding2 = new NodeModul(item2.m_name, item2.m_number, item2);
                adding2.Parent = adding;
                adding.Children.Add(adding2);
            }
        }



        public ShipWindowVM(Shipment co)
        {
            Ship = co;
            foreach (var item in Ship.SetterOuts)
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
                NodeModul adding = new NodeModul(item.m_name, item.m_number, item);
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
                foreach (var item in vm.selectedSetter.Products)
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

            if (win.ShowDialog() == true)
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

            if (win.ShowDialog() == true)
            {
                Ship.Modules.Add(vm.selectedModule);
                RepositoryMSSQL<Shipment> repo = new RepositoryMSSQL<Shipment>();
                repo.Save();

                NodeModul node = new NodeModul(vm.selectedModule.m_name, vm.selectedModule.m_number, vm.selectedModule);
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
            if (p is RoutedPropertyChangedEventArgs<object> e)
            {
                SelectedNode = e.NewValue as Node;
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Список заказов из ФП
        //--------------------------------------------------------------------------------
        public ICommand SelectContractCommand => new LambdaCommand(OnSelectContractCommandExecuted, CanSelectContractCommand);
        private bool CanSelectContractCommand(object p) => true;
        private void OnSelectContractCommandExecuted(object p)
        {
            var order = (p as MouseButtonEventArgs).Source as OrderFP;
            if (order == null)
                throw new Exception("Ошибка выбора заказа");

            Ship.c_number = order.doc_name;
            Ship.c_customer = order.cli_name;
            Ship.c_schet = order.PactNo;

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

        public ICommand DblClickCommand => new LambdaCommand(OnDblClickCommandExecuted, CanDblClickCommand);
        private bool CanDblClickCommand(object p) => true;
        private void OnDblClickCommandExecuted(object p)
        {
            if (SelectedNode == null)
                return;

            if (SelectedNode.Item is Module mod)
            {
                ModulWindow win = new ModulWindow();
                ModulWindowVM vm = new ModulWindowVM(mod);
                win.DataContext = vm;
                win.ShowDialog();
            }
            else if (SelectedNode.Item is Product prod)
            {
                ProdWindow win = new ProdWindow();
                ProdWindowVM vm = new ProdWindowVM(prod);
                win.DataContext = vm;
                win.ShowDialog();

            }
            else if (SelectedNode.Item is SetterOut setOut)
            {
                SetterWindow win = new SetterWindow();
                SetterWindowVM vm = new SetterWindowVM(setOut);
                win.DataContext = vm;
                win.ShowDialog();
            }

            //switch(SelectedNode.Item )
            //{
            //    case SetterOut:
            //        break;

            //}
        }

        //--------------------------------------------------------------------------------
        // Команда Поиск по номеру
        //--------------------------------------------------------------------------------
        public ICommand SearchCommand => new LambdaCommand(OnSearchCommandExecuted, CanSearchCommand);
        private bool CanSearchCommand(object p) => true;
        private void OnSearchCommandExecuted(object p)
        {
            //foreach(var setter in Ship.SetterOuts)
            //{
            //    foreach(var prod in setter.Products)
            //    {
            //        if(prod.g_number.Contains(SearchNumber))
            //        {

            //        }

            //        foreach(var mod in prod.Modules)
            //        {
            //            if(mod.m_number.Contains(SearchNumber))
            //            {

            //            }
            //        }
            //    }
            //}

            Node res = SearchItemTree(listComposite, SearchNumber);
            if (res != null)
            {
                res.IsSelected = true;
                SelectedNode = res;

                while(res.Parent != null)
                {
                    res.Parent.IsExpanded = true;
                    res = res.Parent;
                }
               
            }
        }


        #endregion

        private Node SearchItemTree(IEnumerable<Node> nodes, string Text)
        {
            if (nodes == null || string.IsNullOrEmpty(Text))
                return null;

            foreach (var node in nodes)
            {
                if (node.Number.Contains(Text))
                    return node;

                Node res = SearchItemTree(node.Children, Text);
                if (res != null)
                    return res;
            }

            return null;
        }



    }
}
