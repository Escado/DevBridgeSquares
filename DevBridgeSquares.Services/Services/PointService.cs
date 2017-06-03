using DevBridgeSquares.Common.Exceptions;
using DevBridgeSquares.Entities.Enums;
using DevBridgeSquares.Entities.Models;
using DevBridgeSquares.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DevBridgeSquares.Entities.Params;
using DevBridgeSquares.Entities.ViewModels;

namespace DevBridgeSquares.Services.Services
{
    public interface IPointService
    {
        /// <summary>
        /// Gets all possible squares from points in list.
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        Dictionary<string, List<Point>> FindSquares(string listName);
        /// <summary>
        /// Uploads data from file and returns some basic statistics.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="list"></param>
        /// <param name="listName"></param>
        /// <param name="totals"></param>
        void Upload(Stream str, ListOptionType list, string listName, out UploadTotals totals);
        /// <summary>
        /// Adds a new list of points.
        /// </summary>
        /// <param name="listName"></param>
        void Add(string listName);
        /// <summary>
        /// Adds a new point to list.
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="point"></param>
        void Add(string listName, Point point);
        /// <summary>
        /// Gets paginated view of points from list.
        /// </summary>
        /// <param name="par"></param>
        /// <param name="totalEntries"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        IEnumerable<Point> Get(PointParams par, out int totalEntries, out int page);
        /// <summary>
        /// Removes point from list by id.
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="Id"></param>
        void Remove(string listName, int Id);
        /// <summary>
        /// Removes list.
        /// </summary>
        /// <param name="listName"></param>
        void Remove(string listName);
        /// <summary>
        /// Removes points from list.
        /// </summary>
        /// <param name="listName"></param>
        void Clear(string listName);
        /// <summary>
        /// Returns string of data to be downloaded.
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        string GetDownloadData(string listName);
    }

    public class PointService : IPointService
    {
        private const int LIST_POINT_LIMIT = 10000; // TODO: Possble global config candidate ? 
        private readonly IPointRepository _pointRepository;

        public PointService(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        public void Add(string listName, Point point)
        {
            if (!_pointRepository.IsListPresent(listName)) { throw new DVBSException(DVBSCode.Point.ListDoesntExist); }

            if (_pointRepository.IsListFull(listName)) { throw new DVBSException(DVBSCode.Point.LimitExceeded); }

            var dbPoint = _pointRepository.GetByCoordinates(listName, point.X, point.Y);

            if (dbPoint == point) throw new DVBSException(DVBSCode.Point.DuplicatePoint);

            _pointRepository.Add(listName, point);
        }

        public void Add(string listName)
        {
            if (string.IsNullOrWhiteSpace(listName)) { throw new DVBSException(DVBSCode.Point.ListNameEmpty); }

            if (_pointRepository.IsListPresent(listName)) { throw new DVBSException(DVBSCode.Point.ListAlreadyExists); }

            _pointRepository.Add(listName);
        }

        public void Clear(string listName)
        {
            if (string.IsNullOrWhiteSpace(listName)) { throw new DVBSException(DVBSCode.Point.ListNameEmpty); }

            if (!_pointRepository.IsListPresent(listName)) { throw new DVBSException(DVBSCode.Point.ListDoesntExist); }

            _pointRepository.Clear(listName);
        }

        public Dictionary<string, List<Point>> FindSquares(string listName)
        {
            if (string.IsNullOrWhiteSpace(listName)) { throw new DVBSException(DVBSCode.Point.ListNameEmpty); }

            if (!_pointRepository.IsListPresent(listName)) { throw new DVBSException(DVBSCode.Point.ListDoesntExist); }

            var points = _pointRepository.Get(listName);

            if (points.Count() < 4) { throw new DVBSException(DVBSCode.Point.SampleTooSmall); }

            var result = new Dictionary<string, List<Point>>();
            var scannedPoints = new List<List<int>>();
            var sameXAxis = new List<Point>();
            var sameYAxis = new List<Point>();
            var resultPoints = new List<Point>();
            var dist = 0;
            var id = "";
            foreach (var mainPoint in points)
            {

                // Selects all points on the X axis and goes through them.
                sameXAxis = points.Where(x => x.Y == mainPoint.Y && x.Id != mainPoint.Id).ToList();
                if (sameXAxis.Count > 0)
                {
                    foreach (var secondaryPoint in sameXAxis)
                    {
                        // This is so that no two points would be scanned twice
                        if (scannedPoints.Any(x => x.Contains(secondaryPoint.Id) && x.Contains(mainPoint.Id)))
                        {
                            continue;
                        }

                        dist = Math.Abs(mainPoint.X - secondaryPoint.X);

                        var aboveMainPoint = points.FirstOrDefault(x => x.X == mainPoint.X && x.Y == mainPoint.Y + dist);
                        var aboveSecondaryPoint = points.FirstOrDefault(x => x.X == secondaryPoint.X && x.Y == secondaryPoint.Y + dist);

                        var belowMainPoint = points.FirstOrDefault(x => x.X == mainPoint.X && x.Y == mainPoint.Y - dist);
                        var belowSecondaryPoint = points.FirstOrDefault(x => x.X == secondaryPoint.X && x.Y == secondaryPoint.Y - dist);

                        // if there are two points above the ones we selected we add them all to squares list.
                        if (aboveMainPoint != null && aboveSecondaryPoint != null)
                        {
                            AppendSquaresResult(new List<Point>() { mainPoint, secondaryPoint, aboveMainPoint, aboveSecondaryPoint }, ref result);
                        }

                        // Same with points belowe the ones we selected.
                        if (belowMainPoint != null && belowSecondaryPoint != null)
                        {
                            AppendSquaresResult(new List<Point>() { mainPoint, secondaryPoint, belowMainPoint, belowSecondaryPoint }, ref result);
                        }

                        scannedPoints.Add(new List<int>() { mainPoint.Id, secondaryPoint.Id });
                    }
                }
            }

            return result;
        }

        private void AppendSquaresResult(List<Point> points, ref Dictionary<string, List<Point>> result)
        {
            var id = string.Join(",", points.OrderBy(x => x.Id).Select(x => x.Id.ToString()).ToArray());
            if (!result.ContainsKey(id))
            {
                result.Add(id, points);
            }
        }

        public IEnumerable<Point> Get(PointParams par, out int totalEntries, out int page)
        {
            if (string.IsNullOrWhiteSpace(par.listName)) { throw new DVBSException(DVBSCode.Point.ListNameEmpty); }

            if (!_pointRepository.IsListPresent(par.listName)) { throw new DVBSException(DVBSCode.Point.ListDoesntExist); }

            var result = _pointRepository.Get(par, out totalEntries, out page);

            return result;

        }

        public string GetDownloadData(string listName)
        {
            if (string.IsNullOrWhiteSpace(listName)) { throw new DVBSException(DVBSCode.Point.ListNameEmpty); }

            if (!_pointRepository.IsListPresent(listName)) { throw new DVBSException(DVBSCode.Point.ListDoesntExist); }

            var points = _pointRepository.Get(listName);

            StringBuilder result = new StringBuilder();

            foreach (var item in points)
            {
                result.Append($"{item.X} {item.Y}{Environment.NewLine}");
            }

            result.Remove(result.Length - Environment.NewLine.Length, Environment.NewLine.Length);

            return result.ToString();
        }

        public void Remove(string listName, int Id)
        {
            if (string.IsNullOrWhiteSpace(listName)) { throw new DVBSException(DVBSCode.Point.ListNameEmpty); }

            if (!_pointRepository.IsListPresent(listName)) { throw new DVBSException(DVBSCode.Point.ListDoesntExist); }

            _pointRepository.Remove(listName, Id);
        }

        public void Remove(string listName)
        {
            if (string.IsNullOrWhiteSpace(listName)) { throw new DVBSException(DVBSCode.Point.ListNameEmpty); }

            if (!_pointRepository.IsListPresent(listName)) { throw new DVBSException(DVBSCode.Point.ListDoesntExist); }

            _pointRepository.Remove(listName);
        }

        public void Upload(Stream str, ListOptionType list, string listName, out UploadTotals totals)
        {
            if (string.IsNullOrWhiteSpace(listName)) { throw new DVBSException(DVBSCode.Point.ListNameEmpty); }

            if (list == ListOptionType.Current && !_pointRepository.IsListPresent(listName)) throw new DVBSException(DVBSCode.Point.ListDoesntExist);

            if (list == ListOptionType.New && _pointRepository.IsListPresent(listName)) throw new DVBSException(DVBSCode.Point.ListAlreadyExists);

            totals = new UploadTotals();

            using (StreamReader sr = new StreamReader(str))
            {
                List<Point> points = new List<Point>();

                string text = sr.ReadToEnd();

                var data = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                totals.totalLinesCount = data.Length;

                foreach (var item in data)
                {
                    var tokens = item.Split(' ');

                    if (tokens.Length == 2)
                    {
                        if (int.TryParse(tokens[0], out int x) && int.TryParse(tokens[1], out int y))
                        {
                            points.Add(new Point { X = x, Y = y });
                        }
                    }
                    else
                    {
                        totals.badLinesCount++;
                    }
                }

                if (list == ListOptionType.New)
                {
                    _pointRepository.Add(listName);

                    var uniques = points.Distinct();

                    totals.duplicatesCount = points.Count - uniques.Count();

                    if (uniques.Count() > LIST_POINT_LIMIT)
                    {
                        _pointRepository.Add(listName, uniques.Take(LIST_POINT_LIMIT));
                    }
                    else
                    {
                        _pointRepository.Add(listName, uniques);
                    }
                    totals.insertedCount = uniques.Count() > LIST_POINT_LIMIT ? LIST_POINT_LIMIT : uniques.Count();
                }
                else
                {
                    var dbPoints = _pointRepository.Get(listName);

                    var uniques = points.Distinct().Where(x => !dbPoints.Any(z => x == z)).ToList();

                    if (dbPoints.Count() + uniques.Count() > LIST_POINT_LIMIT)
                    {
                        uniques = uniques.Take(LIST_POINT_LIMIT - dbPoints.Count()).ToList();
                    }

                    totals.duplicatesCount = points.Where(x => dbPoints.Any(z => x == z)).Count() + points.Count - points.Distinct().Count();
                    totals.insertedCount = uniques.Count();
                    _pointRepository.Add(listName, uniques);
                }

                totals.IsLimitReached = _pointRepository.IsListFull(listName);
            }
        }
    }
}
