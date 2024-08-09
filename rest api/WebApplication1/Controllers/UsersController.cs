using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Shared.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public UsersController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-users")]
        public ActionResult<IEnumerable<UserModel>> GetUsers()
        {
            var users = _context.Users.Select(u => new UserModel
            {
                ID = u.ID,
                ADI = u.ADI,
                SOYADI = u.SOYADI,
                KULLANICI_ADI = u.KULLANICI_ADI
            }).ToList();

            return Ok(users);
        }
    }
}
