using DevBridgeSquares.Common.Exceptions;
using DevBridgeSquares.Entities.Models;
using DevBridgeSquares.Entities.Params;
using DevBridgeSquares.Entities.Params.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevBridgeSquares.Repositories.Repositories
{
    public interface IPointRepository
    {
        IEnumerable<Point> Get(PointParams par, out int totalEntries, out int page);

        List<string> GetListNames();
        bool IsListPresent(string listName);

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
            var r = new Random();
            var randomCount = r.Next(0, 10);
            _points = new Dictionary<string, List<Point>>();
            _points.Add(DEFAULT_NAME, new List<Point>());
            for (int i = 0; i < randomCount; i++)
            {
                _points[DEFAULT_NAME].Add(new Point(r.Next(-20, 20), r.Next(-20, 20)));
            }
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

        public IEnumerable<Point> Get(PointParams par, out int totalEntries, out int page)
        {
            if (!_points.ContainsKey(par.listName)) { throw new DVBSException(DVBSCode.Point.ListDoesntExist); }

            totalEntries = _points[par.listName].Count;
            page = par.page;

            var result = _points[par.listName]
                            .Skip(par.perPage * (par.page - 1))
                            .Take(par.perPage);

            return result;
        }

        public List<string> GetListNames() => _points.Keys.ToList();

        public bool IsListPresent(string listName) => _points.ContainsKey(listName);

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
