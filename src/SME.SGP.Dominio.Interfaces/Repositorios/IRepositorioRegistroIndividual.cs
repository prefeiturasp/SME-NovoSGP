using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioRegistroIndividual : IRepositorioBase<RegistroIndividual>
    {
        Task<RegistroIndividual> ObterPorAlunoData(long turmaId, long componenteCurricularId, long alunoCodigo, DateTime data);

        Task<PaginacaoResultadoDto<RegistroIndividual>> ObterPorAlunoPeriodoPaginado(long turmaId, long componenteCurricularId, long alunoCodigo, DateTime dataInicio, DateTime dataFim, Paginacao paginacao);

        Task<IEnumerable<UltimoRegistroIndividualAlunoTurmaDto>> ObterUltimosRegistrosPorAlunoTurma(long turmaId);
        Task<SugestaoTopicoRegistroIndividualDto> ObterSugestaoTopicoPorMes(int mes);
    }
}
