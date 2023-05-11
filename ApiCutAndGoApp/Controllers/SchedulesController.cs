using CutAndGo.Models;
using CutAndGo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NSwag.Annotations;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTag("SCHEDULES")]
    public class SchedulesController : ControllerBase {
        
        private IRepositoryHairdresser repo;

        public SchedulesController(IRepositoryHairdresser repo) {
            this.repo = repo;
        }

        #region CRUD ACTIONS
        // GET: /api/schedules/FindSchedule/{scheduleId}/{getRows}
        /// <summary>Obtiene un HORARIO de la tabla SCHEDULES.</summary>
        /// <param name="scheduleId">ID (GUID) del horario.</param>
        /// <param name="getRows">Determina si se deben recuperar, a su vez, sus registros de horarios (Boolean).</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet] [Route("[action]/{scheduleId}/{getRows}")] [Authorize]
        public async Task<ActionResult<Schedule>> FindSchedule(int scheduleId, bool getRows) {
            Schedule? schedule = await this.repo.FindScheduleAsync(scheduleId, getRows);
            return (schedule != null) ? Ok(schedule) : NotFound();
        }

        // GET: /api/schedules/FindActiveSchedule/{hairdresserId}/{getRows}
        /// <summary>Obtiene el HORARIO activo de la tabla SCHEDULES.</summary>
        /// <param name="hairdresserId">ID (GUID) de la peluquería.</param>
        /// <param name="getRows">Determina si se deben recuperar, a su vez, sus registros de horarios (Boolean).</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet] [Route("[action]/{hairdresserId}/{getRows}")] [Authorize]
        public async Task<ActionResult<Schedule>> FindActiveSchedule(int hairdresserId, bool getRows) {
            Schedule? schedule = await this.repo.FindActiveScheduleAsync(hairdresserId, getRows);
            return (schedule != null) ? Ok(schedule) : NotFound();
        }

        // GET: /api/schedules/FindActiveSchedule/{hairdresserId}
        /// <summary>Obtiene los nombres de los HORARIOS de una peluquería, filtrada por ID</summary>
        /// <param name="hairdresserId">ID (GUID) de la peluquería.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]/{hairdresserId}")] [Authorize]
        public async Task<ActionResult<List<string>>> GetNameSchedules(int hairdresserId) {
            List<string> scheduleIds = await this.repo.GetNameSchedulesAsync(hairdresserId);
            return (scheduleIds != null) ? Ok(scheduleIds) : NotFound();

        }

        // GET: /api/schedules/GetSchedules/{hairdresserId}/{getRows}
        /// <summary>Obtiene los HORARIOS de una peluquería, filtrada por ID</summary>
        /// <param name="hairdresserId">ID (GUID) de la peluquería.</param>
        /// <param name="getRows">Determina si se deben recuperar, a su vez, sus registros de horarios (Boolean).</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]/{hairdresserId}/{getRows}")] [Authorize]
        public async Task<ActionResult<List<Schedule>>> GetSchedules(int hairdresserId, bool getRows) {
            List<Schedule> schedules = await this.repo.GetSchedulesAsync(hairdresserId, getRows);
            return (schedules != null) ? Ok(schedules) : NotFound();
        }

        // POST: /api/schedules/create
        /// <summary>Crea un nuevo HORARIO en la tabla SCHEDULES.</summary>
        /// <remarks>Propiedades necesarias: HairdresserId, Name, Active</remarks>
        /// <response code="200">OK. Horario creado satisfactoriamente.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>     
        /// <response code="409">Conflict. El registro no ha podido ser creado satisfactoriamente.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost] [Route("[action]")] [Authorize]
        public async Task<ActionResult> Create(ScheduleRegister schedule) {
            Response response = await this.repo.InsertScheduleAsync(schedule.HairdresserId, schedule.Name, schedule.Active);
            if (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) {
                return Ok(new { satisfactoryId = response.SatisfactoryId });
            } else {
                return Conflict(new {
                    ErrorCode = response.ErrorCode,
                    ErrorMessage = response.ErrorMessage
                });
            }
        }

        // PUT: /api/schedules/update
        /// <summary>Actualiza un HORARIO de la tabla SCHEDULES.</summary>
        /// <remarks>Propiedades necesarias:ScheduleId, HairdresserId, Name, Active</remarks>
        /// <response code="200">OK. Modificación realizada satisfactoriamente.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. Se ha producido un error en la actualización. Error devuelto en la respuesta.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut] [Route("[action]")] [Authorize]
        public async Task<ActionResult> Update(ScheduleUpdates schedule) {
            Response response = await this.repo.UpdateScheduleAsync(schedule.ScheduleId, schedule.HairdresserId, schedule.Name, schedule.Active);
            if (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) {
                return Ok(new { satisfactoryId = response.SatisfactoryId });
            } else {
                return Conflict(new {
                    ErrorCode = response.ErrorCode,
                    ErrorMessage = response.ErrorMessage
                });
            }
        }

        // DELETE: /api/schedules/delete
        /// <summary>Elimina un HORARIO de la tabla SCHEDULES.</summary>
        /// <param name="scheduleId">ID (GUID) del horario.</param>
        /// <response code="200">OK. Modificación realizada satisfactoriamente.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. Se ha producido un error en la actualización. Error devuelto en la respuesta.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete] [Route("[action]/{scheduleId}")] [Authorize]
        public async Task<ActionResult> Delete(int scheduleId) {
            Response response = await this.repo.DeleteScheduleAsync(scheduleId);
            if (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) {
                return Ok(new { satisfactoryId = response.SatisfactoryId });
            } else {
                return Conflict(new {
                    ErrorCode = response.ErrorCode,
                    ErrorMessage = response.ErrorMessage
                });
            }
        }
        #endregion

        #region EXTRA ACTIONS
        // PUT: /api/schedules/update
        /// <summary>Activa un horario de la tabla SCHEDULES.</summary>
        /// <param name="hairdresserId">ID (GUID) de la peluquería.</param>
        /// <param name="scheduleId">ID (GUID) del horario.</param>
        /// <response code="200">OK. Modificación realizada satisfactoriamente.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. Se ha producido un error en la actualización. Error devuelto en la respuesta.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut] [Route("[action]/{hairdresserId}/{scheduleId}")] [Authorize]
        public async Task<ActionResult> ActivateSchedule(int hairdresserId, int scheduleId) {
            Response response = await this.repo.ActivateScheduleAsync(hairdresserId, scheduleId);
            if (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) {
                return Ok(new { satisfactoryId = response.SatisfactoryId });
            } else {
                return Conflict(new {
                    ErrorCode = response.ErrorCode,
                    ErrorMessage = response.ErrorMessage
                });
            }
        }
        #endregion
    }
}
