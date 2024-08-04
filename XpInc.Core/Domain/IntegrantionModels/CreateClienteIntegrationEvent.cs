using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using XpInc.Core.Messages.IntegrationMessages;

namespace XpInc.Core.Domain.IntegrantionModels
{
    public class CreateClienteIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; private set; }
        public string NomeCompleto { get; private set; }
        public string Cpf { get; private set; }
        public string TelefoneCelular { get; private set; }

        public CreateClienteIntegrationEvent(Guid id, string nomeCompleto, string cpf, string telefoneCelular)
        {
            Id = id;
            AggregateId = id;
            NomeCompleto = nomeCompleto;
            Cpf = cpf;
            TelefoneCelular = telefoneCelular;
        }
    }
}
