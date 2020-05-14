using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamentoTurmaDisciplina
    {
        Task<AuditoriaPersistenciaDto> Salvar(long id, FechamentoTurmaDisciplinaDto entidadeDto, bool componenteSemNota = false);

        Task GerarPendenciasFechamento(long disciplinaId, Turma turma, PeriodoEscolar periodoEscolar, FechamentoTurmaDisciplina fechamento, Usuario usuarioLogado, bool componenteSemNota = false);

        Task Reprocessar(long fechamentoId);

        void VerificaPendenciasFechamento(long fechamentoId);

        void GerarNotificacaoAlteracaoLimiteDias(Turma turma, Usuario usuarioLogado, Ue ue, int bimestre, string alunosComNotaAlterada);
    }
}
