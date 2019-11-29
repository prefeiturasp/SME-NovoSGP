using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDre
    {
        void Sincronizar(IEnumerable<Dre> entidades);
        IEnumerable<Dre> ObterPorCodigos(string[] codigo);
    }
}
