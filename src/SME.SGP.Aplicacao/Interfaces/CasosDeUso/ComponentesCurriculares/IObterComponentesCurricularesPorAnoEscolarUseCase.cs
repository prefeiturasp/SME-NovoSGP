using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterComponentesCurricularesPorAnoEscolarUseCase
    {
        Task<IEnumerable<ComponenteCurricularEol>> Executar(string codigoUe, Modalidade modalidade, int anoLetivo, string[] anosEscolares);
    }
}