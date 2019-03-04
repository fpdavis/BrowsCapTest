using System;

namespace BrowsCapTest
{
    internal class Program
    {
        #region Configuration

        private static readonly int iNodesToTest = 1000;
        private static readonly int iInitializationsToTest = 5;
        private static readonly int bVerbosity = 0; // 0 to 3
        private static readonly bool bRandom = true; // false starts at end of file and works backwords

        #endregion Configuration

        private static void Main(string[] args)
        {
            BrowsCapTestCore.Start(iInitializationsToTest, iNodesToTest, bVerbosity, bRandom);
            Console.ReadKey();
        }
    }
}