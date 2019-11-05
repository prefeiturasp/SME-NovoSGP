using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ComandosDiasLetivos : IComandosDiasLetivos
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ComandosDiasLetivos(IRepositorioPeriodoEscolar repositorioPeriodoEscolar, IRepositorioEvento repositorioEvento)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public DiasLetivosDto CalcularDiasLetivos(long tipoCalendarioId)
        {
            var diasLetivosCalendario = BuscarDiasLetivos(tipoCalendarioId);
            var eventos = repositorioEvento.ObterEventosPorTipoDeCalendario(tipoCalendarioId);
            return null;
        }

        private int BuscarDiasLetivos(long tipoCalendarioId)
        {
            int diasLetivosPeriodoEscolar = 0;
            var periodoEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendarioId);
            periodoEscolar.ToList().ForEach(x => diasLetivosPeriodoEscolar += (x.PeriodoFim - x.PeriodoInicio).Days);
            return diasLetivosPeriodoEscolar;
        }
    }
}