using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaRecuperacaoParalela
    {
        Task<RecuperacaoParalelaListagemDto> Listar(FiltroRecuperacaoParalelaDto filtro);

        Task<PaginacaoResultadoDto<RecuperacaoParalelaTotalResultadoDto>> ListarTotalResultado(int? periodo, string dreId, string ueId, int? cicloId, string turmaId, string ano, int anoLetivo, int? pagina);

        Task<IEnumerable<RecuperacaoParalelaTotalResultadoDto>> ListarTotalResultadoEncaminhamento(int? periodo, string dreId, string ueId, int? cicloId, string turmaId, string ano, 
            int anoLetivo, int? numeroPagina);

        Task<RecuperacaoParalelaTotalEstudanteDto> TotalEstudantes(int? periodo, string dreId, string ueId, int? cicloId, string turmaId, string ano, int anoLetivo);

        Task<RecuperacaoParalelaTotalEstudantePorFrequenciaDto> TotalEstudantesPorFrequencia(int? periodo, string dreId, string ueId, int? cicloId, string turmaId, string ano, int anoLetivo);
    }
}