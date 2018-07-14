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
                Console.WriteLine("Let me alone - Resetter of Twitter");

                Args options = Args.ParseArgs(args);

                if (options.command == Args.Command.None)
                    throw new InvalidArgumentException();
                
                Console.Write("Are you sure? (y/n) ");
                string c = Console.ReadLine();
                if (c == "y" || c == "Y")
                {
                    Tokens tokens = Credential.GetTokens(CONSUMER_KEY, CONSUMER_SECRET, TOKEN_FILE_NAME);
                    Resetter resetter = new Resetter(tokens);

                    if (options.command == Args.Command.Kick || options.command == Args.Command.All)
                        resetter.KickAllFollowers(options.silence);

                    if (options.command == Args.Command.Erase || options.command == Args.Command.All)
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
            Console.WriteLine("usage: let-me-alone.exe [command] [option]");
            Console.WriteLine();
            Console.WriteLine("command");
            Console.WriteLine("\tkick          : Kick all followers. (block then unblock)");
            Console.WriteLine("\terase         : Erase all tweets and retweets.");
            Console.WriteLine("\tall           : Erase all tweets and retweets, and kick all followers. (kick + all)");
            Console.WriteLine("option");
            Console.WriteLine("\t-s, --silence : Don't describe any message.");
        }
    }
}
