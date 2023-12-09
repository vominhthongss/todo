using Data;
using Template;
using todo.Models;

namespace Repositories;

public class MissonRepository : TRepository<Misson, ApplicationDbContext>
{
    public MissonRepository(ApplicationDbContext context) : base(context)
    {
    }
}