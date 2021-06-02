using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IConciliacaoFrequenciaTurmasSyncUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagem);

    }
}
