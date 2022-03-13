namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly DataContext _dataContext;

        public GenericRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<T> GetAll()
        {
            return _dataContext.Set<T>().AsNoTracking();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dataContext.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<T> FindByIdAsync(int id)
        {
            return await _dataContext.Set<T>().FindAsync(id);
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dataContext.Set<T>().AddAsync(entity);
            await SaveAllAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dataContext.Set<T>().Update(entity);
            await SaveAllAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dataContext.Set<T>().Remove(entity);
            await SaveAllAsync();
        }

        public async Task<bool> ExistAsync(int id)
        {
            return await _dataContext.Set<T>().AnyAsync(e => e.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public string Desencriptar(string passh)
        {
            string pass = string.Empty;
            Byte[] desenc = Convert.FromBase64String(passh);

            pass = System.Text.Encoding.Unicode.GetString(desenc);
            return pass;
        }

        //public async Task<T> Desencriptar(string passh)
        //{
        //    string pass = string.Empty;
        //    byte[] desenc = Convert.FromBase64String(passh);

        //    MD5 md5 = MD5.Create();
        //    TripleDES tripledes = TripleDES.Create();

        //    tripledes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(pass));
        //    tripledes.Mode = CipherMode.ECB;

        //    ICryptoTransform transform = tripledes.CreateDecryptor();
        //    byte[] result = transform.TransformFinalBlock(desenc, 0, desenc.Length);

        //    var result1 = Encoding.Unicode.GetString(result);
        //    return result1;
        //}
    }
}
