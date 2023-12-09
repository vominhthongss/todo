using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace todo.Models;


[Table("User")]
[Index(nameof(Account))]
public class User : IEntity
{
    [Key]
    [Column(TypeName = "varchar(32)")]
    public string? Id { get; set; } = ModelsHelper.NewId();

    //[Column(TypeName = "nvarchar(32)")]
    [MaxLength(50)]
    public string? Account { get; set; }


    [Column(TypeName = "varchar(128)")]
    [JsonIgnore]
    public string? HashedPassword { get; set; }

    [MaxLength(50)]
    public string? Role { get; set; } // 3. 役割


    public String Password
    {
        set
        {
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(value);
        }
    }

    [Column(TypeName = "varchar(50)")]
    public string? Email { get; set; } // 6. メール

    [MaxLength(20)]
    public string? Surname { get; set; }
    [MaxLength(30)]
    public string? Name { get; set; }

    [MaxLength(15)]
    public string? Phone { get; set; }
}

[ExtendObjectType(typeof(User),
    IgnoreProperties = new[] { nameof(User.HashedPassword) })]
public class UserExtensions
{
}
