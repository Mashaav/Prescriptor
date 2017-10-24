﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Model
{
    public partial class Prescription
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
        public DateTime DrugSubmissionDate { get; set; }

        [Display(Name = "Payment Method")]

        public Payment PaymentMethod { get; set; }
        public bool Outdated { get; set; }

        public Patient Patient { get; set; }

    }
}