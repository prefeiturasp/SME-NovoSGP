using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterComponenteCurricularLancaNotaUseCase
    {
        Task<bool> Executar(long componenteCurricularId);
    }
}
