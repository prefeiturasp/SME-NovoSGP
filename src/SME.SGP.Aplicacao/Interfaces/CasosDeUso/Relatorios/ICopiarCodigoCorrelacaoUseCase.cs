using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ICopiarCodigoCorrelacaoUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
