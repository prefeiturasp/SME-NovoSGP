using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaDevolutiva
    {
        Task<IEnumerable<PendenciaDevolutiva>> ObterPendenciasDevolutivaPorTurmaComponente(long turmaId, long componenteId);
        Task<IEnumerable<PendenciaDevolutiva>> ObterPendenciasDevolutivaPorPendencia(long pendenciaId);
        Task Salvar(PendenciaDevolutiva pendenciaDevolutiva);
        Task ExcluirPorTurmaComponente(long turmaId, long componenteId);
        Task ExlusaoLogicaPorTurmaComponente(long turmaId, long componenteId);
        Task ExcluirPorId(long id);
        Task<IEnumerable<string>> ObterCodigoComponenteComDiarioBordoSemDevolutiva(long turmaId, string ueId);
        Task<Turma> ObterTurmaPorPendenciaId(long pendenciaId);
        Task<bool> ExistePendenciasDevolutivaPorTurmaComponente(long turmaId, long componenteId);
        Task<IEnumerable<long>> ObterIdsPendencias(int anoLetivo, string codigoUE);
    }
}
