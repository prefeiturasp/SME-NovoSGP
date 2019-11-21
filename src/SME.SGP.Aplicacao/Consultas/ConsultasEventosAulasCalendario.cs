using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasEventosAulasCalendario : IConsultasEventosAulasCalendario
    {
        private readonly IComandosDiasLetivos comandosDiasLetivos;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioEvento repositorioEvento;

        public ConsultasEventosAulasCalendario(
            IRepositorioEvento repositorioEvento,
            IComandosDiasLetivos comandosDiasLetivos,
            IRepositorioAula repositorioAula)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.comandosDiasLetivos = comandosDiasLetivos ?? throw new ArgumentNullException(nameof(comandosDiasLetivos));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public List<DateTime> ObterDias(IEnumerable<AulaDto> aulas)
        {
            List<DateTime> dias = new List<DateTime>();
            dias.AddRange(aulas.Select(x => x.DataAula));
            return dias.Distinct().ToList();
        }

        public async Task<IEnumerable<EventosAulasCalendarioDto>> ObterEventosAulasMensais(FiltroEventosAulasCalendarioDto filtro)
        {
            List<DateTime> diasLetivos = new List<DateTime>();
            List<DateTime> diasNaoLetivos = new List<DateTime>();
            List<DateTime> totalDias = new List<DateTime>();

            var diasPeriodoEscolares = comandosDiasLetivos.BuscarDiasLetivos(filtro.TipoCalendarioId);
            var diasAulas = await repositorioAula.ObterAulas(filtro.TipoCalendarioId, filtro.TurmaId, filtro.UeId);
            var eventos = repositorioEvento.ObterEventosPorTipoDeCalendarioDreUe(filtro.TipoCalendarioId, filtro.DreId, filtro.UeId);

            var diasEventosNaoLetivos = comandosDiasLetivos.ObterDias(eventos, diasNaoLetivos, Dominio.EventoLetivo.Nao);
            var diasEventosLetivos = comandosDiasLetivos.ObterDias(eventos, diasLetivos, Dominio.EventoLetivo.Sim);
            var aulas = ObterDias(diasAulas);

            diasEventosNaoLetivos.RemoveAll(x => !diasPeriodoEscolares.Contains(x));
            aulas.RemoveAll(x => !diasPeriodoEscolares.Contains(x));

            totalDias.AddRange(aulas);
            totalDias.AddRange(diasEventosLetivos);
            totalDias.AddRange(diasEventosNaoLetivos);

            return MapearParaDto(totalDias);
        }

        public async Task<IEnumerable<EventosAulasTipoCalendarioDto>> ObterTipoEventosAulas(FiltroEventosAulasCalendarioMesDto filtro)
        {
            var eventosAulas = new List<EventosAulasTipoCalendarioDto>();
            var aulas = await repositorioAula.ObterAulas(filtro.TipoCalendarioId, filtro.TurmaId, filtro.UeId, filtro.Mes);
            var eventos = await repositorioEvento.ObterEventosPorTipoDeCalendarioDreUeMes(filtro.TipoCalendarioId, filtro.DreId, filtro.UeId, filtro.Mes);

            var diasAulas = ObterDiasAulas(aulas);
            var diasEventos = ObterDiasEventos(eventos, filtro.Mes);
            diasAulas.AddRange(diasEventos);
            return MapearParaDtoTipo(eventosAulas, diasAulas);
        }

        private static IEnumerable<EventosAulasTipoCalendarioDto> MapearParaDtoTipo(List<EventosAulasTipoCalendarioDto> eventosAulas, List<KeyValuePair<int, string>> diasAulas)
        {
            foreach (var dia in diasAulas.Select(x => x.Key).Distinct())
            {
                var qtdEventosAulas = diasAulas.Where(x => x.Key == dia).Count();
                eventosAulas.Add(new EventosAulasTipoCalendarioDto
                {
                    Dia = dia,
                    QuantidadeDeEventosAulas = qtdEventosAulas,
                    TiposEvento = diasAulas.Where(x => x.Key == dia).Select(w => w.Value).ToList()
                });
            }

            return eventosAulas.OrderBy(x => x.Dia);
        }

        private List<EventosAulasCalendarioDto> MapearParaDto(List<DateTime> dias)
        {
            List<EventosAulasCalendarioDto> eventosAulas = new List<EventosAulasCalendarioDto>();
            for (int mes = 1; mes <= 12; mes++)
            {
                eventosAulas.Add(new EventosAulasCalendarioDto
                {
                    Mes = mes,
                    EventosAulas = dias
                                    .Where(w => w.Month == mes)
                                    .Distinct()
                                    .Count()
                });
            }
            return eventosAulas;
        }

        private List<KeyValuePair<int, string>> ObterDiasAulas(IEnumerable<AulaDto> aulas)
        {
            List<KeyValuePair<int, string>> dias = new List<KeyValuePair<int, string>>();
            foreach (var aula in aulas)
            {
                dias.Add(new KeyValuePair<int, string>(aula.DataAula.Day, "Aula"));
            }
            return dias;
        }

        private List<KeyValuePair<int, string>> ObterDiasEventos(IEnumerable<Dominio.Evento> eventos, int mes)
        {
            List<KeyValuePair<int, string>> dias = new List<KeyValuePair<int, string>>();
            foreach (var evento in eventos)
            {
                //se o evento ir para o próximo mês automaticamente ele já não irá nesse for
                for (DateTime dia = evento.DataInicio; dia <= evento.DataFim; dia = dia.AddDays(1))
                {
                    if (dia.Month != mes) break;
                    dias.Add(new KeyValuePair<int, string>(dia.Day, evento.Descricao));
                }
            }
            return dias;
        }
    }
}