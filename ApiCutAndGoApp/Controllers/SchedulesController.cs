using CutAndGo.Models;
using CutAndGo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase {
        
        private IRepositoryHairdresser repo;

        public SchedulesController(IRepositoryHairdresser repo) {
            this.repo = repo;
        }

        [HttpGet] [Route("[action]/{scheduleId}/{getRows}")] [Authorize]
        public async Task<ActionResult<Schedule?>> FindSchedule(int scheduleId, bool getRows) {
            return await this.repo.FindScheduleAsync(scheduleId, getRows);
        }
        
        [HttpGet] [Route("[action]/{hairdresserId}/{getRows}")] [Authorize]
        public async Task<ActionResult<Schedule?>> FindActiveSchedule(int hairdresserId, bool getRows) {
            return await this.repo.FindActiveScheduleAsync(hairdresserId, getRows);
        }

        [HttpGet] [Route("[action]/{hairdresserId}")] [Authorize]
        public async Task<ActionResult<List<string>>> GetNameSchedules(int hairdresserId) {
            return await this.repo.GetNameSchedulesAsync(hairdresserId);
        }

        [HttpGet] [Route("[action]/{hairdresserId}/{getRows}")] [Authorize]
        public async Task<ActionResult<List<Schedule>>> GetSchedules(int hairdresserId, bool getRows) {
            return await this.repo.GetSchedulesAsync(hairdresserId, getRows);
        }

        [HttpPost] [Route("[action]")] [Authorize]
        public async Task<ActionResult<Response>> InsertSchedule(Schedule schedule) {
            return await this.repo.InsertScheduleAsync(schedule.HairdresserId, schedule.Name, schedule.Active);
        }
        
        [HttpPut] [Route("[action]")] [Authorize]
        public async Task<ActionResult<Response>> UpdateSchedule(Schedule schedule) {
            return await this.repo.UpdateScheduleAsync(schedule.ScheduleId, schedule.HairdresserId, schedule.Name, schedule.Active);
        }

        [HttpDelete] [Route("[action]/{scheduleId}")] [Authorize]
        public async Task<ActionResult<Response>> DeleteScheduleAsync(int scheduleId) {
            return await this.repo.DeleteScheduleAsync(scheduleId);
        }

        [HttpPut] [Route("[action]/{hairdresserId}/{scheduleId}")] [Authorize]
        public async Task<ActionResult<Response>> ActivateSchedule(int hairdresserId, int scheduleId) {
            return await this.repo.ActivateScheduleAsync(hairdresserId, scheduleId);
        }
        
    }
}
