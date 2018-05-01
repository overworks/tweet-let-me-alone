using System;
using System.IO;
using System.Diagnostics;
using CoreTweet;
using Newtonsoft.Json;

namespace Mh.Twitter
{
    class Credential
    {
        class AccessTokens
        {
            public string ACCESS_TOKEN { get; set; }
            public string ACCESS_TOKEN_SECRET { get; set; }
        }

        public static Tokens GetTokens(string consumerKey, string consumerSecret, string tokenFileName)
        {
            Tokens tokens = null;

            if (File.Exists(tokenFileName))
            {
                using (StreamReader reader = File.OpenText(tokenFileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    AccessTokens at = (AccessTokens)serializer.Deserialize(reader, typeof(AccessTokens));
                    tokens = Tokens.Create(consumerKey, consumerSecret, at.ACCESS_TOKEN, at.ACCESS_TOKEN_SECRET);
                }
            }
            else
            {
                OAuth.OAuthSession session = OAuth.Authorize(consumerKey, consumerSecret);
                Uri authorizeUri = session.AuthorizeUri;
                Process.Start(authorizeUri.AbsoluteUri);
                Console.Write("Input PIN: ");
                string pin = Console.ReadLine();
                tokens = OAuth.GetTokens(session, pin);

                AccessTokens at = new AccessTokens() { ACCESS_TOKEN = tokens.AccessToken, ACCESS_TOKEN_SECRET = tokens.AccessTokenSecret };
                using (StreamWriter writer = File.CreateText(tokenFileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, at);
                }
            }

            return tokens;
        }
    }
}
