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
            Name = (modul.m_zip ? "[ЗИП] " : "") + name; 
            Item = modul;
            Number = Num;
            isRoot = modul.idShipment != null;
        }
    }
}
