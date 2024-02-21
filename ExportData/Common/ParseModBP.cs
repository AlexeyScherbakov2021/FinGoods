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
            Regex regNum = new Regex(@"\d{8,}");
            Regex regFW = new Regex(@"[\w,./]+\b");


            Match resPO = regPO.Match(lines);
            if (resPO.Success)
            {
                Match resFW = regFW.Match(lines, resPO.Index + resPO.Length);
                numberFW = resFW.Value.Trim();
                numberFW = Regex.Replace(numberFW, @"[\t\n\r\ ]{1,}", " ");
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

            var resZIP = regZIP.Match(lines);

            var listNumbers = regNum.Matches(lines);

            foreach (Match item in listNumbers)
            {
                module = repoModel.Items.FirstOrDefault(p => p.m_number == item.Value);
                if (module == null)
                {
                    module = new Modules();
                    //module.m_modTypeId = 1;
                    RepositoryMSSQL<ModuleType> repoTypeMod = new RepositoryMSSQL<ModuleType>();
                    module.ModuleType = repoTypeMod.Items.FirstOrDefault(it => it.id == 1);
                    repoModel.Add(module);
                    //product.Modules.Add(module);
                }
                module.m_numberFW = numberFW;
                module.m_number = item.Value;
                if (string.IsNullOrEmpty(module.m_name))
                    module.m_name = module.ModuleType.mt_name;

                if(resZIP.Success && item.Index > resZIP.Index)
                    module.m_zip = true;

                if(!product.Modules.Contains(module))
                    product.Modules.Add(module);

            }



            //foreach (var it in modules)
            //{
            //    module = repoModel.Items.FirstOrDefault(p => p.m_number == it.m_number);
            //    if (module == null)
            //    {
            //        it.m_modTypeId = 1;
            //        repoModel.Add(it);
            //        product.Modules.Add(it);
            //    }
            //    else
            //    {
            //        module.m_numberFW = numberFW;
            //        if (string.IsNullOrEmpty(module.m_name))
            //            module.m_name = module.ModuleType.mt_name;
            //        product.Modules.Add(module);
            //    }
            //}
        }
    }
}
