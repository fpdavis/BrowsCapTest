# BrowsCapTest

Test/Performance harness for evaluating changes to BrowscapXmlHelper as used in nopCommerce. 
Two projects are included for testing .Net Core and .Net Framework builds.

You must update "userAgentStringsPath" and "crawlerOnlyUserAgentStringsPath" in the "Shared\BrowsCapTestCore.cs" to the proper location.

* userAgentStringsPath = "\NopCommerce4\Presentation\Nop.Web\App_Data\browscap.xml"
* crawlerOnlyUserAgentStringsPath = "NopCommerce4\Presentation\Nop.Web\App_Data\browscap.crawlersonly.xml"
* You may also change the number "2" on Line 52 to a "3" to also test the RegexOptions.Compiled option. This option has proven to be painfully slow in newer .Net releases and a no-op in some older versions.

		for (int iTestLoop = 0; iTestLoop < 2; iTestLoop++)
		to
		for (int iTestLoop = 0; iTestLoop < 3; iTestLoop++)

In "BrowsCapTest - Core\Program.cs" and/or ""BrowsCapTest - Framework\Program.cs" you may modify the following settings as desired:

* iNodesToTest - Number of agent strings to test.
* iInitializationsToTest - number of times to initialize BrowscapXmlHelper class before using it.
* bVerbosity - 0 to 3, the higher the number the more verbose the output is.
* bRandom - If true then random nodes from browscap.xml are selected. If set to false then the program starts at the last node and works its way backwards. The last node in the list takes the most amount
of time to match on, the first node takes the least amount of time to match on.
