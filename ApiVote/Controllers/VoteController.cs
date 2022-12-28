using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiVote.Data;
using ApiVote.Data;
using ApiVote.UserInfo;

namespace ApiVote.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly ApiContext _context;

        public VoteController(ApiContext context)
        {
            _context = context;
        }

        //Create/Edit
        [HttpPost]
        public ActionResult<User> CreateEdit(User user)
        {
            if (user.Id == 0)
            {
                _context.Users.Add(user);
            }
            else
            {
                var userInDb = _context.Users.Find(user.Id);
                if (userInDb == null)
                {
                    return NotFound();
                }
                userInDb = user;
            }
            _context.SaveChanges();
            return Ok(user);
        }
        //Get/user/{id}
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var result = _context.Users.Find(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public ActionResult<User>Delete(int id)
        {
            var result = _context.Users.Find(id);
            if (result == null)
            {
                return NotFound();
            }
            _context.Users.Remove(result);
            _context.SaveChanges();
            return NoContent();
        }
        //GetAll
        [HttpGet()]
        public ActionResult<IEnumerable<User>> GetAll() 
        {
            var result = _context.Users.ToList();
            if (result.Count == 0) 
            {
                return NotFound();
            }
            return (result);
        }
    }
}
