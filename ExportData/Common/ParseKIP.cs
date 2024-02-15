using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
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
            Step step = Step.None;
            string nameKIP = "";
            RepositoryMSSQL<ProductType> repoType = new RepositoryMSSQL<ProductType>();
            StringList sl = new StringList(lines.Split(new char[] { '\n' }));

            List<Product> listProd = new List<Product>();
            Product prod = new Product();

            foreach (var item in sl)
            {
                switch (step)
                {
                    case Step.None:
                        step = Step.Name;
                        prod.g_name = item;
                        nameKIP = item;
                        break;

                    case Step.Name:
                    case Step.Number:
                        StringList slNumber = new StringList(item.Split(new char[] { ' ' }));
                        if (slNumber[0] == "№")
                        {
                            prod.g_number = slNumber[1];
                            step = Step.Number;

                            if (slNumber.Count > 2)
                            {
                                if (slNumber[2] == "БИ")
                                    prod.g_numberBI = slNumber[4];
                            }

                            if (slNumber.Count > 5)
                                throw new Exception("Кип имеет более 2 полей.");

                            listProd.Add(prod);
                            step = Step.Name;
                            prod = new Product();
                            prod.g_name = nameKIP;
                            break;

                        }
                        else
                        {
                            step = Step.Name;
                            prod.g_name = item;
                            break;
                        }

                }
            }

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
