using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IAtribuirResponsavelGeralDoPlanoUseCase
    {
        Task<bool> Executar(long planoAEEId, string responsavelRF, string responsavelNome);
    }
}
