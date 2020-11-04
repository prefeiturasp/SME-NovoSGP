using SME.SGP.Infra.Dtos.EscolaAqui.DashboardAdesao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterTotaisAdesaoUseCase
    {
        Task<IEnumerable<TotaisAdesaoResultado>> Executar(string codigoDre, string codigoUe);
    }
    
}