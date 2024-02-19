using DocumentFormat.OpenXml.ExtendedProperties;
using ExportData.Models;
using ExportData.Repository;
using Irony;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExportData.Common
{
    internal class ParseModBP : ParseLineList
    {
        private readonly Product product;
        private readonly RepositoryMSSQL<Modules> repoModel = new RepositoryMSSQL<Modules>();


        public ParseModBP(Product prod)
        {
            product = prod;
        }


        public override void GetElements(string lines)
        {
            List<Modules> modules = new List<Modules>();
            Modules module;
            string numberFW = "";

            Regex regPO = new Regex(@"ПО\s");
            Regex regModbus = new Regex(@"modbus", RegexOptions.IgnoreCase);
            Regex regZIP = new Regex(@"ЗИП", RegexOptions.IgnoreCase);
            Regex regNum = new Regex(@"[\d]+");
            Regex regFW = new Regex(@"[\w./]+\b");

            Match resPO = regPO.Match(lines);
            if (resPO.Success)
            {
                Match resFW = regFW.Match(lines, resPO.Index + resPO.Length);
                numberFW = resFW.Value.Trim();
            }

            int index = 0;

            while (index < resPO.Index)
            {
                Match resNum = regNum.Match(lines, index);
                index = resNum.Index + resNum.Length;
                if (!resNum.Success || index >= resPO.Index)
                    break;

                module = new Modules();
                module.m_numberFW = numberFW;
                module.m_number = resNum.Value;

                modules.Add(module);
            }

            Match resZIP = regZIP.Match(lines);
            if(resZIP.Success)
            {
                index = resZIP.Index + resZIP.Length;
                while (true)
                {
                    Match resNum = regNum.Match(lines, index);
                    index = resNum.Index + resNum.Length;
                    if (!resNum.Success )
                        break;

                    module = new Modules();
                    module.m_numberFW = numberFW;
                    module.m_number = resNum.Value;
                    modules.Add(module);
                }

            }

            Match resModbus = regModbus.Match(lines);
            if(resModbus.Success )
            {
                Match resNum = regNum.Match(lines, resModbus.Index + resModbus.Length);
                module = new Modules();
                //module.m_modTypeId = 1;
                module.m_number = resNum.Value;
                modules.Add(module);
            }

            #region Удалить
            //StringList sl = new StringList(lines.Split(new char[] { '\n' }));
            //Modules mod = new Modules();
            //string numberFW = "";

            //foreach (var item in sl)
            //{
            //    StringList sl2 = new StringList(item.Split(new char[] { ';', ' ', '.' }));

            //    for (int i = 0; i < sl2.Count; i++)
            //    {
            //        if (sl2[i] == "ПО")
            //        {
            //            i++;
            //            numberFW = sl2[i];
            //            break;
            //        }

            //        if (!string.IsNullOrEmpty(sl2[i]) && sl2[i].All(it => it <= '9' && it >= '0'))
            //        {
            //            mod.m_number = sl2[i];
            //            modules.Add(mod);
            //            mod = new Modules();
            //        }
            //    }
            //}

            //foreach (var item in modules)
            //    item.m_numberFW = numberFW;
            #endregion

            foreach (var it in modules)
            {
                module = repoModel.Items.FirstOrDefault(p => p.m_number == it.m_number);
                if (module == null)
                {
                    it.m_modTypeId = 1;
                    repoModel.Add(it);
                    product.Modules.Add(it);
                }
                else
                {
                    module.m_numberFW = numberFW;
                    product.Modules.Add(module);
                }
            }
        }
    }
}
