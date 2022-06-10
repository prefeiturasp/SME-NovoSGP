using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaDiariaAluno
    {
        Task<PaginacaoResultadoDto<QuantidadeAulasDiasPorBimestreAlunoCodigoTurmaDisciplinaDto>> ObterQuantidadeAulasDiasTipoFrequenciaPorBimestreAlunoCodigoTurmaDisciplina(Paginacao paginacao, int bimestre,string codigoAluno,long turmaId,string aulaDisciplinaId);
    }
}
