using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Customer.Add
{
    public class AddIndividualDto
    {
        [MinLength(3)]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MinLength(3)]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(11)]
        public string Pesel { get; set; } = string.Empty;

        [MinLength(5)]
        [MaxLength(100)]
        public string Address { get; set; } = string.Empty;

        [MinLength(5)]
        [MaxLength(256)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MinLength(6)]
        [MaxLength(50)]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
