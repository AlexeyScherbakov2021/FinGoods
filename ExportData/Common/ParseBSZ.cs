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
    //enum Step { None, Name, Number, BI };

    internal class ParseBSZ : ParseLineList
    {
        private readonly Product product;
        private readonly RepositoryMSSQL<Product> repoProd = new RepositoryMSSQL<Product>();

        public ParseBSZ(Product prod)
        {
            product = prod;
        }

        public override void GetElements(string lines)
        {
            string nameBSZ = "";
            List<Product> listProd = new List<Product>();
            Product prod;
            int nextIndex;

            Regex regKIP = new Regex(@"БСЗ[\w()-|\s]+");
            Regex regNum = new Regex(@"[.|\d]+");

            // находим БСЗ
            Match resBSZ = regKIP.Match(lines);
            int index = resBSZ.Index + resBSZ.Length;
            nameBSZ = resBSZ.Value.Trim();

            do
            {
                // поиск следующего БСЗ
                resBSZ = regKIP.Match(lines, index);
                nextIndex = int.MaxValue;
                if (resBSZ.Success)
                    nextIndex = resBSZ.Index;

                while (index < nextIndex)
                {
                    // поиск номера
                    Match resNum = regNum.Match(lines, index);
                    if (!resNum.Success || (resNum.Success && resNum.Index > nextIndex))
                        break;

                    prod = new Product();
                    prod.g_name = nameBSZ;
                    prod.g_number = resNum.Value.Trim();
                    if(listProd.Count > 0)
                    {
                        // добавление промежуточных номеров из диапазона
                        int defis = lines.IndexOf("-", index, resNum.Index - index);
                        if(defis >= 0)
                        {
                            long startNum = long.Parse(listProd.Last().g_number) + 1;
                            long endNum = long.Parse(prod.g_number);
                            if (endNum - startNum > 20)
                                throw new Exception($"Добавлени БСЗ. Номеров более 20. {startNum} - {endNum}");
                            while (startNum < endNum)
                            {
                                Product prod2 = new Product();
                                prod2.g_name = nameBSZ;
                                prod2.g_number = startNum.ToString();
                                listProd.Add(prod2);
                                startNum++;
                            }
                        }
                    }

                    index = resNum.Index + resNum.Length;
                    listProd.Add(prod);
                }
                index = resBSZ.Index + resBSZ.Length;
                nameBSZ = resBSZ.Value.Trim();
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
            //            nameBSZ = item;
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
            //                prod.g_name = nameBSZ;
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
                    it.g_ProductTypeId = 1;
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
