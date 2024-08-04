using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpInc.Core.Domain;

namespace XpInc.Core.Data
{
    public interface IRepository<T, Context> : IDisposable where T : Entity where Context : DbContext
    {
        Task<T> Add(T entity); 
        Task Update(T entity);
        Task Delete(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid id);   
        IUnitOfWork UnitOfWork { get; }
    }
}
