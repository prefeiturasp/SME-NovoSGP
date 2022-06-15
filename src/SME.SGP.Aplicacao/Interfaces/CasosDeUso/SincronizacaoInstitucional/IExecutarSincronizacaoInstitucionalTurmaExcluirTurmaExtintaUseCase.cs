using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IExecutarSincronizacaoInstitucionalTurmaExcluirTurmaExtintaUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}