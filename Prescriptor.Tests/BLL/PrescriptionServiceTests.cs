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
    public class PrescriptionServiceTests
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

        [Fact(DisplayName = "SortPrescriptions Should Sort Ascending")]
        public void SortPrescriptions_ShouldSort_Ascending()
        {
            var context = GetContext();
            var service = new PrescriptionService(context);

            var prescriptions = service.SortPrescriptions(nameof(Support.PrescriptionsSortOrder.IdDesc), service.GetPrescriptions());
            var prescriptionsSortedManually = service.GetPrescriptions().OrderByDescending(p => p.ID);

            var prescriptionFirst = prescriptions.First();
            var prescriptionSortedManuallyFirst = prescriptionsSortedManually.First();

            Assert.Equal(prescriptionSortedManuallyFirst.ID, prescriptionFirst.ID);
        }

        [Fact(DisplayName = "GetPrescriptions Shouldn't Return Null")]
        public void GetPrescriptions_ShouldntReturn_Null()
        {
            var context = GetContext();
            var service = new PrescriptionService(context);

            var prescriptions = service.GetPrescriptions();
            Assert.NotNull(prescriptions);
        }


        [Theory(DisplayName = "PrescriptionExists Shouldn't Find Prescription")]
        [InlineData(4)]//Note: Passes.
        [InlineData(0)]//Note: Passes.
        [InlineData(1)]//Note: Fails as should -> there is a prescription in context, that contains id = 1.
        public void PrescriptionExists_ShouldntFind_Precription(int id)
        {
            var context = GetContext();
            var service = new PrescriptionService(context);

            var result = service.PrescriptionExists(id);
            Assert.False(result);
        }
    }
}