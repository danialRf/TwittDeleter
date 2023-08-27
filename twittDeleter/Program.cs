using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters;

class Program
{
    static async Task Main(string[] args)
    {
        // Initialize Tweetinvi
        var client = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");

        // Rate Limiting
        int deletedTweetsCount = 0;
        const int maxDeletionsPerDay = 3200;

        while (deletedTweetsCount < maxDeletionsPerDay)
        {
            // Fetch User's Tweets
            var user = await client.Users.GetAuthenticatedUserAsync();
            var timelineParameter = new GetUserTimelineParameters(user.Id)
            {
                PageSize = 200 // Fetch last 200 tweets
            };

            var timelineTweets = await client.Timelines.GetUserTimelineAsync(timelineParameter);

            foreach (var tweet in timelineTweets)
            {
                if (deletedTweetsCount >= maxDeletionsPerDay)
                {
                    break; // Stop if we reached the daily limit
                }

                if (tweet.Text.ToLower().Contains("hello"))
                {
                    await tweet.DestroyAsync();  // Corrected this line
                    deletedTweetsCount++;
                    await Task.Delay(1000); // 1-second delay
                }
            }
        }
    }
}
