using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevBridgeSquares.Repositories.Repositories;
using DevBridgeSquares.Entities.Params;
using DevBridgeSquares.Entities.Models;
using DevBridgeSquares.Services.Services;
using Microsoft.AspNetCore.Http;
using DevBridgeSquares.Entities.Enums;
using DevBridgeSquares.Entities.ViewModels;
using System.Text;
using DevBridgeSquares.Entities.Params.Base;

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
        public IActionResult GetLists()
        {
            var result = _pointRepository.GetListNames();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Add(string listName, Point point)
        {
            _pointService.Add(listName, point);
            return new StatusCodeResult(201);
        }

        [HttpPost]
        public IActionResult Delete(string listName, int Id)
        {
            _pointRepository.Remove(listName, Id);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetListPoints(PointParams param)
        {
            var result = _pointService.Get(param, out int totalEntries, out int page);

            return Ok(new
            {
                entries = result,
                totalEntries = totalEntries,
                page = page
            });
        }

        [HttpPost]
        public IActionResult Import(ListOptionType list, string listName)
        {
            if (Request.Form.Files.Count == 0) { return new StatusCodeResult(StatusCodes.Status204NoContent); }

            var file = Request.Form.Files[0];

            if (!file.ContentType.Equals("text/plain")) { return new StatusCodeResult(StatusCodes.Status415UnsupportedMediaType); }

            _pointService.Upload(file.OpenReadStream(), list, listName, out UploadTotals totals);

            return Ok(totals);
        }

        [HttpPost]
        public IActionResult ClearList(string listName)
        {
            _pointService.Clear(listName);
            return Ok();
        }

        [HttpPost]
        public IActionResult DeleteList(string listName)
        {
            _pointService.Remove(listName);
            return Ok();
        }

        [HttpPost]
        public IActionResult CreateList(string listName)
        {
            _pointService.Add(listName);
            return Ok();
        }

        [HttpGet]
        public IActionResult Download(string listName)
        {
            return File(Encoding.ASCII.GetBytes(_pointService.GetDownloadData(listName)), "text/plain", $"{listName}.txt");
        }

        [HttpGet]
        public IActionResult FindSquares(string listName, Params par)
        {
            var result = _pointService.FindSquares(listName);

            var totalEntries = result.Count;   
            
            return Ok(new { page = par.page, result = result.Skip(par.perPage * (par.page - 1)).Take(par.perPage), totalEntries = totalEntries });
        }
    }
}
