using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APILibrary.Models.Posts;
using APILibrary.Models.Likes;
using APILibrary.Models.Users;

namespace APILibrary.Repositories
{
    public interface ILikeRepository
    {
        Like GetLikesWithId(int id);

        IEnumerable<Like> GetLikedBy(string owner);

        IEnumerable<Like> GetLikes();

        IEnumerable<Like> ShowLikes(int postId);

        Like AddLike(int postId, User user);

        void Update(int postId, User user);

        void ApplyPatch(Like like, Dictionary<string, object> patches);

        void Delete(int postId, User user);
    }
}
