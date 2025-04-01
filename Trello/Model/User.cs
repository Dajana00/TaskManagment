using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
namespace Trello.Model
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public override string Email { get; set; }
        [Required]
        public override string UserName { get; set; }
        [Required]
        public override string PhoneNumber { get; set; }

        public override string PasswordHash { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserCard> UserCards { get; set; } = new HashSet<UserCard>();

        public ICollection<UserProject> UserProjects { get; set; } = new HashSet<UserProject>(); 
        public ICollection<Comment> Comments { get; set; }  = new HashSet<Comment>();   


        public User() { }

    }
}
