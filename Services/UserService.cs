using System;
using System.Collections.Generic;
using System.Linq;
using AuthenticationWebApi.Entities;
using AuthenticationWebApi.Interfaces;
using AuthenticationWebApi.Helpers;
using Microsoft.Extensions.Options;
using AuthenticationWebApi.Models;

namespace AuthenticationWebApi.Services
{
    public class UserService : IUserService
    {

        private readonly AppSettings _appSettings;
        private readonly UsersContext _usersContext;

        public UserService(IOptions<AppSettings> appSettings, UsersContext usersContext)
        {
            _appSettings = appSettings.Value;
            _usersContext = usersContext;
        }

        public User Authenticate(AuthenticateModel authenticateModel)
        {
            User user = getUser(authenticateModel, _usersContext);

            if (user == null)
            {
                return null;
            }

            TokenHandler tokenHandler = new TokenHandler(_appSettings);
            user.Token = tokenHandler.CreateToken(user);

            return user.WithoutPassword();
        }


        public IEnumerable<User> getAll()
        {

            var users = (from user in _usersContext.users
                         select user).WithoutPasswords();
            return users;
        }

        private static User getUser(AuthenticateModel authenticateModel, UsersContext usersContext)
        {
            var user = usersContext.users.FirstOrDefault(u => u.UserName == authenticateModel.UserName && u.Password == authenticateModel.Password);
            if (user == null)
            {
                return null;
            }
            else
            {
                return user.WithoutPassword();
            }
        }

        public bool delete(string userName)
        {
            try
            {
                User user = _usersContext.users.FirstOrDefault(u => u.UserName == userName);
                if (user != null)
                {
                    _usersContext.users.Remove(user);
                    _usersContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
