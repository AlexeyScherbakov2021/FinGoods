using ExportData.Models;
using ExportData.Repository;
using Irony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            StringList sl = new StringList(lines.Split(new char[] { '\n' }));
            Modules mod = new Modules();

            foreach (var item in sl)
            {
                StringList sl2 = new StringList(item.Split(new char[] { ';', ' ', '.' }));

                for (int i = 0; i < sl2.Count; i++)
                {
                    if (string.IsNullOrEmpty(sl2[i]))
                        continue;

                    if (sl2[i].All(it => it <= '9' && it >= '0'))
                    {
                        mod.m_number = sl2[i];
                        mod.m_name = name;
                        modules.Add(mod);
                        mod = new Modules();
                    }
                    else if (string.IsNullOrEmpty(name))
                        name = item;
                }
            }

            foreach (var it in modules)
            {
                mod = repoModel.Items.FirstOrDefault(p => p.m_number == it.m_number);
                if (mod == null)
                {
                    it.m_modTypeId = 92;
                    repoModel.Add(it);
                    product.Modules.Add(it);
                }
                else
                {
                    mod.m_name = name;
                    product.Modules.Add(mod);
                }
            }
        }

    }
}
