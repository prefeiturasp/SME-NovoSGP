using SME.Background.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Background.Core
{
    public class Servidor<T> : IDisposable
        where T: IWorker
    {
        readonly T worker;

        public  Servidor(T worker)
        {
            this.worker = worker;
        }

        public void Dispose()
        {
            worker?.Dispose();
        }

        public void Registrar()
        {
            worker.Registrar();
        }
    }
}
