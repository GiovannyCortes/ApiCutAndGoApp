using ApiCutAndGoApp.Repositores;
using CutAndGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase {

        private RepositoryHairdresser repo;

        public ServicesController(RepositoryHairdresser repo) {
            this.repo = repo;
        }

        [HttpGet] [Route("[action]/{serviceId}")]
        public async Task<ActionResult<Service?>> FindService(int serviceId) {
            return await this.repo.FindServiceAsync(serviceId);
        }

        [HttpGet] [Route("[action]/{appointmentId}")]
        public async Task<ActionResult<List<Service>>> GetServicesByAppointment(int appointmentId) {
            return await this.repo.GetServicesByAppointmentAsync(appointmentId);
        }

        [HttpGet] [Route("[action]/{hairdresserId}")]
        public async Task<ActionResult<List<Service>>> GetServicesByHairdresser(int hairdresserId) {
            return await this.repo.GetServicesByHairdresserAsync(hairdresserId);
        }

        [HttpGet] [Route("[action]")]
        public async Task<ActionResult<List<Service>>> GetServicesByIdentification(List<int> app_services) {
            return await this.repo.GetServicesByIdentificationAsync(app_services);
        }

        [HttpPost] [Route("[action]")]
        public async Task<ActionResult<Response>> InsertService(Service service) {
            return await this.repo.InsertServiceAsync(service.HairdresserId, service.Name, service.Price, service.TiempoAprox);
        }
        
        [HttpPut] [Route("[action]")]
        public async Task<ActionResult<Response>> UpdateService(Service service) {
            return await this.repo.UpdateServiceAsync(service.HairdresserId, service.Name, service.Price, service.TiempoAprox);
        }
        
        [HttpDelete] [Route("[action]/{serviceId}")]
        public async Task<ActionResult<Response>> DeleteService(int serviceId) {
            return await this.repo.DeleteServiceAsync(serviceId);
        }

    }
}
