using CutAndGo.Models;
using CutAndGo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase {
        
        private IRepositoryHairdresser repo;

        public ServicesController(IRepositoryHairdresser repo) {
            this.repo = repo;
        }

        [HttpGet] [Route("[action]/{serviceId}")] [Authorize]
        public async Task<ActionResult<Service?>> FindService(int serviceId) {
            return await this.repo.FindServiceAsync(serviceId);
        }

        [HttpGet] [Route("[action]/{appointmentId}")] [Authorize]
        public async Task<ActionResult<List<Service>>> GetServicesByAppointment(int appointmentId) {
            return await this.repo.GetServicesByAppointmentAsync(appointmentId);
        }

        [HttpGet] [Route("[action]/{hairdresserId}")] [Authorize]
        public async Task<ActionResult<List<Service>>> GetServicesByHairdresser(int hairdresserId) {
            return await this.repo.GetServicesByHairdresserAsync(hairdresserId);
        }

        [HttpGet] [Route("[action]")] [Authorize]
        public async Task<ActionResult<List<Service>>> GetServicesByIdentification(List<int> app_services) {
            return await this.repo.GetServicesByIdentificationAsync(app_services);
        }

        [HttpPost] [Route("[action]")] [Authorize]
        public async Task<ActionResult<Response>> InsertService(Service service) {
            return await this.repo.InsertServiceAsync(service.HairdresserId, service.Name, service.Price, service.TiempoAprox);
        }
        
        [HttpPut] [Route("[action]")] [Authorize]
        public async Task<ActionResult<Response>> UpdateService(Service service) {
            return await this.repo.UpdateServiceAsync(service.HairdresserId, service.Name, service.Price, service.TiempoAprox);
        }
        
        [HttpDelete] [Route("[action]/{serviceId}")] [Authorize]
        public async Task<ActionResult<Response>> DeleteService(int serviceId) {
            return await this.repo.DeleteServiceAsync(serviceId);
        }
        
    }
}
