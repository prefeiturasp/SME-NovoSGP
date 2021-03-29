using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeDiasLetivosPorCalendarioQueryHandler : IRequestHandler<ObterQuantidadeDiasLetivosPorCalendarioQuery, DiasLetivosDto>
    {
        private readonly IMediator mediator;

        public ObterQuantidadeDiasLetivosPorCalendarioQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<DiasLetivosDto> Handle(ObterQuantidadeDiasLetivosPorCalendarioQuery request, CancellationToken cancellationToken)
        {
            //se for letivo em um fds que esteja no calendário somar
            bool estaAbaixo = false;

            //buscar os dados
            var periodoEscolar = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(request.TipoCalendarioId));
            var diasLetivosCalendario = BuscarDiasLetivos(periodoEscolar);
            var eventos = await mediator.Send(new ObterEventosPorTipoDeCalendarioDreUeQuery(request.TipoCalendarioId, request.DreCodigo, request.UeCodigo, false, false));
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorIdQuery(request.TipoCalendarioId));

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
            var diasLetivosPermitidos = await ObterDiasLetivos(tipoCalendario.Modalidade, anoLetivo);

            estaAbaixo = diasLetivos < diasLetivosPermitidos;

            return new DiasLetivosDto
            {
                Dias = diasLetivos,
                EstaAbaixoPermitido = estaAbaixo
            };
        }

        private async Task<int> ObterDiasLetivos(ModalidadeTipoCalendario modalidade, int anoLetivo)
        {
            var parametros = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(TipoParametroSistema.EjaDiasLetivos, anoLetivo));

            return Convert.ToInt32(modalidade == ModalidadeTipoCalendario.EJA ?
                            await ObterParametroDiasLetivosEja(parametros) :
                            await ObterParametroDiasLetivosFundMedio(parametros));
        }

        private async Task<string> ObterParametroDiasLetivosEja(IEnumerable<ParametrosSistema> parametros)
        {
            return parametros.FirstOrDefault(a => a.Nome == "EjaDiasLetivos").Valor;
        }

        private async Task<string> ObterParametroDiasLetivosFundMedio(IEnumerable<ParametrosSistema> parametros)
        {
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
