using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEventoFechamento : IRepositorioBase<EventoFechamento>
    {
        EventoFechamento ObterPorIdFechamento(long fechamentoId);
        Task<bool> UeEmFechamento(DateTime dataReferencia, string dreCodigo, string ueCodigo, int bimestre, long tipoCalendarioId);
        Task<IEnumerable<PeriodoEscolarDto>> ObterPeriodosEmAberto(long ueId, DateTime dataReferencia);
    }
}