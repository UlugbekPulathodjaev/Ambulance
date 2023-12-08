using Ambulance.Data;
using Ambulance.DTOs.Patients;
using Ambulance.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Ambulance.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;


        public PatientController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<List<Patient>>> GetAllPatients()
        {
            try
            {
                var cache = _cache.Get("GetAllPatients");
                if (cache == null)
                {
                    var patients = await _context.Patients.ToListAsync();
                    _cache.Set("GetAllPatients", patients);
                }

                return Ok(_cache.Get("GetAllPatients"));
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while processing the request. Details: " + ex.Message);
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<Patient>>> GetByIdPatients(int id)
        {
            try
            {
                var cache = _cache.Get("GetByIdPatients");
                if (cache == null)
                {

                    var patients = await _context.Patients.FindAsync(id);
                    if (patients == null)
                        return BadRequest("Patient not found.");
                }

                return Ok(_cache.Get("GetByIdPatients"));
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while processing the request. Details: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<Patient>>> AddPatients(PatientCreateDto patientDto)
        {
            try
            {
                var patient = new Patient
                {
                    FirstName = patientDto.FirstName,
                    LastName = patientDto.LastName,
                    AmbulanceCarNumber = patientDto.AmbulanceCarNumber,
                    HealthStatus = patientDto.HealthStatus
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                return Ok(await GetAllPatients());
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while processing the request. Details: " + ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Patient>> UpdatePatients(int id, PatientUpdateDto patientDto)
        {
            try
            {
                var dbPatient = await _context.Patients.FindAsync(id);

                if (dbPatient == null)
                    return NotFound($"Patient with ID {id} not found.");

                dbPatient.FirstName = patientDto.FirstName;
                dbPatient.LastName = patientDto.LastName;
                dbPatient.HealthStatus = patientDto.HealthStatus;

                await _context.SaveChangesAsync();

                return Ok(dbPatient);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while processing the request. Details: " + ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<Patient>> DeletePatients(int id)
        {
            try
            {
                var dbPatient = await _context.Patients.FindAsync(id);

                if (dbPatient == null)
                    return NotFound($"Patient with ID {id} not found.");

                _context.Patients.Remove(dbPatient);
                await _context.SaveChangesAsync();

                return Ok(dbPatient);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while processing the request. Details: " + ex.Message);
            }
        }



    }
}
