using DevBridgeSquares.Common.Exceptions;
using DevBridgeSquares.Entities.Models;
using DevBridgeSquares.Repositories.Repositories;
using DevBridgeSquares.Services.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DevBridgeSquares.Test.Service
{
    public class PointServiceTests
    {
        private Mock<IPointRepository> _pointRepository;

        private IPointService _pointService;

        public PointServiceTests()
        {
            _pointRepository = new Mock<IPointRepository>();

            _pointService = new PointService(_pointRepository.Object);
        }

        [Fact]
        public void PointServiceTests_AddPoint_Success()
        {
            string listName = "List";

            var coordX = 1;
            var coordY = 1;
            var id = 0;

            var point = new Point()
            {
                X = coordX,
                Y = coordY,
                Id = id
            };

            _pointRepository.Setup(t => t.GetByCoordinates(listName, coordX, coordY));
            _pointRepository.Setup(x => x.IsListPresent(listName)).Returns(true);

            _pointService.Add(listName, point);

            _pointRepository.Verify(p => p.Add(It.Is<string>(x => x.Equals(listName)), It.Is<Point>(x => x == point)));
        }

        [Fact]
        public void PointServiceTests_AddPoint_NoList()
        {
            string listName = "List";

            var coordX = 1;
            var coordY = 1;
            var id = 0;

            var point = new Point()
            {
                X = coordX,
                Y = coordY,
                Id = id
            };

            _pointRepository.Setup(x => x.IsListPresent(listName)).Returns(false);

            DVBSException ex = Assert.Throws<DVBSException>(() => _pointService.Add(listName, point));

            Assert.Equal(ex.Code, DVBSCode.Point.ListDoesntExist);
        }

        [Fact]
        public void PointServiceTests_AddPoint_Duplicate()
        {
            string listName = "List";

            var coordX = 1;
            var coordY = 1;
            var id = 0;

            var point = new Point()
            {
                X = coordX,
                Y = coordY,
                Id = id
            };

            _pointRepository.Setup(t => t.GetByCoordinates(listName, coordX, coordY)).Returns(point);
            _pointRepository.Setup(x => x.IsListPresent(listName)).Returns(true);

            DVBSException ex = Assert.Throws<DVBSException>(() => _pointService.Add(listName, point));

            Assert.Equal(ex.Code, DVBSCode.Point.DuplicatePoint);
        }

        [Fact]
        public void PointServiceTests_AddPoint_ListFull()
        {
            string listName = "List";

            var coordX = 1;
            var coordY = 1;
            var id = 0;

            var point = new Point()
            {
                X = coordX,
                Y = coordY,
                Id = id
            };

            _pointRepository.Setup(x => x.IsListPresent(listName)).Returns(true);
            _pointRepository.Setup(x => x.IsListFull(listName)).Returns(true);

            DVBSException ex = Assert.Throws<DVBSException>(() => _pointService.Add(listName, point));

            Assert.Equal(ex.Code, DVBSCode.Point.LimitExceeded);
        }
    }
}
