using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasAprovacaoUeUseCase
    {
        Task<PainelEducacionalAprovacaoUeRetorno> ObterAprovacao(FiltroAprovacaoUeDto filtro);
    }
}
