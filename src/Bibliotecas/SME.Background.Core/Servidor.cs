using SME.Background.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Background.Core
{
    public class Servidor<T>
        where T: IProcessador, new ()
    {
        T processador;

        public  Servidor()
        {
            processador = new T();
        }

        public void Registrar()
        {
            processador.Registrar();
        }
    }
}
