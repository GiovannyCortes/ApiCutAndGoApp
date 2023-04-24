using ApiCutAndGoApp.Repositores;
using ApiCutAndGoApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CredentialsController : ControllerBase {

        private RepositoryHairdresser repo;

        public CredentialsController(RepositoryHairdresser repo) {
            this.repo = repo;
        }

        [HttpGet] [Route("[action]/{email}/{password}")]
        public async Task<ActionResult<User?>> LogIn(string email, string password) {
            return await this.repo.LogInAsync(email, password);
        }

    }
}
