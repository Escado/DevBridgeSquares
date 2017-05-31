using DevBridgeSquares.Common.Exceptions;
using DevBridgeSquares.Entities.Models;
using DevBridgeSquares.Repositories.Repositories;
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
        void Add(string listName, Point point);
    }

    public class PointService : IPointService
    {
        private readonly IPointRepository _pointRepository;

        public PointService(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        public void Add(string listName, Point point)
        {
            if (!_pointRepository.IsListPresent(listName)) { throw new DVBSException(DVBSCode.Point.ListDoesntExist); }
            _pointRepository.Add(listName, point);
        }

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
