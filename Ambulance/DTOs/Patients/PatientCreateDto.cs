namespace Ambulance.DTOs.Patients
{
    public class PatientCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HealthStatus { get; set; }
        public string AmbulanceCarNumber { get; set; }
    }
}
