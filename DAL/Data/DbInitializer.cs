using System;
using System.Linq;
using DAL.Models;

namespace DAL.Data
{
    public static class DbInitializer
    {
        public static void Initialize(PrescriptorContext context)
        {
            context.Database.EnsureCreated();

            if(context.Prescriptions.Any())
            {
                return;
            }

            var patients = new Patient[]
            {
                new Patient{Name="Jacob", LastName="Knick",BirthDate=new DateTime(1983,04,17),PhoneNumber="123456789"},
                new Patient{Name="Adam", LastName="Faraday",BirthDate=new DateTime(1968,09,10),PhoneNumber="112345678"},
                new Patient{Name="Christine", LastName="Moses",BirthDate=new DateTime(1970,1,1),PhoneNumber="123456789"},
            };

            foreach(Patient p in patients)
            {
                context.Patients.Add(p);
            }
            context.SaveChanges();

            var prescriptions = new Prescription[]
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
        }
    }
}
