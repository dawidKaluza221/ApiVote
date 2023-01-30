using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiVote.Data;
using ApiVote.UserInfo;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authorization;

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
        /*
        private User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var name = claim.Where(x => x.Type==ClaimTypes.Name).FirstOrDefault();
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new User
                {
                    Name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    GivenName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,

                };
            }
            return null;
        }

        //Get/Roll
        [HttpGet("Admins")]
        //[Authorize]
        public IActionResult AdminEndPoint() 
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var currentUser = GetCurrentUser();
            return Ok($" Siema {currentUser.Email} jestes {currentUser.Role}");
        }
        //Get/Public
        [HttpGet("Public")]
        public IActionResult Public()
        {
            return Ok(" Siema  jestes ");
        }*/

        //Get/AllPoll
        [HttpGet()]
        public ActionResult<OwnerPoll> GetAllPoll()
        {
            var result = _context.OwnerPolls.ToList();
            if (result.Count() == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }
       
        //Get/Poll/{id}
        [HttpGet("{id}")]
        public ActionResult<OwnerPoll> GetPoll(int id)
        {
            var result = _context.OwnerPolls.Find(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        //Get/Question/{id}
        [HttpGet("{id}")]
        public ActionResult<PoolQuestions> GetQuestion(int id)
        {
            var result = _context.PoolQuestion.Where(x => x.ID_Poll.Equals(id)).ToList();
            if (result.Count() == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        //Get/AllQuestion
        [HttpGet]
        public ActionResult<PoolQuestions> GetAllQuestion()
        {
            var result = _context.PoolQuestion.ToList();
            if (result.Count() == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        //Get/Answer/{id}
        [HttpGet("{id}")]
        public ActionResult<Answers> GetAnswer(int id)
        {
            var result = _context.Answer.Where(x => x.IDQuestion.Equals(id)).ToList();
            if (result.Count() == 0)
            {
                return NotFound();
            }

            return Ok(result);
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
        //GetAll/CounterOwnerPollAnswer/
        [HttpGet]
        public ActionResult<Answers> GetCountePollinAnswer()
        {
            var result = _context.Answer.Count()/2;
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        //GetAll/Answers/
        [HttpGet]
        public ActionResult<Answers> GetAllAnswers()
        {
            var result = _context.Answer.ToList();
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        //GetAll/users
        [HttpGet]
        public ActionResult<User> GetAll()
        {
            var result = _context.Users.ToList();
            if (result.Count == 0)
            {
                return NotFound();
            }
            return Ok(result);
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

        //CreateQuestion/CreateAnswer
        [HttpPost]
        public ActionResult<Answers> CreateAnswer(Answers answer)
        {
            if (_context.PoolQuestion.Find(answer.IDQuestion) == null)
            {
                return NotFound("Error you don't have souch Poll ID in database");
            }
            if (answer.IDAnswer == 0)
            {
                _context.Answer.Add(answer);
            }
            _context.SaveChanges();
            return Ok(answer);
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
        [HttpPost]
        public ActionResult<Answers> VoteAnswer(Answers answer)
        {
            if (_context.Answer.Find(answer.IDAnswer) == null)
            {
                return NotFound("Error you don't have souch answer in database");
            }
            else
            {
                var OwnerInDb = _context.Answer.Find(answer.IDAnswer);
                if (OwnerInDb == null)
                {
                    return NotFound();
                }
                answer = OwnerInDb;
                answer.CounterAnswer++;
                var Aswers = _context.Answer.Where(x => x.IDQuestion == answer.IDQuestion).ToList();
                _context.SaveChanges();
                return Ok(Aswers.ToList());
            }
            
        }

        [HttpDelete("{id}")]
        public ActionResult<Answers> Delete(int id)
        {
            var result = _context.Answer.Find(id);
            if (result == null)
            {
                return NotFound();
            }
            _context.Answer.Remove(result);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
