using System.Security.Cryptography;

namespace ClassLibrary
{
	public class Message
	{
		public User? Author { get; set; }
        public byte[] Content { get; set; }
        public Message(User author, byte[] content)
        {
            Author = author;
			Content = content;            
        }
	}
}