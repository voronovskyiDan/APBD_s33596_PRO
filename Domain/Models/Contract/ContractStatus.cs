using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Contract
{
    public enum ContractStatus
    {
        PendingPayment,
        Signed,
        Expired,
        Deleted
    }
}
