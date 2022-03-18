namespace MAV.Web.Helpers
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;

    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboStatuses();
        IEnumerable<SelectListItem> GetComboApplicantTypes();
        IEnumerable<SelectListItem> GetComboMaterials();
        IEnumerable<SelectListItem> GetComboUsers();
        IEnumerable<SelectListItem> GetComboRoles();
        IEnumerable<SelectListItem> GetComboInterns();
        IEnumerable<SelectListItem> GetComboApplicants();
        IEnumerable<SelectListItem> GetComboOwners();
        IEnumerable<SelectListItem> GetComboMaterialTypes();

    }
}
