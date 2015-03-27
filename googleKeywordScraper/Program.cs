using System;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;

namespace googleKeywordScraper
{
	class MainClass
	{
		//All strings inbetween <h3 class="r"><a href="/url?q= and &amp;
		const string regexPattern = @"(?<=<h3 class=\""r\""><a href=\""\/url\?q=)(.*?)(?=&amp;)";
		public static void Main (string[] args)
		{
			args = Sample ();
			if (args.Length < 2 || args.Length > 3) {
				ShowError ("Invalid Amount of Parameters: Must have 2 or 3 parameters. 1: keywords delimited by +'s and 2: number of results (10, 50, 100) 3: Website url for which you want a ranking");
				return;
			}

			if (args [1] != "10" && args [1] != "50" && args [1] != "100") {
				ShowError ("Invalid Number of results: " + args[1] + " should be 10, 50, or 100");
				return;
			}

			string keywords = args[0];
			string numResults = args[1];
			string rankingUrl = null;
			if (args.Length > 2) {
				string url = args [2];
				if (url.Substring (0, 7) != "http://")
					url = "http://" + url;
				Uri myUri = new Uri(url);   
				rankingUrl = myUri.Host.ToLower();
			}

			WebClient fetch = new WebClient();
			string data = fetch.DownloadString("https://www.google.com/search?q=" + keywords + "&num=" + numResults + "&as_qdr=all&ei=LrUVVf7UMrPfsAS7lICgCw&sa=N&biw=1440&bih=690");
			string[] results = TopUrls (data);

			int rank = -1;

			for (int i = 0; i < results.Length; i += 1) {
				Uri eachUri = new Uri(results[i]);
				if (eachUri.Host.ToLower () == rankingUrl) {
					rank = i + 1;
					break;
				}
			}

			//build json
			string joined = string.Join ("\",\n\"", results);

			string rankingJson = "";
			if (rankingUrl != null) {
				rankingJson = ",\n\"ranking\": " + rank;
			}
			Console.WriteLine ("{\"results\": [\n\"" + joined + "\"\n]" + rankingJson + "\n}");
		}

		public static string[] TopUrls(string data) {
			Regex regex = new Regex (regexPattern);
			MatchCollection collection = regex.Matches (data);
			return collection.Cast<Match>()
				.Select(m => m.Value)
				.ToArray();
		}

		public static string[] Sample() {
			string keywords = "web+application+development+orlando";
			string numResults = "100";
			string rankingSite = "cloudspace.com";
			return new string[] { keywords, numResults, rankingSite};
		}

		public static void ShowError(string error) {
			Console.WriteLine ("{\"error\": \"" + error +"\"}");
		}
	}
}
