using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevBridgeSquares.Common.Extensions;
using System.Linq.Expressions;

namespace DevBridgeSquares.Common.Exceptions
{
    /// <summary>
    /// This exception type should be thrown for problems when we would like the client to see specific message, but there is no other more appropriate custom Rw exception type
    /// </summary>
    public class DVBSException : Exception
    {
        public Enum Code { get; set; }
        public string Field { get; set; }

        public DVBSException(Enum code, string message, string field)
            : base(code.GetDescription() + " " + message)
        {
            Code = code;
            Field = field;
        }

        public DVBSException(Enum code, string message)
            : base(code.GetDescription() + " " + Environment.NewLine + message)
        {
            Code = code;
        }

        public DVBSException(Enum code)
            : base(code.GetDescription())
        {
            Code = code;
        }
    }
}
