namespace ProductApp.Services
{
    public interface IPriceCalculator
    {
        decimal AddTax(decimal price, decimal taxRate);
    }
}

