using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group_Project.Models
{
    public class DoctorC
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DoctorID { get; set; }
        [Required] 
        public string Name { get; set; }
        [Required]
        public string Specialization { get; set; }
        public int Payment { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
    }
}

