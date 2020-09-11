using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Index = AssemblyLoadTest.Client.Pages.Index;

namespace AssemblyLoadTest.Tests
{
    [TestClass]
    public class TestIndexPage
    {
        private readonly HttpClient _httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:55513")};

        [TestInitialize]
        public async Task Init()
        {
            try
            {
                var result = await _httpClient.GetAsync("/");
                result.EnsureSuccessStatusCode();
            }
            catch
            {
                throw new ApplicationException("Start the AssemblyLoadTest.Server by viewing in Visual Studio before running tests.");
            }
        }


        /// <summary>
        /// This test passed when run outside the browser. When the code is run within the browser all asserts would fail.
        /// </summary>
        [TestMethod]
        public async Task TestOutsideBrowser()
        {
            var sut = new Index();

            sut.GetType().GetProperty("Client", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(sut, _httpClient);

            await (Task) sut.GetType().GetMethod("OnInitializedAsync", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(sut, new object[0]);

            Assert.IsTrue(sut.SharedInterfaceMatches ?? false);
            Assert.IsTrue(sut.SharedAssemblyMatches ?? false);
            Assert.AreEqual("Alice", sut.Name);
            Assert.IsNull(sut.ExceptionText);
        }
    }
}
