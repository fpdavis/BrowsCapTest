using System;
using BrowsCapTest;

namespace BrowseCapTest
{
    internal class Program
    {
        #region Configuration

        private static readonly int iNodesToTest = 2; // Max 115689
        private static readonly int iInitializationsToTest = 2;
        private static readonly bool bVerbose = false;

        #endregion Configuration

        private static void Main(string[] args)
        {
            BrowsCapTestCore.Start(iInitializationsToTest, iNodesToTest, bVerbose);
            Console.ReadKey();
        }
    }
}