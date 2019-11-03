using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class Evento : EntidadeBase, ICloneable
    {
        public Evento()
        {
            Excluido = false;
        }

        public DateTime? DataFim { get; set; }
        public DateTime DataInicio { get; set; }
        public string Descricao { get; set; }
        public string DreId { get; set; }
        public long? EventoPaiId { get; private set; }
        public bool Excluido { get; set; }
        public FeriadoCalendario FeriadoCalendario { get; set; }
        public long? FeriadoId { get; set; }
        public EventoLetivo Letivo { get; set; }
        public string Nome { get; set; }

        public TipoCalendario TipoCalendario { get; set; }

        public long TipoCalendarioId { get; set; }

        public EventoTipo TipoEvento { get; set; }

        public long TipoEventoId { get; set; }

        public string UeId { get; set; }

        public void AdicionarTipoEvento(EventoTipo tipoEvento)
        {
            TipoEvento = tipoEvento;
        }

        public object Clone()
        {
            return new Evento
            {
                AlteradoEm = AlteradoEm,
                AlteradoPor = AlteradoPor,
                AlteradoRF = AlteradoRF,
                CriadoEm = CriadoEm,
                CriadoPor = CriadoPor,
                CriadoRF = CriadoRF,
                DataFim = DataFim,
                DataInicio = DataInicio,
                Descricao = Descricao,
                DreId = DreId,
                Excluido = Excluido,
                FeriadoCalendario = FeriadoCalendario,
                FeriadoId = FeriadoId,
                Id = Id,
                Letivo = Letivo,
                Nome = Nome,
                TipoCalendario = TipoCalendario,
                TipoCalendarioId = TipoCalendarioId,
                TipoEvento = TipoEvento,
                TipoEventoId = TipoEventoId,
                UeId = UeId
            };
        }

        public bool DeveSerEmDiaLetivo()
        {
            TipoEventoObrigatorio();
            if (TipoEvento.Letivo == EventoLetivo.Sim && Letivo != EventoLetivo.Sim)
            {
                throw new NegocioException("O evento deve ser do tipo 'Letivo'.");
            }
            else
            {
                if (TipoEvento.Letivo == EventoLetivo.Nao && Letivo != EventoLetivo.Nao)
                {
                    throw new NegocioException("O evento não deve ser do tipo 'Letivo'.");
                }
            }
            return TipoEvento.Letivo == EventoLetivo.Sim;
        }

        public void EstaNoAnoLetivoDoCalendario()
        {
            if (TipoCalendario == null)
            {
                throw new NegocioException("O tipo de calendário não foi preenchido.");
            }
            if (TipoCalendario.AnoLetivo != DataInicio.Year)
            {
                throw new NegocioException("A data do evento deve ser no mesmo ano do calendário selecionado.");
            }
        }

        public void EstaNoPeriodoLetivo(IEnumerable<PeriodoEscolar> periodos)
        {
            if (!periodos.Any(c => c.PeriodoInicio >= DataInicio && c.PeriodoFim <= DataInicio))
            {
                throw new NegocioException("Não é permitido cadastrar um evento nesta data pois essa data não está dentro do 'Período Letivo'.");
            }
        }

        public void Excluir()
        {
            if (Excluido)
                throw new NegocioException("Este evento já está excluido.");

            Excluido = true;
        }

        public IEnumerable<Evento> ObterRecorrencia(PadraoRecorrencia padraoRecorrencia, PadraoRecorrenciaMensal padraoRecorrenciaMensal, DateTime dataInicio, DateTime dataFinal, IEnumerable<DayOfWeek> diasDaSemana, int repeteACada, DateTime? diaDeOcorrencia)
        {
            if (Id == 0)
                throw new NegocioException("Só é possível aplicar recorrência em eventos já registrados.");

            if (dataFinal == DateTime.MinValue)
                throw new NegocioException("A data final é obrigatória para calcular a recorrência de eventos.");
            if (padraoRecorrencia == PadraoRecorrencia.Semanal)
                return ObterRecorrenciaSemanal(dataInicio, dataFinal, diasDaSemana, repeteACada);
            else
            {
                if (padraoRecorrenciaMensal == PadraoRecorrenciaMensal.NoDia && !diaDeOcorrencia.HasValue)
                    throw new NegocioException("O dia de ocorrência é obrigatório para calcular esse tipo recorrência de eventos.");

                return ObterRecorrenciaMensal(padraoRecorrenciaMensal, dataInicio, dataFinal, diaDeOcorrencia, repeteACada, diasDaSemana.FirstOrDefault());
            }
        }

        public bool PermiteConcomitancia()
        {
            TipoEventoObrigatorio();
            return TipoEvento.Concomitancia;
        }

        public void ValidaPeriodoEvento()
        {
            TipoEventoObrigatorio();
            if (TipoEvento.TipoData == EventoTipoData.InicioFim && !DataFim.HasValue)
            {
                throw new NegocioException("Neste tipo de evento a data final do evento deve ser informada.");
            }
            else
            {
                if (TipoEvento.TipoData == EventoTipoData.Unico)
                {
                    DataFim = DataInicio;
                }
            }
        }

        private void AdicionaEventoPorRecorrenciaMensal(PadraoRecorrenciaMensal padraoRecorrenciaMensal, List<Evento> eventos, DateTime dataAtual, DateTime diaOcorrencia)
        {
            if (padraoRecorrenciaMensal == PadraoRecorrenciaMensal.NoDia && dataAtual.Day == diaOcorrencia.Day)
            {
                var evento = (Evento)Clone();
                evento.DataInicio = dataAtual;
                evento.DataFim = dataAtual;
                evento.EventoPaiId = Id;
                eventos.Add(evento);
            }
        }

        private void AdicionaEventosMensais(PadraoRecorrenciaMensal padraoRecorrenciaMensal, DateTime dataInicial, DateTime diaOcorrencia, List<Evento> eventos)
        {
            var primeiroDiaDoMes = new DateTime(dataInicial.Year, dataInicial.Month, 1);
            var ultimoDiaDoMes = primeiroDiaDoMes.AddMonths(1).AddDays(-1);

            for (DateTime data = dataInicial; data <= ultimoDiaDoMes; data = data.AddDays(1))
            {
                AdicionaEventoPorRecorrenciaMensal(padraoRecorrenciaMensal, eventos, data, diaOcorrencia);
            }
        }

        private void AdicionaEventosPorDiasDaSemana(IEnumerable<DayOfWeek> diasDaSemana, List<Evento> eventos, DateTime dataInicial)
        {
            var sabado = ObterSabado(dataInicial);
            for (DateTime data = dataInicial; data <= sabado; data = data.AddDays(1))
            {
                if (diasDaSemana.Contains(data.DayOfWeek))
                {
                    var evento = (Evento)Clone();
                    evento.DataInicio = data;
                    evento.DataFim = data;
                    evento.EventoPaiId = Id;
                    eventos.Add(evento);
                }
            }
        }

        private DateTime ObterDomingo(DateTime data)
        {
            if (data.DayOfWeek == DayOfWeek.Sunday)
                return data;
            int diferenca = (7 + (data.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return data.AddDays(-1 * diferenca).Date;
        }

        private DateTime ObterProximaDataRecorrenciaMensal(DateTime dataAtual, PadraoRecorrenciaMensal padraoRecorrenciaMensal, DayOfWeek diaDaSemana)
        {
            if (padraoRecorrenciaMensal != PadraoRecorrenciaMensal.NoDia)
            {
                dataAtual = ObterProximoDiaDaSemanaDoMes(dataAtual, padraoRecorrenciaMensal, diaDaSemana);
            }
            return dataAtual;
        }

        private DateTime ObterProximoDiaDaSemanaDoMes(DateTime dataAtual, PadraoRecorrenciaMensal padraoRecorrenciaMensal, DayOfWeek diaDaSemana)
        {
            int diferenca;
            if (padraoRecorrenciaMensal == PadraoRecorrenciaMensal.Ultima)
            {
                var ultimoDiaDoMes = new DateTime(dataAtual.Year, dataAtual.Month, 1).AddMonths(1).AddDays(-1);
                diferenca = diaDaSemana - ultimoDiaDoMes.DayOfWeek;
                return ultimoDiaDoMes.AddDays(diferenca);
            }

            dataAtual = new DateTime(dataAtual.Year, dataAtual.Month, 1);
            var multiplicador = 1;
            dataAtual = dataAtual.AddDays(7 * ((int)padraoRecorrenciaMensal));
            if (dataAtual.DayOfWeek > diaDaSemana)
            {
            }
            else
            {
                multiplicador = ((int)padraoRecorrenciaMensal - 1);
                dataAtual = dataAtual.AddDays(-(7 * multiplicador));
            }
            diferenca = diaDaSemana - dataAtual.DayOfWeek;
            return dataAtual.AddDays(diferenca * multiplicador);
        }

        private IEnumerable<Evento> ObterRecorrenciaMensal(PadraoRecorrenciaMensal padraoRecorrenciaMensal, DateTime dataInicio, DateTime dataFinal, DateTime? diaOcorrencia, int repeteACada, DayOfWeek diaDaSemana)
        {
            var eventos = new List<Evento>();
            var dataAtual = dataInicio;
            while (dataAtual <= dataFinal)
            {
                dataAtual = ObterProximaDataRecorrenciaMensal(dataAtual, padraoRecorrenciaMensal, diaDaSemana);
                var evento = (Evento)Clone();
                evento.DataInicio = dataAtual;
                evento.DataFim = dataAtual;
                evento.EventoPaiId = Id;
                eventos.Add(evento);
                dataAtual = dataAtual.AddMonths(repeteACada);
                //dataAtual = new DateTime(dataAtual.Year, dataAtual.AddMonths(repeteACada).Month, 1);
            }

            return eventos;
        }

        private IEnumerable<Evento> ObterRecorrenciaSemanal(DateTime dataInicio, DateTime dataFinal, IEnumerable<DayOfWeek> diasDaSemana, int repeteACada)
        {
            var dataInicial = ObterDomingo(dataInicio);
            if (dataInicial.DayOfWeek < dataInicio.DayOfWeek)
                dataInicial = dataInicio;

            var eventos = new List<Evento>();
            while (dataInicial <= dataFinal)
            {
                AdicionaEventosPorDiasDaSemana(diasDaSemana, eventos, dataInicial);
                dataInicial = dataInicial.AddDays(7 * repeteACada);
                dataInicial = ObterDomingo(dataInicial);
            }
            return eventos;
        }

        private DateTime ObterSabado(DateTime data)
        {
            if (data.DayOfWeek == DayOfWeek.Saturday)
                return data;
            int diferenca = (((int)DayOfWeek.Saturday - (int)data.DayOfWeek + 7) % 7);
            return data.AddDays(diferenca);
        }

        private void TipoEventoObrigatorio()
        {
            if (TipoEvento == null)
            {
                throw new NegocioException("O tipo de evento não foi encontrado.");
            }
        }
    }
}