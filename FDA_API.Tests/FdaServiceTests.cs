using FDA_API.Integration.Abstractions;
using FDA_API.Integration.Models;
using FDA_API.Integration.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FDA_API.Tests
{
	public class FdaServiceTests
	{
		private IFdaService _service;

		[SetUp]
		public void Setup()
		{
			var clientMock = new Mock<IFdaClient>();
			_service = new FdaService(clientMock.Object);
		}

		[Test]
		public void FindDateWithFewestCount_NullInput()
		{
			var result = _service.FindDateWithFewestCount(null);

			Assert.IsNotNull(result);
			Assert.AreEqual(string.Empty, result);
		}

		[Test]
		public void FindDateWithFewestCount_EmptyInput()
		{
			var countResults = new List<CountResult>();
			var result = _service.FindDateWithFewestCount(countResults);

			Assert.IsNotNull(result);
			Assert.AreEqual(string.Empty, result);
		}

		[Test]
		public void FindDateWithFewestCount_InputWithEmptyTime()
		{
			var countResults = new List<CountResult>
			{
				new CountResult
				{
					Count = 1,
					Time = string.Empty
				},
				new CountResult
				{
					Count = 2,
					Time = string.Empty
				},
				new CountResult
				{
					Count = 3,
					Time = string.Empty
				}
			};

			var result = _service.FindDateWithFewestCount(countResults);

			Assert.AreEqual(string.Empty, result);
		}

		[Test]
		public void FindDateWithFewestCount_CorrectInput()
		{
			var expected = DateTime.Today.Date.ToShortDateString();
			var countResults = new List<CountResult>
			{
				new CountResult
				{
					Count = 1,
					Time = expected
				},
				new CountResult
				{
					Count = 2,
					Time = DateTime.Today.AddDays(1).Date.ToShortDateString()
				},
				new CountResult
				{
					Count = 3,
					Time = DateTime.Today.AddDays(2).Date.ToShortDateString()
				}
			};

			var result = _service.FindDateWithFewestCount(countResults);

			Assert.IsNotNull(result);
			Assert.AreEqual(expected, result);
		}
	}
}