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
using WeatherForecastData.Models;
using WeatherForecastData.Pages;

namespace WeatherForecastData.Tests.Pages
{
    [TestClass]
    public class WeatherPageTests
    {
        [TestMethod]
        public void OnGet_ReturnsPageResult_WhenModelStateIsInvalid()
        {
            // Arrange
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
            var pageModel = new WeatherModel()
            {
                PageContext = pageContext,
                TempData = tempData,
                Url = new UrlHelper(actionContext)
            };
            pageModel.ModelState.AddModelError("ZipCode.Text", "The Text field is required.");

            // Act
            var result = pageModel.OnGet(null);

            // Assert
            var pageResult = result as NotFoundResult;
            Assert.IsNotNull(pageResult, "OnGet returns Null PageResult when Model State is Invalid");
            Assert.IsNull(pageModel.WeatherInfo, "OnPost returns NON-Null WeatherInfo when Model State is Invalid");
        }

        [TestMethod]
        public void OnGet_ReturnsPageResultAndWeatherInfo_WhenDataIsValid()
        {
            // Arrange
            const int zipCode = 95661;
            const bool fromCache = false;
            var finalTestData = GenerateTestJSONFinalData(fromCache, zipCode);
            var pageModel = new WeatherModel();

            // Act
            var result = pageModel.OnGet(finalTestData);

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
