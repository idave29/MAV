namespace MAV.Web.Data.Entities
{
    using System.Collections.Generic;
    public class ApplicantType
    {
        public int Id { get; set; }
        public ICollection<Applicant> Applicants { get; set; }
    }
}
