namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IApplicantTypeRepository : IGenericRepository<ApplicantType>
    {
        IEnumerable<SelectListItem> GetComboApplicantTypes();
        IQueryable GetAplicantTypes();
        Task<ApplicantType> GetByIdAplicantTypeAsync(int id);
        ApplicantTypeRequest GetApplicantTypeById(int id);
        IEnumerable<ApplicantTypeRequest> GetApplicantTypesByName(string name);
    }
}
