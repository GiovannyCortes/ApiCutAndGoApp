using ApiCutAndGoApp.Repositores;
using CutAndGo.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentServicesController : ControllerBase {

        private RepositoryHairdresser repo;

        public AppointmentServicesController(RepositoryHairdresser repo) {
            this.repo = repo;
        }

        [HttpGet] [Route("[action]/{appointmentId}/{serviceId}")]
        public async Task<ActionResult<Appointment_Service?>> FindAppointmentService(int appointmentId, int serviceId) {
            return await this.repo.FindAppointmentServiceAsync(appointmentId, serviceId);
        }

        [HttpGet] [Route("[action]/{appointmentId}")]
        public async Task<ActionResult<List<int>>> GetAppointmentServicesIDs(int appointmentId) {
            return await this.repo.GetAppointmentServicesIDsAsync(appointmentId);
        }
        
        [HttpGet] [Route("[action]/{appointmentId}")]
        public async Task<ActionResult<List<Appointment_Service>>> GetAppointmentServices(int appointmentId) {
            return await this.repo.GetAppointmentServicesAsync(appointmentId);
        }

        [HttpPost] [Route("[action]/{appointmentId}/{serviceId}")]
        public async Task<ActionResult<Response>> InsertAppointmentService(int appointmentId, int serviceId) { 
            return await this.repo.InsertAppointmentServiceAsync(appointmentId, serviceId);
        }

        [HttpDelete] [Route("[action]/{appointmentId}/{serviceId}")]
        public async Task<ActionResult<Response>> DeleteAppointmentService(int appointmentId, int serviceId) {
            return await this.repo.DeleteAppointmentServiceAsync(appointmentId, serviceId);
        }

    }
}
