using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prescriptor.Web.Controllers;
using Xunit;

namespace Prescriptor.Tests.Web
{
    public class PrescriptionsControllerTests
    {
        private PrescriptorContext GetContext()
        {
            var options = new DbContextOptionsBuilder<PrescriptorContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new PrescriptorContext(options);

            var patients = new[]
            {
                new Patient{Name="Jacob", LastName="Knick",BirthDate=new DateTime(1983,04,17),PhoneNumber="123456789"},
                new Patient{Name="Adam", LastName="Faraday",BirthDate=new DateTime(1968,09,10),PhoneNumber="112345678"},
                new Patient{Name="Christine", LastName="Moses",BirthDate=new DateTime(1970,1,1),PhoneNumber="123456789"}
            };

            foreach (Patient p in patients)
            {
                context.Patients.Add(p);
            }
            context.SaveChanges();

            var prescriptions = new[]
            {
                new Prescription{DrugName="Placebox", PrescriptionCreationDate=new DateTime(2017,12,17), PaymentMethod=Prescription.Payment.PrivatePay, PatientID=1},
                new Prescription{DrugName="Rutinox", PrescriptionCreationDate=new DateTime(2018,04,1), PaymentMethod=Prescription.Payment.Medicare, PatientID=2},
                new Prescription{DrugName="Placebox", PrescriptionCreationDate=new DateTime(2016,1,16), PaymentMethod=Prescription.Payment.Other, PatientID=3}
            };


            foreach (Prescription p in prescriptions)
            {
                context.Prescriptions.Add(p);
            }
            context.SaveChanges();

            return context;
        }

        //Note: When started as single unit test - passes, otherwise not -> Run separately from other unit tests.
        [Fact(DisplayName = "Index Should Contain More Than 0 Entities")]
        public async Task Index_ShouldContain_Entities()
        {
            var context = GetContext();
            var controller = new PrescriptionsController(context, null);
            var resultTask = controller.Index(null, null);
            await resultTask;

            Assert.NotNull(resultTask);

            var result = resultTask.Result;
            var model = (List<Prescription>)((ViewResult)result).Model;

            Assert.NotEqual(0, model.Count);
        }

        //Note: Should pass.
        [Fact(DisplayName = "Details Should Return Task<IActionResult>")]
        public void Details_ShouldReturn_TaskIActionResult()
        {
            var context = GetContext();
            var controller = new PrescriptionsController(context, null);
            var resultTask = controller.Details(0);
            Assert.IsType<Task<IActionResult>>(resultTask);
        }

        [Theory(DisplayName = "Details Should Contain Prescription Entity")]
        [InlineData(1)]//Note: Run separately for actual results.
        [InlineData(2)]//Note: Run separately for actual results.
        [InlineData(1952)]//Note: Should fail.
        public void Details_ShouldContain_PrescriptionEntity(int id)
        {
            using (var context = GetContext())
            using (var controller = new PrescriptionsController(context,null))
            {
                var resultTask = controller.Details(id);
                resultTask.Wait();

                Assert.NotNull(resultTask);

                var result = resultTask.Result;
                var model = (Prescription)((ViewResult)result).Model;
                Assert.NotNull(model);
            }
        }
    }
}
