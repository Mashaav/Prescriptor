using System.Linq;
using System.Threading.Tasks;
using DAL.Models;

namespace BLL.Interfaces
{
    public interface IPatientService
    {
        IQueryable<Patient> SortPatients(string sortOrder, IQueryable<Patient> patients);
        IQueryable<Patient> SearchPatients(string searchString, IQueryable<Patient> patients);
        IQueryable<Patient> GetPatients();
        Task<Patient> GetPatient(int? id);
        Task AddPatient(Patient patient);
        Task UpdatePatient(Patient patient);
        Task DeletePatient(int id);
        bool PatientExists(int id);
    }
}