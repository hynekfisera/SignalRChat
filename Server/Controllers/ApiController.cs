using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
	[Route("/api")]
	[ApiController]
	public class ApiController : ControllerBase
	{
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;
		public ApiController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}
		public JsonResult Test()
		{
			return new JsonResult(new { Message = "Hello World!" });
		}
		[HttpPost("login")]
		public JsonResult Login([FromForm] string email, [FromForm] string password)
		{
			return new JsonResult(new {Email = email, Password = password});
			//var result = await _signInManager.PasswordSignInAsync(email, password, true, false);
			//if (result.Succeeded)
			//{
			//	//var user = await _userManager.GetUserAsync(User);
			//	return new JsonResult(new { Success = true });
			//}
			//else
			//{
			//	return new JsonResult(new { Success = false });
			//}
		}
	}
}
