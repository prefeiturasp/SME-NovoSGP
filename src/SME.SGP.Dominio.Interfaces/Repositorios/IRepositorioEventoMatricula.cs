using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEventoMatricula : IRepositorioBase<EventoMatricula>
    {
        bool CheckarEventoExistente(SituacaoMatriculaAluno tipo, DateTime dataEvento, string codigoAluno);
    }
}
