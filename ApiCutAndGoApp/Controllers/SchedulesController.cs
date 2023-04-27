using ApiCutAndGoApp.Repositores;
using CutAndGo.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase {

        private RepositoryHairdresser repo;

        public SchedulesController(RepositoryHairdresser repo) {
            this.repo = repo;
        }

        [HttpGet] [Route("[action]/{scheduleId}/{getRows}")]
        public async Task<ActionResult<Schedule?>> FindSchedule(int scheduleId, bool getRows) {
            return await this.repo.FindScheduleAsync(scheduleId, getRows);
        }
        
        [HttpGet] [Route("[action]/{hairdresserId}/{getRows}")]
        public async Task<ActionResult<Schedule?>> FindActiveSchedule(int hairdresserId, bool getRows) {
            return await this.repo.FindActiveScheduleAsync(hairdresserId, getRows);
        }

        [HttpGet] [Route("[action]/{hairdresserId}")]
        public async Task<ActionResult<List<string>>> GetNameSchedules(int hairdresserId) {
            return await this.repo.GetNameSchedulesAsync(hairdresserId);
        }

        [HttpGet] [Route("[action]/{hairdresserId}/{getRows}")]
        public async Task<ActionResult<List<Schedule>>> GetSchedules(int hairdresserId, bool getRows) {
            return await this.repo.GetSchedulesAsync(hairdresserId, getRows);
        }

        [HttpPost] [Route("[action]")]
        public async Task<ActionResult<Response>> InsertSchedule(Schedule schedule) {
            return await this.repo.InsertScheduleAsync(schedule.HairdresserId, schedule.Name, schedule.Active);
        }
        
        [HttpPut] [Route("[action]")]
        public async Task<ActionResult<Response>> UpdateSchedule(Schedule schedule) {
            return await this.repo.UpdateScheduleAsync(schedule.ScheduleId, schedule.HairdresserId, schedule.Name, schedule.Active);
        }

        [HttpDelete] [Route("[action]/{scheduleId}")]
        public async Task<ActionResult<Response>> DeleteScheduleAsync(int scheduleId) {
            return await this.repo.DeleteScheduleAsync(scheduleId);
        }

        [HttpPut] [Route("[action]/{hairdresserId}/{scheduleId}")]
        public async Task<ActionResult<Response>> ActivateSchedule(int hairdresserId, int scheduleId) {
            return await this.repo.ActivateScheduleAsync(hairdresserId, scheduleId);
        }

    }
}
