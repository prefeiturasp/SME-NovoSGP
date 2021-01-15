using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosDiasLetivos : IComandosDiasLetivos
    {
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ComandosDiasLetivos(
            IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
            IRepositorioEvento repositorioEvento,
            IRepositorioTipoCalendario repositorioTipoCalendario,
            IRepositorioParametrosSistema repositorioParametrosSistema)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }

        public async Task<List<DateTime>> BuscarDiasLetivos(long tipoCalendarioId)
        {
            List<DateTime> dias = new List<DateTime>();
            var periodoEscolar = await repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendarioId);
            periodoEscolar
                .ToList()
                .ForEach(x => dias
                    .AddRange(
                        Enumerable
                        .Range(0, 1 + (x.PeriodoFim - x.PeriodoInicio).Days)
                        .Select(y => x.PeriodoInicio.AddDays(y))
                        .Where(w => EhDiaUtil(w))
                        .ToList())
            );

            return dias;
        }

        private async Task<string> ObterParametroDiasLetivosFundMedio(int anoLetivo)
        {
            var parametros = await repositorioParametrosSistema.ObterParametrosPorTipoEAno(TipoParametroSistema.EjaDiasLetivos, anoLetivo);
            return parametros.FirstOrDefault(a => a.Nome == "EjaDiasLetivos").Valor;
        }

        private async Task<string> ObterParametroDiasLetivosEja(int anoLetivo)
        {
            var parametros = await repositorioParametrosSistema.ObterParametrosPorTipoEAno(TipoParametroSistema.EjaDiasLetivos, anoLetivo);
            return parametros.FirstOrDefault(a => a.Nome == "FundamentalMedioDiasLetivos").Valor;
        }

        public List<DateTime> ObterDias(IEnumerable<Dominio.Evento> eventos, List<DateTime> dias, Dominio.EventoLetivo eventoTipo)
        {
            eventos
                            .Where(w => w.Letivo == eventoTipo)
                            .ToList()
                            .ForEach(x => dias
                                .AddRange(
                                    Enumerable
                                    .Range(0, 1 + (x.DataFim - x.DataInicio).Days)
                                    .Select(y => x.DataInicio.AddDays(y))
                                    .Where(w => (eventoTipo == Dominio.EventoLetivo.Nao
                                                && EhDiaUtil(w))
                                            || eventoTipo == Dominio.EventoLetivo.Sim)
                            ));
            return dias.Distinct().ToList();
        }

        public bool VerificarSeDataLetiva(IEnumerable<Evento> eventos, DateTime data)
        {
            bool possuiEventoLetivo = eventos.Any(x => x.Letivo == EventoLetivo.Sim);

            bool possuiEventoNaoLetivo = eventos.Any(x => x.Letivo == EventoLetivo.Nao);

            bool ehDiaUtil = EhDiaUtil(data);

            if (possuiEventoLetivo) return true;

            if (ehDiaUtil && !possuiEventoNaoLetivo) return true;

            return false;
        }

        private bool EhDiaUtil(DateTime data)
        {
            return data.DayOfWeek != DayOfWeek.Saturday && data.DayOfWeek != DayOfWeek.Sunday;
        }
    }
}