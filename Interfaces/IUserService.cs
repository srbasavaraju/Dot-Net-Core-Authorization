using System.Collections.Generic;
using AuthenticationWebApi.Entities;
using AuthenticationWebApi.Models;

namespace AuthenticationWebApi.Interfaces
{
    public interface IUserService
    {
        User Authenticate(AuthenticateModel authenticateModel);
        IEnumerable<User> getAll();

        bool delete(string userName);
    }
}
