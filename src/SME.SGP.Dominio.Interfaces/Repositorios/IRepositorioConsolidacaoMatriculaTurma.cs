using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConsolidacaoMatriculaTurma
    {
        Task<IEnumerable<InformacoesEscolaresPorDreEAnoDto>> ObterGraficoMatriculasAsync(int anoLetivo, long dreId, long ueId, string ano, Modalidade modalidade, int? semestre);
    }
}
