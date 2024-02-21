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
            if (!resBSZ.Success)
                return;


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

            foreach (var it in listProd)
            {
                Product prAdd = repoProd.Items.FirstOrDefault(p => p.g_number == it.g_number);
                if (prAdd == null)
                {
                    prAdd = new Product();
                    prAdd.g_avr = false;
                    prAdd.g_akb = false;
                    prAdd.g_cooler = false;
                    prAdd.g_skm = false;

                }
                prAdd.g_number = it.g_number;
                prAdd.g_numberBI = it.g_numberBI;

                if (prAdd.ProductType == null)
                {
                    RepositoryMSSQL<ProductType> repoTypeProd = new RepositoryMSSQL<ProductType>();
                    var resNameType = Regex.Match(it.g_name, @"БСЗ[А-я]*");
                    prAdd.ProductType = repoTypeProd.Items.FirstOrDefault(
                        item => item.gt_name.Contains(resNameType.Value));

                    if(prAdd.ProductType == null)
                        prAdd.ProductType = repoTypeProd.Items.FirstOrDefault(item => item.id == 1);
                }

                if (string.IsNullOrEmpty(nameBSZ))
                    prAdd.g_name = it.g_name;

                if (string.IsNullOrEmpty(prAdd.g_name))
                    prAdd.g_name = prAdd.ProductType.gt_name;


                if (!product.SetterOut.Product.Contains(prAdd))
                    product.SetterOut.Product.Add(prAdd);
            }

        }


    }
}
