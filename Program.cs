using System;
using System.Diagnostics;
using System.Reflection;
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
                Console.WriteLine("Let me alone - Tweet account resetter v" + GetVersionString());

                Args options = Args.ParseArgs(args);

                if (options.command == Args.Command.None)
                    throw new InvalidArgumentException();

                string consumerKey = CONSUMER_KEY;
                string consumerSecret = CONSUMER_SECRET;

                // 커스텀 컨슈머 정보를 사용하는 경우.
                if (options.consumer)
                {
                    Console.Write("Consumer Key: ");
                    consumerKey = Console.ReadLine();
                    Console.Write("Consumer Secret: ");
                    consumerSecret = Console.ReadLine();
                }
                
                Tokens tokens = Credential.GetTokens(consumerKey, consumerSecret, TOKEN_FILE_NAME);
                Resetter resetter = new Resetter(tokens);

                Console.Write("Are you sure? (y/n) ");
                string c = Console.ReadLine();
                if (c == "y" || c == "Y")
                {
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
                Console.WriteLine("TwitterException occured.");
                foreach (var error in e.Errors)
                {
                    Console.WriteLine("{0}: {1}", error.Code, error.Message);
                }
            }
        }

        static string GetVersionString()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return string.Format("{0}.{1}.{2}", fileVersionInfo.FileMajorPart, fileVersionInfo.FileMinorPart, fileVersionInfo.FileBuildPart);
        }

        static void DisplayUsage()
        {
            Console.WriteLine("usage: let-me-alone.exe [command] [option]");
            Console.WriteLine();
            Console.WriteLine("command");
            Console.WriteLine("\tkick           : Kick all followers. (block then unblock)");
            Console.WriteLine("\terase          : Erase all tweets and retweets.");
            Console.WriteLine("\tall            : Erase all tweets and retweets, and kick all followers. (kick + all)");
            Console.WriteLine("option");
            Console.WriteLine("\t-s, --silence  : Don't describe any message.");
            Console.WriteLine("\t-c, --consumer : Use custom consumer.");
        }
    }
}
