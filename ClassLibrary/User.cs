using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	[Serializable]
	public class User
	{
        public Guid UserId { get; set; }
		public string? Email { get; set; }
        public byte[] rsaPublicKey { get; set; }
        public User()
        {
            rsaPublicKey = new byte[20];
        }

        public User(Guid userId, string email, byte[] key)
        {
            UserId = userId;
            Email = email;
            rsaPublicKey = key;
        }
    }
}
