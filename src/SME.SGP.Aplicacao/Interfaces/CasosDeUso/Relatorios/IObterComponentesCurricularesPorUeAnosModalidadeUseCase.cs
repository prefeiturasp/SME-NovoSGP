using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterComponentesCurricularesPorUeAnosModalidadeUseCase
    {
        Task<IEnumerable<ComponenteCurricularEol>> Executar(string[] anos, int anoLetivo, string codigoUe, Modalidade modalidade);
    }
}
