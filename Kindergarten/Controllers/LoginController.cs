using Kindergarten.Data;
using Kindergarten.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Kindergarten.Controllers
{
    public class LoginController : Controller
    {
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        private readonly MyDbContext _context;

        public LoginController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Login(string ReturnUrl = "/")
        {
            UserLoginModel loginrequest = new UserLoginModel();
            loginrequest.ReturnUrl = ReturnUrl;
            return View(loginrequest);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel request)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Logins
                    .Where(x => x.UserName == request.Username)
                    .FirstOrDefault();
                if (user is null)
                {
                    ModelState.AddModelError("", "Incorrect Login attempt");
                    return View(request);
                }
                else
                {
                    if (!VerifyPassword(request.Password, user.HashedPassword, user.SaltPassword))
                    {
                        ModelState.AddModelError("", "Incorrect Login attempt");
                        return View(request);
                    }
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.UserName)
                    };

                    //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity
                    var principal = new ClaimsPrincipal(identity);

                    //SignInAsync is a Extension method for Sign in a principal for the specified scheme.
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        principal, new AuthenticationProperties() { IsPersistent = request.RememberMe });

                    return RedirectToAction("Guardian");

                }
            }
            ModelState.AddModelError("", "Error Request");
            return View(request);
        }

        private bool VerifyPassword(string password, string hashedPassword, byte[] saltPassword)
        {
                var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, saltPassword, iterations, hashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hashedPassword));
        }

        public IActionResult Guardian()
        {
            return View();
        }
        public IActionResult view()
        {
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }
        public IActionResult Update()
        {
            return View();
        }
        public IActionResult ForgetPass()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
    }


}
