using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpInc.Core.Messages
{
    public class Event : Message, INotification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }
    }
}
