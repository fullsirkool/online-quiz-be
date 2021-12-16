using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Quiz.Models;

namespace Online_Quiz.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase {
        private readonly OnlineQuizContext _context;

        public RolesController(OnlineQuizContext context) {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRole() {
            return await _context.Roles.ToListAsync();
        }

        private bool RoleExists(int id) {
            return _context.Roles.Any(e => e.RoleId == id);
        }
    }
}
