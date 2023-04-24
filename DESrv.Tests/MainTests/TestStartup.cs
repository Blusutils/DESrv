using Blusutils.DESrv.Configuration;

namespace Blusutils.DESrv.Tests.MainTests;

public class TestStartup : Tests
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void TestDESrvStartup()
    {
        var cts = new CancellationTokenSource();
        var thr = new Thread(() => bootstrapper.Start(cts.Token));
        thr.Start();
        try
        {
            cts.Cancel();
            Assert.Pass();
        }
        catch (Exception ex)
        {
            if (ex is not SuccessException) Assert.Fail(ex.ToString());
        }
    }
}