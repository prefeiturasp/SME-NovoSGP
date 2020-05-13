using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtividadeAvaliativa : IRepositorioBase<AtividadeAvaliativa>
    {
        Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadesCalendarioProfessorPorMesDia(string dreCodigo, string ueCodigo, string turmaCodigo, string codigoRf, DateTime dataReferencia);
        Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadesCalendarioProfessorPorMes(string dreCodigo, string ueCodigo, int mes, int ano, string turmaCodigo);
        Task<PaginacaoResultadoDto<AtividadeAvaliativa>> Listar(DateTime? dataAvaliacao, string dreId, string ueID, string nomeAvaliacao, int? tipoAvaliacaoId, string turmaId, Paginacao paginacao);

        IEnumerable<AtividadeAvaliativa> ListarPorIds(IEnumerable<long> ids);

        Task<IEnumerable<AtividadeAvaliativa>> ListarPorTurmaDisciplinaPeriodo(string turmaId, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);

        Task<AtividadeAvaliativa> ObterAtividadeAvaliativa(DateTime date, string disciplinaId, string turmaId, string ueId);

        IEnumerable<AtividadeAvaliativa> ObterAtividadesAvaliativasSemNotaParaNenhumAluno(string turmaCodigo, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);

        Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadesPorDia(string dreId, string ueId, DateTime data, string rf, string turmaId);

        Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadesPorMes(string dreId, string ueId, int mes, int ano, string rf, string turmaId);

        Task<AtividadeAvaliativa> ObterPorIdAsync(long id);

        Task<bool> VerificarSeExisteAvaliacao(DateTime dataAvaliacao, string ueId, string turmaId, string professorRf, string disciplinaId);

        Task<bool> VerificarSeJaExisteAvaliacaoComMesmoNome(string nomeAvaliacao, string dreId, string ueID, string turmaId, string[] disciplinasId, string professorRf, DateTime periodoInicio, DateTime periodoFim, long? id);

        Task<bool> VerificarSeJaExisteAvaliacaoNaoRegencia(DateTime dataAvaliacao, string dreId, string ueId, string turmaId, string[] disciplinasId, string professorRf, long? id);

        Task<bool> VerificarSeJaExisteAvaliacaoRegencia(DateTime dataAvaliacao, string dreId, string ueID, string turmaId, string[] disciplinasId, string[] disciplinaContidaId, string codigoRf, long? id);

        Task<bool> VerificarSeJaExistePorTipoAvaliacao(long tipoAvaliacaoId);
    }
}