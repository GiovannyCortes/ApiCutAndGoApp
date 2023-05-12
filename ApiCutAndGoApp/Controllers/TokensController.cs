using ApiCutAndGoApp.Helpers;
using ApiCutAndGoApp.Repositores;
using CutAndGo.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTag("TOKENS")]
    public class TokensController : ControllerBase {

        private IRepositoryHairdresser repo;
        private RepositoryHairdresser priv_repo;
        private HelperOAuthToken helper;

        public TokensController(IRepositoryHairdresser repo, RepositoryHairdresser priv_repo, HelperOAuthToken helper) {
            this.repo = repo;
            this.helper = helper;
            this.priv_repo = priv_repo;
        }

        // GET: /api/tokens/GenerateToken
        /// <summary>{Gestión de Tokens}</summary>
        /// <response code="200">OK. Cliente identificado, token devuelto</response>
        /// <response code="401">Unauthorized. Credenciales incorrectas.</response>  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]")]
        public ActionResult<string> GenerateToken() {
            return Ok(this.priv_repo.GenerateToken());
        }

        // POST: /api/tokens/UserAssignTokenAsync
        /// <summary>{Gestión de Tokens}</summary>
        /// <param name="userId">ID (GUID) del usuario.</param>
        /// <param name="token">Token del usuario</param>
        /// <response code="200">OK. Cliente identificado, asignación completada</response>
        /// <response code="401">Unauthorized. Credenciales incorrectas.</response>  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost] [Route("[action]")] [Authorize]
        public async Task<ActionResult<bool>> UserAssignTokenAsync(int userId, string token) {
            return Ok(await this.repo.UserAssignTokenAsync(userId, token));
        }

        // GET: /api/tokens/UserValidateTokenAsync
        /// <summary>{Gestión de Tokens}</summary>
        /// <param name="userId">ID (GUID) del usuario.</param>
        /// <param name="token">Token del usuario</param>
        /// <response code="200">OK. Cliente identificado, comprobación realizada</response>
        /// <response code="401">Unauthorized. Credenciales incorrectas.</response>  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]")] [Authorize]
        public async Task<ActionResult<bool>> UserValidateTokenAsync(int userId, string token) {
            return Ok(await this.repo.UserValidateTokenAsync(userId, token));
        }

        // GET: /api/tokens/HairdresserValidateTokenAsync
        /// <summary>{Gestión de Tokens}</summary>
        /// <param name="hairdresserId">ID (GUID) de la peluquería.</param>
        /// <param name="token">Token de la peluquería</param>
        /// <response code="200">OK. Cliente identificado, comprobación realizada</response>
        /// <response code="401">Unauthorized. Credenciales incorrectas.</response>  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]")] [Authorize]
        public async Task<ActionResult<bool>> HairdresserValidateTokenAsync(int hairdresserId, string token) {
            return Ok(await this.repo.HairdresserValidateTokenAsync(hairdresserId, token));
        }

    }
}