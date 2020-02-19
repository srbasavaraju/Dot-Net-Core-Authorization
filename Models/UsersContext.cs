using Microsoft.EntityFrameworkCore;
using AuthenticationWebApi.Entities;

namespace AuthenticationWebApi.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> dbContextOptions) : base(dbContextOptions) { }
        public DbSet<AuthenticateModel> AuthenticateModel { get; set; }

        public DbSet<User> users { get; set; }
    }
}
