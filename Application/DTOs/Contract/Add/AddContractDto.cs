using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Contract.Add
{
    public class AddContractDto
    {
        public int CustomerId { get; set; }
        public int ProdcutId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AdditionalSupportYears { get; set; }
    }
}
