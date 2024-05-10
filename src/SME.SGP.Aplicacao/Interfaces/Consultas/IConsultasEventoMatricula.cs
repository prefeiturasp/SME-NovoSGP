using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IConsultasEventoMatricula
    {
        EventoMatriculaDto ObterUltimoEventoAluno(string codigoAluno, DateTime dataLimite);
    }
}
