using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConsolidacaoRegistroIndividualMedia
    {
        Task<IEnumerable<RegistroItineranciaMediaPorAnoDto>> ObterRegistrosItineranciasMediaPorAnoAsync(int anoLetivo, long dreId, Modalidade modalidade);
        Task<IEnumerable<GraficoBaseQuantidadeDoubleDto>> ObterRegistrosItineranciasMediaPorTurmaAsync(int anoLetivo, long ueId, Modalidade modalidade);
    }
}
