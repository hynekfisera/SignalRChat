using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
	public class User
	{
        public Guid UserId { get; set; }
		public string? Email { get; set; }
        public User(Guid userId, string email)
        {
            UserId = userId;
            Email = email;
        }
    }
}
