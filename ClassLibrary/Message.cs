namespace ClassLibrary
{
	public class Message
	{
		public User? Author { get; set; }
        public string? Content { get; set; }
        public Message(User author, string content)
        {
            Author = author;
			Content = content;            
        }
		public override string ToString()
		{
			return $"{Author.Email}: {Content}";
		}
	}
}