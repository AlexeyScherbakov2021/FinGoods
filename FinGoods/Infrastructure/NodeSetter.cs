using FinGoods.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.Infrastructure
{
    internal class NodeSetter : Node
    {
        public NodeSetter(string name, string Num, SetterOut setter) 
        { 
            Name = name; 
            Number = Num;
            Item = setter;
            isRoot = setter.idShipment != null;
        }
        //public string Name { get; set; }
        //public string Number { get; set; }
        //public object Item { get; set; }

        //public bool isRoot { get; set; }
        public ObservableCollection<Node> Children { get ; set; } = new ObservableCollection<Node>();
    }
}
