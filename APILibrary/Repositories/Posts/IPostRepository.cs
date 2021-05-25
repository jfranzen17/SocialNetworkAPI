using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APILibrary.Models.Posts;
using APILibrary.Models.Users;

namespace APILibrary.Repositories
{
    public interface IPostRepository
    {
        Post GetPostWithId(int id);

        IEnumerable<Post> GetPostsWithHiddenStatus(bool isHidden);

        IEnumerable<Post> GetPostCreatedBy(string createdBy);

        IEnumerable<Post> GetPosts();

        Post Add(PostDto post, User user);

        void Update(Post post);

        void ApplyPatch(Post post, Dictionary<string, object> patches);

        void Delete(Post post);
    }
}
