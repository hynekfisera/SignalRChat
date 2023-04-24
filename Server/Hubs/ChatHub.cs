using ClassLibrary;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography;

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
			byte[] key = user.rsaPublicKey;
			await Clients.Others.SendAsync("RequestKeys", key);
		}

		public async Task SendKeys(byte[] key, byte[] iv)
		{
			await Clients.Others.SendAsync("ReceiveKeys", key, iv);
		}
	}
}
