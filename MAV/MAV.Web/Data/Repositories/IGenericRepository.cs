namespace MAV.Web.Data.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IGenericRepository<T> where T : class
    {
        //SELECT *
        IQueryable<T> GetAll();

        //SELECT CON WHERE
        Task<T> GetByIdAsync(int id);

        //SELECT CON WHERE
        Task<T> FindByIdAsync(int id);

        //METODOS CRUD
        Task<T> CreateAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistAsync(int id);

        string Desencriptar(string passh);
    }
}
