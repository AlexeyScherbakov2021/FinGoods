using FinGoods.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.Infrastructure
{
    internal class NodeSetter : INode
    {
        public NodeSetter(string name, SetterOut setter) 
        { 
            Name = name; 
            Item = setter;
            isRoot = setter.id_Shipment != null;
        }
        public string Name { get; set; }
        public object Item { get; set; }

        public bool isRoot { get; set; }
        public ObservableCollection<INode> Children { get ; set; } = new ObservableCollection<INode>();
    }
}
