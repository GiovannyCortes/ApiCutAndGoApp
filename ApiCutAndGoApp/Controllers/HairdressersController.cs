using CutAndGo.Models;
using CutAndGo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NSwag.Annotations;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTag("HAIRDRESSERS")]
    public class HairdressersController : ControllerBase {
        
        private IRepositoryHairdresser repo;

        public HairdressersController(IRepositoryHairdresser repo) {
            this.repo = repo;
        }

        #region CRUD ACTIONS
        // GET: /api/hairdressers/FindHairdresser/{hairdresserId}
        /// <summary>Obtiene una PELUQUERÍA de la tabla HAIRDRESSERS, filtrado por su Id.</summary>
        /// <param name="hairdresserId">ID (GUID) de la peluquería.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet] [Route("[action]/{hairdresserId}")] [Authorize]
        public async Task<ActionResult<Hairdresser>> FindHairdresser(int hairdresserId) {
            Hairdresser? hairdresser = await this.repo.FindHairdresserAsync(hairdresserId);
            return (hairdresser != null) ? Ok(hairdresser) : NotFound();
        }

        // GET: /api/hairdressers/GetHairdressers
        /// <summary>Obtiene una lista con todas las PELUQUERÍAS de la BBDD.</summary>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet] [Route("[action]")] [Authorize]
        public async Task<ActionResult<List<Hairdresser>>> GetHairdressers() {
            return Ok(await this.repo.GetHairdressersAsync());
        }

        // GET: /api/hairdressers/GetHairdressersByUser/{userId}
        /// <summary>Obtiene una lista con todas las PELUQUERÍAS del usuario filtrado por Id.</summary>
        /// <param name="userId">ID (GUID) del usuario.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet] [Route("[action]/{userId}")] [Authorize]
        public async Task<ActionResult<List<Hairdresser>>> GetHairdressersByUser(int userId) {
            return Ok(await this.repo.GetHairdressersByUserAsync(userId));
        }

        // GET: /api/hairdressers/GetHairdressersByUser/{userId}
        /// <summary>Obtiene una lista de PELUQUERÍAS cuyo nombre contenga la cadena especificada.</summary>
        /// <param name="filterName">Cadena de texto referida un nombre de peluquería</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet] [Route("[action]/{filterName}")] [Authorize]
        public async Task<ActionResult<List<Hairdresser>>> GetHairdressersByFilterName(string filterName) {
            return Ok(await this.repo.GetHairdressersByFilterNameAsync(filterName));
        }

        // POST: /api/hairdressers/Create
        /// <summary>Crea una nueva PELUQUERÍA en la tabla HAIRDRESSERS.</summary>
        /// <remarks>Propiedades necesarias: Name, Phone, Address, PostalCode, userId</remarks>
        /// <response code="200">OK. Objeto creado satisfactoriamente. Nuevo ID devuelto</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. La cita no ha podido ser creada satisfactoriamente.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost] [Route("[action]")] [Authorize]
        public async Task<ActionResult> Create(HairdresserRegister hdrs) {
            Response response = await this.repo.InsertHairdresserAsync(hdrs.Name, hdrs.Phone, hdrs.Address, hdrs.PostalCode, hdrs.ImageExtension, hdrs.UserId);
            return (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) ? Ok(new { satisfactoryId = response.SatisfactoryId }) : Conflict();
        }

        // PUT: /api/hairdressers/Update
        /// <summary>Actualiza una PELUQUERÍA de la tabla HAIRDRESSERS.</summary>
        /// <remarks>Propiedades necesarias: HairdresserId, Name, Phone, Address, PostalCode</remarks>
        /// <response code="200">OK. Modificación realizada satisfactoriamente. ID editado, devuelto</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. Se ha producido un error en la actualización.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut] [Route("[action]")] [Authorize]
        public async Task<ActionResult> Update(HairdresserUpdates hdrs) {
            Response response = await this.repo.UpdateHairdresserAsync(hdrs.HairdresserId, hdrs.Name, hdrs.Phone, hdrs.Address, hdrs.PostalCode, hdrs.ImageExtension);
            return (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) ? Ok(new { satisfactoryId = response.SatisfactoryId }) : Conflict();
        }

        // DELETE: /api/hairdressers/Delete/{hairdresserId}
        /// <summary>Elimina una PELUQUERÍA de la tabla HAIRDRESSERS.</summary>
        /// <param name="hairdresserId">ID (GUID) de la peluquería.</param>
        /// <response code="200">OK. Eliminación ejecutada satisfactoriamente. ID eliminado, devuelto</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. La cita no ha podido ser eliminada satisfactoriamente.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete] [Route("[action]/{hairdresserId}")] [Authorize]
        public async Task<ActionResult> Delete(int hairdresserId) {
            Response response = await this.repo.DeleteHairdresserAsync(hairdresserId);
            return (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) ? Ok(new { satisfactoryId = response.SatisfactoryId }) : Conflict();
        }
        #endregion

        #region EXTRA ACTIONS
        // GET: /api/hairdressers/GetHairdresserEmails/{hairdresserId}
        /// <summary>Obtiene todos los emails de los administradores de una PELUQUERÍA, filtrado por su Id.</summary>
        /// <param name="hairdresserId">ID (GUID) de la peluquería.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet] [Route("[action]/{hairdresserId}")] [Authorize]
        public async Task<ActionResult<List<string>>> GetHairdresserEmails(int hairdresserId) {
            return Ok(await this.repo.GetHairdresserEmailsAsync(hairdresserId));
        }
        #endregion

    }
}
