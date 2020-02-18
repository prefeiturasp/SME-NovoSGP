using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamentoTurmaDisciplina
    {
        Task<AuditoriaFechamentoTurmaDto> Salvar(long id, FechamentoTurmaDisciplinaDto entidadeDto);

        Task GerarPendenciasFechamento(long disciplinaId, Turma turma, PeriodoEscolar periodoEscolar, FechamentoTurmaDisciplina fechamento, Usuario usuarioLogado);

        Task Reprocessar(long fechamentoId);
    }
}
