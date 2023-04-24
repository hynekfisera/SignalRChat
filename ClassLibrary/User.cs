using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	public class User
	{
        public Guid UserId { get; set; }
		public string? Email { get; set; }
        public byte[] rsaPublicKey { get; set; }

		public User(Guid userId, string email, byte[] key)
        {
            UserId = userId;
            Email = email;
            rsaPublicKey = key;
        }
    }
}
