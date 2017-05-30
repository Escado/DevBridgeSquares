using DevBridgeSquares.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevBridgeSquares.Entities.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string SelectedList { get; set; }
    }
}
