using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaRecuperacaoParalela
    {
        Task<RecuperacaoParalelaListagemDto> Listar(FiltroRecuperacaoParalelaDto filtro);

        Task<PaginacaoResultadoDto<RecuperacaoParalelaTotalResultadoDto>> ListarTotalResultado(long dreId, long ueId, int cicloId, int turmaId, int ano, int? pagina);

        Task<RecuperacaoParalelaTotalEstudanteDto> TotalEstudantes(long dreId, long ueId, int cicloId, int turmaId, int ano);

        Task<RecuperacaoParalelaTotalEstudantePorFrequenciaDto> TotalEstudantesPorFrequencia(long dreId, long ueId, int cicloId, int turmaId, int ano);
    }
}