using MediatR;
using XpInc.RendaFixa.API.Models.Entities;

namespace XpInc.RendaFixa.API.Application.Queries
{
    public class GetAllRendaFixaQuery : IRequest<IEnumerable<RendaFixaProduto>>
    {
        public DateTime? DataReferencia { get; set; }
        public bool AtualizaCache { get; set; } = false;

        public GetAllRendaFixaQuery(bool atualizaCache = false, DateTime? dataReferencia = null)
        {
            AtualizaCache = atualizaCache;
            DataReferencia = dataReferencia;
        }
    }
}
