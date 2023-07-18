using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group_Project.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        [Required]
        public string PatientName { get; set;}
        [Required]
        public string Gender { get; set; }
        [Required]
        public string City { get; set;}
        [Required]
        public string Disease { get; set;}
        [Required]
        public virtual DoctorC Doctor { get; set;}
        [Required]
        public string Date { get; set;}
        [Required]
        public string Time { get; set;}
        [Required]
        public string Payment { get; set;}
        [Required]
        public string PhoneNumber { get; set;}
        
    }
}
