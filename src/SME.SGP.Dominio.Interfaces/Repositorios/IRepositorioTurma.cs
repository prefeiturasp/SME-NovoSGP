using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTurma
    {
        IEnumerable<Turma> MaterializarCodigosTurma(string[] idTurmas, out string[] codigosNaoEncontrados);
        Task<IEnumerable<Turma>> SincronizarAsync(IEnumerable<Turma> entidades, IEnumerable<Ue> ues);
        Task<bool> AtualizarTurmaParaHistorica(string turmaId);
        Task<bool> SalvarAsync(TurmaParaSyncInstitucionalDto turma, long ueId);
        Task ExcluirTurmaExtintaAsync(string turmaCodigo, long turmaId);
        Task<bool> AtualizarTurmaSincronizacaoInstitucionalAsync(TurmaParaSyncInstitucionalDto turma, bool deveMarcarHistorica = false);
    }
}