using System;
using System.Linq;
using BLL.Services;
using BLL.Utils;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Prescriptor.Tests.BLL
{
    public class PatientServiceTests
    {
        private PrescriptorContext GetContext()
        {
            var options = new DbContextOptionsBuilder<PrescriptorContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new PrescriptorContext(options);

            var patients = new[]
            {
                new Patient
                {
                    Name = "Jacob",
                    LastName = "Knick",
                    BirthDate = new DateTime(1983, 04, 17),
                    PhoneNumber = "123456789"
                },
                new Patient
                {
                    Name = "Adam",
                    LastName = "Faraday",
                    BirthDate = new DateTime(1968, 09, 10),
                    PhoneNumber = "112345678"
                },
                new Patient
                {
                    Name = "Christine",
                    LastName = "Moses",
                    BirthDate = new DateTime(1970, 1, 1),
                    PhoneNumber = "123456789"
                }
            };

            foreach (Patient p in patients)
            {
                context.Patients.Add(p);
            }
            context.SaveChanges();

            var prescriptions = new[]
            {
                new Prescription
                {
                    DrugName = "Placebox",
                    PrescriptionCreationDate = new DateTime(2017, 12, 17),
                    PaymentMethod = Prescription.Payment.PrivatePay,
                    PatientID = 1
                },
                new Prescription
                {
                    DrugName = "Rutinox",
                    PrescriptionCreationDate = new DateTime(2018, 04, 1),
                    PaymentMethod = Prescription.Payment.Medicare,
                    PatientID = 2
                },
                new Prescription
                {
                    DrugName = "Placebox",
                    PrescriptionCreationDate = new DateTime(2016, 1, 16),
                    PaymentMethod = Prescription.Payment.Other,
                    PatientID = 3
                }
            };


            foreach (Prescription p in prescriptions)
            {
                context.Prescriptions.Add(p);
            }
            context.SaveChanges();

            return context;
        }

        [Fact(DisplayName = "SortPatients Should Sort Ascending")]
        public void SortPatients_ShouldSort_Ascending()
        {
            var context = GetContext();
            var service = new PatientService(context);

            var patients = service.SortPatients(nameof(Support.PatientsSortOrder.PatientIdDesc), service.GetPatients());
            var patientsSortedManually = service.GetPatients().OrderByDescending(p => p.ID);

            var patientFirst = patients.First();
            var patientSortedManuallyFirst = patientsSortedManually.First();

            Assert.Equal(patientSortedManuallyFirst.ID, patientFirst.ID);
        }

        [Fact(DisplayName = "GetPatients Shouldn't Return Null")]
        public void GetPatients_ShouldntReturn_Null()
        {
            var context = GetContext();
            var service = new PatientService(context);

            var patients = service.GetPatients();
            Assert.NotNull(patients);
        }


        [Theory(DisplayName = "PatientsExists Shouldn't Find Patient")]
        [InlineData(4)]//Note: Passes.
        [InlineData(0)]//Note: Passes.
        [InlineData(1)]//Note: Fails as should -> there is a patient in context, that contains id = 1.
        public void PatientsExists_ShouldntFind_Patient(int id)
        {
            var context = GetContext();
            var service = new PatientService(context);

            var result = service.PatientExists(id);
            Assert.False(result);
        }
    }
}