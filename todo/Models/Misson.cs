using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todo.Models;

[Table("Misson")]
public class Misson : IEntity
{


    [Key]
    [Column(TypeName = "varchar(32)")]
    public string? Id { get; set; } = ModelsHelper.NewId();

    [Column(TypeName = "varchar(32)")]
    [MaxLength(100)]
    public string? Name { get; set; }


}
