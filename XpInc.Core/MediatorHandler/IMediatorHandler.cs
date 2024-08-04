using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpInc.Core.Messages;

namespace XpInc.Core.MediatorHandler
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<ValidationResult> EnviarComando<T>(T comando) where T : Command;
        Task<TResponse> BuscarQuery<TResponse>(IRequest<TResponse> query) where TResponse : class;
    }
}
