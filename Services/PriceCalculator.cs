using System;

namespace ProductApp.Services
{
        public class PriceCalculator : IPriceCalculator
    {
        public decimal AddTax(decimal price, decimal taxRate)
        {
            if (price < 0) throw new ArgumentOutOfRangeException(nameof(price));
            if (taxRate < 0) throw new ArgumentOutOfRangeException(nameof(taxRate));
            // round to cents
            return Math.Round(price * (1 + taxRate), 2, MidpointRounding.AwayFromZero);
        }
    }
}
