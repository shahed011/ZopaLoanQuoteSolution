﻿using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZopaLoanQuoteSolution.DataTypes;
using ZopaLoanQuoteSolution.Exceptions;
using ZopaLoanQuoteSolution.Extensions;

namespace ZopaLoanQuoteSolution.Core
{
    public class Zopa36MonthsLoanQuoteGenerator : ILoanQuoteGenerator
    {
        private const int LoanLengthInMonths = 36;
        private const int MonthsInYear = 12;

        public void CalculateAndDisplay(string fileName, string requestedAmountString)
        {
            var requestedAmount = Convert.ToInt32(requestedAmountString);
            if (!IsRequestedAmountValid(requestedAmount))
            {
                Console.WriteLine("Requested amount is not valid.");
                Console.WriteLine("You can request of amount between £1000 and £15000 inclusive with any £100 increment.");
                return;
            }

            var availableLoans = GetListOfLoansFromCsv(fileName).OrderBy(x => x.Rate).ToList();
            var usableLoans = GetUsableLoans(availableLoans, requestedAmount).ToList();

            if (!IsQuoteAvailable(usableLoans, requestedAmount))
            {
                Console.WriteLine("It is not possible to provide a quote at that time.");
                return;
            }

            var newRate = GetRateFromUsableLoans(usableLoans, requestedAmount);
            var payment = GetMonthlyPayment(newRate, requestedAmount);

            DisplayLoanOffer(newRate, payment, requestedAmount);
        }

        private static void DisplayLoanOffer(double newRate, double payment, int requestedAmount)
        {
            Console.WriteLine($"Requested amount: {requestedAmount:C}");
            Console.WriteLine($"Rate: {newRate:p1}");
            Console.WriteLine($"Monthly repayment: {payment:C}");
            Console.WriteLine($"Total repayment: {payment * LoanLengthInMonths:C}");
        }

        public bool IsRequestedAmountValid(int amount)
        {
            return (amount % 100 == 0) && (amount >= 1000 && amount <= 15000);
        }

        public double GetMonthlyPayment(double newRate, int requestedAmount)
        {
            var ratePerPeriod = newRate / MonthsInYear;
            return Math.Round(ratePerPeriod * requestedAmount / (1 - Math.Pow(1 + ratePerPeriod, -LoanLengthInMonths)), 2);
        }

        public IEnumerable<AvailableLoans> GetUsableLoans(List<AvailableLoans> availableLoans, int requestedAmount)
        {
            return availableLoans.TakeWhileAggregate(0, (sum, loan) => sum + loan.Available,
                sum => sum < requestedAmount);
        }

        public bool IsQuoteAvailable(List<AvailableLoans> usableLoans, int requestedAmount)
        {
            return usableLoans.Select(x => x.Available).Sum() >= requestedAmount;
        }

        public double GetRateFromUsableLoans(List<AvailableLoans> usableLoans, int requestedAmount)
        {
            var sum = 0.0;
            var targetAmount = requestedAmount;

            foreach (var loan in usableLoans)
            {
                if (loan.Available > targetAmount)
                {
                    sum += loan.Rate * targetAmount;
                }
                else
                {
                    sum += loan.Rate * loan.Available;
                    targetAmount -= loan.Available;
                }
            }

            return Math.Round(sum / requestedAmount, 3);
        }

        private static List<AvailableLoans> GetListOfLoansFromCsv(string fileName)
        {
            List<AvailableLoans> availableLoans;

            try
            {
                using (var reader = new StreamReader(fileName))
                {
                    var csvReader = new CsvReader(reader);
                    availableLoans = csvReader.GetRecords<AvailableLoans>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new LoanQuoteException("Error while reading CSV file", ex);
            }

            return availableLoans;
        }
    }
}