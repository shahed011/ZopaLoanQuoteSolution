using System;

namespace ZopaLoanQuoteSolution.Exceptions
{
    public class LoanQuoteException : Exception
    {
        public LoanQuoteException(string message, Exception ex)
        : base(message, ex)
        {
        }
    }
}
