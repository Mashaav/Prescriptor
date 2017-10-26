using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;

namespace BLL.Interfaces
{
    public interface IPrescriptionService
    {
        IQueryable<Prescription> GetPrescriptions();
        IQueryable<Prescription> SearchPrescriptions(string searchString, IQueryable<Prescription> prescriptions, Dictionary<string, string> payments);
        IQueryable<Prescription> SortPrescriptions(string sortOrder, IQueryable<Prescription> prescriptions);
        Task<Prescription> GetPrescription(int? id);
        Task<Prescription> EditGetPrescription(int? id);
        Task EditUpdatePrescription(Prescription prescription);
        Task AddPrescription(Prescription prescription);
        Task DeletePrescription(int id);
        bool PrescriptionExists(int id);
    }
}