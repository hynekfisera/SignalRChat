using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using System.Windows.Forms;

namespace Client.Services
{
	public class ChatService
	{
		public delegate void MessageReceivedHandler(ClassLibrary.Message message);
		public event MessageReceivedHandler? OnMessageReceived;
		public delegate void KeysRequestedHandler();
		public event KeysRequestedHandler? OnKeysRequested;
		public delegate void KeysReceivedHandler(byte[] key, byte[] iv);
		public event KeysReceivedHandler? OnKeysReceived;
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
				_connection.On("RequestKeys", () =>
				{
					OnKeysRequested.Invoke();
				});
				_connection.On<byte[], byte[]>("ReceiveKeys", (key, iv) =>
				{
					OnKeysReceived.Invoke(key, iv);
				}
				);
				_connection.StartAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task Login(User user)
		{
			try
			{
				await _connection.InvokeAsync("Login", user);
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

		public async Task SendKeys(byte[] key, byte[] iv)
		{
			try
			{
				await _connection.InvokeAsync("SendKeys", key, iv);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
