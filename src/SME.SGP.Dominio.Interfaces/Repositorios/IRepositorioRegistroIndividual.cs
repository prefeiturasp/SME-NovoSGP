using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioRegistroIndividual : IRepositorioBase<RegistroIndividual>
    {
        Task<RegistroIndividual> ObterPorAlunoData(long turmaId, long componenteCurricularId, long alunoCodigo, DateTime data);

        Task<IEnumerable<RegistroIndividual>> ObterPorAlunoPeriodo(long turmaId, long componenteCurricularId, long alunoCodigo, DateTime dataInicio, DateTime dataFim);
    }
}
