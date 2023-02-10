using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.AspNetCore.Identity;

namespace Identity.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AuthorizationController(IOpenIddictApplicationManager applicationManager, 
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _applicationManager = applicationManager;
            _userManager = userManager;
            _signInManager = signInManager; 
        }

        [HttpPost("~/connect/token"), Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ?? null;
            if (request == null)
                throw new Exception();


            if (!request.IsPasswordGrantType())
                 throw new NotImplementedException("The specified grant is not implemented.");

            // If "Grant_Type" == Password 
            // Need to validate the User Identity.
            if (request.IsPasswordGrantType())
            {
                string userPass = request.Password ?? string.Empty;
                string userMail = request.Username ?? string.Empty;

                var result = await _userManager.FindByEmailAsync(userMail);
                if (result == null)
                    return NotFound($"The E-mail '{userMail}' is not registered.");

                if (!await _userManager.CheckPasswordAsync(result, userPass))
                    return Ok("E-mail or password not valid.");


                ClaimsPrincipal claimsPrincipalPassword;
                ClaimsIdentity identityPassword = new ClaimsIdentity(
                        TokenValidationParameters.DefaultAuthenticationType, 
                        Claims.Email, 
                        Claims.Role);

                identityPassword.AddClaims(new[] {

                    new Claim(Claims.Subject, result.Id),
                    new Claim(Claims.Name, "Username"),
                    new Claim(Claims.Email, "Email"),
                });


                claimsPrincipalPassword = new ClaimsPrincipal(identityPassword);
                claimsPrincipalPassword.SetResources("metallica:bonjovi");

                return SignIn(claimsPrincipalPassword, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            else {
                throw new Exception("Error while validating Authentication");
            }

            // ClaimsPrincipal claimsClientCredentials;

            // // Note: the client credentials are automatically validated by OpenIddict:
            // // if client_id or client_secret are invalid, this action won't be invoked.
            // 
            // var application = await _applicationManager.FindByClientIdAsync(request.ClientId ?? "") ??
            //     throw new InvalidOperationException("The application cannot be found.");
            // 
            // // Create a new ClaimsIdentity containing the claims that
            // // will be used to create an id_token, a token or a code.
            // var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType, Claims.Name, Claims.Role);
            // 
            // // Use the client_id as the subject identifier.
            // 
            // identity.AddClaim(Claims.Subject, "UserSubject");
            // identity.AddClaim(Claims.Name, "UserName");
            // 
            // claimsClientCredentials = new ClaimsPrincipal(identity);
            // claimsClientCredentials.SetResources("console");
            // 
            // // identity.AddClaims(Claims.Subject,
            // //     await _applicationManager.GetClientIdAsync(application),
            // //     Destinations.AccessToken, Destinations.IdentityToken);
            // 
            // 
            // return SignIn(claimsClientCredentials, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
    }
}

// ?client_id=my-client&redirect_uri=https%3A%2F%2Foidcdebugger.com%2Fdebug&scope=openid%20profile%20offline_access&response_type=token%20id_token&response_mode=fragment&nonce=ujihoe7seq
