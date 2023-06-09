﻿using CutAndGo.Models;
using CutAndGo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;
using NSwag.Annotations;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTag("ADMINS")]
    public class AdminsController : ControllerBase {
        
        private IRepositoryHairdresser repo;

        public AdminsController(IRepositoryHairdresser repo) { 
            this.repo = repo;
        }

        #region CRUD ACTIONS
        // GET: /api/admins/FindAdmin/{hairdresserId}/{userId}
        /// <summary>Obtiene un ADMINISTRADOR de la tabla ADMINS, filtrando por su PK.</summary>
        /// <param name="hairdresserId">ID (GUID) de la peluquería.</param>
        /// <param name="userId">ID (GUID) del usuario.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>      
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>     
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("[action]/{hairdresserId}/{userId}")]
        [Authorize]
        public async Task<ActionResult<Admin?>> FindAdmin(int hairdresserId, int userId) {
            Admin? admin = await this.repo.FindAdminAsync(hairdresserId, userId);
            return (admin != null) ? Ok(admin) : NotFound();
        }

        // GET: /api/admins/GetAdmins/{hairdresserId}
        /// <summary>Obtiene una lista de ADMINISTRADORES de la tabla ADMINS, filtrando por la peluquería.</summary>
        /// <param name="hairdresserId">ID (GUID) de la peluquería.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>      
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>     
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]/{hairdresserId}")] [Authorize]
        public async Task<ActionResult<List<Admin>>> GetAdmins(int hairdresserId) {
            return Ok(await this.repo.GetAdminsAsync(hairdresserId));
        }

        // POST: /api/admins/Create
        /// <summary>Crea un nuevo ADMINISTRADOR en la tabla ADMINS.</summary>
        /// <remarks>Propiedades necesarias: HairdresserId, UserId, Role</remarks>
        /// <response code="200">OK. Objeto creado satisfactoriamente.</response>
        /// <response code="409">Conflict. El administrador no ha podido ser creado satisfactoriamente.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost] [Route("[action]")] [Authorize]
        public async Task<ActionResult> Create(Admin admin) {
            Response response = await this.repo.InsertAdminAsync(admin.HairdresserId, admin.UserId, admin.Role);
            if (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) {
                return Ok(response.SatisfactoryId);
            } else {
                return Conflict(new {
                    ErrorCode = response.ErrorCode,
                    ErrorMessage = response.ErrorMessage
                });
            }
        }

        // PUT: /api/admins/Update
        /// <summary>Actualiza el role de un ADMINISTRADOR de la tabla ADMINS.</summary>
        /// <remarks>Propiedades necesarias: HairdresserId, UserId, Role</remarks>
        /// <response code="200">OK. Modificación realizada satisfactoriamente.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. Se ha producido un error en la actualización. Error devuelto en la respuesta.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut] [Route("[action]")] [Authorize]
        public async Task<ActionResult> Update(Admin admin) {
            Response response = await this.repo.UpdateAdminAsync(admin.HairdresserId, admin.UserId, admin.Role);
            if (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) {
                return Ok(response.SatisfactoryId);
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

        // DELETE: /api/users/Delete/{hairdresserId}/{userIdOther}
        /// <summary>Elimina un ADMINISTRADOR de la tabla ADMINS.</summary>
        /// <param name="hairdresserId">ID (GUID) de la peluquería (PK).</param>
        /// <param name="userIdOther">ID (GUID) del usuario afectado (PK).</param>
        /// <response code="200">OK. Eliminación ejecutada satisfactoriamente.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. El administrador no ha podido ser eliminado satisfactoriamente.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete] [Route("[action]/{hairdresserId}/{userIdOther}")] [Authorize]
        public async Task<ActionResult> Delete(int hairdresserId, int userIdOther) {
            Response response = await this.repo.DeleteAdminAsync(hairdresserId, userIdOther);
            if (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) {
                return Ok(response.SatisfactoryId);
            } else {
                return Conflict(new {
                    ErrorCode = response.ErrorCode,
                    ErrorMessage = response.ErrorMessage
                });
            }
        }
        #endregion

        #region EXTRA ACTIONS
        // GET: /api/admins/AdminExist/{hairdresserId}/{userId}
        /// <summary>Se verifica la existencia de un ADMINISTRADOR en la tabla ADMINS con la PK indicada.</summary>
        /// <param name="hairdresserId">ID (GUID) de la peluquería (PK).</param>
        /// <param name="userId">ID (GUID) del usuario afectado (PK).</param>
        /// <response code="200">OK. Devuelve TRUE o FALSE.</response>      
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>     
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]/{hairdresserId}/{userId}")] [Authorize]
        public async Task<ActionResult<bool>> AdminExist(int hairdresserId, int userId) {
            return Ok(await this.repo.AdminExistAsync(hairdresserId, userId));
        }

        // GET: /api/admins/CompareAdminRole/{hairdresserId}/{userIdOther}
        /// <summary>Se verifica si el ADMINISTRADOR actual posee privilegios sobre el admin indicado por ID.</summary>
        /// <param name="hairdresserId">ID (GUID) de la peluquería (PK).</param>
        /// <param name="userIdOther">ID (GUID) del usuario afectado (PK).</param>
        /// <response code="200">OK. Devuelve TRUE o FALSE.</response>      
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>     
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]/{hairdresserId}/{userIdOther}")] [Authorize]
        public async Task<ActionResult<bool>> CompareAdminRole(int hairdresserId, int userIdOther) {
            return Ok(await this.repo.CompareAdminRoleAsync(hairdresserId, userIdOther));
        }
        #endregion

    }
}
