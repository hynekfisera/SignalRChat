namespace Client
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			tbEmail = new TextBox();
			tbPassword = new TextBox();
			btnLogin = new Button();
			lbChat = new ListBox();
			tbMessage = new TextBox();
			btnSend = new Button();
			SuspendLayout();
			// 
			// tbEmail
			// 
			tbEmail.Location = new Point(12, 12);
			tbEmail.Name = "tbEmail";
			tbEmail.PlaceholderText = "Email";
			tbEmail.Size = new Size(100, 23);
			tbEmail.TabIndex = 0;
			tbEmail.Text = "test@hynekfisera.cz";
			// 
			// tbPassword
			// 
			tbPassword.Location = new Point(118, 12);
			tbPassword.Name = "tbPassword";
			tbPassword.PlaceholderText = "Heslo";
			tbPassword.Size = new Size(100, 23);
			tbPassword.TabIndex = 1;
			tbPassword.Text = "Zgbspb-8vbmak";
			// 
			// btnLogin
			// 
			btnLogin.Location = new Point(224, 12);
			btnLogin.Name = "btnLogin";
			btnLogin.Size = new Size(75, 23);
			btnLogin.TabIndex = 2;
			btnLogin.Text = "Přihlásit se";
			btnLogin.UseVisualStyleBackColor = true;
			btnLogin.Click += btnLogin_Click;
			// 
			// lbChat
			// 
			lbChat.Enabled = false;
			lbChat.FormattingEnabled = true;
			lbChat.ItemHeight = 15;
			lbChat.Location = new Point(12, 41);
			lbChat.Name = "lbChat";
			lbChat.Size = new Size(776, 394);
			lbChat.TabIndex = 3;
			// 
			// tbMessage
			// 
			tbMessage.Enabled = false;
			tbMessage.Location = new Point(305, 12);
			tbMessage.Name = "tbMessage";
			tbMessage.PlaceholderText = "Zpráva";
			tbMessage.Size = new Size(402, 23);
			tbMessage.TabIndex = 4;
			// 
			// btnSend
			// 
			btnSend.Enabled = false;
			btnSend.Location = new Point(713, 11);
			btnSend.Name = "btnSend";
			btnSend.Size = new Size(75, 23);
			btnSend.TabIndex = 5;
			btnSend.Text = "Odeslat";
			btnSend.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(btnSend);
			Controls.Add(tbMessage);
			Controls.Add(lbChat);
			Controls.Add(btnLogin);
			Controls.Add(tbPassword);
			Controls.Add(tbEmail);
			Name = "Form1";
			Text = "Form1";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TextBox tbEmail;
		private TextBox tbPassword;
		private Button btnLogin;
		private ListBox lbChat;
		private TextBox tbMessage;
		private Button btnSend;
	}
}