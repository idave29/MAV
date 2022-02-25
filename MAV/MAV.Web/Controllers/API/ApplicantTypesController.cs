namespace MAV.Web.Controllers.API
{
    using MAV.Web.Data.Repositories;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme) ]
    public class ApplicantTypesController : Controller
    {
        private readonly IApplicantTypeRepository applicantTypeRepository;

        public ApplicantTypesController(IApplicantTypeRepository applicantTypeRepository)
        {
            this.applicantTypeRepository = applicantTypeRepository;
        }

        [HttpGet]
        public IActionResult GetApplicantTypes()
        {
            return Ok(this.applicantTypeRepository.GetAplicantTypes());
            //return Ok(this.applicantTypeRepository.GetApplicantTypesByName("No deudor"));
            //return Ok(this.applicantTypeRepository.GetApplicantTypeById(1));
        }
        [HttpPost]
        public async Task<IActionResult> PostApplicantType([FromBody] MAV.Common.Models.ApplicantTypeRequest applicantType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entityApplicantType = new MAV.Web.Data.Entities.ApplicantType
            {
                Name = applicantType.Name
            };
            var newApplicantType = await this.applicantTypeRepository.CreateAsync(entityApplicantType);
            return Ok(newApplicantType);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicantType([FromRoute] int id, [FromBody] MAV.Common.Models.ApplicantTypeRequest applicantType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != applicantType.Id)
            {
                return BadRequest();
            }
            var oldApplicantType = await this.applicantTypeRepository.GetByIdAsync(id);
            if (oldApplicantType == null)
            {
                return BadRequest("Id not found");
            }
            oldApplicantType.Name = applicantType.Name;
            var updateApplicantType = await this.applicantTypeRepository.UpdateAsync(oldApplicantType);
            return Ok(updateApplicantType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicantType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var oldApplicantType = await this.applicantTypeRepository.GetByIdAsync(id);
            if (oldApplicantType == null)
            {
                return BadRequest("Id not found");
            }
            await this.applicantTypeRepository.DeleteAsync(oldApplicantType);
            return Ok(oldApplicantType);
        }
    }
}
