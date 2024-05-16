using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.Models
{
    internal class GrafikExcel
    {
        public int number { get; set; }
        public string buyer { get; set; }
        public string specif { get; set; }
        public string numFP { get; set; }
        public string order { get; set; }
        public string name {  get; set; }
        public string oprList { get; set; }
        public string cardOrder { get; set; }
        public string customer { get; set; }    
        public string objInstall { get; set; }

        public string AllInf => number.ToString() + " | " + 
            buyer + " | " +
            specif + " | " +
            numFP + " | " +
            order + " | " + 
            cardOrder + " | " +
            oprList + " | " +
            objInstall;
    }
}
