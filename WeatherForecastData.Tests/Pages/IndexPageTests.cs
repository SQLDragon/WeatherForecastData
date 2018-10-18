using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using WeatherForecastData.Models;
using WeatherForecastData.Pages;
using WeatherForecastData.Repositories;

namespace WeatherForecastData.Tests.Pages
{
    [TestClass]
    public class IndexPageTests
    {
        [TestMethod]
        public void OnGet_PopulatesThePageModel_WithAListOfRegions()
        {
            // Arrange
            var mockRegionsRepo = new Mock<IRegionsRepository>();
            var mockWeatherRepo = new Mock<IWeatherRepository>();
            var expectedRegions = new RegionsRepository().GetRegionsSelectList();
            mockRegionsRepo.Setup(x => x.GetRegionsSelectList()).Returns(expectedRegions);
            var pageModel = new IndexModel(mockRegionsRepo.Object, mockWeatherRepo.Object);

            // Act
            pageModel.OnGet();

            // Assert
            var actualRegions = pageModel.Regions;
            Assert.IsNotNull(actualRegions, "Regions List in OnGet is Null!");
            Assert.AreEqual(51, actualRegions.Count(), "Regions List Count is Incorrect!");
            CollectionAssert.AreEqual(
                expectedRegions.OrderBy(r => r.Value).Select(r => r.Text).ToList(),
                actualRegions.OrderBy(r => r.Value).Select(r => r.Text).ToList(),
                "Regions List is Not a match!");
        }

        [TestMethod]
        public void OnPost_ReturnsPageResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockRegionsRepo = new Mock<IRegionsRepository>();
            var mockWeatherRepo = new Mock<IWeatherRepository>();
            var expectedRegions = new RegionsRepository().GetRegionsSelectList();
            mockRegionsRepo.Setup(x => x.GetRegionsSelectList()).Returns(expectedRegions);
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var pageContext = new PageContext(actionContext)
            {
                ViewData = viewData
            };
            var pageModel = new IndexModel(mockRegionsRepo.Object, mockWeatherRepo.Object)
            {
                PageContext = pageContext,
                TempData = tempData,
                Url = new UrlHelper(actionContext)
            };
            pageModel.ModelState.AddModelError("ZipCode.Text", "The Text field is required.");

            // Act
            var result = pageModel.OnPost();

            // Assert
            var pageResult = result as PageResult;
            Assert.IsNotNull(pageResult, "OnPost returns Null PageResult when Model State is Invalid");
            Assert.IsNull(pageModel.WeatherInfo, "OnPost returns NON-Null WeatherInfo when Model State is Invalid");
        }

        [TestMethod]
        public void OnPost_ReturnsPageResult_WhenZipCodeIsInvalid()
        {
            // Arrange
            var mockRegionsRepo = new Mock<IRegionsRepository>();
            var mockWeatherRepo = new Mock<IWeatherRepository>();
            var expectedRegions = new RegionsRepository().GetRegionsSelectList();
            mockRegionsRepo.Setup(x => x.GetRegionsSelectList()).Returns(expectedRegions);
            var address = new Address() { ZipCode = "1234A" };
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var pageContext = new PageContext(actionContext)
            {
                ViewData = viewData
            };
            var pageModel = new IndexModel(mockRegionsRepo.Object, mockWeatherRepo.Object)
            {
                PageContext = pageContext,
                TempData = tempData,
                Url = new UrlHelper(actionContext),
                Address = address
            };

            // Act
            var result = pageModel.OnPost();

            // Assert
            var pageResult = result as PageResult;
            Assert.IsNotNull(pageResult, "OnPost returns Null PageResult when ZipCode is Invalid");
            Assert.IsNull(pageModel.WeatherInfo, "OnPost returns NON-Null WeatherInfo when ZipCode is Invalid");
        }

        [TestMethod]
        public void OnPost_ReturnsPageResultAndWeatherInfo_WhenDataIsValid()
        {
            // Arrange
            const int zipCode = 95661;
            const bool fromCache = false;
            var finalTestData = GenerateTestJSONFinalData(fromCache, zipCode);
            var mockRegionsRepo = new Mock<IRegionsRepository>();
            var mockWeatherRepo = new Mock<IWeatherRepository>();
            var expectedRegions = new RegionsRepository().GetRegionsSelectList();
            mockRegionsRepo.Setup(x => x.GetRegionsSelectList()).Returns(expectedRegions);
            mockWeatherRepo.Setup(x => x.GetWeatherData(zipCode)).Returns(finalTestData);
            var address = new Address() { ZipCode = "95661" };
            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var pageContext = new PageContext(actionContext)
            {
                ViewData = viewData
            };
            var pageModel = new IndexModel(mockRegionsRepo.Object, mockWeatherRepo.Object)
            {
                PageContext = pageContext,
                TempData = tempData,
                Url = new UrlHelper(actionContext),
                Address = address
            };

            // Act
            var result = pageModel.OnPost();

            // Assert
            var pageResult = result as PageResult;
            Assert.IsNotNull(pageResult, "OnPost returns Null PageResult when Data is valid");
            Assert.IsNotNull(pageModel.WeatherInfo, "OnPost returns Null WeatherInfo when Data is valid");
            Assert.AreEqual(zipCode.ToString(), pageModel.WeatherInfo.ZipCode, "ZipCode in WeatherInfo is Incorrect!");
            Assert.AreEqual(2, pageModel.WeatherInfo.ForcastInfo.Count, "ForecastInfo.Count in WeatherInfo is Incorrect!");
            Assert.IsFalse(pageModel.WeatherInfo.FromCache, "FromCache in WeatherInfo is Incorrect!");
        }

        private static WeatherData GenerateTestJSONFinalData(bool fromCache, int zipCode)
        {
            return new WeatherData()
            {
                FromCache = fromCache,
                ZipCode = zipCode.ToString(),
                ForcastInfo = new List<ForecastData>() { new ForecastData(), new ForecastData() }
            };
        }
    }
}
