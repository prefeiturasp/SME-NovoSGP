using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ICadastrarParecerPAAIPlanoAEEUseCase
    {
        Task<bool> Executar(long planoAEEId, PlanoAEECadastroParecerDto planoDto);
    }
}
