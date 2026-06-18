using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IConsultasProcessoExecutando
    {
        Task<bool> ExecutandoCalculoFrequencia(string turmaId, string disciplinaId, int bimestre);
    }
}
