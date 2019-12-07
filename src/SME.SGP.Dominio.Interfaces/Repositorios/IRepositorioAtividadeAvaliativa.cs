using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtividadeAvaliativa : IRepositorioBase<AtividadeAvaliativa>
    {
        Task<PaginacaoResultadoDto<AtividadeAvaliativa>> Listar(DateTime? dataAvaliacao, string dreId, string ueID, string nomeAvaliacao, int? tipoAvaliacaoId, string turmaId, Paginacao paginacao);

        IEnumerable<AtividadeAvaliativa> ListarPorIds(IEnumerable<long> ids);

        Task<IEnumerable<AtividadeAvaliativa>> ListarPorTurmaDisciplinaPeriodo(string turmaId, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);

        Task<AtividadeAvaliativa> ObterAtividadeAvaliativa(DateTime date, string disciplinaId, string turmaId, string ueId);

        Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadesPorDia(string dreId, string ueId, DateTime data, string rf, string turmaId);

        Task<IEnumerable<AtividadeAvaliativa>> ObterAtividadesPorMes(string dreId, string ueId, int mes, int ano, string rf, string turmaId);

        Task<AtividadeAvaliativa> ObterPorIdAsync(long id);

        Task<bool> VerificarSeExisteAvaliacao(DateTime dataAvaliacao, string ueId, string turmaId, string professorRf, string disciplinaId);

        Task<bool> VerificarSeJaExisteAvaliacaoComMesmoNome(string nomeAvaliacao, string dreId, string ueID, string turmaId, string professorRf, DateTime periodoInicio, DateTime periodoFim);

        Task<bool> VerificarSeJaExisteAvaliacaoNaoRegencia(DateTime dataAvaliacao, string dreId, string ueId, string turmaId, string professorRf);

        Task<bool> VerificarSeJaExisteAvaliacaoRegencia(DateTime dataAvaliacao, string dreId, string ueID, string turmaId, string disciplinaId, string[] disciplinaContidaId, string codigoRf);

        Task<bool> VerificarSeJaExistePorTipoAvaliacao(long tipoAvaliacaoId);
    }
}