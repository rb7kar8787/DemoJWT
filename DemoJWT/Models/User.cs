using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoJWT.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }= string .Empty;
        public string Password { get; set; }= string .Empty;

    }
    public class Jwt
    {
        public string key { get; set; }= string.Empty;
        public string Issuer { get; set; } = string.Empty; 

        public string Audience { get; set; }= string .Empty;

        public string? Subject { get; set; }
    }
}
