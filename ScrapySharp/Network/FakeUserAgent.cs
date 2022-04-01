namespace ScrapySharp.Network
{
    public class FakeUserAgent
    {
        private readonly string name;
        private readonly string userAgent;

        public FakeUserAgent(string name, string userAgent)
        {
            this.name = name;
            this.userAgent = userAgent;
        }

        public string Name => name;

        public string UserAgent => userAgent;
    }
}