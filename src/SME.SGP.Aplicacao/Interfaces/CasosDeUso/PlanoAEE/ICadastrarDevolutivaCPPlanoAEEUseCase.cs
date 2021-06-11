using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ICadastrarDevolutivaCPPlanoAEEUseCase
    {
        Task<bool> Executar(long planoAEEId, PlanoAEECadastroDevolutivaDto planoDto);
    }
}
