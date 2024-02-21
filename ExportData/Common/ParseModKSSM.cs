using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
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
    internal class ParseModKSSM : ParseLineList
    {
        private readonly Product product;
        private readonly RepositoryMSSQL<Modules> repoModel = new RepositoryMSSQL<Modules>();

        public ParseModKSSM(Product prod)
        {
            product = prod;
        }

        public override void GetElements(string lines)
        {
            List<Modules> modules = new List<Modules>();
            Modules module;
            int index = 0;
            int indexZIP = lines.Length;
            string numberFW = "";

            Regex regZIP = new Regex(@"ЗИП", RegexOptions.IgnoreCase);
            Regex regNum = new Regex(@"\d{8,}");

            Match resZIP = regZIP.Match(lines);
            if (resZIP.Success)
                indexZIP = resZIP.Index;

            var resNumber = regNum.Matches(lines);

            if (resNumber.Count == 0)
                return;

            foreach (Match it in resNumber)
            {
                if (it.Index > indexZIP)
                    break;
                index = it.Index + it.Length;
            }

            numberFW = lines.Substring(index, indexZIP - index).Trim();

            foreach (Match item in resNumber)
            {
                module = repoModel.Items.FirstOrDefault(p => p.m_number == item.Value);
                if (module == null)
                {
                    module = new Modules();
                    module.m_number = item.Value;
                    //module.m_numberFW = numberFW;
                    //modNew.m_modTypeId = 71;
                    RepositoryMSSQL<ModuleType> repoTypeMod = new RepositoryMSSQL<ModuleType>();
                    module.ModuleType = repoTypeMod.Items.FirstOrDefault(it => it.id == 71);

                    repoModel.Add(module);
                    //product.Modules.Add(module);
                }

                module.m_numberFW = numberFW;
                if( string.IsNullOrEmpty(module.m_name))
                    module.m_name = module.ModuleType.mt_name;

                if (resZIP.Success && item.Index > resZIP.Index)
                    module.m_zip = true;

                if (!product.Modules.Contains(module))
                    product.Modules.Add(module);
            }
        }

    }
}
