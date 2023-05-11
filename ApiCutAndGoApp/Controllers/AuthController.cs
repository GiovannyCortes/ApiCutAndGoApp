using ApiCutAndGoApp.Helpers;
using CutAndGo.Interfaces;
using CutAndGo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NSwag.Annotations;
using NugetHairdressersAzure.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTag("AUTH")]
    public class AuthController : ControllerBase {

        private IRepositoryHairdresser repo;
        private HelperOAuthToken helper;

        public AuthController(IRepositoryHairdresser repo, HelperOAuthToken helper) {
            this.repo = repo;
            this.helper = helper;
        }

        // Necesitamos un método para validad a nuestro usuario y devolver el Token de acceso. Dicho método SIEMPRE debe ser POST

        // POST: /api/auth/LogIn
        /// <summary>Obtención del Token de seguridad para los métodos de la API.</summary>
        /// <remarks>Propiedades necesarias: UserMail, Password</remarks>
        /// <response code="200">OK. Cliente identificado, token generado y devuelto</response>
        /// <response code="401">Unauthorized. Credenciales incorrectas.</response>  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost] [Route("[action]")]
        public async Task<ActionResult> LogIn(LogIn user) {
            User? usuario = await this.repo.LogInAsync(user.UserMail, user.Password);
            if (usuario == null) { // El usuario no ha superado el LogIn
                return Unauthorized();
            } else {
                // Debemos crear unas credenciales dentro del Token
                SigningCredentials credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);

                // Serialización del usuario para almacenarlo en el Token
                string jsonUsuario = JsonConvert.SerializeObject(usuario);
                Claim[] informacion = new[] {
                    new Claim("UserData", jsonUsuario)
                };

                // El Token se genera con una clase y debemos indicar los datos que conforman dicho Token
                JwtSecurityToken token = new JwtSecurityToken(
                    claims: informacion,
                    issuer: this.helper.Issuer,
                    audience: this.helper.Audience,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    notBefore: DateTime.UtcNow
                );

                return Ok(new {
                    response = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
        }

    }
}
