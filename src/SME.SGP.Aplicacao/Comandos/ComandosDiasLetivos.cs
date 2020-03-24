using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ComandosDiasLetivos : IComandosDiasLetivos
    {
        private const string ChaveDiasLetivosEja = "EjaDiasLetivos";
        private const string ChaveDiasLetivosFundMedio = "FundamentalMedioDiasLetivos";
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ComandosDiasLetivos(
            IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
            IRepositorioEvento repositorioEvento,
            IRepositorioTipoCalendario repositorioTipoCalendario,
            IRepositorioParametrosSistema repositorioParametrosSistema)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
        }

        public List<DateTime> BuscarDiasLetivos(long tipoCalendarioId)
        {
            List<DateTime> dias = new List<DateTime>();
            var periodoEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendarioId);
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

        public DiasLetivosDto CalcularDiasLetivos(FiltroDiasLetivosDTO filtro)
        {
            //se for letivo em um fds que esteja no calendário somar
            bool estaAbaixo = false;

            //buscar os dados
            var periodoEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendario(filtro.TipoCalendarioId);
            var diasLetivosCalendario = BuscarDiasLetivos(periodoEscolar);
            var eventos = repositorioEvento.ObterEventosPorTipoDeCalendarioDreUe(filtro.TipoCalendarioId, filtro.DreId, filtro.UeId, false, false);
            var tipoCalendario = repositorioTipoCalendario.ObterPorId(filtro.TipoCalendarioId);

            if (tipoCalendario == null)
                throw new NegocioException("Tipo de calendario não encontrado");

            var anoLetivo = tipoCalendario.AnoLetivo;

            List<DateTime> diasEventosNaoLetivos = new List<DateTime>();
            List<DateTime> diasEventosLetivos = new List<DateTime>();

            //transforma em dias
            diasEventosNaoLetivos = ObterDias(eventos, diasEventosNaoLetivos, EventoLetivo.Nao);
            diasEventosLetivos = ObterDias(eventos, diasEventosLetivos, EventoLetivo.Sim);

            //adicionar os finais de semana letivos se houver
            //se não houver dia letivo em fds não precisa adicionar
            foreach (var dia in diasEventosLetivos.Where(x => !EhDiaUtil(x)))
            {
                if (periodoEscolar.Any(w => w.PeriodoInicio <= dia && dia <= w.PeriodoFim))
                    diasLetivosCalendario.Add(dia);
            }

            //retirar eventos não letivos que não estão no calendário
            diasEventosNaoLetivos = diasEventosNaoLetivos.Where(w => diasLetivosCalendario.Contains(w)).ToList();
            //retirar eventos não letivos que não contenha letivo
            diasEventosNaoLetivos = diasEventosNaoLetivos.Where(w => !diasEventosLetivos.Contains(w)).ToList();

            //subtrai os dias nao letivos
            var diasLetivos = diasLetivosCalendario.Distinct().Count() - diasEventosNaoLetivos.Distinct().Count();

            //verificar se eh eja ou nao
            var diasLetivosPermitidos = Convert.ToInt32(tipoCalendario.Modalidade == ModalidadeTipoCalendario.EJA ?
                repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.EjaDiasLetivos, anoLetivo) :
                repositorioParametrosSistema.ObterValorPorTipoEAno(TipoParametroSistema.FundamentalMedioDiasLetivos, anoLetivo));

            estaAbaixo = diasLetivos < diasLetivosPermitidos;

            return new DiasLetivosDto
            {
                Dias = diasLetivos,
                EstaAbaixoPermitido = estaAbaixo
            };
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

        private List<DateTime> BuscarDiasLetivos(IEnumerable<PeriodoEscolar> periodoEscolar)
        {
            List<DateTime> dias = new List<DateTime>();
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

        private bool EhDiaUtil(DateTime data)
        {
            return data.DayOfWeek != DayOfWeek.Saturday && data.DayOfWeek != DayOfWeek.Sunday;
        }
    }
}