using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Contract
{
    public class ContractDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProfuctId { get; set; }
        public string SoftwareVersion { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTime? SigningDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
