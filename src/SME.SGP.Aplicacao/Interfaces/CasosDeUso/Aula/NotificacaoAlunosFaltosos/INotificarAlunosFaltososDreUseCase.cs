using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface INotificarAlunosFaltososDreUseCase : IUseCase<MensagemRabbit, bool>
    {
    }
}