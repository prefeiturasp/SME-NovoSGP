using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEventoFechamento : IRepositorioBase<EventoFechamento>
    {
        EventoFechamento ObterPorIdFechamento(long fechamentoId);
        Task<IEnumerable<PeriodoEscolar>> ObterPeriodosFechamentoEmAberto(long ueId, DateTime dataReferencia);
        Task<bool> UeEmFechamento(DateTime dataReferencia, long tipoCalendarioId, bool ehModalidadeInfantil, int bimestre = 0);
        Task<PeriodoFechamentoBimestre> UeEmFechamentoVigente(DateTime dataReferencia, long id, bool modalidadeEhInfantil, int bimestre);
    }
}