using System;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoPendenciaFechamento
    {
        bool ValidarAulasReposicaoPendente(long fechamentoId, Turma turma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);

        bool ValidarAulasSemFrequenciaRegistrada(long fechamentoId, Turma turma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);

        bool ValidarAulasSemPlanoAulaNaDataDoFechamento(long fechamentoId, Turma turma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);

        bool ValidarAvaliacoesSemNotasParaNenhumAluno(long fechamentoId, string codigoTurma, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);
    }
}