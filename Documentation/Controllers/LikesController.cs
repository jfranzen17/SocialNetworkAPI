using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using APILibrary.Repositories.Users;
using APILibrary.Repositories;
using APILibrary.Models.Users;
using APILibrary.Models.Likes;
using APILibrary.Models.Posts;

namespace Documentation.Controllers.Likes
{
    [ApiController]
    [Route("api/likes")]
    public class LikesController : ControllerBase
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public LikesController(ILikeRepository likeRepository, IUserRepository userRepository, IPostRepository postRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _likeRepository = likeRepository;
        }

        //GET api/likes
        [HttpGet]
        public IEnumerable<Like> GetAllQueried([FromQuery] LikeQueryDto likeQuery)
        {
            return RunLikeQuery(likeQuery);
        }

        //GET api/likes/LikeSwitch
        [HttpPost]
        [Route("{postid:int}/{userid:int}")]
        public ActionResult LikeSwitch(int postId, int userId)
        {
            try
            {
                var user = _userRepository.GetUserWithId(userId);
                var post = _postRepository.GetPostWithId(postId);

                var like = _likeRepository.AddLike(postId, user);
                post.LikedBy.Add(like);

                return NoContent();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        //GET api/likes/5
        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<Like> GetLike(int id)
        {
            var like = _likeRepository.GetLikesWithId(id);
            if (like is null)
                return NotFound(like);
            return like;
        }

        //GET api/likes/CreateLike
        [HttpPost]
        public ActionResult CreateLike(LikeDto likeDto)
        {
            try
            {
                var user = _userRepository.GetUserWithId(likeDto.UserId);

                var like = _likeRepository.AddLike(likeDto.PostId, user);
                return CreatedAtAction(nameof(GetLike), new { id = like }, like);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        //GET api/likes/DeleteLike
        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult DeleteLike(int postId, UserDto userDto)
        {
            var user = _userRepository.GetUserWithId(userDto.UserId);
            var like = _likeRepository.GetLikesWithId(postId);
            if (like is null)
                return NotFound($"No post with {postId} found");
            if (like.User.Guid == user.Guid)
                _likeRepository.Delete(like.PostId, user);
            return NoContent();
        }

        //GET api/likes/ReplaceLike
        [HttpPut]
        [Route("{id:int}")]
        public ActionResult ReplaceLike(int postId, UserDto userDto)
        {
            var like = _likeRepository.GetLikesWithId(postId);
            var user = _userRepository.GetUserWithId(userDto.UserId);
            if (like is null)
                return NotFound($"No like with {postId} found");
            var putLike = (postId, user);
            _likeRepository.Update(postId, user);
            return NoContent();
        }

        //GET api/likes/UpdateLike
        [HttpPatch]
        [Route("{id:int}")]
        public ActionResult UpdateLike(int id, Dictionary<string, object> patches)
        {
            var like = _likeRepository.GetLikesWithId(id);
            if (like is null)
                return NotFound($"No post with {id} found");
            _likeRepository.ApplyPatch(like, patches);
            return NoContent();
        }

        private IEnumerable<Like> RunLikeQuery(LikeQueryDto likeQueryDto)
        {
            if (likeQueryDto.IsEmpty)
                return _likeRepository.GetLikes();
            else if (!(likeQueryDto.User is null))
                return _likeRepository.GetLikedBy(likeQueryDto.User);
            else
                throw new NotSupportedException("The query combination selected is not supported");
        }

    }
}
