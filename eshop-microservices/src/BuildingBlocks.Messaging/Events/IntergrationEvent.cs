﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events
{
    public record IntergrationEvent
    {
        public Guid Id => Guid.NewGuid();
        public DateTime OccurredOn
        {
            get
            {
                return DateTime.Now;
            }
        }

        public string EventType => GetType().AssemblyQualifiedName ?? string.Empty;
    }
}
