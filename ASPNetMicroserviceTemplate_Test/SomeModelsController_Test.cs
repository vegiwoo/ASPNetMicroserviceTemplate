using System.Diagnostics;

namespace ASPNetMicroserviceTemplate_Test;

public class SomeModelsController_Test
{
    [OneTimeSetUp]
    public void Setup()
    {
        Trace.Listeners.Add(new ConsoleTraceListener()); 
        Trace.WriteLine("Setup");
    }

    [Test]
    public void Test1()
    {
        Trace.WriteLine("Test1");
        Assert.Pass();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        Trace.WriteLine("TearDown");
        Trace.Flush();
    }
}