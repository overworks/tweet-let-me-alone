using System;
using CoreTweet;

namespace Mh.Twitter.Resetter
{
    class Program
    {
        const string CONSUMER_KEY = "<your consumer key>";
        const string CONSUMER_SECRET = "<your consumer secret>";

        const string TOKEN_FILE_NAME = "token.json";

        static void Main(string[] args)
        {
            try
            {
                Options options = Options.ParseArgs(args);

                if (options.opt == Options.Opt.None)
                    throw new InvalidArgumentException();
                
                Console.Write("Are you sure? (y/n) ");
                string c = Console.ReadLine();
                if (c == "y" || c == "Y")
                {
                    Tokens tokens = Credential.GetTokens(CONSUMER_KEY, CONSUMER_SECRET, TOKEN_FILE_NAME);
                    Resetter resetter = new Resetter(tokens);

                    if (options.opt == Options.Opt.Kick || options.opt == Options.Opt.All)
                        resetter.KickAllFollowers(options.silence);

                    if (options.opt == Options.Opt.Erase || options.opt == Options.Opt.All)
                        resetter.EraseAllTweets(options.silence);
                }
            }
            catch (InvalidArgumentException)
            {
                DisplayUsage();
            }
            catch (TwitterException e)
            {
                foreach (var error in e.Errors)
                {
                    Console.WriteLine("{0} - {1}", error.Code, error.Message);
                }
            }
        }

        static void DisplayUsage()
        {
            Console.WriteLine("usage: let-me-alone.exe [options]");
            Console.WriteLine();
            Console.WriteLine("option:");
            Console.WriteLine("\t-e, --erase        : Erase all tweets and retweets.");
            Console.WriteLine("\t-k, --kick         : Kick all followers(block then unblock)");
            Console.WriteLine("\t-a, --all          : Erase all tweets and retweets, and kick all followers(-k, -e)");
            Console.WriteLine("\t-s, --silence      : Don't describe any message.");
        }
    }
}
