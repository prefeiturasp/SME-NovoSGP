using SME.SGP.Infra.Dtos.PainelEducacional.Reclassificacao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasReclassificacaoPainelEducacionalUseCase
    {
        Task<IEnumerable<PainelEducacionalReclassificacaoDto>> ObterReclassificacao(string codigoDre, string codigoUe, int anoLetivo, string anoTurma);
    }
}
