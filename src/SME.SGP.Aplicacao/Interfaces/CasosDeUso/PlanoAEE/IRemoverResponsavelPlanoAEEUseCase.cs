using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IRemoverResponsavelPlanoAEEUseCase
    {
        Task<bool> Executar(long encaminhamentoId);    
    }
}