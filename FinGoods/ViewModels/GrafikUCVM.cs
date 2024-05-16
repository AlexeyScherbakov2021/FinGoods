using FinGoods.Models;
using FinGoods.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FinGoods.ViewModels
{
    internal class GrafikUCVM : Observable
    {
        public static List<GrafikExcel> listGrafikStat;

        private ObservableCollection<GrafikExcel> _listGrafik;
        public ObservableCollection<GrafikExcel> listGrafik
        {
            get => _listGrafik;
            set
            {
                Set(ref _listGrafik, value);
                _listGrafikViewSource.Source = value;
                _listGrafikViewSource.Filter += OnFilterList;
                _listGrafikViewSource.View?.Refresh();
            }
        }

        public GrafikExcel selectGrafik { get; set; }

        private string _Filtr;
        public string Filtr
        {
            get => _Filtr;
            set
            {
                if (_Filtr != value)
                {
                    _Filtr = value;
                    _listGrafikViewSource.View.Refresh();
                }
            }
        }

        CollectionViewSource _listGrafikViewSource = new CollectionViewSource();
        public ICollectionView listGrafikView => _listGrafikViewSource?.View;


        public GrafikUCVM()
        {
            //RepositoryExcel repo = new RepositoryExcel();
            if (GrafikUCVM.listGrafikStat == null)
            {
                listGrafikStat = new List<GrafikExcel>();
                listGrafik = new ObservableCollection<GrafikExcel>();
                LoadAsync();
                //repo.GetListAsync(listGrafik);
            }
            else
                listGrafik = new ObservableCollection<GrafikExcel>(GrafikUCVM.listGrafikStat);
        }


        private async void LoadAsync()
        {
            await Task.Run(() =>
            {
                LoadFromExcel();
            });
            listGrafikStat = listGrafik.ToList();

        }


        private void LoadFromExcel()
        {
            RepositoryExcel repo = new RepositoryExcel();
            try
            {
                repo.GetListFromExcel(listGrafik);
            }
            catch
            {
                
            }
        }



        //--------------------------------------------------------------------------------
        // Фильтрация 
        //--------------------------------------------------------------------------------
        private void OnFilterList(object Sender, FilterEventArgs E)
        {
            if (!(E.Item is GrafikExcel mk) || string.IsNullOrEmpty(Filtr)) return;

            if (!mk.cardOrder.ToLower().Contains(Filtr.ToLower()) 
                && !mk.specif.ToLower().Contains(Filtr.ToLower()))
                E.Accepted = false;
        }

    }
}
