using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.DTOs.Authentications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto, CancellationToken ct)
        {
            var result = await _authenticationService.LoginAsync(loginDto, ct);
            return ToActionResult(result);
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto, CancellationToken ct)
        {
            var result = await _authenticationService.RegisterAsync(registerDto, ct);
            return ToActionResult(result);
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmail(string email, CancellationToken ct = default)
        {
            var result = await _authenticationService.CheckEmailAsync(email, ct);
            return ToActionResult(result);
        }

        [Authorize]
        [HttpGet("currentuser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser( CancellationToken ct = default)
        {
            var result = await _authenticationService.GetCurrentUserAsync(GetEmailFromToken(), ct);
            return ToActionResult(result);
        }
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDTO>> GetUserAddress( CancellationToken ct = default)
        {
            var result = await _authenticationService.GetAddressAsync(GetEmailFromToken(), ct);
            return ToActionResult(result);
        }
        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO addressDTO, CancellationToken ct = default)
        {
            var result = await _authenticationService.UpdateAddressAsync(GetEmailFromToken(), addressDTO, ct);
            return ToActionResult(result);
        }



    }
}
