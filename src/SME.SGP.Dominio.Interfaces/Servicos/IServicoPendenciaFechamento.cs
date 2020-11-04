using System;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoPendenciaFechamento
    {
        Task<int> ValidarAulasReposicaoPendente(long fechamentoId, Turma turma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);

        Task<int> ValidarAulasSemFrequenciaRegistrada(long fechamentoId, Turma turma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);

        Task<int> ValidarAulasSemPlanoAulaNaDataDoFechamento(long fechamentoId, Turma turma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);

        int ValidarAvaliacoesSemNotasParaNenhumAluno(long fechamentoId, string codigoTurma, long disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);

        Task<int> ValidarPercentualAlunosAbaixoDaMedia(FechamentoTurmaDisciplina fechamentoTurma);
        Task<AuditoriaPersistenciaDto> Aprovar(long pendenciaId);
        bool VerificaPendenciasFechamento(long fechamentoId);

        Task<AuditoriaPersistenciaDto> AtualizarPendencia(long pendenciaId, SituacaoPendencia situacaoPendencia);

        int ObterQuantidadePendenciasGeradas();
        Task<int> ValidarAlteracaoExtemporanea(long fechamentoId, string turmaCodigo, long disciplinaId);
    }
}