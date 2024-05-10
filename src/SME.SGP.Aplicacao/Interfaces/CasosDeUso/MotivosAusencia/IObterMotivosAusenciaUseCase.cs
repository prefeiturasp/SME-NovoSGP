using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterMotivosAusenciaUseCase
    {
        Task<IEnumerable<OpcaoDropdownDto>> Executar();
    }
}
