using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaRecuperacaoParalela
    {
        Task<RecuperacaoParalelaListagemDto> Listar(FiltroRecuperacaoParalelaDto filtro);

        Task<object> ListarPeriodo();
    }
}