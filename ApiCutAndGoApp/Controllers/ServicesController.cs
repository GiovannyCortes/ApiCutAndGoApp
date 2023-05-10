using CutAndGo.Models;
using CutAndGo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NSwag.Annotations;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTag("SERVICES")]
    public class ServicesController : ControllerBase {
        
        private IRepositoryHairdresser repo;

        public ServicesController(IRepositoryHairdresser repo) {
            this.repo = repo;
        }

        #region SERVICES
        // GET: /api/services/FindService/{serviceId}
        /// <summary>Obtiene un SERVICIO de la tabla SERVICES, filtrado por su Id.</summary>
        /// <param name="serviceId">ID (GUID) del servicio.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet] [Route("[action]/{serviceId}")] [Authorize]
        public async Task<ActionResult<Service>> FindService(int serviceId) {
            Service? service = await this.repo.FindServiceAsync(serviceId);
            return (service != null) ? Ok(service) : NotFound();
        }

        // GET: /api/services/GetServicesByAppointment/{appointmentId}
        /// <summary>Obtiene una LISTA DE SERVICIOS de una determinada cita.</summary>
        /// <param name="appointmentId">ID (GUID) de la cita.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]/{appointmentId}")] [Authorize]
        public async Task<ActionResult<List<Service>>> GetServicesByAppointment(int appointmentId) {
            return Ok(await this.repo.GetServicesByAppointmentAsync(appointmentId));
        }

        // GET: /api/services/GetServicesByHairdresser/{hairdresserId}
        /// <summary>Obtiene una LISTA DE SERVICIOS de una determinada peluquería</summary>
        /// <param name="hairdresserId">ID (GUID) de la cita.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]/{hairdresserId}")] [Authorize]
        public async Task<ActionResult<List<Service>>> GetServicesByHairdresser(int hairdresserId) {
            return Ok(await this.repo.GetServicesByHairdresserAsync(hairdresserId));
        }

        // GET: /api/services/GetServicesByIdentification
        /// <summary>Obtiene una LISTA DE SERVICIOS filtrados por una lista de identificadores de servicio</summary>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]")] [Authorize]
        public async Task<ActionResult<List<Service>>> GetServicesByIdentification(List<int> app_services) {
            return Ok(await this.repo.GetServicesByIdentificationAsync(app_services));
        }

        // POST: /api/services/Create
        /// <summary>Crea un nuevo SERVICIO en la tabla SERVICES.</summary>
        /// <remarks>Propiedades necesarias: HairdresserId, Name, Price, TiempoAprox</remarks>
        /// <response code="200">OK. Objeto creado satisfactoriamente. Nuevo ID devuelto</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. La cita no ha podido ser creada satisfactoriamente.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost] [Route("[action]")] [Authorize]
        public async Task<ActionResult> Insert(Service service) {
            Response response = await this.repo.InsertServiceAsync(service.HairdresserId, service.Name, service.Price, service.TiempoAprox);
            return (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) ? Ok(response.SatisfactoryId) : Conflict();
        }

        // PUT: /api/services/Update
        /// <summary>Actualiza un SERVICIO de la tabla SERVICES.</summary>
        /// <remarks>Propiedades necesarias: ServiceId, Name, Price, TiempoAprox</remarks>
        /// <response code="200">OK. Modificación realizada satisfactoriamente. ID editado, devuelto</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. Se ha producido un error en la actualización.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut] [Route("[action]")] [Authorize]
        public async Task<ActionResult> Update(Service service) {
            Response response = await this.repo.UpdateServiceAsync(service.ServiceId, service.Name, service.Price, service.TiempoAprox);
            return (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) ? Ok(response.SatisfactoryId) : Conflict();
        }

        // DELETE: /api/services/Delete/{serviceId}
        /// <summary>Elimina un SERVICIO de la tabla SERVICES.</summary>
        /// <param name="serviceId">ID (GUID) del servicio.</param>
        /// <response code="200">OK. Eliminación ejecutada satisfactoriamente. ID eliminado, devuelto</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. La cita no ha podido ser eliminada satisfactoriamente.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete] [Route("[action]/{serviceId}")] [Authorize]
        public async Task<ActionResult> Delete(int serviceId) {
            Response response = await this.repo.DeleteServiceAsync(serviceId);
            return (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) ? Ok(response.SatisfactoryId) : Conflict();
        }
        #endregion
    }
}
