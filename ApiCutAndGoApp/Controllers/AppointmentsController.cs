using CutAndGo.Models;
using CutAndGo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NSwag.Annotations;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTag("APPOINTMENTS")]
    public class AppointmentsController : ControllerBase {
        
        private IRepositoryHairdresser repo;

        public AppointmentsController(IRepositoryHairdresser repo) {
            this.repo = repo;
        }

        #region CRUD ACTIONS
        // GET: /api/appointments/FindAppointment/{appointmentId}
        /// <summary>Obtiene una CITA de la tabla APPOINTMENTS, filtrando por su Id.</summary>
        /// <param name="appointmentId">ID (GUID) de la cita.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>      
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>     
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet] [Route("[action]/{appointmentId}")] [Authorize]
        public async Task<ActionResult<Appointment?>> FindAppointment(int appointmentId) {
            Appointment? appointment = await this.repo.FindAppoinmentAsync(appointmentId);
            return (appointment != null) ? Ok(appointment) : NotFound();
        }

        // GET: /api/appointments/GetAppointmentsByUser/{userId}
        /// <summary>Obtiene una lista de CITAS de la tabla APPOINTMENTS, filtrando por usuario.</summary>
        /// <param name="userId">ID (GUID) del usuario del que obtener las citas.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>      
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>     
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]/{userId}")] [Authorize]
        public async Task<ActionResult<List<Appointment>>> GetAppointmentsByUser(int userId) {
            return Ok(await this.repo.GetAppointmentsByUserAsync(userId));
        }

        // GET: /api/appointments/GetAppointmentsByHairdresser/{hairdresserId}
        /// <summary>Obtiene una lista de CITAS de la tabla APPOINTMENTS, filtrando por peluquería.</summary>
        /// <param name="hairdresserId">ID (GUID) de la peluquería de la que obtener las citas.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>      
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>     
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]/{hairdresserId}")] [Authorize]
        public async Task<ActionResult<List<Appointment>>> GetAppointmentsByHairdresser(int hairdresserId) {
            return Ok(await this.repo.GetAppointmentsByHairdresserAsync(hairdresserId));
        }

        // POST: /api/appointments/create
        /// <summary>Crea una nueva CITA en la tabla APPOINTMENTS.</summary>
        /// <remarks>Propiedades necesarias: UserId, HairdresserId, Date, Time</remarks>
        /// <response code="200">OK. Objeto creado satisfactoriamente. Nuevo ID devuelto</response>
        /// <response code="409">Conflict. La cita no ha podido ser creada satisfactoriamente.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost] [Route("[action]")] [Authorize]
        public async Task<ActionResult> Create(Appointment appointment) {
            Response response = await this.repo.InsertAppointmentAsync(appointment.UserId, appointment.HairdresserId, appointment.Date, appointment.Time);
            return (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) ? Ok(response.SatisfactoryId) : Conflict();
        }

        // PUT: /api/appointments/update
        /// <summary>Actualiza una CITA de la tabla APPOINTMENTS.</summary>
        /// <remarks>Propiedades necesarias: AppointmentId, Date, Time</remarks>
        /// <response code="200">OK. Modificación realizada satisfactoriamente. ID editado, devuelto</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. Se ha producido un error en la actualización.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut] [Route("[action]")] [Authorize]
        public async Task<ActionResult> Update(Appointment appointment) {
            Response response = await this.repo.UpdateAppointmentAsync(appointment.AppointmentId, appointment.Date, appointment.Time);
            return (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) ? Ok(response.SatisfactoryId) : Conflict();
        }

        // DELETE: /api/appointments/delete/{appointmentId}
        /// <summary>Elimina una CTIA de la tabla APPOINTMENTS.</summary>
        /// <param name="appointmentId">ID (GUID) de la cita.</param>
        /// <response code="200">OK. Eliminación ejecutada satisfactoriamente. ID eliminado, devuelto</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. La cita no ha podido ser eliminada satisfactoriamente.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete] [Route("[action]/{appointmentId}")] [Authorize]
        public async Task<ActionResult> Delete(int appointmentId) {
            Response response = await this.repo.DeleteAppointmentAsync(appointmentId);
            return (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) ? Ok(response.SatisfactoryId) : Conflict();
        }
        #endregion

        #region EXTRA ACTIONS
        // PUT: /api/appointments/ApproveAppointment/{appointmentId}
        /// <summary>Se aprueba una CITA de la tabla APPOINTMENTS</summary>
        /// <param name="appointmentId">ID (GUID) de la cita.</param>
        /// <response code="200">OK. Modificación realizada satisfactoriamente. ID editado, devuelto</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. Se ha producido un error en la actualización.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut] [Route("[action]/{appointmentId}")] [Authorize]
        public async Task<ActionResult> ApproveAppointment(int appointmentId) {
            Response response = await this.repo.ApproveAppointmentAsync(appointmentId);
            return (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) ? Ok(response.SatisfactoryId) : Conflict();
        }
        #endregion

    }
}
