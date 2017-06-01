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
            ListDoesntExist = 20001,

            [Description("Point doesn't exist.")]
            PointDoesntExist = 20002,

            [Description("Point with the same coordinates already exists.")]
            DuplicatePoint = 20003,

            [Description("List name must be present.")]
            ListNameEmpty = 20004,

            [Description("File contains errors. Unable to upload.")]
            UploadFailed = 20005,

            [Description("List with the same name already exists.")]
            ListAlreadyExists = 20006,

            [Description("Maximum amount of points in a list cannot exceed 10000.")]
            LimitExceeded = 20007,

            [Description("Atleast 4 points are needed for a square.")]
            SampleTooSmall = 20008,
        }
    }
}
