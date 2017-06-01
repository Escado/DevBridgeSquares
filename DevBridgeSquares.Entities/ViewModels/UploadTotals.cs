using System;
using System.Collections.Generic;
using System.Text;

namespace DevBridgeSquares.Entities.ViewModels
{
    public class UploadTotals
    {
        public int insertedCount = 0;
        public int badLinesCount = 0;
        public int totalLinesCount = 0;
        public int duplicatesCount = 0;
        public bool IsLimitReached = false;
    }
}
