using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEventoFechamento : IRepositorioBase<EventoFechamento>
    {
        EventoFechamento ObterPorIdFechamento(long fechamentoId);
        Task<bool> SmeEmFechamento(DateTime dataReferencia, long tipoCalendarioId, int bimestre = 0);
        Task<IEnumerable<PeriodoEscolar>> ObterPeriodosFechamentoEmAberto(long ueId, DateTime dataReferencia);
        Task<bool> UeEmFechamento(DateTime dataReferencia, long tipoCalendarioId, bool ehModalidadeInfantil, int bimestre = 0);
    }
}