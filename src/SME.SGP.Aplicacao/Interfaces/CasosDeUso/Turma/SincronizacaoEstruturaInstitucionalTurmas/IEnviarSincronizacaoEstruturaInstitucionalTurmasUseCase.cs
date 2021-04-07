using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IEnviarSincronizacaoEstruturaInstitucionalTurmasUseCase : IUseCase<MensagemRabbit, bool>
    {
    }
}