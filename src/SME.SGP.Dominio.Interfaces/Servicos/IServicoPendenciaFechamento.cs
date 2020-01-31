using System;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoPendenciaFechamento
    {
        void ValidarAulasReposicaoPendente(long fechamentoId, Turma turma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);

        void ValidarAvaliacoesSemNotasParaNenhumAluno(long fechamentoId, string codigoTurma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);
    }
}