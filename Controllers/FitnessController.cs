using FitnessTakip.Data;
using FitnessTakip.DTOs;
using FitnessTakip.Model;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTakip.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FitnessController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FitnessController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<dtoListUsersRequest> GetUsers()
        {
            List<dtoListUsersRequest> dto = _context.Users.Select(x => new dtoListUsersRequest()
            {
                Id = x.Id,
                Name = x.Name,
                Created = x.Created
            }).ToList();
            return dto;
        }

        [HttpGet("{id}")]
        public dtoListUsersRequest getUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user is null)
                return new dtoListUsersRequest();
            return new dtoListUsersRequest
            {
                Id = user.Id,
                Name = user.Name,
                Created = user.Created
            };
        }

        [HttpPost]
        public dtoListUsersRequest AddUser([FromBody] dtoListUsersRequest user)
        {
            if (user.Id is not 0)
            {
                var userToUpdate = _context.Users.Find(user.Id);
                if (userToUpdate is null)
                    return new dtoListUsersRequest();
                userToUpdate.Name = user.Name;
                userToUpdate.Created = user.Created;
                _context.Users.Update(userToUpdate);
                _context.SaveChanges();
                return new dtoListUsersRequest
                {
                    Id = userToUpdate.Id,
                    Name = userToUpdate.Name,
                    Created = userToUpdate.Created
                };
            }
            var newUser = new User
            {
                Name = user.Name,
                Created = user.Created
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return new dtoListUsersRequest
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Created = newUser.Created
            };
        }

        [HttpDelete("{id}")]
        public bool DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user is null)
                return false;
            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}
