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
            public string CONSUMER_KEY { get; set; }
            public string CONSUMER_SECRET { get; set; }
            public string ACCESS_TOKEN { get; set; }
            public string ACCESS_TOKEN_SECRET { get; set; }
        }

        public static Tokens GetTokens(string consumerKey, string consumerSecret, string tokensFileName)
        {
            Tokens tokens = null;

            bool isTokensFileValid = false;
            if (File.Exists(tokensFileName))
            {
                using (StreamReader reader = File.OpenText(tokensFileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    AccessTokens at = (AccessTokens)serializer.Deserialize(reader, typeof(AccessTokens));
                    if (at.CONSUMER_KEY == consumerKey && at.CONSUMER_SECRET == consumerSecret)
                    {
                        tokens = Tokens.Create(consumerKey, consumerSecret, at.ACCESS_TOKEN, at.ACCESS_TOKEN_SECRET);
                        if (tokens.Account.VerifyCredentials() != null)
                            isTokensFileValid = true;
                    }
                }
            }

            if (!isTokensFileValid)
            {
                tokens = CreateTokensByPin(consumerKey, consumerSecret);
                SaveTokensFile(tokens, tokensFileName);
            }

            return tokens;
        }

        static Tokens CreateTokensByPin(string consumerKey, string consumerSecret)
        {
            OAuth.OAuthSession session = OAuth.Authorize(consumerKey, consumerSecret);
            Uri authorizeUri = session.AuthorizeUri;
            Process.Start(authorizeUri.AbsoluteUri);
            Console.Write("Input PIN: ");
            string pin = Console.ReadLine();
            Tokens tokens = OAuth.GetTokens(session, pin);
            return tokens;
        }

        static void SaveTokensFile(Tokens tokens, string tokensFileName)
        {
            AccessTokens at = new AccessTokens();
            at.CONSUMER_KEY = tokens.ConsumerKey;
            at.CONSUMER_SECRET = tokens.ConsumerSecret;
            at.ACCESS_TOKEN = tokens.AccessToken;
            at.ACCESS_TOKEN_SECRET = tokens.AccessTokenSecret;

            using (StreamWriter writer = File.CreateText(tokensFileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, at);
            }
        }
    }
}
