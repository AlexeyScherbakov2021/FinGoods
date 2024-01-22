using FinGoods.Commands;
using FinGoods.Infrastructure;
using FinGoods.Models;
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
                Ship.SetterOuts.Add(vm.selectedSetter);
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
                Ship.Products.Add(vm.selectedProduct);
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
            }
        }


        public ICommand DeleteCommand => new LambdaCommand(OnDeleteCommandExecuted, CanDeleteCommand);
        private bool CanDeleteCommand(object p) => SelectedNode != null && SelectedNode.isRoot;
        private void OnDeleteCommandExecuted(object p)
        {
            listComposite.Remove(SelectedNode);
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
