using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters;

class Program
{
    static async Task Main(string[] args)
    {
        string[] neededInfo =
        {
            "CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET"
        };
        
        for (int i = 0; i < neededInfo.Length; i++) 
        { 
            await Console.Out.WriteLineAsync( $"Enter your {neededInfo[i] }");
            string answer = Console.ReadLine();
            neededInfo[i] = answer;
        }


        // Initialize Tweetinvi
        var client = new TwitterClient(neededInfo[0], neededInfo[1], neededInfo[2], neededInfo[3]);

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
                Console.WriteLine("write the keyword you want to delete the twitts which contains it");
                string answer = Console.ReadLine() ;
                if (tweet.Text.ToLower().Contains(answer))
                {
                    await tweet.DestroyAsync();  // Corrected this line
                    deletedTweetsCount++;
                    await Task.Delay(1000); // 1-second delay
                }
            }
        }
    }
}
