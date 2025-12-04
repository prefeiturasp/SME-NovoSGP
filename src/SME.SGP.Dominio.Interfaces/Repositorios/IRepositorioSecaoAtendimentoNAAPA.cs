using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSecaoAtendimentoNAAPA : IRepositorioBase<SecaoEncaminhamentoNAAPA>
    {
        Task<IEnumerable<SecaoQuestionarioDto>> ObterSecoesQuestionarioDto(int modalidade, long? encaminhamentoNAAPAId = null);
        Task<IEnumerable<SecaoEncaminhamentoNAAPA>> ObterSecoesEncaminhamentoPorModalidade(int? modalidade, long? encaminhamentoNAAPAId = null);
        Task<SecaoQuestionarioDto> ObterSecaoQuestionarioDtoPorId(long secaoId);
        Task<PaginacaoResultadoDto<AtendimentoNAAPASecaoItineranciaDto>> ObterSecoesItineranciaDtoPaginado(long encaminhamentoNAAPAId, Paginacao paginacao);
        Task<AtendimentoNAAPAItineranciaAtendimentoDto> ObterAtendimentoSecaoItinerancia(long secaoId);
        Task<IEnumerable<AtendimentosProfissionalAtendimentoNAAPAConsolidadoDto>> ObterQuantidadeAtendimentosProfissionalPorUeAnoLetivoMes(long ueId, int mes, int anoLetivo);
        Task<IEnumerable<SecaoQuestionarioDto>> ObterSecoesEncaminhamentoPorModalidades(TipoQuestionario tipoQuestionario, int[] modalidades = null);

        Task<IEnumerable<SecaoEncaminhamentoNAAPA>> ObterSecoesEncaminhamentoIndividual(long? encaminhamentoNAAPAId = null);
    }
}
