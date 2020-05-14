using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoDiaLetivo : IServicoDiaLetivo
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ServicoDiaLetivo(IRepositorioEvento repositorioEvento,
                                IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public bool ValidarSeEhDiaLetivo(DateTime data, long tipoCalendarioId, string dreId, string ueId)
        {
            var periodoEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendarioData(tipoCalendarioId, data.Local());

            if (periodoEscolar == null)
                return false;

            if (!repositorioEvento.EhEventoLetivoPorTipoDeCalendarioDataDreUe(tipoCalendarioId, data.Local(), dreId, ueId))
                return false;

            return data.DayOfWeek != DayOfWeek.Saturday && data.DayOfWeek != DayOfWeek.Sunday;
        }

        public bool ValidarSeEhDiaLetivo(DateTime dataInicio, DateTime dataFim, long tipoCalendarioId, bool ehLetivo = false, long tipoEventoId = 0)
        {
            DateTime dataInicial = dataInicio.Date;
            DateTime dataFinal = dataFim.Date;
            var periodoEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendarioData(tipoCalendarioId, dataInicial, dataFinal);
            if (periodoEscolar == null)
                return false;
            if (ehLetivo && tipoEventoId != (int)TipoEvento.LiberacaoExcepcional)
                return ValidaSeEhFinalSemana(dataInicio, dataFim);
            return true;
        }

        public bool ValidaSeEhLiberacaoExcepcional(DateTime data, long tipoCalendarioId, string ueId)
        {
            try
            {
                List<Evento> eventos = repositorioEvento.EhEventoLetivoPorLiberacaoExcepcional(tipoCalendarioId, data, ueId);
                // EventoLetivo
                if (eventos.Exists(x => x.TipoEvento.Codigo == Convert.ToInt32(TipoEvento.LiberacaoExcepcional)))
                {
                    if (eventos.Exists(x => (x.TipoEvento.Codigo != 6) && (x.Letivo == EventoLetivo.Sim)))
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidaSeEhFinalSemana(DateTime inicio, DateTime fim)
        {
            for (DateTime data = inicio; data <= fim; data = data.AddDays(1))
                if (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                    return false;
            return true;
        }
    }
}