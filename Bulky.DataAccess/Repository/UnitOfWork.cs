using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAcess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository CategoryRepo{ get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            CategoryRepo = new CategoryRepository(_db);
        }
        

        public void Save()
        {
            
            _db.SaveChanges();
        }
    }
}
