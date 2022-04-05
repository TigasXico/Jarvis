using System;
using System.IO;

namespace ScrapySharp.Network
{
    public class WebResource : IDisposable
    {
        private readonly MemoryStream content;
        private readonly string lastModified;
        private readonly Uri absoluteUrl;
        private readonly bool forceDownload;
        private readonly string contentType;

        public WebResource( MemoryStream content , string lastModified , Uri absoluteUrl , bool forceDownload , string contentType )
        {
            this.content = content;
            this.lastModified = lastModified;
            this.absoluteUrl = absoluteUrl;
            this.forceDownload = forceDownload;
            this.contentType = contentType;
        }

        public void Dispose()
        {
            content.Dispose();
        }

        public MemoryStream Content => content;

        public string LastModified => lastModified;

        public Uri AbsoluteUrl => absoluteUrl;

        public bool ForceDownload => forceDownload;

        public string ContentType => contentType;

        public string GetTextContent()
        {
            content.Position = 0;
            using ( var reader = new StreamReader( content ) )
            {
                return reader.ReadToEnd();
            }
        }
    }
}