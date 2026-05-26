using MiniSocial.Domain.Entities;
using MiniSocial.Domain.Interfaces.Repositories;

namespace MiniSocial.Infrastructure.Data.Repositories;

public class EfcUserRepository : IUserRepository
{
    private readonly MiniSocialDbContext _dbcontext;

    public EfcUserRepository(MiniSocialDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public User Create(User user)
    {
        throw new NotImplementedException();
    }

    public User? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public User? Update(User user)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }
}