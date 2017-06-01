using DevBridgeSquares.Common.Exceptions;
using DevBridgeSquares.Entities.Enums;
using DevBridgeSquares.Entities.Models;
using DevBridgeSquares.Entities.Params;
using DevBridgeSquares.Entities.Params.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DevBridgeSquares.Repositories.Repositories
{
    public interface IPointRepository
    {
        /// <summary>
        /// Gets paged points.
        /// </summary>
        /// <param name="par"></param>
        /// <param name="totalEntries"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        IEnumerable<Point> Get(PointParams par, out int totalEntries, out int page);

        /// <summary>
        /// Gets all points in list.
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        IEnumerable<Point> Get(string listName);

        /// <summary>
        /// Gets names of point lists.
        /// </summary>
        /// <returns></returns>
        List<string> GetListNames();

        /// <summary>
        /// Returns true if list with the same name exists.
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        bool IsListPresent(string listName);

        /// <summary>
        /// Adds a new point list.
        /// </summary>
        /// <param name="listName"></param>
        void Add(string listName);

        /// <summary>
        /// Adds a new point to point list.
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="point"></param>
        void Add(string listName, Point point);

        /// <summary>
        /// Gets count of points in the list.
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        int GetPointsCount(string listName);

        /// <summary>
        /// Adds points to point list.
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="points"></param>
        void Add(string listName, IEnumerable<Point> points);

        /// <summary>
        /// Removes list from points list.
        /// </summary>
        /// <param name="listName"></param>
        void Remove(string listName);

        /// <summary>
        /// Removes point from list.
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="point"></param>
        void Remove(string listName, Point point);

        /// <summary>
        /// Removes points from list.
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="points"></param>
        void Remove(string listName, IEnumerable<Point> points);

        /// <summary>
        /// Removes point by id from list.
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="Id"></param>
        void Remove(string listName, int Id);

        /// <summary>
        /// Removes points by id from list.
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="Ids"></param>
        void Remove(string listName, IEnumerable<int> Ids);

        /// <summary>
        /// Removes all points from list.
        /// </summary>
        /// <param name="listName"></param>
        void Clear(string listName);
    }

    public class PointRepository : IPointRepository
    {
        private const string DEFAULT_NAME = "Default";
        private Dictionary<string, List<Point>> _points;
        private int _indexer;

        public PointRepository()
        {
            var r = new Random();
            var randomCount = r.Next(20, 30);
            _points = new Dictionary<string, List<Point>>();
            _indexer = 0;
            _points.Add(DEFAULT_NAME, new List<Point>());
        }

        public void Add(string listName) => _points.Add(listName, new List<Point>());

        public void Add(string listName, Point point)
        {
            point.Id = _indexer++;
            _points[listName].Add(point);
        }

        public void Add(string listName, IEnumerable<Point> points)
        {
            foreach (var item in points)
            {
                item.Id = _indexer++;
            }
            _points[listName].AddRange(points);
        }

        public void Clear(string listName) => _points[listName].Clear();

        public IEnumerable<Point> Get(PointParams par, out int totalEntries, out int page)
        {
            var sort = typeof(Point).GetProperties().FirstOrDefault(x => x.Name.ToLower().Equals(par.sort));
            totalEntries = _points[par.listName].Count;
            page = par.page;

            var result = _points[par.listName];
            
            if (par.sortDir.ToLower().Equals("asc"))
            {
                result = result.OrderBy(x => sort?.GetValue(x, null)).ToList();
            }
            else
            {
                result = result.OrderByDescending(x => sort?.GetValue(x, null)).ToList();
            }

            return result
                .Skip(par.perPage * (par.page - 1))
                .Take(par.perPage);
        }

        public IEnumerable<Point> Get(string listName) => _points[listName];

        public List<string> GetListNames() => _points.Keys.ToList();

        public int GetPointsCount(string listName) => _points[listName].Count;

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
            var point = _points[listName].FirstOrDefault(x => x.Id == Id);
            if (point != null) { _points[listName].Remove(point); }
        }

        public void Remove(string listName, IEnumerable<int> Ids)
        {
            throw new NotImplementedException();
        }

        public void Remove(string listName) => _points.Remove(listName);
    }
}
