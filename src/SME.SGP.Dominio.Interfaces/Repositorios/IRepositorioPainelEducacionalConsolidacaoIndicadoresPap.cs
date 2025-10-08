using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalConsolidacaoIndicadoresPap
    {
        Task<int?> ObterUltimoAnoConsolidado();
        Task<IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto>> ObterContagemDificuldadesConsolidadaGeral(IEnumerable<DadosMatriculaAlunoTipoPapDto> dadosMatriculaAlunos, CancellationToken cancellationToken);
        Task ExcluirConsolidacaoPorAno(int ano);
        Task SalvarConsolidacaoSme(IList<PainelEducacionalConsolidacaoPapSme> consolidacao);
        Task SalvarConsolidacaoUe(IList<PainelEducacionalConsolidacaoPapUe> consolidacao);
        Task SalvarConsolidacaoDre(IList<PainelEducacionalConsolidacaoPapDre> consolidacao);
    }
}