using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAcess.Data;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
                _db = db;
        }
        public void Update(Product product)
        {
            // handle if delete imageurl when update the prod
            var objFromDB = _db.Products.FirstOrDefault(x => x.Id == product.Id);
            if(objFromDB != null)
            {
                objFromDB.Title = product.Title;
                objFromDB.ISBN = product.ISBN;
                objFromDB.Price = product.Price;
                objFromDB.Price50 = product.Price50;
                objFromDB.Price100 = product.Price100;
                objFromDB.ListPrice = product.ListPrice;
                objFromDB.Description = product.Description;
                objFromDB.CategoryId = product.CategoryId;
                objFromDB.Author = product.Author;
                if(product.ImagUrl != null)
                {
                    objFromDB.ImagUrl = product.ImagUrl;
                }
            }
        }
    }
}
