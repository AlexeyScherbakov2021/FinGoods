using FinGoods.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.ViewModels
{
    internal class CardsOrderWindowVM
    {
        public CardOrder cardOrder { get; set; }

        public CardsOrderWindowVM()
        {
        }

        public CardsOrderWindowVM(CardOrder co)
        {
            cardOrder = co;
        }
    }

   
}
