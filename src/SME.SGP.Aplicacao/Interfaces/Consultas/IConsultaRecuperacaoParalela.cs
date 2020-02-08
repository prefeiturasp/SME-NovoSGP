using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaRecuperacaoParalela
    {
        Task<RecuperacaoParalelaListagemDto> Listar(FiltroRecuperacaoParalelaDto filtro);

        Task<RecuperacaoParalelaTotalEstudanteDto> TotalEstudantes(long dreId, long ueId, int cicloId, int turmaId, int ano);

        Task<RecuperacaoParalelaTotalEstudantePorFrequenciaDto> TotalEstudantesPorFrequencia(long dreId, long ueId, int cicloId, int turmaId, int ano);
    }
}