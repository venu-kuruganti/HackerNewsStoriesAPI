namespace HackerNewsStories.Models
{
    public class Story
    {
        public int id { get; set; }

        public int score { get; set; }

        public string title { get; set; }

        public string text { get; set; }

        public string by { get; set; }

        public string url { get; set; }

        public int CommentCount { get; set; }

        public int time { get; set; }



    }
}
