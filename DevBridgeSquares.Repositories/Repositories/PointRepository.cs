using DevBridgeSquares.Entities.Models;
using DevBridgeSquares.Entities.Params.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevBridgeSquares.Repositories.Repositories
{
    public interface IPointRepository
    {
        IEnumerable<Point> Get(Params par);

        void Add(string listName, Point point);
        void Add(string listName, IEnumerable<Point> points);

        void Remove(string listName, Point point);
        void Remove(string listName, IEnumerable<Point> points);
        void Remove(string listName, int Id);
        void Remove(string listName, IEnumerable<int> Ids);

        void Clear(string listName);
    }

    public class PointRepository : IPointRepository
    {
        private const string DEFAULT_NAME = "Default";
        private string _activeList;
        private Dictionary<string, List<Point>> _points;

        public PointRepository()
        {
            _points = new Dictionary<string, List<Point>>();
            _points.Add(DEFAULT_NAME, new List<Point>());
        }

        public void Add(string listName, Point point)
        {
            throw new NotImplementedException();
        }

        public void Add(string listName, IEnumerable<Point> points)
        {
            throw new NotImplementedException();
        }

        public void Clear(string listName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Point> Get(Params par)
        {
            throw new NotImplementedException();
        }

        public void Remove(string listName, Point point)
        {
            throw new NotImplementedException();
        }

        public void Remove(string listName, IEnumerable<Point> points)
        {
            throw new NotImplementedException();
        }

        public void Remove(string listName, int Id)
        {
            throw new NotImplementedException();
        }

        public void Remove(string listName, IEnumerable<int> Ids)
        {
            throw new NotImplementedException();
        }
    }
}
