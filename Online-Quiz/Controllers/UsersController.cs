using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Quiz.Models;

namespace Online_Quiz.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly OnlineQuizContext _context;

        public UsersController(OnlineQuizContext context) {
            _context = context;
        }

        [HttpGet("CheckDuplicate/{email}")]
        public bool CheckDuplicate(string? email) {
            return _context.Users.Where(u => u.Email == email).Count() > 0;
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user) {
            if (_context.Users.Where(u => u.Email == user.Email).Count() > 0) {
                return null;
            } else {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = user.UserId }, user);
            }

        }

        private bool UserExists(int id) {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
