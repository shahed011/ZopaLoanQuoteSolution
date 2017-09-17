using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ZopaLoanQuoteSolution.Core;
using ZopaLoanQuoteSolution.DataTypes;

namespace ZopaLoanQuoteSolutionTest
{
    public class ZopaLoanQuoteGeneratorTest
    {
        private readonly ZopaLoanQuoteGenerator _testGenerator;
        private readonly List<AvailableLoans> _availableLoans;

        public ZopaLoanQuoteGeneratorTest()
        {
            _testGenerator = new ZopaLoanQuoteGenerator();

            var loan1 = new AvailableLoans {Lender = "A", Rate = 0.07, Available = 600};
            var loan2 = new AvailableLoans {Lender = "B", Rate = 0.10, Available = 200};
            var loan3 = new AvailableLoans {Lender = "C", Rate = 0.06, Available = 400};

            _availableLoans = new List<AvailableLoans> {loan1, loan2, loan3};
        }

        [Fact]
        public void FindUsableLoansForRequestedAmountTest_FoundTwo()
        {
            var test = _testGenerator.GetUsableLoans(_availableLoans.OrderBy(x => x.Rate).ToList(), 1000).ToList();

            Assert.Equal(2, test.Count);
            Assert.Equal(new List<string>{"C", "A"}, test.Select(x => x.Lender));
        }

        [Fact]
        public void IsQuoteAvailableTest_NotAvailable()
        {
            Assert.False(_testGenerator.IsQuoteAvailable(GetUsableLoans(1500), 1500));
        }

        [Fact]
        public void IsQuoteAvailableTest_Available()
        {
            Assert.True(_testGenerator.IsQuoteAvailable(GetUsableLoans(1100), 1100));
        }

        [Fact]
        public void GetRateTest_Success()
        {
            Assert.Equal(0.066, _testGenerator.GetRateFromUsableLoans(GetUsableLoans(1000), 1000));
        }

        [Fact]
        public void GetMonthleyPaymentTest_Success()
        {
            Assert.Equal(30.88, Math.Round(_testGenerator.GetMonthlyPayment(0.07, 1000), 2));
        }

        private List<AvailableLoans> GetUsableLoans(int amount)
        {
            return _testGenerator.GetUsableLoans(_availableLoans.OrderBy(x => x.Rate).ToList(), amount).ToList();
        }
    }
}
