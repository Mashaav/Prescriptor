using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Prescriptor.Models
{
    public class Prescription
    {
        public enum Payment
        {
            PrivatePay, Medicare, Other
        }

        public int ID { get; set; }

        public int PatientID { get; set; }

        [Display(Name = "Drug Name")]
        public string DrugName { get; set; }

        [Display(Name = "Drug Submission Date")]
        public DateTime DrugSubmissionDate { get; set;}
        [Display(Name = "Payment Method")]
        public Payment PaymentMethod { get; set; }
        public bool Outdated { get; set; }

        public Patient Patient { get; set; }

        }
    }

    public static class Extension
    {
        public static bool IsOutdated(DateTime expirationDate)
        {
            bool Outdated = false;

            if ((expirationDate - DateTime.Now).TotalDays < 30) Outdated = true;
            else Outdated = false;

           return Outdated;
        }
    }