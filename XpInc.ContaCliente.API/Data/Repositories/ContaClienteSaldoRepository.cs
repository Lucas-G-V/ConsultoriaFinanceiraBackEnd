using Microsoft.EntityFrameworkCore;
using XpInc.ContaCliente.API.Data.Contexts;
using XpInc.ContaCliente.API.Models.Entities;
using XpInc.ContaCliente.API.Models.Interfaces;
using XpInc.Core.Data;
using XpInc.Core.Utils;

namespace XpInc.ContaCliente.API.Data.Repositories
{
    public class ContaClienteSaldoRepository : Repository<ContaClienteSaldo, ContaClienteContext>, IContaClienteSaldoRepository
    {
        public ContaClienteSaldoRepository(ContaClienteContext context) : base(context)
        {
        }

        public Task<ContaClienteSaldo> GetByCpf(string cpf)
        {
            return _dbSet.Where(x => x.CPF == FormatarString.ExtractNumbers(cpf) && x.Ativo).FirstOrDefaultAsync();
        }
    }
}
