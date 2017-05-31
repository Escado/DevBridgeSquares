using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DevBridgeSquares.Common.Exceptions
{
    public static class DVBSCode
    {

        public enum General
        {
            [Description("Unknown error")]
            UnknownError = 10000,

            [Description("Invalid request")]
            InvalidRequest = 10001,            
        }

        public enum Point
        {
            [Description("List doesn't exist.")]
            ListDoesntExist = 20001
        }
    }
}
