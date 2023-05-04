using CutAndGo.Models;
using CutAndGo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NSwag.Annotations;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTag("USERS")]
    public class UsersController : ControllerBase {
        
        private IRepositoryHairdresser repo;

        public UsersController(IRepositoryHairdresser repo) {
            this.repo = repo;
        }

        #region CRUD ACTIONS
        // GET: /api/users/{userId}
        /// <summary>Obtiene un USUARIO de la tabla USERS, filtrando por su ID.</summary>
        /// <param name="userId">ID (GUID) del usuario.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>      
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>     
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet] [Route("[action]/{userId}")] [Authorize]
        public async Task<ActionResult<User?>> FindUser(int userId) {
            User? user = await this.repo.FindUserAsync(userId);
            return (user != null)? Ok(user) : NotFound();
        }

        // POST: /api/users/create
        /// <summary>Crea un nuevo USUARIO en la tabla USERS.</summary>
        /// <remarks>Propiedades necesarias: PasswordRead, Name, LastName, Phone, Email, EmailConfirmed</remarks>
        /// <response code="200">OK. Devuelve el objeto una vez ha sido creado.</response>
        /// <response code="409">Conflict. El usuario no ha podido ser creado satisfactoriamente.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost] [Route("[action]")]
        public async Task<ActionResult<User?>> Create(User user) {
            User? usuario = await this.repo.InsertUserAsync(user.PasswordRead, user.Name, user.LastName, user.Phone, user.Email, user.EmailConfirmed);
            return (usuario != null)? Ok(usuario) : Conflict();
        }

        // PUT: /api/users/update
        /// <summary>Actualiza un USUARIO de la tabla USERS.</summary>
        /// <remarks>Propiedades necesarias: UserId, Name, LastName, Phone, Email</remarks>
        /// <response code="200">OK. Modificación realizada satisfactoriamente.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. Se ha producido un error en la actualización. Error devuelto en la respuesta.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut] [Route("[action]")] [Authorize]
        public async Task<ActionResult> Update(User user) {
            Response response = await this.repo.UpdateUserAsync(user.UserId, user.Name, user.LastName, user.Phone, user.Email);
            if (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) {
                return Ok();
            } else {
                return Conflict(new {
                    ErrorCode = response.ErrorCode,
                    ErrorMessage = response.ErrorMessage
                });
            }
        }

        // DELETE: /api/users/delete/{userId}
        /// <summary>Elimina un USUARIO de la tabla USERS.</summary>
        /// <param name="userId">ID (GUID) del usuario.</param>
        /// <response code="200">OK. Eliminación ejecutada satisfactoriamente.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. El usuario no ha podido ser eliminado satisfactoriamente.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete] [Route("[action]/{userId}")] [Authorize]
        public async Task<ActionResult> Delete(int userId) {
            Response response = await this.repo.DeleteUserAsync(userId);
            if (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) {
                return Ok();
            } else {
                return Conflict(new {
                    ErrorCode = response.ErrorCode,
                    ErrorMessage = response.ErrorMessage
                });
            }
        }
        #endregion

        #region EXTRA ACTIONS
        // GET: /api/users/UserIsAdmin/{userId}
        /// <summary>Se verifica si un usuario es administrador</summary>
        /// <param name="userId">ID (GUID) del usuario.</param>
        /// <response code="200">OK. Devuelve TRUE o FALSE.</response>      
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>     
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]/{userId}")] [Authorize]
        public async Task<ActionResult<bool>> UserIsAdmin(int userId) {
            return Ok(await this.repo.UserIsAdminAsync(userId));
        }

        // GET: /api/users/EmailExist/{userId}
        /// <summary>Se verifica si un email existe</summary>
        /// <param name="email">Email del usuario.</param>
        /// <response code="200">OK. Devuelve TRUE o FALSE.</response>      
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>     
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]/{email}")] [Authorize]
        public async Task<ActionResult<bool>> EmailExist(string email) {
            return Ok(await this.repo.EmailExistAsync(email));
        }

        // PUT: /api/users/ValidateEmail/{userId}
        /// <summary>Se valida el email de un usuario existente</summary>
        /// <remarks>Propiedades necesarias: UserId</remarks>
        /// <response code="200">OK. Modificación realizada satisfactoriamente.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="409">Conflict. Se ha producido un error en la actualización. Error devuelto en la respuesta.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut] [Route("[action]")] [Authorize]
        public async Task<ActionResult> ValidateEmail(int userId) {
            Response response = await this.repo.ValidateEmailAsync(userId);
            if (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) {
                return Ok();
            } else {
                if (response.ErrorCode == (int)IRepositoryHairdresser.ResponseErrorCodes.RecordNotFound) {
                    return NotFound();
                }
                return Conflict(new {
                    ErrorCode = response.ErrorCode,
                    ErrorMessage = response.ErrorMessage
                });
            }
        }
        #endregion
    }
}
