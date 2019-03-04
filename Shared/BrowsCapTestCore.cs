using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BrowseCapTest;

namespace BrowsCapTest
{
    internal static class BrowsCapTestCore
    {
        #region Configuration

        private static readonly string userAgentStringsPath =
            "C:\\Users\\pdavis.UMPH\\Documents\\Visual Studio 2017\\Projects\\NopCommerce4-Local\\Presentation\\Nop.Web\\App_Data\\browscap.xml";

        private static readonly string crawlerOnlyUserAgentStringsPath =
            "C:\\Users\\pdavis.UMPH\\Documents\\Visual Studio 2017\\Projects\\NopCommerce4-Local\\Presentation\\Nop.Web\\App_Data\\browscap.crawlersonly.xml";

        #endregion Configuration

        private static BrowscapXmlHelper oBrowscapXmlHelper0;
        private static NewBrowseCapTest.BrowscapXmlHelper oBrowscapXmlHelper1;
        private static NewBrowseCapTest.BrowscapXmlHelper oBrowscapXmlHelper2;

        public static void Start(int iInitializationsToTest, int iNodesToTest = 10, int iVerbosity = 0, bool bRandom = true)
        {
            Console.WriteLine("BrowdCapTest Starting\n");
            Console.WriteLine("Start time: " + DateTime.Now + System.Environment.NewLine);

            Console.WriteLine($"Testing {iInitializationsToTest} initializations");

            var oStopwatch = System.Diagnostics.Stopwatch.StartNew();

            List<XElement> agentNames = LoadAgentNames(userAgentStringsPath);

            if (!bRandom && iNodesToTest > agentNames.Count) iNodesToTest = agentNames.Count;

            Console.WriteLine("Testing {0} {1}Agent Names out of a possible {2}\n", iNodesToTest, bRandom ? "Random " : "", agentNames.Count);

            string sAgentName;

            bool isCrawlerResult;
            bool shouldBeCrawler;

            int iCorrectMatches = 0;
            int iIncorrectMatches = 0;

            int iNodeToTest;
            Random oRandom = new Random();

            for (int iTestLoop = 0; iTestLoop < 2; iTestLoop++)
            {
                RunTestTitle(iTestLoop);

                #region Test Initialization Time

                decimal dAccumulator = 0;

                for (int i = 1; i <= iInitializationsToTest; i++)
                {
                    Console.Write($"\r   Initializing {i}/{iInitializationsToTest}");

                    oStopwatch.Reset();
                    oStopwatch.Start();
                    RunInitializationTest(iTestLoop, userAgentStringsPath, crawlerOnlyUserAgentStringsPath);
                    oStopwatch.Stop();

                    dAccumulator += oStopwatch.ElapsedMilliseconds;

                    if (iVerbosity > 2)
                    {
                        Console.WriteLine($"\r   {i} Initialization Time: {oStopwatch.ElapsedMilliseconds}ms");
                    }
                }

                decimal dAverage = dAccumulator / iInitializationsToTest;
                Console.WriteLine("\r                                    ");
                Console.WriteLine($"   Initialization Average: {dAverage}ms");

                if (iVerbosity > 1)
                {
                    Console.WriteLine($"\r   Total Time: {dAccumulator}ms");
                }

                #endregion Test Initialization Time

                #region Test isCrawler

                iCorrectMatches = 0;
                iIncorrectMatches = 0;
                dAccumulator = 0;

                for (int i = 1; i <= iNodesToTest; i++)
                {
                    Console.Write($"\r   Checking {i}/{iNodesToTest}");

                    if (bRandom)
                    {
                        iNodeToTest = oRandom.Next(0, agentNames.Count);
                    }
                    else
                    {
                        iNodeToTest = agentNames.Count - i;
                    }

                    oStopwatch.Reset();
                    oStopwatch.Start();

                    sAgentName = agentNames[iNodeToTest].Attribute("name").Value.Replace("*", "");
                    isCrawlerResult = RunIsCrawlerTest(iTestLoop, sAgentName);
                    oStopwatch.Stop();

                    dAccumulator += oStopwatch.ElapsedMilliseconds;

                    shouldBeCrawler = false;
                    foreach (var element in agentNames[iNodeToTest].Elements().ToList())
                    {
                        if ((element.Attribute("name")?.Value.ToLower() ?? string.Empty) == "crawler" && (element.Attribute("value")?.Value.ToLower() ?? string.Empty) == "true")
                            shouldBeCrawler = true;
                    }

                    if (iVerbosity > 2)
                    {
                        Console.WriteLine($"\r   {i} Match Node: {iNodeToTest} - {sAgentName}");
                        Console.WriteLine($"   {i} Match Time: {oStopwatch.ElapsedMilliseconds}ms");
                    }

                    if (isCrawlerResult == shouldBeCrawler)
                    {
                        iCorrectMatches++;
                    }
                    else
                    {
                        iIncorrectMatches++;

                        if (iVerbosity > 0)
                        {
                            Console.WriteLine($"\r   {i} Incorrect Match: " + sAgentName);
                            Console.WriteLine($"   {i} isCrawlerResult: {isCrawlerResult}");
                        }
                    }
                }

                dAverage = dAccumulator / iNodesToTest;
                Console.WriteLine("\r                                    ");
                Console.WriteLine($"   isCrawler() Average: {dAverage}ms");

                if (iVerbosity > 1)
                {
                    Console.WriteLine($"   Total Time: {dAccumulator}ms\n");
                }

                Console.WriteLine($"   Correct Matches: {iCorrectMatches}");
                Console.WriteLine($"   Incorrect Matches: {iIncorrectMatches}\n");

                #endregion Test isCrawler
            }

            Console.WriteLine("End time: " + DateTime.Now);
            Console.WriteLine($"BrowsCapTest Complete\n");
        }

        private static List<XElement> LoadAgentNames(string userAgentStringsPath)
        {
            List<XElement> agentNames;

            using (var oStreamReader = new StreamReader(userAgentStringsPath))
            {
                agentNames = XDocument.Load(oStreamReader).Root?.Element("browsercapitems")?.Elements("browscapitem").ToList();
            }

            return agentNames;
        }

        private static void RunTestTitle(int iTestNumber)
        {
            switch (iTestNumber)
            {
                case 0:
                    Console.WriteLine("Original 4.0");
                    break;

                case 1:
                    Console.WriteLine("Sam's Code, no Compilation");
                    break;

                case 2:
                    Console.WriteLine("Sam's Code, with Compilation");
                    break;
            }
        }

        private static void RunInitializationTest(int iTestNumber, string userAgentStringsPath, string crawlerOnlyUserAgentStringsPath)
        {
            switch (iTestNumber)
            {
                case 0:
                    // Original 4.0
                    oBrowscapXmlHelper0 = new BrowscapXmlHelper(userAgentStringsPath, crawlerOnlyUserAgentStringsPath);
                    break;

                case 1:
                    // Sam's Code, no Compilation
                    oBrowscapXmlHelper1 = new NewBrowseCapTest.BrowscapXmlHelper(userAgentStringsPath, crawlerOnlyUserAgentStringsPath);
                    break;

                case 2:
                    // Sam's Code, with Compilation
                    oBrowscapXmlHelper2 = new NewBrowseCapTest.BrowscapXmlHelper(userAgentStringsPath, crawlerOnlyUserAgentStringsPath, true);
                    break;
            }
        }

        private static bool RunIsCrawlerTest(int iTestNumber, string sUserAgent)
        {
            switch (iTestNumber)
            {
                case 0:
                    // Original 4.0
                    return oBrowscapXmlHelper0.IsCrawler(sUserAgent);

                case 1:
                    // Sam's Code, no Compilation
                    return oBrowscapXmlHelper1.IsCrawler(sUserAgent);

                case 2:
                    // Sam's Code, with Compilation
                    return oBrowscapXmlHelper2.IsCrawler(sUserAgent);
            }

            return false;
        }
    }
}