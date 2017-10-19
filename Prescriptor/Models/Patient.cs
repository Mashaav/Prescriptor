using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Prescriptor.Models
{
    public class Patient
    {
        //[Key]
        public int ID { get; set; }
        public string Name { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
