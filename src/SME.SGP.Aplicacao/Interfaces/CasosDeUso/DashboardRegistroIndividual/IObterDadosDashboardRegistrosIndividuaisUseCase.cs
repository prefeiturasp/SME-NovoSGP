using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterDadosDashboardRegistrosIndividuaisUseCase
    {
        Task<IEnumerable<GraficoBaseQuantidadeDoubleDto>> Executar(FiltroDasboardRegistroIndividualDTO filtro);
    }
}
