using CutAndGo.Models;
using CutAndGo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleRowsController : ControllerBase {
        
        private IRepositoryHairdresser repo;

        public ScheduleRowsController(IRepositoryHairdresser repo) {
            this.repo = repo;
        }

        [HttpGet] [Route("[action]/{scheduleRowId}")] [Authorize]
        public async Task<ActionResult<Schedule_Row?>> FindScheduleRow(int scheduleRowId) {
            return await this.repo.FindScheduleRowAsync(scheduleRowId);
        }
        
        [HttpGet] [Route("[action]/{scheduleId}")] [Authorize]
        public async Task<ActionResult<List<Schedule_Row>>> GetScheduleRows(int scheduleId) {
            return await this.repo.GetScheduleRowsAsync(scheduleId);
        }

        [HttpGet] [Route("[action]/{hairdresserId}")] [Authorize]
        public async Task<ActionResult<List<Schedule_Row>>> GetActiveScheduleRows(int hairdresserId) {
            return await this.repo.GetActiveScheduleRowsAsync(hairdresserId);
        }

        [HttpPost] [Route("[action]")] [Authorize]
        public async Task<ActionResult<Response>> InsertScheduleRows(Schedule_Row srow) {
            return await this.repo.InsertScheduleRowsAsync(srow.ScheduleId, 
                                                           srow.Start, srow.End,
                                                           srow.Monday, srow.Tuesday, srow.Wednesday, 
                                                           srow.Thursday, srow.Friday, srow.Saturday, srow.Sunday);
        }

        [HttpGet] [Route("[action]")] [Authorize]
        public async Task<ActionResult<Response>> ValidateScheduleRow(Schedule_Row scheduleRow) {
            return await this.repo.ValidateScheduleRowAsync(scheduleRow);
        }

        [HttpDelete] [Route("[action]/{scheduleRowId}")] [Authorize]
        public async Task<ActionResult<Response>> DeleteScheduleRows(int scheduleRowId) {
            return await this.repo.DeleteScheduleRowsAsync(scheduleRowId);
        }
        
    }
}
