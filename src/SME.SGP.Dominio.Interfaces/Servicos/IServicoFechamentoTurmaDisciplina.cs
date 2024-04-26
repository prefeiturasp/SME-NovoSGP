using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamentoTurmaDisciplina
    {
        Task<AuditoriaPersistenciaDto> Salvar(long id, FechamentoTurmaDisciplinaDto entidadeDto, bool componenteSemNota = false, bool processamento = false);

        Task Reprocessar(long fechamentoId, Usuario usuario = null);

        Task GerarNotificacaoAlteracaoLimiteDias(Turma turma, Usuario usuarioLogado, Ue ue, int bimestre, string alunosComNotaAlterada);
    }
}
