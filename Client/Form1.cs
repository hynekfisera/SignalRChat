using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using ClassLibrary;
using Client.Services;
using Newtonsoft.Json;

namespace Client
{
	public partial class Form1 : Form
	{
		readonly string API_URL = "https://localhost:7072/api";
		private HttpClient _httpClient;
		private User? _user;
		private readonly ChatService _chatService;

		public Form1()
		{
			InitializeComponent();
			_httpClient = new HttpClient();
			_chatService = new ChatService();
			_chatService.OnMessageReceived += OnMessageRecieved;
			_chatService.Connect();
		}

		private async void btnLogin_Click(object sender, EventArgs e)
		{
			var email = tbEmail.Text;
			var password = tbPassword.Text;
			var client = new HttpClient();
			var data = new MultipartFormDataContent();
			data.Add(new StringContent(email), "email");
			data.Add(new StringContent(password), "password");
			var res = client.PostAsync($"{API_URL}/login", data).Result.Content
				.ReadAsStringAsync()
				.Result;
			LoginResponseModel loginResponse = JsonConvert.DeserializeObject<LoginResponseModel>(res);
			if (loginResponse.Success)
			{
				MessageBox.Show($"Login successful. User ID: {loginResponse.User}");
				_user = new User(Guid.Parse(loginResponse.User), email);
				tbEmail.Enabled = false;
				tbPassword.Enabled = false;
				btnLogin.Enabled = false;
				tbMessage.Enabled = true;
				btnSend.Enabled = true;
			}
			else
			{
				MessageBox.Show("Login failed.");
			}
		}
		public class LoginResponseModel
		{
			public bool Success { get; set; }
			public string? User { get; set; }
		}

		private async void btnSend_Click(object sender, EventArgs e)
		{
			string content = tbMessage.Text;
			await _chatService.SendMessage(new ClassLibrary.Message(_user, content));
		}

		private void OnMessageRecieved(ClassLibrary.Message message)
		{
			lbChat.Invoke(new Action(() => lbChat.Items.Add(message)));
		}
	}
}