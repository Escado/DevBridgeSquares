using DevBridgeSquares.Entities.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DevBridgeSquares.Services.Services
{
    public interface IPointService
    {
        Dictionary<int, List<Point>> FindSquares(string listName);
        void Upload(string listName, MemoryStream str);
    }

    public class PointService : IPointService
    {
        public Dictionary<int, List<Point>> FindSquares(string listName)
        {
            throw new NotImplementedException();
        }

        public void Upload(string listName, MemoryStream str)
        {
            throw new NotImplementedException();
        }
    }
}
