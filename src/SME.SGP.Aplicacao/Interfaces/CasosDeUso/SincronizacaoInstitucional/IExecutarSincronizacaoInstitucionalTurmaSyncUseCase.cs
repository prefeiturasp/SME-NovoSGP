using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IExecutarSincronizacaoInstitucionalTurmaSyncUseCase : IUseCase<MensagemRabbit, bool>
    {
    }
}