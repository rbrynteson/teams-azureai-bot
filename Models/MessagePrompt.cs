namespace EchoBot.Models
{
    public class MessagePrompt
    {
        public string prompt { get; set; }
        public int temperature { get; set; }
        public float top_p { get; set; }
        public int frequency_penalty { get; set; }
        public int presence_penalty { get; set; }
        public int max_tokens { get; set; }
        public object stop { get; set; }
    }
}
