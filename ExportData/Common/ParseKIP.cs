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
            //Regex regNumSmall = new Regex(@"[.|\d]+");
            //Regex regBI = new Regex(@"БИ\s№\s");
            Regex regBI = new Regex(@"БИ\s№\s[\d.]+");
            //Regex regUS = new Regex(@"УС\s");
            Regex regUS = new Regex(@"УС\s\D+\d+");
            //Regex regN = new Regex(@"№");

            var listKIP = regKIP.Matches(lines);
            var listNum = regNum.Matches(lines);

            //nextIndex = listKIP.Count > 1 ? listKIP[1].Index : int.MaxValue;
            //nameKIP = listKIP[0].Value;

            int currNextKIP = 0;
            nextIndex = -1;

            for(int i = 0; i < listNum.Count; i++) 
            //foreach (Match match in listNum)
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
                    NumBI = Regex.Replace(resBI.Value.Trim(), @"БИ\s№\s", "");
                
                Match resUSIK = regUS.Match(lines, listNum[i].Index, lenFind);
                if (resUSIK.Success)
                    NumUSIK = Regex.Replace(resUSIK.Value.Trim(), @"УС\s\D+", "");

                string number = listNum[i].Value;
                prod = repoProd.Items.FirstOrDefault(p => p.g_number == number);
                if(prod == null)
                {
                    prod = new Product();
                    RepositoryMSSQL<ProductType> repoTypeProd = new RepositoryMSSQL<ProductType>();
                    prod.ProductType = repoTypeProd.Items.FirstOrDefault(item => item.id == 24);

                    prod.g_number = number;
                    prod.g_avr = false;
                    prod.g_akb = false;
                    prod.g_cooler = false;
                    prod.g_skm = false;
                }

                prod.g_numberBI = NumBI;
                prod.g_numberUSIKP = NumUSIK;

                if (string.IsNullOrEmpty(prod.g_name))
                    prod.g_name = prod.ProductType.gt_name;

                if (!product.SetterOut.Product.Contains(prod))
                    product.SetterOut.Product.Add(prod);

            }


            //return;

            //// находим КИП
            //Match resKIP = regKIP.Match(lines);
            //int index = resKIP.Index + resKIP.Length;
            //nameKIP = resKIP.Value.Trim();

            //do {
            //    // поиск следующего КИП
            //    resKIP = regKIP.Match(lines, index);
            //    nextIndex = int.MaxValue;
            //    if (resKIP.Success)
            //        nextIndex = resKIP.Index;

            //    while ((index +  1) < nextIndex)
            //    {
            //        // поиск номера
            //        Match resNum = regNum.Match(lines, index);
            //        if (!resNum.Success || (resNum.Success && resNum.Index > nextIndex))
            //            break;

            //        prod = new Product();
            //        prod.g_name = nameKIP;
            //        prod.g_number = resNum.Value.Trim();
            //        index = resNum.Index + resNum.Length;

            //        // поиск БИ
            //        Match resBI = regBI.Match(lines, index);
            //        if (resBI.Success && resBI.Index < nextIndex)
            //        {
            //            resNum = regNumSmall.Match(lines, resBI.Index);
            //            prod.g_numberBI = resNum.Value.Trim();
            //            index = resNum.Index + resNum.Length;
            //        }

            //        // поиск УС ИКП
            //        Match resUS = regUS.Match(lines, index);
            //        if (resUS.Success && resUS.Index < nextIndex)
            //        {
            //            resNum = regNumSmall.Match(lines, resUS.Index);
            //            Match resN = regN.Match(lines, resUS.Index);
            //            if (resN.Index > resNum.Index)
            //            {
            //                prod.g_numberUSIKP = resNum.Value.Trim();
            //                index = resNum.Index + resNum.Length;
            //            }
            //        }

            //        listProd.Add(prod);
            //    }
            //    index = resKIP.Index + resKIP.Length;
            //    nameKIP = resKIP.Value.Trim();
            //} while (nextIndex < int.MaxValue);

            //foreach (var it in listProd)
            //{
            //    Product prAdd = repoProd.Items.FirstOrDefault(p => p.g_number == it.g_number);
            //    if (prAdd == null)
            //    {
            //        prAdd = new Product();
            //        it.g_avr = false;
            //        it.g_akb = false;
            //        it.g_cooler = false;
            //        it.g_skm = false;
            //    }

            //    prAdd.g_numberBI = it.g_numberBI;
            //    if(string.IsNullOrEmpty(prAdd.g_name))
            //        prAdd.g_name = it.g_name;

            //    //it.g_ProductTypeId = 18;
            //    if (prAdd.ProductType == null)
            //    {
            //        RepositoryMSSQL<ProductType> repoTypeProd = new RepositoryMSSQL<ProductType>();
            //        prAdd.ProductType = repoTypeProd.Items.FirstOrDefault(item => item.id == 24);
            //    }

            //    if (!product.SetterOut.Product.Contains(prAdd))
            //        product.SetterOut.Product.Add(prAdd);
            //}

        }
    }
}
