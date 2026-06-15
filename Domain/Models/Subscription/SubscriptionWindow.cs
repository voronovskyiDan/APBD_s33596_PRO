using Domain.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Subscription
{
    public class SubscriptionWindow
    {
        public DateTime Start { get; private set; }

        public DateTime End { get; private set; }

        private SubscriptionWindow(DateTime start, DateTime end)
        {
            if (end < start)
                throw new BadRequestException("End date must be greater than or equal to start date.");
            End = end;
            Start = start;
        }

        public static SubscriptionWindow Create(DateTime start, int renewalPeriodMonths)
        {
            return new SubscriptionWindow(start, start.AddMonths(renewalPeriodMonths));
        }

        public bool IsExpired() =>
            DateTime.UtcNow > End;

        public bool Contains(DateTime dt) => dt >= Start && dt <= End;

        public void Advance(int renewalPeriodMonths)
        {
            Start = End;
            End = End.AddMonths(renewalPeriodMonths);
        }
    }
}
