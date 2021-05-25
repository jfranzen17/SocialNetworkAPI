using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using APILibrary.Repositories.Users;
using APILibrary.Repositories;
using APILibrary.Models.Posts;

namespace Documentation.Controllers.Posts
{
    //Controller route api/posts
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public PostsController(IPostRepository postRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        //GET api/posts
        [HttpGet]
        public IEnumerable<Post> GetAllQueried([FromQuery] PostQueryDto postQuery)
        {
            return RunPostQuery(postQuery);
        }

        //GET api/posts/5
        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<Post> GetPost(int id)
        {
            var post = _postRepository.GetPostWithId(id);
            if (post is null)
                return NotFound(post);
            return post;
        }

        //GET api/CreatePost
        [HttpPost]
        public ActionResult CreatePost(PostDto postDto)
        {
            try
            {
                var user = _userRepository.GetUserWithId(postDto.CreatedBy);

                var post = _postRepository.Add(postDto, user);
                return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        //GET api/posts/ReplacePost
        [HttpPut]
        [Route("{id:int}")]
        public ActionResult ReplacePost(int id, PostDto postPut)
        {
            var post = _postRepository.GetPostWithId(id);
            if (post is null)
                return NotFound($"No post with {id} found");
            var putPost = new Post(id, postPut, null);
            _postRepository.Update(putPost);
            return NoContent();
        }

        //GET api/posts/UpdatePost
        [HttpPatch]
        [Route("{id:int}")]
        public ActionResult UpdatePost(int id, Dictionary<string, object> patches)
        {
            var post = _postRepository.GetPostWithId(id);
            if (post is null)
                return NotFound($"No post with {id} found");
            _postRepository.ApplyPatch(post, patches);
            return NoContent();
        }

        //GET api/posts/DeletePost
        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult DeletePost(int id)
        {
            var post = _postRepository.GetPostWithId(id);
            if (post is null)
                return NotFound($"No post with {id} found");
            _postRepository.Delete(post);
            return NoContent();
        }


        private List<Post> GetLikesOrThrow(PostDto postDto)
        {
            var likes = new List<Post>();
            foreach (var item in postDto.LikedBy)
            {
                var post = _postRepository.GetPostWithId(item);
                likes.Add(post);
                if (post is null)
                {
                    throw new ValidationException($"The comment with id {item} does not exist");
                }
            }
            return likes;
        }

        private IEnumerable<Post> RunPostQuery(PostQueryDto postQueryDto)
        {
            if (postQueryDto.IsEmpty)
                return _postRepository.GetPosts();
            else if (!(postQueryDto.Hidden is null))
                return _postRepository.GetPostsWithHiddenStatus((bool)postQueryDto.Hidden);
            else if (!(postQueryDto.CreatedBy is null))
                return _postRepository.GetPostCreatedBy(postQueryDto.CreatedBy);
            else
                throw new NotSupportedException("The query combination selected is not supported");
        }
    }
}
