using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Blusutils.DESrv.PDK.ConnectionInterfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blusutils.DESrv.Tests.ConnectionTests;
public class TcpConntectionTests : Tests {

    BaseTcpProcessor tcpProcessor;

    [SetUp]
    public void Setup() {
        tcpProcessor = new("127.0.0.1", 8801);
    }

    [Test]
    public async Task Connection() {
        try {
            tcpProcessor.Run();
        } catch (SocketException e) { Assert.Fail(e.ToString()); }

        bool flag = false;

        tcpProcessor.NewDataEvent += (_, data) => {
            if (data.First() == 10) Assert.Pass();
        };

        var cl = new TcpClient("127.0.0.1", 8801);
        await cl.GetStream().WriteAsync(new byte[] { 10 });

    }

    [Test]
    public async Task Sending() { // TODO: fix
        try {
            tcpProcessor.Run();
        } catch (SocketException e) { Assert.Fail(e.ToString()); }

        tcpProcessor.NewClientEvent += async (client) => {
            await client.GetStream().WriteAsync(new byte[] { 10 });
        };

        var cl = new TcpClient("127.0.0.1", 8801);
        var mem = new Memory<byte>();
        await cl.GetStream().ReadAsync(mem);
        if (mem.ToArray().First() == 10) Assert.Pass();
    }
}
