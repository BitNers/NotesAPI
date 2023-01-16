using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotesAPI.Database;
using NotesAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UserController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public struct LoginStruct
        {
            public string email { get; set; } 
            public string password { get; set; }
        }

        [HttpPost("login")]
        public async Task<string> GetLogin([FromBody] LoginStruct loginStruct)
        {
            if (loginStruct.email.Equals("") || loginStruct.password.Equals(""))
                return "Must have email and password filled";


            var usrFound = await _context.Users.Where(op => op.Email == loginStruct.email).FirstOrDefaultAsync() ;

            if (usrFound == null)
                return "User not found.";

            byte[] passwordSaltBytes = Convert.FromBase64String(usrFound.PasswordSalt ?? "");

            string generatedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                  password: loginStruct.password ?? "1",
                  salt: passwordSaltBytes,
                  prf: KeyDerivationPrf.HMACSHA1,
                  iterationCount: 10000,
                  numBytesRequested: 256 / 8));

            bool passwordMatch = (generatedPassword == usrFound.Password);

            if (!passwordMatch)
                return "Invalid credentials";

            try
            {
                var claimsIdentity = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Email, usrFound.Email.ToString()),
                    new Claim(ClaimTypes.Name,  usrFound.Username.ToString()),
                    new Claim(ClaimTypes.Role, "User")
                    }, CookieAuthenticationDefaults.AuthenticationScheme
                    );

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5),

                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            }
            catch (Exception err) {
                Console.WriteLine("[X] Erro: ", err.Message);
                throw;
            }

            return "ok, logado!";
        }

        [HttpGet("logout")]
        public async Task<string> LogoutAsync() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return "Session cleared.";
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserModel(int id)
        {
            var userModel = await _context.Users.FindAsync(id);

            if (userModel == null)
            {
                return NotFound();
            }

            return userModel;
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserModel>> PostUserModel(UserModel userModel)
        {

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create()) { rng.GetNonZeroBytes(salt); }

            userModel.PasswordSalt = Convert.ToBase64String(salt);

            userModel.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: userModel.Password ?? "1",
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

            _context.Users.Add(userModel);


            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserModel", new { id = userModel.UserID }, userModel);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserModel(int id)
        {
            var userModel = await _context.Users.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }

            _context.Users.Remove(userModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserModelExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
