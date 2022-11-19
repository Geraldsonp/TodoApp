using TodoApp.Models.Entity;

namespace TodoApp.Services.Interfaces;

public interface IUnitOfWork
{
    void SaveChanges();
}