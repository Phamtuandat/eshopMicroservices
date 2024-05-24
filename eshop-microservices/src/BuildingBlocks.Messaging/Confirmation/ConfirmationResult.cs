using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Confirmation
{
    public record ConfirmationResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
