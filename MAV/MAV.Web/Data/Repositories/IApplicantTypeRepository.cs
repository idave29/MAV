namespace MAV.Web.Data.Repositories
{
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    public interface IApplicantTypeRepository : IGenericRepository<ApplicantType>
    {
        IEnumerable<SelectListItem> GetComboApplicantTypes();
    }
}
