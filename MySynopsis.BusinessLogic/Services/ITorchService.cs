using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySynopsis.BusinessLogic.Services
{
    public interface ITorchService : IDisposable
    {
        bool IsTorchAvailable { get; }
        TorchStatus Status{ get; }
        bool TrySetTorchStatus(TorchStatus status);
    }

    public enum TorchStatus
    {
        Off,
        On,
        Unavailable
    }
}
