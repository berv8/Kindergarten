using System.ComponentModel.DataAnnotations;

namespace Kindergarten.Data
{
    public class Login
    {
            [Key]
            public int Id { get; set; }
            public string UserName { get; set; } = null!;
            public string HashedPassword { get; set; } = null!;
            public byte[] SaltPassword { get; set; } = null!;
        
    }
}
