using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ISincronizarAulasInfantilUseCase
    {
        void Executar();

        Task<bool> Executar(long codigoTurma);
    }
}