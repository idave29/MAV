using MAV.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace MAV.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext dataContext;
        public CombosHelper(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboStatuses()
        {
            var list = this.dataContext.Statuses.Select(st => new SelectListItem
            {
                Text = st.Name,
                Value = $"{st.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un estado...)",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboApplicantTypes()
        {
            var list = this.dataContext.ApplicantTypes.Select(at => new SelectListItem
            {
                Text = at.Name,
                Value = $"{at.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un tipo de solicitante...)",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboInterns()
        {
            var list = this.dataContext.Interns.Where(a => a.User.Deleted == false).Select(t => new SelectListItem
            {
                Text = t.User.FullName,
                Value = $"{t.Id}"
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un becario...)",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboApplicants()
        {
            var list = dataContext.Applicants.Where(item => item.Debtor != true).Where(a => a.User.Deleted == false)
                .Select(
                c => new SelectListItem
                {
                    Text = c.User.FullName,
                    Value = $"{c.Id}"
                }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un solicitante...)",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboMaterials()
        {
            var list = dataContext.Materials.Where(item => item.Status.Id != 2).Where(item => item.Status.Id != 4).Where(item => item.Deleted == false)
                .Select(
                c => new SelectListItem
                {
                    Text = string.Format("{0} - {1}", c.Name, c.Label),
                    Value = $"{c.Id}"
                }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un material...)",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboUsers()
        {
            var list = this.dataContext.Users.Where(a => a.Deleted == false)
                .Select(
                c => new SelectListItem
                {
                    Text = string.Format("{0} - {1}", c.UserName, c.FullName),
                    Value = $"{c.UserName}"
                }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un usuario...)",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboRoles()
        {
            var list = dataContext.Roles.Select(
                c => new SelectListItem
                {
                    Text = c.Name,
                    Value = $"{c.Name}"
                }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un rol...)",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboMaterialTypes()
        {
            var list = dataContext.MaterialTypes.Select(
                c => new SelectListItem
                {
                    Text = c.Name,
                    Value = $"{c.Id}"
                }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un tipo de material...)",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboOwners()
        {
            var list = dataContext.Owners.Where(a => a.User.Deleted == false).Select(
                c => new SelectListItem
                {
                    Text = c.User.FullName,
                    Value = $"{c.Id}"
                }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un responsable...)",
                Value = "0"
            });
            return list;
        }

    }
}
