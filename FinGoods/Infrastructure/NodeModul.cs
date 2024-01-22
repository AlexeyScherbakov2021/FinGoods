using FinGoods.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.Infrastructure
{
    internal class NodeModul : INode
    {
        public NodeModul(string name, Module modul) 
        { 
            Name = name; 
            Item = modul;
            isRoot = modul.idShipment != null;
        }
        public string Name { get; set; }
        public bool isRoot { get; set; }
        public object Item { get; set; }
    }
}
