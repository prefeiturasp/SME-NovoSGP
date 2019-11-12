using System;

namespace SME.SGP.Infra
{
    public interface IServicoLog
    {
        void Registrar(Exception ex);

        void Registrar(string mensagem);
    }
}