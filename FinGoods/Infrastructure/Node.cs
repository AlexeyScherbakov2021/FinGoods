using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.Infrastructure
{
    internal abstract class Node
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string NodeName => Name + (string.IsNullOrEmpty(Number) ? "" : " (№ " + Number + ")");
        public object Item { get; set; }
        public bool isRoot { get; set; }
        //ObservableCollection<INode> Children { get; set; }
    }
}
