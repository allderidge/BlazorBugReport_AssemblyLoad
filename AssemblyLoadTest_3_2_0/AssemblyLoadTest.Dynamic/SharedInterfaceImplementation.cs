using AssemblyLoadTest.Shared;

namespace AssemblyLoadTest.Dynamic
{
    public class SharedInterfaceImplementation : ISharedInterface
    {
        public string HelloWorldName => "Alice";
    }
}
