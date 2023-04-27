using ApiCutAndGoApp.Repositores;
using CutAndGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase {

        private RepositoryHairdresser repo;

        public AppointmentsController(RepositoryHairdresser repo) {
            this.repo = repo;
        }

        [HttpGet] [Route("[action]/{appointmentId}")]
        public async Task<ActionResult<Appointment?>> FindAppointment(int appointmentId) {
            return await this.repo.FindAppoinmentAsync(appointmentId);
        }
        
        [HttpGet] [Route("[action]/{userId}")]
        public async Task<ActionResult<List<Appointment>>> GetAppointmentsByUser(int userId) {
            return await this.repo.GetAppointmentsByUserAsync(userId);
        }
        
        [HttpGet] [Route("[action]/{hairdresserId}")]
        public async Task<ActionResult<List<Appointment>>> GetAppointmentsByHairdresser(int hairdresserId) {
            return await this.repo.GetAppointmentsByHairdresserAsync(hairdresserId);
        }

        [HttpPost] [Route("[action]")]
        public async Task<ActionResult<Response>> InsertAppointment(Appointment appointment) {
            return await this.repo.InsertAppointmentAsync(appointment.UserId, appointment.HairdresserId, appointment.Date, appointment.Time);
        }
        
        [HttpPut] [Route("[action]")]
        public async Task<ActionResult<Response>> UpdateAppointment(Appointment appointment) {
            return await this.repo.UpdateAppointmentAsync(appointment.AppointmentId, appointment.Date, appointment.Time);
        }

        [HttpDelete] [Route("[action]/{appointmentId}")]
        public async Task<ActionResult<Response>> DeleteAppointment(int appointmentId) {
            return await this.repo.DeleteAppointmentAsync(appointmentId);
        }

        [HttpPut] [Route("[action]/{appointmentId}")]
        public async Task<ActionResult<Response>> ApproveAppointment(int appointmentId) {
            return await this.repo.ApproveAppointmentAsync(appointmentId);
        }

    }
}
