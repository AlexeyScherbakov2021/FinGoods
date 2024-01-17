using FinGoods.Models;
using FinGoods.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGoods.ViewModels
{
    internal class ProdWindowVM
    {
        public Product product { get; set; }
        private readonly RepositoryMSSQL<ProductType> repoGT = new RepositoryMSSQL<ProductType>();
        public List<ProductType> listProdType { get; set; }

        public ProdWindowVM()
        {
        }

        public ProdWindowVM(Product g)
        {
            listProdType = new List<ProductType>(repoGT.Items);
            product = g;

        }
    }
}
