using ClosedXML.Excel;
using FinGoods.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.Repository
{
    internal class RepositoryExcel
    {
        public RepositoryExcel()
        {
        }

        //public ObservableCollection<GrafikExcel> GetList()
        //{
        //    ObservableCollection<GrafikExcel> listResult = new ObservableCollection<GrafikExcel>();
        //    int row = 5;

        //    using (XLWorkbook wb = new XLWorkbook(@"s:\Общие документы НГК\Карта заказов\График поставок к плану поставок март.xlsm"))
        //    {
        //        var ws = wb.Worksheets.Worksheet(1);
        //        while (!ws.Cell(row, 1).Value.IsBlank)
        //        {
        //            GrafikExcel item = new GrafikExcel();
        //            item.number = int.Parse(ws.Cell(row, 1).Value.ToString());
        //            item.buyer = ws.Cell(row, 3).Value.ToString();
        //            item.specif = ws.Cell(row, 4).Value.ToString();
        //            item.numFP = ws.Cell(row, 5).Value.ToString();
        //            item.order = ws.Cell(row, 6).Value.ToString();
        //            item.name = ws.Cell(row, 7).Value.ToString();
        //            item.oprList = ws.Cell(row, 20).Value.ToString();
        //            item.cardOrder = ws.Cell(row, 21).Value.ToString();
        //            item.customer = ws.Cell(row, 31).Value.ToString();
        //            listResult.Add(item);
        //            row++;
        //        }
        //    }
        //    return listResult;
        //}
        //public async Task<IEnumerable<GrafikExcel>> GetListFromExcel()
        //{
        //    await Task.Run(() => 
        //    {
        //        LoadFromExcel();
        //    });
        //}

        //private IEnumerable<GrafikExcel> LoadFromExcel()
        //{
        //    List< GrafikExcel> listResult = new List<GrafikExcel> ();

        //    int row = 5;
        //    using (XLWorkbook wb = new XLWorkbook(@"s:\Общие документы НГК\Карта заказов\График поставок к плану поставок март.xlsm"))
        //    {
        //        var ws = wb.Worksheets.Worksheet(1);
        //        while (!ws.Cell(row, 1).Value.IsBlank)
        //        {
        //            GrafikExcel item = new GrafikExcel();
        //            item.number = int.Parse(ws.Cell(row, 1).Value.ToString());
        //            item.buyer = ws.Cell(row, 3).Value.ToString();
        //            item.specif = ws.Cell(row, 4).Value.ToString();
        //            item.numFP = ws.Cell(row, 5).Value.ToString();
        //            item.order = ws.Cell(row, 6).Value.ToString();
        //            item.name = ws.Cell(row, 7).Value.ToString();
        //            item.oprList = ws.Cell(row, 20).Value.ToString();
        //            item.cardOrder = ws.Cell(row, 21).Value.ToString();
        //            item.customer = ws.Cell(row, 31).Value.ToString();
        //            //yield return item;
        //            listResult.Add(item);
        //            row++;
        //        }
        //    }
        //    return listResult;
        //}





        public void GetListFromExcel(ObservableCollection<GrafikExcel> listResult)
        {
            //ObservableCollection<GrafikExcel> listResult = new ObservableCollection<GrafikExcel>();
            int row = 5;

            using (XLWorkbook wb = new XLWorkbook(@"\\NGK-As-02\Department\Общие документы НГК\Карта заказов\График поставок к плану поставок март.xlsm"))
            {
                var ws = wb.Worksheets.Worksheet(1);
                while (!ws.Cell(row, 1).Value.IsBlank)
                {
                    GrafikExcel item = new GrafikExcel();
                    item.number = int.Parse(ws.Cell(row, 1).Value.ToString());
                    item.buyer = ws.Cell(row, 3).Value.ToString();
                    item.specif = ws.Cell(row, 4).Value.ToString();
                    item.numFP = ws.Cell(row, 5).Value.ToString();
                    item.order = ws.Cell(row, 6).Value.ToString();
                    item.name = ws.Cell(row, 7).Value.ToString();
                    item.oprList = ws.Cell(row, 20).Value.ToString();
                    item.cardOrder = ws.Cell(row, 21).Value.ToString();
                    item.customer = ws.Cell(row, 31).Value.ToString();
                    item.objInstall = ws.Cell(row, 29).Value.ToString();

                    App.Current.Dispatcher.BeginInvoke((Action) delegate ()
                    {
                        listResult.Add(item);
                    });
                    row++;
                }
            }
        }

        //public async void GetListAsync(ObservableCollection<GrafikExcel> listResult)
        //{
        //    //ObservableCollection<GrafikExcel> listResult = new ObservableCollection<GrafikExcel>();
        //    //listResult.Clear();
        //    await Task.Run( () => LoadFromExcel(listResult));
        //}

        private void LoadFromExcel(ObservableCollection<GrafikExcel> listResult)
        {
            int row = 5;
            using (XLWorkbook wb = new XLWorkbook(@"s:\Общие документы НГК\Карта заказов\График поставок к плану поставок март.xlsm"))
            {
                var ws = wb.Worksheets.Worksheet(1);
                while (!ws.Cell(row, 1).Value.IsBlank)
                {
                    GrafikExcel item = new GrafikExcel();
                    item.number = int.Parse(ws.Cell(row, 1).Value.ToString());
                    item.buyer = ws.Cell(row, 3).Value.ToString();
                    item.specif = ws.Cell(row, 4).Value.ToString();
                    item.numFP = ws.Cell(row, 5).Value.ToString();
                    item.order = ws.Cell(row, 6).Value.ToString();
                    item.name = ws.Cell(row, 7).Value.ToString();
                    item.oprList = ws.Cell(row, 20).Value.ToString();
                    item.cardOrder = ws.Cell(row, 21).Value.ToString();
                    item.customer = ws.Cell(row, 31).Value.ToString();
                    App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        listResult.Add(item);
                    });
                    row++;
                }
            }

        }

    }
}
