using todo.Models;
using Data;
using Template;

namespace Controllers;

public class MissonsController : TController<Misson, TRepository<Misson, ApplicationDbContext>>
{
    public MissonsController(TRepository<Misson, ApplicationDbContext> repository) : base(repository)
    {
    }
}
