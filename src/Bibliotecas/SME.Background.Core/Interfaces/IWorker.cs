using System;

namespace SME.Background.Core.Interfaces
{
    public interface IWorker : IDisposable
    {
        void Registrar();
    }
}
