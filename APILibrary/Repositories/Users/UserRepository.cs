using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APILibrary.Models.Users;
using APILibrary.Models.Posts;
using APILibrary.Repositories.Users;

namespace APILibrary.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly Dictionary<int, User> _users = new Dictionary<int, User>();

        public UserRepository()
        {
            var user = new User(1, "Admin", "admin@gmail.com");
            var user1 = new User(2, "User", "user@gmail.com");

            _users.Add(1, user);
            _users.Add(2, user1);
        }

        public User Add(UserDto userDto)
        {
            var id = _users.Count + 1;
            var newUser = new User(id, userDto.Name, userDto.Email);
            _users.Add(id, newUser);
            return newUser;
        }

        public void Delete(User user)
        {
            _users.Remove(user.Id);
        }
        public void ApplyPatch(User user, Dictionary<string, object> patches)
        {
            ApplyPatch<User>(user, patches);
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
        public void Update(User user)
        {
            _users.Remove(user.Id);
            _users.Add(user.Id, user);
        }
        public User GetUserWithId(int id)
        {
            try
            {
                return _users[id];
            }
            catch (IndexOutOfRangeException e)
            {

                throw e;
            }
        }

        public IEnumerable<User> GetUsers()
        {
            return _users.Values;
        }

        public IEnumerable<User> GetUserByEmail(string email)
        {
            return _users.Select(e => e.Value).Where(f => f.Email == email);
        }
        public IEnumerable<User> GetUserByName(string name)
        {
            return _users.Select(e => e.Value).Where(f => f.Name == name);
        }

    }
}
