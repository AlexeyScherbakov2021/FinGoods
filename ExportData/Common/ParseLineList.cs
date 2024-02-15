using ExportData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportData.Common
{
    internal abstract class ParseLineList
    {
        public virtual void GetElements(string s) { }
        //public virtual void GetModules(string s) {}
    }
}
