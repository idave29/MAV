namespace MAV.Web.Data.Repositories
{
    using MAV.Common.Models;
    using MAV.Web.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMaterialRepository : IGenericRepository<Material>
    {
        IEnumerable<SelectListItem> GetComboMaterial();
        //Task<Material> GetByIdMaterialsAsync(int id);
        IQueryable GetMaterialsWithTypeWithStatusAndOwner();
        IQueryable GetMaterialsWithTypeWithStatusAndOwnerandImage();
        IQueryable GetMaterials();
        IQueryable GetMaterialsWithOwner();
        Task<Material> GetByIdWithMaterialTypeOwnerStatusAsync(int id);

        Task<Material> GetBySerialNumWithMaterialTypeOwnerStatusAsync(string serialnum);
        //MaterialRequest GetMaterialWithTypeWithStatusAndOwnerById(int id);
        //MaterialRequest GetMaterialWithTypeAndStatusBySerialNum(string num);
        MaterialRequest GetMaterialBySerialNum(string num);
        MaterialResponse ToMaterialResponse(Material material);
        OwnerResponse ToOwnerResponse(Owner material);
        //IEnumerable<MaterialRequest> GetMaterialsWithTypeWithStatusAndOwnerByBrand(string brand);
        //IEnumerable<MaterialRequest> GetMaterialsWithTypeWithStatusAndOwnerByStatus(string status);
        //IEnumerable<MaterialRequest> GetMaterialsWithTypeWithStatusAndOwnerByType(string type);
        //IEnumerable<MaterialRequest> GetMaterialsWithTypeWithStatusAndOwnerByName(string name);
        //IEnumerable<MaterialRequest> GetMaterialsWithTypeWithStatusAndOwnerByLabel(string label);
        IEnumerable<MaterialResponse> GetAllMaterialsWithTypeWithStatusAndOwner();
        //IEnumerable<LoanRequest> GetMaterialWithLoansById(int id);
        //IEnumerable<LoanRequest> GetMaterialWithLoans();


    }
}
