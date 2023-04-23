using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;

namespace Client.Services
{
	public class ChatService
	{
		public delegate void MessageReceivedHandler(ClassLibrary.Message message);
		public event MessageReceivedHandler? OnMessageReceived;
		private readonly string CONNECTION_URL = "https://localhost:7072/chat";
		private readonly HubConnection _connection;
		public ChatService()
		{
			_connection = new HubConnectionBuilder()
				.WithUrl(CONNECTION_URL)
				.Build();
		}
		public void Connect()
		{
			try
			{
				_connection.On<ClassLibrary.Message>("ReceiveMessage", (message) =>
				{
					OnMessageReceived.Invoke(message);
				});
				_connection.StartAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task SendMessage(ClassLibrary.Message message)
		{
			try
			{
				await _connection.InvokeAsync("SendMessage", message);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
