using CutAndGo.Interfaces;
using CutAndGo.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiCutAndGoApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CredentialsController : ControllerBase {

        private IRepositoryHairdresser repo;

        public CredentialsController(IRepositoryHairdresser repo) {
            this.repo = repo;
        }

        [HttpGet] [Route("[action]/{email}/{password}")]
        public async Task<ActionResult<User?>> LogIn(string email, string password) {
            return await this.repo.LogInAsync(email, password);
        }

    }
}
