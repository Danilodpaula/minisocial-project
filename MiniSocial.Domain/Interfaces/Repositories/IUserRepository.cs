using MiniSocial.Domain.Entities;
namespace MiniSocial.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    public User Create(User user);
    public User? GetById(int id);
    public User? Update(User user);
    public void Delete(int id);
}