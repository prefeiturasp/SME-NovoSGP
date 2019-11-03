using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoEvento : IServicoEvento
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioEventoTipo repositorioEventoTipo;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoEvento(IRepositorioEvento repositorioEvento,
                             IRepositorioEventoTipo repositorioEventoTipo,
                             IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                             IServicoUsuario servicoUsuario)
        {
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new System.ArgumentNullException(nameof(repositorioEventoTipo));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
        }

        public static DateTime ObterProximoDiaDaSemana(DateTime data, DayOfWeek diaDaSemana)
        {
            int diasParaAdicionar = ((int)diaDaSemana - (int)data.DayOfWeek + 7) % 7;
            return data.AddDays(diasParaAdicionar);
        }

        public void GravarRecorrencia(Evento evento, DateTime datatInicial, DateTime? dataFinal, DateTime? diaDeOcorrencia, IEnumerable<DayOfWeek> diasDaSemana, PadraoRecorrencia padraoRecorrencia, PadraoRecorrenciaMensal padraoRecorrenciaMensal, int repeteACada)
        {
            if (!dataFinal.HasValue)
            {
                var periodoEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendario(evento.TipoCalendarioId);
                var periodoAtual = periodoEscolar.FirstOrDefault(c => DateTime.Now >= c.PeriodoInicio && DateTime.Now <= c.PeriodoFim);
                dataFinal = periodoAtual.PeriodoFim;
            }
            var eventos = evento.ObterRecorrencia(padraoRecorrencia, padraoRecorrenciaMensal, datatInicial, dataFinal.Value, diasDaSemana, repeteACada, diaDeOcorrencia);
            foreach (var novoEvento in eventos)
            {
                try
                {
                    Salvar(novoEvento);
                }
                catch (Exception ex)
                {
                }
            }
        }

        public DateTime ObterDomingo(DateTime data)
        {
            int diferenca = (7 + (data.DayOfWeek - DayOfWeek.Monday)) % 7;
            return data.AddDays(-1 * diferenca).Date;
        }

        public async Task Salvar(Evento evento)
        {
            var tipoEvento = repositorioEventoTipo.ObterPorId(evento.TipoEventoId);
            if (tipoEvento == null)
            {
                throw new NegocioException("O tipo do evento deve ser informado.");
            }
            evento.AdicionarTipoEvento(tipoEvento);

            evento.ValidaPeriodoEvento();

            var usuario = await servicoUsuario.ObterUsuarioLogado();

            usuario.PodeCriarEvento(evento);

            if (!evento.PermiteConcomitancia())
            {
                var existeOutroEventoNaMesmaData = repositorioEvento.ExisteEventoNaMesmaDataECalendario(evento.DataInicio, evento.TipoCalendarioId);
                if (existeOutroEventoNaMesmaData)
                {
                    throw new NegocioException("Não é permitido cadastrar um evento nesta data pois esse tipo de evento não permite concomitância.");
                }
            }

            if (evento.DeveSerEmDiaLetivo())
            {
                var periodos = repositorioPeriodoEscolar.ObterPorTipoCalendario(evento.TipoCalendarioId);

                evento.EstaNoPeriodoLetivo(periodos);
            }

            repositorioEvento.Salvar(evento);
        }

        private int ObterSemanaDoAno(DateTime data)
        {
            DayOfWeek dia = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(data);
            if (dia >= DayOfWeek.Monday && dia <= DayOfWeek.Wednesday)
            {
                data = data.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(data, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private void RecorrenciaMensal()
        {
            throw new NotImplementedException();
        }

        private void RecorrenciaSemanal(Evento eventoOriginal, DateTime dataInicio, DateTime? dataFinal, IEnumerable<DayOfWeek> diasDaSemana, int repeteACada)
        {
            var eventos = new List<Evento>();
            dataFinal = dataFinal.HasValue ? dataFinal : new DateTime(DateTime.Now.Year, 12, 31);

            //adicionar eventos na semana atual
            for (DateTime data = dataInicio; data.DayOfWeek < DayOfWeek.Monday; data.AddDays(1))
            {
                var evento = (Evento)eventoOriginal.Clone();
                evento.DataInicio = data;
                evento.DataFim = data;
                eventos.Add(evento);
            }
            //adicionar o numero de semanas a se repetir
            dataInicio.AddDays(7 * repeteACada);
            //pegar a domingo da semana atual
            var domingo = ObterDomingo(dataInicio);

            //iterar nos dias da semana
            //adicionar os dias marcados
            foreach (var diaDaSemana in diasDaSemana)
            {
                for (DateTime data = domingo; data.DayOfWeek < DayOfWeek.Monday; data.AddDays(1))
                {
                    var evento = (Evento)eventoOriginal.Clone();
                    evento.DataInicio = data;
                    evento.DataFim = data;
                    eventos.Add(evento);
                }
            }

            var dataParaAgendar = dataInicio;

            for (int i = 0; i < 10; i++)
            {
                foreach (var diaDaSemana in diasDaSemana)
                {
                    dataParaAgendar = ObterProximoDiaDaSemana(dataInicio, diaDaSemana);
                    var evento = (Evento)eventoOriginal.Clone();
                    evento.DataInicio = dataParaAgendar;
                    evento.DataFim = dataParaAgendar;
                    eventos.Add(evento);
                    if (diaDaSemana >= DayOfWeek.Saturday)
                    {
                        dataParaAgendar = dataParaAgendar.AddDays(7 * repeteACada);
                    }
                }
            }

            for (DateTime data = dataInicio.AddDays(1); data < dataFinal; data.AddDays(1))
            {
                if (diasDaSemana.Contains(data.DayOfWeek))
                {
                    var evento = (Evento)eventoOriginal.Clone();
                    evento.DataInicio = data;
                    evento.DataFim = data;
                    eventos.Add(evento);
                }
            }
        }
    }
}