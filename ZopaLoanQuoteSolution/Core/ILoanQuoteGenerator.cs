using System.Collections.Generic;
using ZopaLoanQuoteSolution.DataTypes;

namespace ZopaLoanQuoteSolution.Core
{
    public interface ILoanQuoteGenerator
    {
        void CalculateAndDisplay(string fileName, string requestedAmountString);

        IEnumerable<AvailableLoans> GetUsableLoans(List<AvailableLoans> availableLoans, int requestedAmount);
        bool IsQuoteAvailable(List<AvailableLoans> usableLoans, int requestedAmount);

        double GetRateFromUsableLoans(List<AvailableLoans> usableLoans, int requestedAmount);
        double GetMonthlyPayment(double newRate, int requestedAmount);
    }
}