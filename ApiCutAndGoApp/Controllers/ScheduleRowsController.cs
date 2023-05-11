using CutAndGo.Models;
using CutAndGo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NSwag.Annotations;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTag("SCHEDULE_ROWS")]
    public class ScheduleRowsController : ControllerBase {
        
        private IRepositoryHairdresser repo;

        public ScheduleRowsController(IRepositoryHairdresser repo) {
            this.repo = repo;
        }

        #region CRUD ACTIONS
        // GET: /api/schedulerows/GetScheduleRows/{scheduleId}
        /// <summary>Obtiene una lista de REGISTROS de un horario filtrando por su ID.</summary>
        /// <param name="scheduleId">ID (GUID) del horario.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>      
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>     
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]/{scheduleId}")] [Authorize]
        public async Task<ActionResult<List<Schedule_Row>>> GetScheduleRows(int scheduleId) {
            return Ok(await this.repo.GetScheduleRowsAsync(scheduleId));
        }

        // GET: /api/schedulerows/GetActiveScheduleRows/{hairdresserId}
        /// <summary>Obtiene una lista de REGISTROS del horario activo de una peluquería fitrada por ID.</summary>
        /// <param name="hairdresserId">ID (GUID) de la peluquería.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet] [Route("[action]/{hairdresserId}")] [Authorize]
        public async Task<ActionResult<List<Schedule_Row>>> GetActiveScheduleRows(int hairdresserId) {
            return Ok(await this.repo.GetActiveScheduleRowsAsync(hairdresserId));
        }

        // GET: /api/schedulerows/FindScheduleRow/{scheduleRowId}
        /// <summary>Obtiene un REGISTRO específico de un horario.</summary>
        /// <param name="scheduleRowId">ID (GUID) del registro del horario.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet] [Route("[action]/{scheduleRowId}")] [Authorize]
        public async Task<ActionResult<Schedule_Row>> FindScheduleRow(int scheduleRowId) {
            Schedule_Row? scheduleRow = await this.repo.FindScheduleRowAsync(scheduleRowId);
            return (scheduleRow != null) ? Ok(scheduleRow) : NotFound();
        }

        // POST: /api/schedulerows/Create
        /// <summary>Crea un nuevo REGISTRO en la tabla SCHEDULE_ROWS.</summary>
        /// <remarks>Propiedades necesarias:ScheduleId, Start (hh:mm:ss), End (hh:mm:ss), Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday</remarks>
        /// <response code="200">OK. Registro creado satisfactoriamente.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>     
        /// <response code="409">Conflict. El registro no ha podido ser creado satisfactoriamente.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost] [Route("[action]")] [Authorize]
        public async Task<ActionResult> Create(Schedule_RowRegister srow) {
            Response response = await this.repo.InsertScheduleRowsAsync(srow.ScheduleId, srow.Start, srow.End, srow.Monday, srow.Tuesday, srow.Wednesday, 
                                                                        srow.Thursday, srow.Friday, srow.Saturday, srow.Sunday);
            if (response.ResponseCode == (int)IRepositoryHairdresser.ResponseCodes.OK) {
                return Ok(new { satisfactoryId = response.SatisfactoryId });
            } else {
                return Conflict(new {
                    ErrorCode = response.ErrorCode,
                    ErrorMessage = response.ErrorMessage
                });
            }
        }


        // DELETE: /api/schedulerows/Delete/{scheduleRowId}
        /// <summary>Elimina un REGISTRO de la tabla SCHEDULE_ROWS.</summary>
        /// <param name="scheduleRowId">ID (GUID) del registro de horario.</param>
        /// <response code="200">OK. Eliminación ejecutada satisfactoriamente.</response>
        /// <response code="401">Unauthorized. Cliente no autorizado.</response>
        /// <response code="409">Conflict. El registro no ha podido ser eliminado satisfactoriamente.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete] [Route("[action]/{scheduleRowId}")] [Authorize]
        public async Task<ActionResult> Delete(int scheduleRowId) {
            Response response = await this.repo.DeleteScheduleRowsAsync(scheduleRowId);
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
