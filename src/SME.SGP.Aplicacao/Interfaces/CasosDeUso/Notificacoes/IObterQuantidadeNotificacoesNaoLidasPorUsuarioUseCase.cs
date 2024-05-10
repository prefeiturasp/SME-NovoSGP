using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterQuantidadeNotificacoesNaoLidasPorUsuarioUseCase
    {
        Task<int> Executar();
    }
}