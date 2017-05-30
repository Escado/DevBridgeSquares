using System;
using System.Collections.Generic;
using System.Text;

namespace DevBridgeSquares.Entities.Params.Base
{
    public class Params
    {
        public string sort { get; set; } = "Id";
        public string sortDir { get; set; } = "ASC";
        public int page { get; set; } = 1;
        public int perPage { get; set; } = 10;
    }
}
