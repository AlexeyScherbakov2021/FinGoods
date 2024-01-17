using FinGoods.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.ViewModels
{
    internal class ShipWindowVM
    {
        public Shipment Ship { get; set; }

        public ShipWindowVM()
        {
        }

        public ShipWindowVM(Shipment co)
        {
            Ship = co;
        }
    }

   
}
