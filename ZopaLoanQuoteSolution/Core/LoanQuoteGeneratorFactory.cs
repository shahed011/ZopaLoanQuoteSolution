namespace ZopaLoanQuoteSolution.Core
{
    public class LoanQuoteGeneratorFactory
    {
        public ILoanQuoteGenerator Create<T>() where T : ILoanQuoteGenerator
        {
            if (typeof(T) == typeof(ZopaLoanQuoteGenerator))
                return new ZopaLoanQuoteGenerator();

            return new ZopaLoanQuoteGenerator();
        }
    }
}