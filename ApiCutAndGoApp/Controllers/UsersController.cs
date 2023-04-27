using CutAndGo.Models;
using Microsoft.AspNetCore.Mvc;
using CutAndGo.Interfaces;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {

        private IRepositoryHairdresser repo;

        public UsersController(IRepositoryHairdresser repo) {
            this.repo = repo;
        }

        [HttpGet] [Route("[action]/{userId}")]
        public async Task<ActionResult> UserIsAdmin(int userId) {
            bool response = await this.repo.UserIsAdminAsync(userId);
            return Ok(response);
        }

        [HttpGet] [Route("[action]/{email}")]
        public async Task<ActionResult> EmailExist(string email) {
            bool response = await this.repo.EmailExistAsync(email);
            return Ok(response);
        }

        [HttpGet] [Route("[action]/{userId}")]
        public async Task<ActionResult<User?>> FindUser(int userId) {
            return await this.repo.FindUserAsync(userId);
        }

        [HttpPost] [Route("[action]")]
        public async Task<ActionResult<User?>> CreateUser(User user) {
            return await this.repo.InsertUserAsync(user.PasswordRead, user.Name, user.LastName, user.Phone, user.Email, user.EmailConfirmed);
        }

        [HttpPut] [Route("[action]")]
        public async Task<ActionResult<Response>> UpdateUser(User user) {
            Response response = await this.repo.UpdateUserAsync(user.UserId, user.Name, user.LastName, user.Phone, user.Email);
            return Ok(response);
        }

        [HttpDelete] [Route("[action]/{userId}")]
        public async Task<ActionResult<Response>> DeleteUser(int userId) {
            Response response = await this.repo.DeleteUserAsync(userId);
            return Ok(response);
        }

        [HttpPut] [Route("[action]")]
        public async Task<ActionResult<Response>> ValidateEmail(int userId) {
            Response response = await this.repo.ValidateEmailAsync(userId);
            return Ok(response);
        }

    }
}
