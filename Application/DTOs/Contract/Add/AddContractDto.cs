using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Contract.Add
{
    public class AddContractDto
    {
        [Required]
        public int? CustomerId { get; set; }

        [Required]
        public int? ProdcutId { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }

        [Range(1,3)]
        public int AdditionalSupportYears { get; set; }
    }
}
