using FinGoods.Models;
using FinGoods.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.ViewModels
{
    internal class ModulWindowVM
    {
        public Module module { get; set; }
        private readonly RepositoryMSSQL<ModuleType> repoMdulType = new RepositoryMSSQL<ModuleType>();
        public List<ModuleType> listModuleType { get; set; }

        public ModulWindowVM()
        {
        }

        public ModulWindowVM(Module m)
        {
            module = m;
            listModuleType = new List<ModuleType>(repoMdulType.Items);
        }
    }
}
