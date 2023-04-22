using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Client
{
	public partial class Form1 : Form
	{
		readonly string API_URL = "https://localhost:7072/api";
		private HttpClient _httpClient;
		public Form1()
		{
			InitializeComponent();
			_httpClient = new HttpClient();
		}

		private async void btnLogin_Click(object sender, EventArgs e)
		{
			var email = tbEmail.Text;
			var password = tbPassword.Text;
			var client = new HttpClient();
			var data = new MultipartFormDataContent();
			data.Add(new StringContent(email), "email");
			data.Add(new StringContent(password), "password");
			var response = client.PostAsync($"{API_URL}/login", data).Result.Content
				.ReadAsStringAsync()
				.Result;
			//string payload = JsonConvert.SerializeObject(new { email, password });
			//var content = new StringContent(payload, Encoding.UTF8, "application/json");
			//var res = await _httpClient.PostAsync($"{API_URL}/login", content);
			//string resContent = await res.Content.ReadAsStringAsync();
			//LoginResponseModel loginResponse = JsonConvert.DeserializeObject<LoginResponseModel>(resContent);
			//if (loginResponse.Success)
			//{
			//	MessageBox.Show($"Login successful. User ID: {loginResponse.User}");
			//}
			//else
			//{
			//	MessageBox.Show("Login failed.");
			//}
		}
		public class LoginResponseModel
		{
            //public bool Success { get; set; }
            //public string? User { get; set; }
            public string Email { get; set; }
			public string Password { get; set; }
        }
	}
}