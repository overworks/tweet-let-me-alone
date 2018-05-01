using System;
using CoreTweet;
using CoreTweet.Core;

namespace Mh.Twitter.Resetter
{
    class Resetter
    {
        private Tokens tokens;

        public Resetter(Tokens tokens)
        {
            this.tokens = tokens;
        }

        public void EraseAllTweets(bool silence)
        {
            var user = tokens.Account.VerifyCredentials(include_email: false);
            long userId = user.Id.Value;

            ListedResponse<Status> timeline = null;
            long? maxId = null;
            do
            {
                timeline = tokens.Statuses.UserTimeline(count: 200, max_id: maxId, exclude_replies: false);
                foreach (var status in timeline)
                {
                    Status retweetedStatus = status.RetweetedStatus;
                    if (retweetedStatus != null)
                    {
                        if (!silence)
                        {
                            Console.WriteLine("unretweeting - " + status.Text);
                        }
                        tokens.Statuses.Unretweet(retweetedStatus.Id, true);
                    }
                    else
                    {
                        if (!silence)
                        {
                            Console.WriteLine("deleting - " + status.Text);
                        }
                        tokens.Statuses.Destroy(status.Id);
                    }

                    if (!maxId.HasValue || status.Id < maxId.Value)
                    {
                        maxId = status.Id - 1L;
                    }
                }
            }
            while (timeline.Count > 0);
        }

        public void KickAllFollowers(bool silence)
        {
            var user = tokens.Account.VerifyCredentials(include_email: false);
            long userId = user.Id.Value;

            long? cursor = null;
            do
            {
                var followerList = tokens.Followers.List(userId, cursor);
                foreach (var follower in followerList)
                {
                    if (!silence)
                    {
                        Console.WriteLine("blocking user : {0} (@{1})", follower.Name, follower.ScreenName);
                    }
                    tokens.Blocks.Create(follower.Id.Value, false, true);
                }
                cursor = followerList.NextCursor;
            }
            while (cursor != null && cursor != 0L);

            cursor = null;
            do
            {
                var blockList = tokens.Blocks.List(cursor);
                foreach (var blocked in blockList)
                {
                    if (!silence)
                    {
                        Console.WriteLine("unblocking user: {0} (@{1})", blocked.Name, blocked.ScreenName);
                    }
                    tokens.Blocks.Destroy(blocked.Id.Value, false, true);
                }
                cursor = blockList.NextCursor;
            }
            while (cursor != null && cursor != 0L);
        }
    }
}
