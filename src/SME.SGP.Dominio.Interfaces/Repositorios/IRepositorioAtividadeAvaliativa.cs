using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtividadeAvaliativa : IRepositorioBase<AtividadeAvaliativa>
    {
        Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadesCalendarioProfessorPorMesDia(string dreCodigo, string ueCodigo, string turmaCodigo, DateTime dataReferencia);
        Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadesCalendarioProfessorPorMes(string dreCodigo, string ueCodigo, int mes, int ano, string turmaCodigo);
        Task<PaginacaoResultadoDto<AtividadeAvaliativa>> Listar(DateTime? dataAvaliacao, string dreId, string ueId, string nomeAvaliacao, int? tipoAvaliacaoId, string turmaId, Paginacao paginacao);

        IEnumerable<AtividadeAvaliativa> ListarPorIds(IEnumerable<long> ids);

        Task<IEnumerable<AtividadeAvaliativa>> ListarPorTurmaDisciplinaPeriodo(string turmaCodigo, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);
        Task<AtividadeAvaliativa> ObterPorAtividadeClassroomId(long atividadeClassroomId);
        Task<IEnumerable<AvaliacoesPorTurmaComponenteDto>> ObterAvaliacoesTurmaComponentePorUeNoPeriodo(long ueId, DateTime dataInicio, DateTime dataFim);
        Task<AtividadeAvaliativa> ObterAtividadeAvaliativa(DateTime dataAvaliacao, string disciplinaId, string turmaId, string ueId);
        Task<IEnumerable<TurmaEComponenteDto>> ObterTurmaEComponenteSemAvaliacaoNoPeriodo(long ueId, long tipoCalendarioId, DateTime dataInicio, DateTime dataFim);
        IEnumerable<AtividadeAvaliativa> ObterAtividadesAvaliativasSemNotaParaNenhumAluno(string turmaCodigo, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo, int tipoAtividadeAvaliativa);

        Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadesPorDia(string dreId, string ueId, DateTime dataAvaliacao, string professorRf, string turmaId);

        Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadesPorMes(string dreId, string ueId, int mes, int ano, string professorRf, string turmaId);

        Task<bool> VerificarSeExisteAvaliacao(DateTime dataAvaliacao, string ueId, string turmaId, string professorRf, string disciplinaId);

        Task<bool> VerificarSeJaExisteAvaliacaoComMesmoNome(string nomeAvaliacao, string dreId, string ueId, string turmaId, string[] disciplinasId, string professorRf, DateTime periodoInicio, DateTime periodoFim, long? id);

        Task<bool> VerificarSeJaExisteAvaliacaoNaoRegencia(DateTime dataAvaliacao, string dreId, string ueId, string turmaId, string[] disciplinasId, string professorRf, long? id);

        Task<bool> VerificarSeJaExisteAvaliacaoRegencia(DateTime dataAvaliacao, string dreId, string ueId, string turmaId, string[] disciplinasId, string[] disciplinasContidaId, string professorRf, long? id);

        Task<bool> VerificarSeJaExistePorTipoAvaliacao(long tipoAvaliacaoId);
        Task<IEnumerable<AtividadeAvaliativa>> ObterPorTurmaDisciplinasPeriodoAsync(string turmaCodigo, string[] disciplinasId, DateTime inicioPeriodo, DateTime fimPeriodo);
        Task<bool> AtividadeImportada(long atividadeId);
    }
}