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
    internal class NodeProd : Node
    {
        public NodeProd(string name, string Num, Product prod) 
        { 
            Name = name; 
            Number = Num;
            Item = prod;
            isRoot = prod.idShipment != null;
            Children = new ObservableCollection<Node>();
        }
    }
}
