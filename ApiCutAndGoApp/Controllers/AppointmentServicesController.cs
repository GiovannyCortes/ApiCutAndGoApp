using CutAndGo.Models;
using CutAndGo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NSwag.Annotations;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTag("APPOINTMENT_SERVICES")]
    public class AppointmentServicesController : ControllerBase {
        
        private IRepositoryHairdresser repo;

        public AppointmentServicesController(IRepositoryHairdresser repo) {
            this.repo = repo;
        }

        #region CRUD ACTIONS
        // GET: /api/appointmentservices/FindAppointmentService/{appointmentId}/{serviceId}
        /// <summary>Obtiene un registro de la tabla APPOINTMENT_SERVICES</summary>
        /// <param name="appointmentId">ID (GUID) de la cita.</param>
        /// <param name="serviceId">ID (GUID) del servicio.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>      
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>     
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet] [Route("[action]/{appointmentId}/{serviceId}")] [Authorize]
        public async Task<ActionResult<Appointment_Service>> FindAppointmentService(int appointmentId, int serviceId) {
            Appointment_Service? appointment_Service = await this.repo.FindAppointmentServiceAsync(appointmentId, serviceId);
            return (appointment_Service != null) ? Ok(appointment_Service) : NotFound();
        }

        // GET: /api/appointmentservices/GetAppointmentServicesIDs/{appointmentId}
        /// <summary>Obtiene una Lista de IDs de registros de la tabla APPOINTMENT_SERVICES</summary>
        /// <param name="appointmentId">ID (GUID) de la cita.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>      
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>     
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]/{appointmentId}")] [Authorize]
        public async Task<ActionResult<List<int>>> GetAppointmentServicesIDs(int appointmentId) {
            return Ok(await this.repo.GetAppointmentServicesIDsAsync(appointmentId));
        }

        // GET: /api/appointmentservices/GetAppointmentServices/{appointmentId}
        /// <summary>Obtiene una Lista de registros (objeto) de la tabla APPOINTMENT_SERVICES</summary>
        /// <param name="appointmentId">ID (GUID) de la cita.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>  
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]/{appointmentId}")] [Authorize]
        public async Task<ActionResult<List<Appointment_Service>>> GetAppointmentServices(int appointmentId) {
            return Ok(await this.repo.GetAppointmentServicesAsync(appointmentId));
        }

        // POST: /api/appointmentservices/create
        /// <summary>Crea un nuevo registro en la tabla APPOINTMENT_SERVICES.</summary>
        /// <remarks>Propiedades necesarias: AppointmentId, ServiceId</remarks>
        /// <response code="200">OK. Objeto creado satisfactoriamente. Nuevo ID devuelto</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>  
        /// <response code="409">Conflict. La cita no ha podido ser creada satisfactoriamente.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost] [Route("[action]/{appointmentId}/{serviceId}")] [Authorize]
        public async Task<ActionResult> Create(int appointmentId, int serviceId) { 
            Response response = await this.repo.InsertAppointmentServiceAsync(appointmentId, serviceId);
            return (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) ? Ok(new { satisfactoryId = response.SatisfactoryId }) : Conflict();
        }

        // DELETE: /api/appointmentservices/delete/{appointmentId}/{serviceId}
        /// <summary>Elimina una CTIA de la tabla APPOINTMENTS.</summary>
        /// <param name="appointmentId">ID (GUID) de la cita.</param>
        /// <param name="serviceId">ID (GUID) del servicio.</param>
        /// <response code="200">OK. Eliminación ejecutada satisfactoriamente. ID eliminado, devuelto</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. La cita no ha podido ser eliminada satisfactoriamente.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete] [Route("[action]/{appointmentId}/{serviceId}")] [Authorize]
        public async Task<ActionResult> Delete(int appointmentId, int serviceId) {
            Response response = await this.repo.DeleteAppointmentServiceAsync(appointmentId, serviceId);
            return (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) ? Ok(new { satisfactoryId = response.SatisfactoryId }) : Conflict();
        }
        #endregion

    }
}