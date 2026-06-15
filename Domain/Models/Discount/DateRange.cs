using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Discount
{
    public class DateRange
    {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public DateRange(DateTime start, DateTime end)
        {
            if (end < start)
                throw new BadRequestException("End date must be greater than or equal to start date.");
            Start = start;
            End = end;
        }

        public bool Contains(DateTime dt) => dt >= Start && dt <= End;
    }
}
