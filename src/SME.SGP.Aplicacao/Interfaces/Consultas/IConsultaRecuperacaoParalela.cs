using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaRecuperacaoParalela
    {
        Task<RecuperacaoParalelaListagemDto> Listar(FiltroRecuperacaoParalelaDto filtro);

        Task<PaginacaoResultadoDto<RecuperacaoParalelaTotalResultadoDto>> ListarTotalResultado(FiltroRecuperacaoParalelaResumoDto filtro);

        Task<IEnumerable<RecuperacaoParalelaTotalResultadoDto>> ListarTotalResultadoEncaminhamento(FiltroRecuperacaoParalelaResumoDto filtro);

        Task<RecuperacaoParalelaTotalEstudanteDto> TotalEstudantes(FiltroRecuperacaoParalelaResumoDto filtro);

        Task<RecuperacaoParalelaTotalEstudantePorFrequenciaDto> TotalEstudantesPorFrequencia(FiltroRecuperacaoParalelaResumoDto filtro);
    }
}