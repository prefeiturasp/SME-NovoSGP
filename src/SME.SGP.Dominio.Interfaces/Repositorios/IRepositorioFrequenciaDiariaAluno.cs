using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaDiariaAluno
    {
        Task<PaginacaoResultadoDto<QuantidadeAulasDiasPorBimestreAlunoCodigoTurmaDisciplinaDto>> ObterQuantidadeAulasDiasTipoFrequenciaPorBimestreAlunoCodigoTurmaDisciplina(Paginacao paginacao, int[] bimestres, string codigoAluno, long turmaId, string[] aulaDisciplinasIds, string professor = null);
    }
}
