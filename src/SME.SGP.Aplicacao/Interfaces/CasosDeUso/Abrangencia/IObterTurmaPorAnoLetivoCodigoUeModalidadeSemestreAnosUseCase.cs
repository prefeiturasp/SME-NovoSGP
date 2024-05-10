using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreAnosUseCase
    {
        Task<IEnumerable<OpcaoDropdownDto>> Executar(string codigoUe, int anoLetivo, Modalidade? modalidade, int semestre, IList<string> anos);
    }
}
