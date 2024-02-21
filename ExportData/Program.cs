using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using ExportData.Common;
using ExportData.Models;
using ExportData.Repository;
using Irony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace ExportData
{
    internal class Program
    {
        enum KindProd {KIP, BSZ, KSSM};

        static void Main(string[] args)
        {
            //string line = "9623103764; 96221203888 (26.4)    " +
            //    "G3 31.08.2022\r\nшунт 100А 75мВ\r\nАКБ 12.2022";

            ////Regex reg = new Regex(@"НГК-КИП-[^\s]+\s[^\s]+\s");
            //Regex reg = new Regex(@"НГК-КИП-[\w()-|\s]+№");
            //Regex redNum = new Regex(@"\s№\s[.|\d]+");
            //Regex redBI = new Regex(@"\d{8,}");
            //Regex redSh = new Regex(@".\s");

            //var res = reg.Matches(line);
            //var res1 = redNum.Matches(line);
            //var res2 = redBI.Matches(line);
            //var res4 = redSh.Match(line, res2.Index + res2.Length);

            //var res3 = redUS.Matches(line);


            RepositoryMSSQL<Product> repoProd = new RepositoryMSSQL<Product>();
            RepositoryMSSQL<SetterOut> repoSet = new RepositoryMSSQL<SetterOut>();
            RepositoryMSSQL<Shipment> repoShip = new RepositoryMSSQL<Shipment>();
            RepositoryMSSQL<Modules> repoModel = new RepositoryMSSQL<Modules>();

            using (XLWorkbook wb = new XLWorkbook("d:\\Work\\C#\\2022\\FinGoods\\Doc\\Журнал 2023.xlsx"))
            {
                int row = 3;
                var sheet = wb.Worksheets.Worksheet(1);

                for (; ; row++)
                {
                    Console.WriteLine($"Обработка строки {row}.");

                    string Number = sheet.Cell(row, 2).GetString().Trim();

                    if (string.IsNullOrEmpty(Number))
                        break;

                    // обработка изделия------------------------------------

                    Product prod = repoProd.Items.FirstOrDefault(it => it.g_number == Number);
                    if (prod == null)
                    {
                        prod = new Product() { g_number = Number, g_ProductTypeId = 7 };
                        repoProd.Add(prod);
                    }

                    prod.g_name = sheet.Cell(row, 1).GetString().Trim();
                    prod.g_numberBox = sheet.Cell(row, 3).GetString().Trim();

                    string plus = sheet.Cell(row, 4).GetString().Trim();
                    prod.g_avr = (plus == "+");

                    plus = sheet.Cell(row, 5).GetString().Trim();
                    prod.g_akb = (plus == "+");

                    plus = sheet.Cell(row, 6).GetString().Trim();
                    prod.g_cooler = (plus == "+");

                    plus = sheet.Cell(row, 8).GetString().Trim();
                    prod.g_skm = (plus == "+");

                    prod.g_questList = sheet.Cell(row, 16).GetString().Trim();

                    // Обработка набора------------------------------------

                    if (prod.SetterOut == null)
                    {
                        prod.SetterOut = new SetterOut();
                        prod.SetterOut.s_name = "_" + prod.g_name;
                        prod.SetterOut.Product.Add(prod);
                        //repoSet.Add(prod.SetterOut);
                    }

                    prod.SetterOut.s_orderNum = sheet.Cell(row, 9).GetString().Trim();//.Replace("\n", " ");
                    prod.SetterOut.s_orderNum = Regex.Replace(prod.SetterOut.s_orderNum, @"[\t\n\r\ ]{1,}", " ");

                    // обработка отгрузки

                    if(prod.SetterOut.Shipment == null)
                    {
                        prod.SetterOut.Shipment = new Shipment();
                        prod.SetterOut.Shipment.SetterOut.Add(prod.SetterOut);
                        //repoShip.Add(prod.SetterOut.Shipment);
                    }

                    var resCardOrder = Regex.Match(sheet.Cell(row, 9).GetString(), @"\b[\w.-]+$");

                    //prod.SetterOut.Shipment.c_schet = sheet.Cell(row, 9).GetString().Trim().Replace("\n", " ");
                    prod.SetterOut.Shipment.c_schet = Regex.Replace(sheet.Cell(row, 9).GetString(), @"\s+\b[\w.-]+$", "");
                    prod.SetterOut.Shipment.c_cardOrder = resCardOrder.Value;
                    prod.SetterOut.Shipment.c_customer = sheet.Cell(row, 15).GetString().Trim();
                    prod.SetterOut.Shipment.c_objectInstall = sheet.Cell(row, 18).GetString().Trim();

                    // добавление в набор доп.изделий

                    // КИПы

                    string s = sheet.Cell(row, 12).GetString();
                    ParseKIP parseKIP = new ParseKIP(prod);
                    parseKIP.GetElements(s);

                    // БСЗ
                    s = sheet.Cell(row, 13).GetString();
                    ParseBSZ parseBSZ = new ParseBSZ(prod);
                    parseBSZ.GetElements(s);

                    // добавление модулей
                    // БП
                    s = sheet.Cell(row, 11).GetString();
                    ParseModBP parseModBP = new ParseModBP(prod);
                    parseModBP.GetElements(s);

                    // БУ
                    s = sheet.Cell(row, 10).GetString();
                    ParseModBU parseModBU = new ParseModBU(prod);
                    parseModBU.GetElements(s);

                    //  КССМ
                    s = sheet.Cell(row, 14).GetString();
                    ParseModKSSM parseModKSSM = new ParseModKSSM(prod);
                    parseModKSSM.GetElements(s);

                    // счетчики
                    s = sheet.Cell(row, 7).GetString();
                    ParseModCounter parseModCounter = new ParseModCounter(prod);
                    parseModCounter.GetElements(s);

                    repoProd.Save();
                }
            }
        }

    }
}
