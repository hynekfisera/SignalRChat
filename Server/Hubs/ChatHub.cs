using ClassLibrary;
using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs
{
	public class ChatHub : Hub
	{
		public async Task SendMessage(Message message)
		{
			await Clients.All.SendAsync("ReceiveMessage", message);
		}

		public async Task Login(User user)
		{
			// we would send our public key here
			await Clients.Others.SendAsync("RequestKeys");
		}

		public async Task SendKeys(byte[] key, byte[] iv)
		{
			await Clients.Others.SendAsync("ReceiveKeys", key, iv);
		}
	}
}
