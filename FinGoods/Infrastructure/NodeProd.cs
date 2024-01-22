using FinGoods.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinGoods.Infrastructure
{
    internal class NodeProd : INode
    {
        public NodeProd(string name, Product prod) 
        { 
            Name = name; 
            Item = prod;
            isRoot = prod.idShipment != null;
        }
        public string Name { get; set; }
        public object Item { get; set; }
        public bool isRoot { get; set; }
        public ObservableCollection<INode> Children { get; set; } = new ObservableCollection<INode>();

    }
}
