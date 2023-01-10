using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterNotificacaoPorIdUseCase : IUseCase<long, NotificacaoDetalheDto>
    {
    }
}