using FinGoods.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.Infrastructure
{
    internal class NodeModul : Node
    {
        public NodeModul(string name, string Num, Module modul) 
        { 
            Name = name; 
            Item = modul;
            Number = Num;
            isRoot = modul.idShipment != null;
        }
        //public string Name { get; set; }
        //public string Number { get; setb; }
        //string NodeName => Name + "";

        //public bool isRoot { get; set; }
        //public object Item { get; set; }
    }
}
