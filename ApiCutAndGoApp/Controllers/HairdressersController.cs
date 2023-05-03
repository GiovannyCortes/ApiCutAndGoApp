using CutAndGo.Models;
using CutAndGo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class HairdressersController : ControllerBase {
        
        private IRepositoryHairdresser repo;

        public HairdressersController(IRepositoryHairdresser repo) {
            this.repo = repo;
        }

        [HttpGet] [Route("[action]/{hairdresserId}")] [Authorize]
        public async Task<ActionResult<Hairdresser?>> FindHairdresser(int hairdresserId) {
            return await this.repo.FindHairdresserAsync(hairdresserId);
        }
        
        [HttpGet] [Route("[action]/{hairdresserId}")] [Authorize]
        public async Task<ActionResult<List<string>>> GetHairdresserEmails(int hairdresserId) {
            return await this.repo.GetHairdresserEmailsAsync(hairdresserId);
        }

        [HttpGet] [Route("[action]")] [Authorize]
        public async Task<ActionResult<List<Hairdresser>>> GetHairdressers() {
            return await this.repo.GetHairdressersAsync();
        }

        [HttpGet] [Route("[action]/{userId}")] [Authorize]
        public async Task<ActionResult<List<Hairdresser>>> GetHairdressersByUser(int userId) {
            return await this.repo.GetHairdressersByUserAsync(userId);
        }

        [HttpGet] [Route("[action]/{filterName}")] [Authorize]
        public async Task<ActionResult<List<Hairdresser>>> GetHairdressersByFilterName(string filterName) {
            return await this.repo.GetHairdressersByFilterNameAsync(filterName);
        }

        [HttpPost] [Route("[action]/{userId}")] [Authorize]
        public async Task<ActionResult<Response>> InsertHairdresser(Hairdresser hairdresser, int userId) {
            return await this.repo.InsertHairdresserAsync(hairdresser.Name, hairdresser.Phone, hairdresser.Address, hairdresser.PostalCode, userId);
        }
        
        [HttpPut] [Route("[action]")] [Authorize]
        public async Task<ActionResult<Response>> UpdateHairdresser(Hairdresser hairdresser) {
            return await this.repo.UpdateHairdresserAsync(hairdresser.HairdresserId, hairdresser.Name, hairdresser.Phone, hairdresser.Address, hairdresser.PostalCode);
        }

        [HttpDelete] [Route("[action]/{hairdresserId}")] [Authorize]
        public async Task<ActionResult<Response>> DeleteHairdresser(int hairdresserId) {
            return await this.repo.DeleteHairdresserAsync(hairdresserId);
        }
        
    }
}
