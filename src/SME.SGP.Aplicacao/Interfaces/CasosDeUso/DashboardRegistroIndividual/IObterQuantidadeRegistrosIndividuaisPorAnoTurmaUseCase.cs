using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCase
    {
        Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDasboardRegistroIndividualDTO filtro);
    }
}
