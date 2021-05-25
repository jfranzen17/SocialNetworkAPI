using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APILibrary.Models.Users;
using APILibrary.Models.Posts;
using APILibrary.Repositories.Users;

namespace APILibrary.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly Dictionary<int, Post> _Posts = new Dictionary<int, Post>();

        public PostRepository(IUserRepository userRepository)
        {
            var user = userRepository.GetUserWithId(1);
            var post = new Post(1)
            {
                Content = "Launch SocialMedia Platform",
                CreatedBy = user,
                Hidden = false,
                LastDate = DateTime.UtcNow,
            };
            var user1 = userRepository.GetUserWithId(2);
            var post1 = new Post(2)
            {
                Content = "Write the first long message on this platform to ",
                CreatedBy = user1,
                Hidden = false,
                LastDate = DateTime.UtcNow,
            };
            var post2 = new Post(3)
            {
                Content = "Create First Like",
                CreatedBy = user,
                Hidden = false,
                LastDate = DateTime.UtcNow,
            };

            _Posts.Add(1, post);
            _Posts.Add(2, post1);
            _Posts.Add(3, post2);

        }

        public Post GetPostWithId(int id)
        {
            _Posts.TryGetValue(id, out Post result);
            return result;
        }

        public IEnumerable<Post> GetPostsWithHiddenStatus(bool isHidden)
        {
            return _Posts.Select(e => e.Value).Where(f => f.Hidden == isHidden);
        }

        public IEnumerable<Post> GetPostCreatedBy(string createdBy)
        {
            return _Posts.Select(e => e.Value).Where(f => f.CreatedBy.Name == createdBy);
        }
        public IEnumerable<Post> GetPostsCreatedByWithHiddenStatus(bool isHidden, string createdBy)
        {
            return _Posts.Select(e => e.Value).Where(f => (f.CreatedBy.Name == createdBy) && (f.Hidden == isHidden));
        }

        public IEnumerable<Post> GetPosts()
        {
            return _Posts.Values;
        }

        public Post Add(PostDto PostDto, User user)
        {
            var id = _Posts.Count + 1;
            var post = new Post(id, PostDto, user);
            _Posts.Add(id, post);
            return post;
        }

        public void Update(Post post)
        {
            _Posts.Remove(post.Id);
            _Posts.Add(post.Id, post);
        }

        public void ApplyPatch(Post post, Dictionary<string, object> patches)
        {
            ApplyPatch<Post>(post, patches);
        }

        public void Delete(Post post)
        {
            _Posts.Remove(post.Id);
        }

        private void ApplyPatch<T>(T original, Dictionary<string, object> patches)
        {
            var properties = original.GetType().GetProperties();
            foreach (var patch in patches)
            {
                foreach (var prop in properties)
                {
                    if (string.Equals(patch.Key, prop.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        prop.SetValue(original, patch.Value);
                    }
                }
            }
        }
    } 
}
