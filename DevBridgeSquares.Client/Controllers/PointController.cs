using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevBridgeSquares.Repositories.Repositories;
using DevBridgeSquares.Entities.Params;
using DevBridgeSquares.Entities.Models;
using DevBridgeSquares.Services.Services;

namespace DevBridgeSquares.Client.Controllers
{
    public class PointController : Controller
    {
        private readonly IPointRepository _pointRepository;
        private readonly IPointService _pointService;

        public PointController(IPointRepository pointRepository, IPointService pointService)
        {
            _pointService = pointService;
            _pointRepository = pointRepository;
        }

        [HttpGet]
        public IActionResult Get(string listName)
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult GetLists()
        {
            var result = _pointRepository.GetListNames();
            return Ok(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Point point, string listName)
        {
            _pointRepository.Add(listName, point);
            return new StatusCodeResult(201);
        }

        [HttpGet]
        public IActionResult GetListPoints(PointParams param)
        {
            var result = _pointRepository.Get(param, out int totalEntries, out int page);

            return Ok(new
            {
                entries = result,
                totalEntries = totalEntries,
                page = page
            });
        }
    }
}
