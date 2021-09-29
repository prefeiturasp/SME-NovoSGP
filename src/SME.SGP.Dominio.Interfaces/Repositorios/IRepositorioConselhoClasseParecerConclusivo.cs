using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseParecerConclusivo : IRepositorioBase<ConselhoClasseParecerConclusivo>
    {
        Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterListaPorTurmaCodigoAsync(long turmaCodigo, DateTime dataConsulta);
        Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterListaPorTurmaIdAsync(long turmaId, DateTime dataConsulta);
        Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterListaVigente(DateTime dataConsulta);
        Task<IEnumerable<ParecerConclusivoSituacaoQuantidadeDto>> ObterParecerConclusivoSituacao(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre);
        Task<IEnumerable<WFAprovacaoParecerConclusivo>> VerificaSePossuiAprovacaoParecerConclusivo(long? conselhoClasseAlunoId);
    }
}
