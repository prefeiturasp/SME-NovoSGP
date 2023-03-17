using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEventoFechamentoConsulta : IRepositorioBase<EventoFechamento>
    {
        Task<EventoFechamento> ObterPorIdFechamento(long fechamentoId);
        Task<bool> UeEmFechamento(DateTime dataReferencia, long tipoCalendarioId, bool ehModalidadeInfantil, int bimestre = 0);
        Task<IEnumerable<PeriodoEscolar>> ObterPeriodosFechamentoEmAberto(long ueId, DateTime dataReferencia, int anoLetivo);
        Task<PeriodoFechamentoBimestre> UeEmFechamentoVigente(DateTime dataReferencia, long id, bool modalidadeEhInfantil, int bimestre);
        Task<PeriodoFechamentoBimestre> UeEmFechamentoBimestre(long tipoCalendarioId, bool ehModalidadeInfantil, int bimestre);
        Task<IEnumerable<PeriodoFechamentoBimestre>> ObterPeriodosFechamentoTurmaInfantil(long tipoCalendarioId, int bimestre);
        public Task<IEnumerable<PeriodoEscolar>> ObterPeriodoFechamentoEmAbertoTurma(string codigoTurma, ModalidadeTipoCalendario modalidade, DateTime dataReferencia);
    }
}