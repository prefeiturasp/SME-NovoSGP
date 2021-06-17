using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IEnviarNotificacaoReestruturacaoPlanoAEEUseCase : IUseCase<MensagemRabbit, bool>
    {
    }
}
