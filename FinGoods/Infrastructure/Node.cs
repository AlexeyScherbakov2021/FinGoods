using FinGoods.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.Infrastructure
{
    internal abstract class Node : Observable
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string NodeName => Name + (string.IsNullOrEmpty(Number) ? "" : " (№ " + Number + ")");
        public object Item { get; set; }
        public bool isRoot { get; set; }

        public Node Parent;
        public ObservableCollection<Node> Children { get; set; }

        private bool _IsExpanded = true;
        public bool IsExpanded { get => _IsExpanded; set { Set(ref _IsExpanded, value); } }

        private bool _IsSelected;
        public bool IsSelected { get => _IsSelected; set { Set(ref _IsSelected, value); } }
    }
}
