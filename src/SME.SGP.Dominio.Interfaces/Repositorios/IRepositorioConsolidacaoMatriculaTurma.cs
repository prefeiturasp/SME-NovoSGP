using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConsolidacaoMatriculaTurma
    {
        Task<IEnumerable<InformacoesEscolaresPorDreEAnoDto>> ObterGraficoMatriculasAsync(int anoLetivo, long dreId, long ueId, AnoItinerarioPrograma[] Anos, Modalidade modalidade, int? semestre);
        Task<long> Inserir(ConsolidacaoMatriculaTurma consolidacao);
        Task LimparConsolidacaoMatriculasTurmasPorAnoLetivo(int anoLetivo);
        Task<bool> ExisteConsolidacaoMatriculaTurmaPorAno(int ano);
        Task<IEnumerable<ModalidadesPorAnoItineranciaProgramaDto>> ObterModalidadesPorAnos(int anoLetivo, long dreId, long ueId, int modalidade, int semestre);
    }
}
