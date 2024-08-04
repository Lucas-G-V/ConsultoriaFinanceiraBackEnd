using XpInc.ContaCliente.API.Data.Contexts;
using XpInc.ContaCliente.API.Models.Entities;
using XpInc.Core.Data;

namespace XpInc.ContaCliente.API.Models.Interfaces
{
    public interface IContaClienteSaldoRepository : IRepository<ContaClienteSaldo, ContaClienteContext>
    {
        Task<ContaClienteSaldo> GetByCpf(string cpf);
    }
}
