using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Background.Core.Interfaces
{
    public interface IWorker: IDisposable
    {
        void Registrar();
    }
}
