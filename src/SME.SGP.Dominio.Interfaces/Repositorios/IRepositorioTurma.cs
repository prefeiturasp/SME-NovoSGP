using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTurma
    {
        void Sincronizar(IEnumerable<Turma> entidades);
    }
}
