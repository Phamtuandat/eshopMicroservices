using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException( string massage) : base(massage) { }

        public BadRequestException(string massage, string detail) : base(massage)
        {
            Detail = detail;
        }

        public string? Detail { get; }
    }
}
