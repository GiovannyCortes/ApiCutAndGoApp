using ApiCutAndGoApp.Repositores;
using CutAndGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleRowsController : ControllerBase {

        private RepositoryHairdresser repo;

        public ScheduleRowsController(RepositoryHairdresser repo) {
            this.repo = repo;
        }

        [HttpGet] [Route("[action]/{scheduleRowId}")]
        public async Task<ActionResult<Schedule_Row?>> FindScheduleRow(int scheduleRowId) {
            return await this.repo.FindScheduleRowAsync(scheduleRowId);
        }
        
        [HttpGet] [Route("[action]/{scheduleId}")]
        public async Task<ActionResult<List<Schedule_Row>>> GetScheduleRows(int scheduleId) {
            return await this.repo.GetScheduleRowsAsync(scheduleId);
        }

        [HttpGet] [Route("[action]/{hairdresserId}")]
        public async Task<ActionResult<List<Schedule_Row>>> GetActiveScheduleRows(int hairdresserId) {
            return await this.repo.GetActiveScheduleRowsAsync(hairdresserId);
        }

        [HttpPost] [Route("[action]")]
        public async Task<ActionResult<Response>> InsertScheduleRows(Schedule_Row srow) {
            return await this.repo.InsertScheduleRowsAsync(srow.ScheduleId, 
                                                           srow.Start, srow.End,
                                                           srow.Monday, srow.Tuesday, srow.Wednesday, 
                                                           srow.Thursday, srow.Friday, srow.Saturday, srow.Sunday);
        }

        [HttpGet] [Route("[action]")]
        public async Task<ActionResult<Response>> ValidateScheduleRow(Schedule_Row scheduleRow) {
            return await this.repo.ValidateScheduleRowAsync(scheduleRow);
        }

        [HttpDelete] [Route("[action]/{scheduleRowId}")]
        public async Task<ActionResult<Response>> DeleteScheduleRows(int scheduleRowId) {
            return await this.repo.DeleteScheduleRowsAsync(scheduleRowId);
        }

    }
}
