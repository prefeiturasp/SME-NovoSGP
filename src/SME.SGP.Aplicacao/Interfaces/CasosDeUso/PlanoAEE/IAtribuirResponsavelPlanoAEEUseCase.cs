using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IAtribuirResponsavelPlanoAEEUseCase
    {
        Task<bool> Executar(long planoAEEId, string responsavelRF);
    }
}