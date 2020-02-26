using System;
using System.Data.Entity;
using System.Linq;

namespace iTemplate.Web.Models
{
  public interface IApplicationDbContext :IDisposable
  {
    IQueryable<T> Query<T>() where T : class;
    void Add<T>(T entity) where T : class;
    void Update<T>(T entity) where T : class;
    void Remove<T>(T entity) where T : class;
    int SaveChanges();
  }
}
