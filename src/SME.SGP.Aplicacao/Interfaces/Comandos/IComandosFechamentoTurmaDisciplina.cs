using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosFechamentoTurmaDisciplina
    {
        Task<IEnumerable<AuditoriaPersistenciaDto>> Salvar(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentosTurma, bool componenteSemNota = false);
        Task Reprocessar(long fechamentoId, Usuario usuario = null);
        Task ProcessarPendentes(int anoLetivo);
    }
}
