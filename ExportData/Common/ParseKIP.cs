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

            Regex regKIP = new Regex(@"НГК-КИП-[\w()-|\s]+");
            Regex regNum = new Regex(@"[.|\d]+");
            Regex regBI = new Regex(@"БИ\s№\s");
            Regex regUS = new Regex(@"УС\s");
            Regex regN = new Regex(@"№");

            // находим КИП
            Match resKIP = regKIP.Match(lines);
            int index = resKIP.Index + resKIP.Length;
            nameKIP = resKIP.Value.Trim();

            do {
                // поиск следующего КИП
                resKIP = regKIP.Match(lines, index);
                nextIndex = int.MaxValue;
                if (resKIP.Success)
                    nextIndex = resKIP.Index;

                while ((index +  1) < nextIndex)
                {
                    // поиск номера
                    Match resNum = regNum.Match(lines, index);
                    if (!resNum.Success || (resNum.Success && resNum.Index > nextIndex))
                        break;

                    prod = new Product();
                    prod.g_name = nameKIP;
                    prod.g_number = resNum.Value.Trim();
                    index = resNum.Index + resNum.Length;

                    // поиск БИ
                    Match resBI = regBI.Match(lines, index);
                    if (resBI.Success && resBI.Index < nextIndex)
                    {
                        resNum = regNum.Match(lines, resBI.Index);
                        prod.g_numberBI = resNum.Value.Trim();
                        index = resNum.Index + resNum.Length;
                    }

                    // поиск УС ИКП
                    Match resUS = regUS.Match(lines, index);
                    if (resUS.Success && resUS.Index < nextIndex)
                    {
                        resNum = regNum.Match(lines, resUS.Index);
                        Match resN = regN.Match(lines, resUS.Index);
                        if (resN.Index > resNum.Index)
                        {
                            prod.g_numberUSIKP = resNum.Value.Trim();
                            index = resNum.Index + resNum.Length;
                        }
                    }

                    listProd.Add(prod);
                }
                index = resKIP.Index + resKIP.Length;
                nameKIP = resKIP.Value.Trim();
            } while (nextIndex < int.MaxValue);

            #region Удалить
            //Step step = Step.None;
            //RepositoryMSSQL<ProductType> repoType = new RepositoryMSSQL<ProductType>();
            //StringList sl = new StringList(lines.Split(new char[] { '\n' }));


            //foreach (var item in sl)
            //{
            //    switch (step)
            //    {
            //        case Step.None:
            //            step = Step.Name;
            //            prod.g_name = item;
            //            nameKIP = item;
            //            break;

            //        case Step.Name:
            //        case Step.Number:
            //            StringList slNumber = new StringList(item.Split(new char[] { ' ' }));
            //            if (slNumber[0] == "№")
            //            {
            //                prod.g_number = slNumber[1];
            //                step = Step.Number;

            //                if (slNumber.Count > 2)
            //                {
            //                    if (slNumber[2] == "БИ")
            //                        prod.g_numberBI = slNumber[4];
            //                }

            //                if (slNumber.Count > 5)
            //                    throw new Exception("Кип имеет более 2 полей.");

            //                listProd.Add(prod);
            //                step = Step.Name;
            //                prod = new Product();
            //                prod.g_name = nameKIP;
            //                break;

            //            }
            //            else
            //            {
            //                step = Step.Name;
            //                prod.g_name = item;
            //                break;
            //            }

            //    }
            //}
            #endregion

            foreach (var it in listProd)
            {
                Product prAdd = repoProd.Items.FirstOrDefault(p => p.g_number == it.g_number);
                if (prAdd != null)
                {
                    prAdd.g_numberBI = it.g_numberBI;
                    prAdd.g_name = it.g_name;
                }
                else
                {
                    it.g_ProductTypeId = 18;
                    it.g_avr = false;
                    it.g_akb = false;
                    it.g_cooler = false;
                    it.g_skm = false;
                    product.SetterOut.Product.Add(it);
                }
            }

        }
    }
}
