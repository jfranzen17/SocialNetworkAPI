using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using APILibrary.Repositories.Users;
using APILibrary.Repositories;
using APILibrary.Models.Users;
using APILibrary.Models.Posts;

namespace Documentation.Controllers
{
    //Controller route api/Users
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //GET api/users/GetUsers
        [HttpGet]
        public IEnumerable<User> GetAllQueried([FromQuery] UserQueryDto userQuery)
        {
            return RunUserQuery(userQuery);
        }

        //GET api/users/5
        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<User> GetUserById(int id)
        {
            User user = _userRepository.GetUserWithId(id);
            if (user is null)
                return NotFound(user);
            return user;
        }

        //GET api/users/CreateUser
        [HttpPost]
        public ActionResult CreateUser(UserDto usertDto)
        {
            try
            {
                var user = _userRepository.Add(usertDto);
                return CreatedAtAction(nameof(usertDto.Name), new { id = user.Id }, user);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        //GET api/users/ReplaceUser
        [HttpPut]
        [Route("{id:int}")]
        public ActionResult ReplaceUser(int id, UserDto userPut)
        {
            var user = _userRepository.GetUserWithId(id);
            if (user is null)
                return NotFound($"No user with {id} found");
            var putUser = new User(id, userPut.Name, userPut.Email);
            _userRepository.Update(putUser);
            return NoContent();
        }

        //GET api/users/UpdateUser
        [HttpPatch]
        [Route("{id:int}")]
        public ActionResult UpdateUser(int id, Dictionary<string, object> patches)
        {
            var user = _userRepository.GetUserWithId(id);
            if (user is null)
                return NotFound($"No post with {id} found");
            _userRepository.ApplyPatch(user, patches);
            return NoContent();
        }

        //GET api/users/DeleteUser
        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult DeleteUser(int id)
        {
            var user = _userRepository.GetUserWithId(id);
            if (user is null)
                return NotFound($"No user with {id} found");
            _userRepository.Delete(user);
            return NoContent();
        }

        private IEnumerable<User> RunUserQuery(UserQueryDto userQueryDto)
        {
            if (userQueryDto.IsEmpty)
                return _userRepository.GetUsers();
            else if (!(userQueryDto.Email is null))
                return _userRepository.GetUserByEmail(userQueryDto.Email);
            else if (!(userQueryDto.Name is null))
                return _userRepository.GetUserByName(userQueryDto.Name);
            else
                throw new NotSupportedException("The query combination selected is not supported");
        }

    }
}
