using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDwithoutEF.Models
{
    public class Employee
    {
        [Key]

        public int EMPLOYEE_ID { get; set; }
        [Required]
        public string EMPLOYEE_FIRST_NAME { get; set; }
        [Required]
        public string EMPLOYEE_LAST_NAME { get; set; }
        [Range(18, int.MaxValue, ErrorMessage = "Should be greater than or equal to 18")]
        public int AGE { get; set; }
        [Required]
        public string POSITION { get; set; }
        [Required]
        public int SALARY { get; set; }
        


    }
}
