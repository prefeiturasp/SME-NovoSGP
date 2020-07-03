using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEventoMatricula : IRepositorioBase<EventoMatricula>
    {
        bool CheckarEventoExistente(SituacaoMatriculaAluno tipo, DateTime dataEvento, string codigoAluno);
        EventoMatricula ObterUltimoEventoAluno(string codigoAluno, DateTime dataLimite);
    }
}
