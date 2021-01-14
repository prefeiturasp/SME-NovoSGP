using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public interface IObterEncaminhamentoPorIdUseCase : IUseCase<long, EncaminhamentoAEERespostaDto>
    {
    }
}