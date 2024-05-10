using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ICadastrarParecerCPPlanoAEEUseCase
    {
        Task<bool> Executar(long planoAEEId, PlanoAEECadastroParecerDto planoDto);
    }
}
