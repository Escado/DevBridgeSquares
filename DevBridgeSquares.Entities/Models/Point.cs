using DevBridgeSquares.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DevBridgeSquares.Entities.Models
{
    public class Point : BaseEntity
    {
        [Required]
        [Range(-5000, 5000)]
        public int X { get; set; }

        [Required]
        [Range(-5000, 5000)]
        public int Y { get; set; }

        public static bool operator ==(Point first, Point second)
        {
            return first.X == second.X && first.Y == second.Y;
        }

        public static bool operator !=(Point first, Point second)
        {
            return first.X != second.X || first.Y != second.Y;
        }

        public override bool Equals(object obj)
        {
            var comparator = obj as Point;
            if (comparator != null)
                return X == comparator.X && Y == comparator.Y;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
