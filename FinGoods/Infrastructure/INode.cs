using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.Infrastructure
{
    internal interface INode
    {
        string Name { get; set; }
        object Item { get; set; }
        bool isRoot { get; set; }
        //ObservableCollection<INode> Children { get; set; }
    }
}
