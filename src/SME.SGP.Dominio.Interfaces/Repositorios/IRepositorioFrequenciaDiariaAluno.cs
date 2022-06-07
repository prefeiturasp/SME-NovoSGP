using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaDiariaAluno
    {
        Task<IEnumerable<QuantidadeAulasDiasPorBimestreAlunoCodigoTurmaDisciplinaDto>> ObterQuantidadeAulasDiasPorBimestreAlunoCodigoTurmaDisciplina(int bimestre,string codigoAluno,long turmaId,string aulaDisciplinaId);
        Task<IEnumerable<ObterMotivoAusenciaAlunoFrequenciaDiariaPorAulasIdsDto>> ObterMotivoAusenciaAlunoFrequenciaDiariaPorAulasIds(int[]aulaIds);
    }
}
