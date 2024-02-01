using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.ViewModels
{
    internal class QueryNameWindowVM : Observable
    {
        private string _Title;
        public string Title { get => _Title; set { Set(ref _Title, value); } }

        private string _Name;
        public string Name { get => _Name; set { Set(ref _Name, value); } }

        private int? _Number;
        public int? Number { get => _Number; set { Set(ref _Number, value); } }
    }
}
