using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpInc.Cache
{
    public interface IMemoryCacheService
    {
        Task AddMemoryCache<T>(string id, T dados);
        Task<TData?> GetById<TData>(string id);
        Task DeleteMemoryCache(string id);
    }
}
