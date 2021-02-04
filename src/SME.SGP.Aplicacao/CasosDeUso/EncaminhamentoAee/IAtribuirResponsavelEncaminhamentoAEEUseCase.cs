using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IAtribuirResponsavelEncaminhamentoAEEUseCase
    {
        Task<bool> Executar(long encaminhamentoId, string rfResponsavel);
    }
}