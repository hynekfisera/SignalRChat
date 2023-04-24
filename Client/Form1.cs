using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Forms;
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
		private byte[] AES_KEY;
		private byte[] AES_IV;
		private RSAParameters RSAKeyInfo;

		public Form1()
		{
			InitializeComponent();
			_httpClient = new HttpClient();
			_chatService = new ChatService();
			_chatService.OnMessageReceived += OnMessageReceived;
			_chatService.OnKeysRequested += OnKeysRequested;
			_chatService.OnKeysReceived += OnKeysReceived;
			_chatService.Connect();
			using (Aes aes = Aes.Create())
			{
				AES_KEY = aes.Key;
				AES_IV = aes.IV;
			}
			using (RSA rsa = RSA.Create())
			{
				RSAKeyInfo = rsa.ExportParameters(true);
			}
		}

		private async void btnLogin_Click(object sender, EventArgs e)
		{
			var email = tbEmail.Text;
			var password = tbPassword.Text;
			var data = new MultipartFormDataContent();
			data.Add(new StringContent(email), "email");
			data.Add(new StringContent(password), "password");
			var res = _httpClient.PostAsync($"{API_URL}/login", data).Result.Content
				.ReadAsStringAsync()
				.Result;
			LoginResponseModel loginResponse = JsonConvert.DeserializeObject<LoginResponseModel>(res);
			if (loginResponse.Success)
			{
				MessageBox.Show($"Login successful. User ID: {loginResponse.User}");
				using (RSA rsa = RSA.Create())
				{
					rsa.ImportParameters(RSAKeyInfo);
					var publicKey = rsa.ExportRSAPublicKey();
					_user = new User(Guid.Parse(loginResponse.User), email, publicKey);
					await _chatService.Login(_user);
				}
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
			await _chatService.SendMessage(new ClassLibrary.Message(_user, AesEncrypt(content, AES_KEY, AES_IV)));
		}

		private void OnMessageReceived(ClassLibrary.Message message)
		{
			lbChat.Invoke(new Action(() => lbChat.Items.Add($"{message.Author.Email}: {AesDecrypt(message.Content, AES_KEY, AES_IV)}")));
		}

		private async void OnKeysRequested(byte[] publicKey)
		{
			using (RSA rsa = RSA.Create())
			{
				rsa.ImportRSAPublicKey(publicKey, out int _);
				var publicRSAKeyInfo = rsa.ExportParameters(false);
				var key = RSAEncrypt(AES_KEY, publicRSAKeyInfo, false);
				var iv = RSAEncrypt(AES_IV, publicRSAKeyInfo, false);
				await _chatService.SendKeys(key, iv);
			}
		}

		private void OnKeysReceived(byte[] key, byte[] iv)
		{
			AES_KEY = RSADecrypt(key, RSAKeyInfo, false);
			AES_IV = RSADecrypt(iv, RSAKeyInfo, false);
		}

		static byte[] AesEncrypt(string plainText, byte[] Key, byte[] IV)
		{
			// Check arguments.
			if (plainText == null || plainText.Length <= 0)
				throw new ArgumentNullException("plainText");
			if (Key == null || Key.Length <= 0)
				throw new ArgumentNullException("Key");
			if (IV == null || IV.Length <= 0)
				throw new ArgumentNullException("IV");
			byte[] encrypted;

			// Create an Aes object
			// with the specified key and IV.
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = Key;
				aesAlg.IV = IV;

				// Create an encryptor to perform the stream transform.
				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

				// Create the streams used for encryption.
				using (MemoryStream msEncrypt = new MemoryStream())
				{
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
						{
							//Write all data to the stream.
							swEncrypt.Write(plainText);
						}
						encrypted = msEncrypt.ToArray();
					}
				}
			}

			// Return the encrypted bytes from the memory stream.
			return encrypted;
		}

		static string AesDecrypt(byte[] cipherText, byte[] Key, byte[] IV)
		{
			// Check arguments.
			if (cipherText == null || cipherText.Length <= 0)
				throw new ArgumentNullException("cipherText");
			if (Key == null || Key.Length <= 0)
				throw new ArgumentNullException("Key");
			if (IV == null || IV.Length <= 0)
				throw new ArgumentNullException("IV");

			// Declare the string used to hold
			// the decrypted text.
			string plaintext = null;

			// Create an Aes object
			// with the specified key and IV.
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = Key;
				aesAlg.IV = IV;

				// Create a decryptor to perform the stream transform.
				ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

				// Create the streams used for decryption.
				using (MemoryStream msDecrypt = new MemoryStream(cipherText))
				{
					using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader srDecrypt = new StreamReader(csDecrypt))
						{

							// Read the decrypted bytes from the decrypting stream
							// and place them in a string.
							plaintext = srDecrypt.ReadToEnd();
						}
					}
				}
			}

			return plaintext;
		}

		public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
		{
			try
			{
				byte[] encryptedData;
				//Create a new instance of RSACryptoServiceProvider.
				using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
				{

					//Import the RSA Key information. This only needs
					//toinclude the public key information.
					RSA.ImportParameters(RSAKeyInfo);

					//Encrypt the passed byte array and specify OAEP padding.  
					//OAEP padding is only available on Microsoft Windows XP or
					//later.  
					encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
				}
				return encryptedData;
			}
			//Catch and display a CryptographicException  
			//to the console.
			catch (CryptographicException e)
			{
				Console.WriteLine(e.Message);

				return null;
			}
		}

		public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
		{
			try
			{
				byte[] decryptedData;
				//Create a new instance of RSACryptoServiceProvider.
				using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
				{
					//Import the RSA Key information. This needs
					//to include the private key information.
					RSA.ImportParameters(RSAKeyInfo);

					//Decrypt the passed byte array and specify OAEP padding.  
					//OAEP padding is only available on Microsoft Windows XP or
					//later.  
					decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
				}
				return decryptedData;
			}
			//Catch and display a CryptographicException  
			//to the console.
			catch (CryptographicException e)
			{
				Console.WriteLine(e.ToString());

				return null;
			}
		}
	}
}