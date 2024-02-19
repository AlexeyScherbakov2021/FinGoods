using ExportData.Models;
using ExportData.Repository;
using Irony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExportData.Common
{
    internal class ParseModBU : ParseLineList
    {
        private readonly Product product;
        private readonly RepositoryMSSQL<Modules> repoModel = new RepositoryMSSQL<Modules>();
        //private Step step = Step.None;

        public ParseModBU(Product prod)
        {
            product = prod;
        }

        public override void GetElements(string lines)
        {
            List<Modules> modules = new List<Modules>();
            Modules module;
            string numberFW = "";
            string shunt = "";
            string akb = "";
            int index = 0;

            Regex regShunt = new Regex(@"шунт", RegexOptions.IgnoreCase);
            Regex regAKB = new Regex(@"АКБ", RegexOptions.IgnoreCase);
            Regex regZIP = new Regex(@"ЗИП", RegexOptions.IgnoreCase);
            Regex regEndLine = new Regex(@".+\b");
            Regex regNum = new Regex(@"\d{8,}");


            int indexStartDop = int.MaxValue;

            Match resAKB = regAKB.Match(lines);
            if (resAKB.Success)
            {
                indexStartDop = resAKB.Index;
                akb = regEndLine.Match(lines, resAKB.Index + resAKB.Length).Value;
            }

            Match resShunt = regShunt.Match(lines);
            if(resShunt.Success)
            {
                indexStartDop = Math.Min(indexStartDop, resShunt.Index);
                shunt = regEndLine.Match(lines, resShunt.Index + resShunt.Length).Value;
            }

            int indexZIP = int.MaxValue;
            Match resZIP = regZIP.Match(lines);
            if (resZIP.Success)
            {
                indexStartDop = Math.Min(indexStartDop, resZIP.Index);
                indexZIP = resZIP.Index + resZIP.Length;
            }

            var resNumber = regNum.Matches(lines);

            foreach (Match it in resNumber)
            {
                if (it.Index > indexZIP)
                    break;
                index = it.Index + it.Length;
            }

            numberFW = lines.Substring( index, indexStartDop - index).Trim();

            foreach(Match item in resNumber)
            {
                module = repoModel.Items.FirstOrDefault(p => p.m_number == item.Value);
                if (module == null)
                {
                    Modules modNew = new Modules();
                    modNew.m_number = item.Value;
                    modNew.m_numberFW = numberFW;
                    modNew.m_modTypeId = 67;
                    repoModel.Add(modNew);
                    product.Modules.Add(modNew);
                    product.g_shunt = shunt;
                }
                else
                {
                    module.m_numberFW = numberFW;
                    product.Modules.Add(module);
                    product.g_shunt = shunt;
                }


            }

            //while (/*index < indexEndNum*/ true)
            //{
            //    Match resNum = regNum.Match(lines, index);
            //    if (!resNum.Success)
            //        break;

            //    module = new Modules();
            //    module.m_number = resNum.Value;
            //    module.m_numberFW = numberFW;
            //    //index = resNum.Index + resNum.Length;
            //}


            //StringList sl = new StringList(lines.Split(new char[] { '\n' }));
            //Modules mod = new Modules();
            //string numberFW = "";

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
            //            modules.Add(mod);
            //            mod = new Modules();
            //            step = Step.Number;
            //        }

            //        else if(step == Step.Number)
            //        {
            //            numberFW = item;
            //            step = Step.None;
            //            break;
            //        }

            //        if (sl2[i] == "АКБ")
            //        {
            //            break;
            //        }

            //        if (sl2[i] == "шунт")
            //        {
            //            break;
            //        }
            //    }
            //}

            //foreach (var item in modules)
            //    item.m_numberFW = numberFW;

            //foreach (var it in modules)
            //{
            //    module = repoModel.Items.FirstOrDefault(p => p.m_number == it.m_number);
            //    if (module == null)
            //    {
            //        it.m_modTypeId = 67;
            //        repoModel.Add(it);
            //        product.Modules.Add(it);
            //    }
            //    else
            //    {
            //        module.m_numberFW = numberFW;
            //        product.Modules.Add(module);
            //    }
            //}
        }
    }
}
