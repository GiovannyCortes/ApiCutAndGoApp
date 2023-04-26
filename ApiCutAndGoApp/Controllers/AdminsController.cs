using CutAndGo.Interfaces;
using CutAndGo.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase {

        private IRepositoryHairdresser repo;

        public AdminsController(IRepositoryHairdresser repo) { 
            this.repo = repo;
        }

        [HttpGet] [Route("[action]/{hairdresserId}/{userId}")]
        public async Task<ActionResult<Admin?>> FindAdmin(int hairdresserId, int userId) {
            return await this.repo.FindAdminAsync(hairdresserId, userId);
        }

        [HttpGet] [Route("[action]/{hairdresserId}")]
        public async Task<ActionResult<List<Admin>>> GetAdmins(int hairdresserId) {
            return await this.repo.GetAdminsAsync(hairdresserId);
        }
        
        [HttpGet] [Route("[action]/{hairdresserId}/{userId}")]
        public async Task<ActionResult<bool>> AdminExist(int hairdresserId, int userId) {
            return await this.repo.AdminExistAsync(hairdresserId, userId);
        }
        
        [HttpGet] [Route("[action]/{hairdresserId}/{userIdMe}/{userIdOther}")]
        public async Task<ActionResult<bool>> CompareAdminRole(int hairdresserId, int userIdMe, int userIdOther) {
            return await this.repo.CompareAdminRoleAsync(hairdresserId, userIdMe, userIdOther);
        }

        [HttpPost] [Route("[action]")] 
        public async Task<ActionResult> InsertAdmin(Admin admin) {
            Response response = await this.repo.InsertAdminAsync(admin.HairdresserId, admin.UserId, admin.Role);
            return Ok(response);
        }
        
        [HttpPut] [Route("[action]")] 
        public async Task<ActionResult> UpdateAdmin(Admin admin) {
            Response response = await this.repo.UpdateAdminAsync(admin.HairdresserId, admin.UserId, admin.Role);
            return Ok(response);
        }
        
        [HttpDelete] [Route("[action]")] 
        public async Task<ActionResult> DeleteAdmin(int hairdresserId, int userIdMe, int userIdOther) {
            Response response = await this.repo.DeleteAdminAsync(hairdresserId, userIdMe, userIdOther);
            return Ok(response);
        }

    }
}
