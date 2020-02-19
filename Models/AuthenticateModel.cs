using System.ComponentModel.DataAnnotations;

namespace AuthenticationWebApi.Models
{
    public class AuthenticateModel
    {
        public int Id { get; set; }
        
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
