using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IRemoverResponsavelEncaminhamentoAEEUseCase
    {
        Task<bool> Executar(long encaminhamentoId);
    }
}