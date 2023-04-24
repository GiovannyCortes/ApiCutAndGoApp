using ApiCutAndGoApp.Models;
using ApiCutAndGoApp.Repositores;
using Microsoft.AspNetCore.Mvc;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {

        private RepositoryHairdresser repo;

        public UsersController(RepositoryHairdresser repo) {
            this.repo = repo;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(User user) {
            await this.repo.InsertUserAsync(user.PasswordRead, user.Name, user.LastName, user.Phone, user.Email, user.EmailConfirmed);
            return Ok();
        }

    }
}
