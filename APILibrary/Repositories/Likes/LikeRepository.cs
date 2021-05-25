using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APILibrary.Models.Likes;
using APILibrary.Models.Posts;
using APILibrary.Models.Users;
using APILibrary.Repositories.Users;

namespace APILibrary.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly Dictionary<int, Like> _Likes = new Dictionary<int, Like>();

        public LikeRepository(IUserRepository userRepository)
        {
            var user = userRepository.GetUserWithId(1);
            var user1 = userRepository.GetUserWithId(2);

            AddLike(1, user);
            AddLike(2, user1);
            AddLike(2, user);
            AddLike(2, user1);
            AddLike(3, user1);
        }

        public Like GetLikesWithId(int likeId)
        {
            _Likes.TryGetValue(likeId, out Like result);
            return result;
        }

        public IEnumerable<Like> GetLikedBy(string owner)
        {
            return _Likes.Select(e => e.Value).Where(f => f.User.Name == owner);
        }

        public IEnumerable<Like> GetLikes()
        {
            return _Likes.Values;
        }

        public IEnumerable<Like> ShowLikes(int postId)
        {
            return _Likes.Select(e => e.Value).Where(f => f.PostId == postId);
        }


        public Like AddLike(int postId, User user)
        {
            var likeid = _Likes.Count() + 1;
            var like = new Like(postId, user);
            _Likes.Add(likeid, like);
            return like;
        }

        public void Update(int postid, User user)
        {
            _Likes.Remove(postid);
            var like = AddLike(postid, user);
            _Likes.Add(postid, like);
        }

        public void ApplyPatch(Like like, Dictionary<string, object> patches)
        {
            ApplyPatch<Like>(like, patches);
        }

        public void Delete(int postId, User user)
        {
            _Likes.Remove(postId);
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
