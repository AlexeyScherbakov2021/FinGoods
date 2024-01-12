using FinGoods.Models;
using FinGoods.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.ViewModels
{
    internal class GoodsWindowVM
    {
        public Goods goods { get; set; }
        private readonly RepositoryMSSQL<GoodsType> repoGT = new RepositoryMSSQL<GoodsType>();
        public List<GoodsType> listGoodsType { get; set; }


        public GoodsWindowVM()
        {
        }

        public GoodsWindowVM(Goods g)
        {
            listGoodsType = new List<GoodsType>(repoGT.Items);
            goods = g;

        }
    }
}
