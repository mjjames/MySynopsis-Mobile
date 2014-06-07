using MySynopsis.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySynopsis.BusinessLogic.Mocks.Services
{
    public class MockTorchService : ITorchService
    {
        public bool IsTorchAvailable
        {
            get { return false; }
        }

        public TorchStatus Status
        {
            get { return TorchStatus.Unavailable; }
        }

        public bool TrySetTorchStatus(TorchStatus status)
        {
            return false;
        }

        public void Dispose()
        {
            
        }
    }
}
