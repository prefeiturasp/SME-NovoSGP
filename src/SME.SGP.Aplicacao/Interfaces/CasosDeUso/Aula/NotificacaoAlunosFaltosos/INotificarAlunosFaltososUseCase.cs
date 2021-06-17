using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface INotificarAlunosFaltososUseCase : IUseCase<MensagemRabbit, bool>
    {
    }
}
