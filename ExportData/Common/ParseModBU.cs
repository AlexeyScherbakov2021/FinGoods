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

            //Regex regShunt = new Regex(@"шунт", RegexOptions.IgnoreCase);
            Regex regShunt = new Regex(@"шунт\s\w+\s\w+\b", RegexOptions.IgnoreCase);
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
                shunt = Regex.Replace(resShunt.Value, @"шунт\s", "").Trim();
                //shunt = regEndLine.Match(lines, resShunt.Index + resShunt.Length).Value;
            }

            int indexZIP = int.MaxValue;
            Match resZIP = regZIP.Match(lines);
            if (resZIP.Success)
            {
                indexStartDop = Math.Min(indexStartDop, resZIP.Index);
                indexZIP = resZIP.Index + resZIP.Length;
            }

            var resNumber = regNum.Matches(lines);

            if (resNumber.Count == 0)
                return;

            foreach (Match it in resNumber)
            {
                if (it.Index > indexZIP)
                    break;
                index = it.Index + it.Length;
            }

            numberFW = lines.Substring( index, indexStartDop - index).Trim();
            numberFW = Regex.Replace(numberFW, @"[\t\n\r\ ]{1,}", " ");

            foreach (Match item in resNumber)
            {
                module = repoModel.Items.FirstOrDefault(p => p.m_number == item.Value);
                if (module == null)
                {
                    module = new Modules();
                    module.m_number = item.Value;
                    //modNew.m_numberFW = numberFW;
                    //module.m_modTypeId = 67;
                    RepositoryMSSQL<ModuleType> repoTypeMod = new RepositoryMSSQL<ModuleType>();
                    module.ModuleType = repoTypeMod.Items.FirstOrDefault(it => it.id == 67); 
                    repoModel.Add(module);
                    //product.Modules.Add(modNew);
                    //product.g_shunt = shunt;
                }

                module.m_numberFW = numberFW;
                product.g_shunt = shunt;
                if (string.IsNullOrEmpty(module.m_name))
                    module.m_name = module.ModuleType.mt_name;

                if(!product.Modules.Contains(module))
                    product.Modules.Add(module);
            }
        }
    }
}
