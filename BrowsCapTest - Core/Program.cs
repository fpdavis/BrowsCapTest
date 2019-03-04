using System;
using BrowsCapTest;

namespace BrowseCapTest
{
    internal class Program
    {
        #region Configuration

        private static readonly int iNodesToTest = 1000;
        private static readonly int iInitializationsToTest = 5;
        private static readonly int iVerbosity = 0; // 0 to 3
        private static readonly bool bRandom = true; // false starts at end of file and works backwords

        #endregion Configuration

        private static void Main(string[] args)
        {
            BrowsCapTestCore.Start(iInitializationsToTest, iNodesToTest, iVerbosity, bRandom);
            Console.ReadKey();
        }
    }
}