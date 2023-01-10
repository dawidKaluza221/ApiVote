using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        //CreateUser/EditUser
        [HttpPost]
        public ActionResult<User> CreateEditUser(User user)
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
        //CreateQuestion/CreateQuestion
        [HttpPost]
        public ActionResult<PoolQuestions> CreateQuestion(PoolQuestions poolQuestion)
        {
            if (_context.OwnerPolls.Find(poolQuestion.ID_Poll) == null)
            {
                return NotFound("Error you don't have souch Poll ID in database");
            }
            if (poolQuestion.ID_Question == 0)
            {
                _context.PoolQuestion.Add(poolQuestion);
            }
           _context.SaveChanges();
            return Ok(poolQuestion);
        }
        //CreatePoll/EditPoll
        [HttpPost]
        public ActionResult<OwnerPoll> CreateEditPoll(OwnerPoll ownerPoll)
        {
            if (_context.Users.Find(ownerPoll.ID_User) == null) 
            {
                return NotFound("Error you don't have souch User ID in database");
            }
            if (ownerPoll.ID_Poll == 0)
            {
                _context.OwnerPolls.Add(ownerPoll);
            }
            else
            {
                if (_context.OwnerPolls.Find(ownerPoll.ID_Poll) == null)
                {
                    return NotFound("Error you don't have souch Poll in database use ID = 0 to create ");
                }
                var OwnerInDb = _context.OwnerPolls.Find(ownerPoll.ID_Poll);
                
                if (OwnerInDb == null)
                {
                    return NotFound();
                }
                OwnerInDb = ownerPoll;
            }
            _context.SaveChanges();
            return Ok(ownerPoll);
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
