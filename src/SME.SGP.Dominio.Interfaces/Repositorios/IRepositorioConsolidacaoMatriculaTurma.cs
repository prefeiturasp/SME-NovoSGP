using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConsolidacaoMatriculaTurma
    {
        Task<IEnumerable<InformacoesEscolaresPorDreEAnoDto>> ObterGraficoMatriculasAsync(int anoLetivo, long dreId, long ueId, string ano, Modalidade modalidade, int? semestre);
        Task<long> Inserir(ConsolidacaoMatriculaTurma consolidacao);
        Task LimparConsolidacaoMatriculasTurmasPorAnoLetivo(int anoLetivo);
    }
}
