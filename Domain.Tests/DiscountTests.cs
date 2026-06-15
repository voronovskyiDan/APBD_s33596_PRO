using Domain.Exceptions;
using Domain.Models.Discount;

namespace Domain.Tests
{
    public class DiscountTests
    {
        [Fact]
        public void Constructor_InvalidPercentage_ThrowsBadRequestException()
        {
            var range = new DateRange(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));

            Assert.Throws<BadRequestException>(() =>
                new Discount("Sale", 0m, range, DiscountType.General));
        }

        [Fact]
        public void IsActive_DateInsideRange_ReturnsTrue()
        {
            var today = DateTime.UtcNow;
            var discount = new Discount(
                "Spring Sale",
                15m,
                new DateRange(today.AddDays(-10), today.AddDays(10)),
                DiscountType.General);

            var result = discount.IsActive(today);

            Assert.True(result);
        }
    }
}