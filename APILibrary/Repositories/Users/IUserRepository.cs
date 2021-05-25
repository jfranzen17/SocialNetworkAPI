using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APILibrary.Models.Users;
using APILibrary.Models.Posts;

namespace APILibrary.Repositories
{
    public interface IUserRepository
    {
        User GetUserWithId(int id);

        IEnumerable<User> GetUsers();

        IEnumerable<User> GetUserByEmail(string email);
        IEnumerable<User> GetUserByName(string name);

        User Add(UserDto userDto);

        void Update(User user);

        void ApplyPatch(User user, Dictionary<string, object> patches);

        void Delete(User user);
    }
}
