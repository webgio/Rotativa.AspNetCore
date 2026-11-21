using System;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Rotativa.AspNetCore.Tests
{

    [Trait("Rotativa.AspNetCore", "testing the base url that is added to the HTML.")]
    public class BaseUrlTest : IDisposable
    {
        private const string ContainerVariable = "DOTNET_RUNNING_IN_CONTAINER";

        private readonly string? originalValue;

        public BaseUrlTest()
        {
            originalValue = Environment.GetEnvironmentVariable(ContainerVariable);
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable(ContainerVariable, originalValue);
        }

        [Fact(DisplayName = "base url is the address the browser used.")]
        public void BaseUrl_IsTheBrowserAddress()
        {
            Environment.SetEnvironmentVariable(ContainerVariable, null);

            Assert.Equal("https://example.com", GetBaseUrl("https", "example.com", 8080));
        }

        [Fact(DisplayName = "outside a container, localhost keeps the port the browser used.")]
        public void BaseUrl_OutsideContainer_KeepsBrowserPort()
        {
            Environment.SetEnvironmentVariable(ContainerVariable, null);

            Assert.Equal("http://localhost:5001", GetBaseUrl("http", "localhost:5001", 8080));
        }

        [Fact(DisplayName = "in a container, localhost uses the port the app listens on.")]
        public void BaseUrl_InContainer_UsesAppPort()
        {
            Environment.SetEnvironmentVariable(ContainerVariable, "true");

            Assert.Equal("http://localhost:8080", GetBaseUrl("http", "localhost:5001", 8080));
        }

        [Fact(DisplayName = "in a container, 127.0.0.1 uses the port the app listens on.")]
        public void BaseUrl_InContainer_LoopbackAddressUsesAppPort()
        {
            Environment.SetEnvironmentVariable(ContainerVariable, "true");

            Assert.Equal("http://127.0.0.1:8080", GetBaseUrl("http", "127.0.0.1:5001", 8080));
        }

        [Fact(DisplayName = "in a container, other host names are kept.")]
        public void BaseUrl_InContainer_KeepsOtherHosts()
        {
            Environment.SetEnvironmentVariable(ContainerVariable, "true");

            Assert.Equal("https://example.com", GetBaseUrl("https", "example.com", 8080));
        }

        private static string GetBaseUrl(string scheme, string host, int localPort)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = scheme;
            httpContext.Request.Host = new HostString(host);
            httpContext.Connection.LocalPort = localPort;

            return new TestViewAsPdf().BaseUrlFor(httpContext);
        }

        private class TestViewAsPdf : ViewAsPdf
        {
            public string BaseUrlFor(HttpContext httpContext)
            {
                return this.GetBaseUrl(httpContext);
            }
        }
    }
}
