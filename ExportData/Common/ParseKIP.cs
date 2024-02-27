using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
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
    enum Step { None, Name, Number, BI };

    internal class ParseKIP : ParseLineList
    {
        private readonly Product product;
        private readonly RepositoryMSSQL<Product> repoProd = new RepositoryMSSQL<Product>();

        public ParseKIP(Product prod)
        {
            product = prod;
        }


        public override void GetElements(string lines)
        {
            string nameKIP = "";
            List<Product> listProd = new List<Product>();
            Product prod;
            int nextIndex;
            string NumBI = "";
            string NumUSIK = "";

            Regex regKIP = new Regex(@"НГК-КИП-[\w()-|\s]+");
            Regex regNum = new Regex(@"\d{8,}");
            Regex regBI = new Regex(@"БИ\s№\s*[\d.]+");
            Regex regUS = new Regex(@"УС\s\D+\d+");
            Regex regZIP = new Regex(@"ЗИП", RegexOptions.IgnoreCase);

            var listKIP = regKIP.Matches(lines);
            var listNum = regNum.Matches(lines);

            int currNextKIP = 0;
            nextIndex = -1;

            var resZip = regZIP.Match(lines);

            for(int i = 0; i < listNum.Count; i++) 
            {
                if (listNum[i].Index > nextIndex)
                {
                    nameKIP = listKIP[currNextKIP].Value.Trim();
                    currNextKIP++;
                    nextIndex = currNextKIP >= listKIP.Count 
                        ? int.MaxValue 
                        : listKIP[currNextKIP].Index;
                }

                int lenFind = i < listNum.Count - 1 
                    ? listNum[i + 1].Index - listNum[i].Index 
                    : lines.Length - listNum[i].Index;

                Match resBI = regBI.Match(lines, listNum[i].Index, lenFind);
                if(resBI.Success)
                    NumBI = Regex.Replace(resBI.Value.Trim(), @"БИ\s№\s*", "");
                
                Match resUSIK = regUS.Match(lines, listNum[i].Index, lenFind);
                if (resUSIK.Success)
                    NumUSIK = Regex.Replace(resUSIK.Value.Trim(), @"УС\s\D+", "");

                string number = listNum[i].Value;
                prod = repoProd.Items.FirstOrDefault(p => p.g_number == number);
                if(prod == null)
                {
                    prod = new Product();
                    RepositoryMSSQL<ProductType> repoTypeProd = new RepositoryMSSQL<ProductType>();
                    var resNameType = Regex.Match(nameKIP, @"НГК-КИП-[А-я)(-]+");

                    prod.ProductType = repoTypeProd.Items.FirstOrDefault(
                        item => item.gt_name.Contains(resNameType.Value));

                    if(prod.ProductType == null)
                        prod.ProductType = repoTypeProd.Items.FirstOrDefault(item => item.id == 24);

                    prod.g_number = number;
                    prod.g_avr = false;
                    prod.g_akb = false;
                    prod.g_cooler = false;
                    prod.g_skm = false;
                }

                if (resZip.Success && listNum[i].Index > resZip.Index)
                    prod.g_zip = true;

                prod.g_numberBI = NumBI;
                prod.g_numberUSIKP = NumUSIK;

                prod.g_name = nameKIP;
                if (string.IsNullOrEmpty(prod.g_name))
                    prod.g_name = prod.ProductType.gt_name;

                if (!product.SetterOut.Product.Contains(prod))
                    product.SetterOut.Product.Add(prod);

            }

        }
    }
}
