using System;
using ZopaLoanQuoteSolution.Core;

namespace ZopaLoanQuoteSolution
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Wrong argument(s)");
                return;
            }

            try
            {
                var factory = new LoanQuoteGeneratorFactory();
                var generator = factory.Create<ZopaLoanQuoteGenerator>();

                generator.CalculateAndDisplay(args[0], args[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }
    }
}
