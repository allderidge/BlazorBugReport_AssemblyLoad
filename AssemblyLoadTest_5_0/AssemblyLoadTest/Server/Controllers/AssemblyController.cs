using System.IO;
using System.Reflection;
using AssemblyLoadTest.Dynamic;
using Microsoft.AspNetCore.Mvc;

namespace AssemblyLoadTest.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssemblyController : ControllerBase
    {
        [HttpGet]
        public DynamicAssembly Get() => GetAssemblyBytes(typeof(SharedInterfaceImplementation).Assembly);

        private DynamicAssembly GetAssemblyBytes(Assembly assembly) =>
            new DynamicAssembly { AssemblyDllBytes = System.IO.File.ReadAllBytes(assembly.Location), AssemblyPdbBytes = TryGetPdp(assembly.Location) };


        private static byte[] TryGetPdp(string assemblyLocation)
        {
            var pdp = Path.ChangeExtension(assemblyLocation, "pdp");
            return pdp != null && System.IO.File.Exists(pdp) ? System.IO.File.ReadAllBytes(pdp) : null;
        }
        
        public class DynamicAssembly
        {
            public byte[] AssemblyDllBytes { get; set; }
            public byte[] AssemblyPdbBytes { get; set; }
        }
    }
}
