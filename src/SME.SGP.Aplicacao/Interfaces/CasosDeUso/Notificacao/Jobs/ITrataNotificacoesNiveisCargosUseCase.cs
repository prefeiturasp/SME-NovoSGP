using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface ITrataNotificacoesNiveisCargosUseCase
    {
        Task<bool> Executar();
    }
}
