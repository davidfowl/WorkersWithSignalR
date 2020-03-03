using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WorkerHostedHub
{
    public class ClockHub : Hub<IClockClient>
    {
    }

    public interface IClockClient
    {
        Task OnTimeReceived(DateTime now);
    }
}