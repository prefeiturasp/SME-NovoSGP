using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtividadeAvaliativa : IRepositorioBase<AtividadeAvaliativa>
    {
        Task<PaginacaoResultadoDto<AtividadeAvaliativa>> Listar(DateTime? dataAvaliacao, string dreId, string ueID, string nomeAvaliacao, int? tipoAvaliacaoId, string turmaId, Paginacao paginacao);
        IEnumerable<AtividadeAvaliativa> ListarPorTurmaDisciplinaPeriodo(string turmaId, string disciplinaId, DateTime inicioPeriodo, DateTime fimPeriodo);
        IEnumerable<AtividadeAvaliativa> ListarPorIds(IEnumerable<long> ids);
    }
}