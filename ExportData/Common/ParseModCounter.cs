using ExportData.Models;
using ExportData.Repository;
using Irony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace ExportData.Common
{
    internal class ParseModCounter : ParseLineList
    {
        private readonly Product product;
        private readonly RepositoryMSSQL<Modules> repoModel = new RepositoryMSSQL<Modules>();
        private string name;

        public ParseModCounter(Product prod)
        {
            product = prod;
        }

        public override void GetElements(string lines)
        {
            List<Modules> modules = new List<Modules>();
            Modules module;

            Regex regNum = new Regex(@"\d{6,}");
            var resNumber = regNum.Matches(lines);

            if (resNumber.Count == 0)
                return;

            string nameModule = Regex.Replace(lines.Substring(0, resNumber[0].Index), @"[\t\n\r\ ]{1,}", " ");
            //Regex.Replace(numberFW, @"[\t\n\r\ ]{1,}", " ")

            foreach (Match item in resNumber)
            {
                module = repoModel.Items.FirstOrDefault(p => p.m_number == item.Value);
                if (module == null)
                {
                    module = new Modules();
                    module.m_number = item.Value;
                    //module.m_modTypeId = 92;
                    RepositoryMSSQL<ModuleType> repoTypeMod = new RepositoryMSSQL<ModuleType>();
                    module.ModuleType = repoTypeMod.Items.FirstOrDefault(it => it.id == 92);

                    repoModel.Add(module);
                    //product.Modules.Add(module);
                }

                if (string.IsNullOrEmpty(module.m_name))
                    module.m_name = nameModule;

                if(!product.Modules.Contains(module))
                    product.Modules.Add(module);
            }

            //foreach (var item in sl)
            //{
            //    StringList sl2 = new StringList(item.Split(new char[] { ';', ' ', '.' }));

            //    for (int i = 0; i < sl2.Count; i++)
            //    {
            //        if (string.IsNullOrEmpty(sl2[i]))
            //            continue;

            //        if (sl2[i].All(it => it <= '9' && it >= '0'))
            //        {
            //            mod.m_number = sl2[i];
            //            mod.m_name = name;
            //            modules.Add(mod);
            //            mod = new Modules();
            //        }
            //        else if (string.IsNullOrEmpty(name))
            //            name = item;
            //    }
            //}

            //foreach (var it in modules)
            //{
            //    mod = repoModel.Items.FirstOrDefault(p => p.m_number == it.m_number);
            //    if (mod == null)
            //    {
            //        it.m_modTypeId = 92;
            //        repoModel.Add(it);
            //        product.Modules.Add(it);
            //    }
            //    else
            //    {
            //        mod.m_name = name;
            //        product.Modules.Add(mod);
            //    }
            //}
        }

    }
}
