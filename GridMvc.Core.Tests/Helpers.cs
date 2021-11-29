using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Moq;

namespace GridMvc.Core.Tests
{
    internal static class Helpers
    {
        public static HttpContext MockHttpContext()
        {
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock.SetupGet(x => x.Query).Returns(new QueryCollection());
            var httpResponseMock = new Mock<HttpResponse>();
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(x => x.Request).Returns(httpRequestMock.Object);
            httpContextMock.SetupGet(x => x.Response).Returns(httpResponseMock.Object);
            return httpContextMock.Object;
        }

        public static T GetAttribute<T>(this PropertyInfo pi)
        {
            return (T)pi.GetCustomAttributes(typeof(T), true).FirstOrDefault();
        }

        public static T GetAttribute<T>(this Type type)
        {
            return (T)type.GetCustomAttributes(typeof(T), true).FirstOrDefault();
        }
    }
}
