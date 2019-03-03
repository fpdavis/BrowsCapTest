using System;

namespace BrowsCapTest
{
    internal class Program
    {
        #region Configuration

        private static readonly int iNodesToTest = 1000; // Max 115689
        private static readonly int iInitializationsToTest = 5;
        private static readonly int bVerbosity = 1;

        #endregion Configuration

        private static void Main(string[] args)
        {
            BrowsCapTestCore.Start(iInitializationsToTest, iNodesToTest, bVerbosity);
            Console.ReadKey();
        }
    }
}