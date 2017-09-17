namespace ZopaLoanQuoteSolution.Core
{
    public class LoanQuoteGeneratorFactory
    {
        public ILoanQuoteGenerator Create<T>() where T : ILoanQuoteGenerator
        {
            if (typeof(T) == typeof(Zopa36MonthsLoanQuoteGenerator))
                return new Zopa36MonthsLoanQuoteGenerator();

            return new Zopa36MonthsLoanQuoteGenerator();
        }
    }
}