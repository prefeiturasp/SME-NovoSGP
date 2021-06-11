using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ICadastrarDevolutivaPAAIPlanoAEEUseCase
    {
        Task<bool> Executar(long planoAEEId, PlanoAEECadastroDevolutivaDto planoDto);
    }
}
