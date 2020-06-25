using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IAlterarAulaRecorrenteUseCase : IUseCase<MensagemRabbit, bool>
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
