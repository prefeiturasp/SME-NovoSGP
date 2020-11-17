using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IExecutarExclusaoPendenciaParametroEvento : IUseCase<MensagemRabbit, bool>
    {
    }
}
