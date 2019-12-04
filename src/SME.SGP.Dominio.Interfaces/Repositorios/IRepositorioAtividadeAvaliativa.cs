using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtividadeAvaliativa : IRepositorioBase<AtividadeAvaliativa>
    {
        Task<PaginacaoResultadoDto<AtividadeAvaliativa>> Listar(DateTime? dataAvaliacao, string dreId, string ueID, string nomeAvaliacao, int? tipoAvaliacaoId, string turmaId, Paginacao paginacao);

        Task<bool> VerificarSeExisteAvaliacao(DateTime dataAvaliacao, string ueId, string turmaId, string professorRf, string disciplinaId);

        Task<bool> VerificarSeJaExisteAvaliacao(DateTime dataAvaliacao, string dreId, string ueId, string turmaId, string professorRf);

        Task<bool> VerificarSeJaExisteAvaliacaoComMesmoNome(string nomeAvaliacao, string dreId, string ueID, string turmaId, string professorRf, DateTime periodoInicio, DateTime periodoFim);

        Task<bool> VerificarSeJaExisteAvaliacaoRegencia(DateTime dataAvaliacao, string dreId, string ueID, string turmaId, string disciplinaId, string codigoRf);
    }
}