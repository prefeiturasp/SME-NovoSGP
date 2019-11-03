using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SME.SGP.Dominio.Teste
{
    public class EventoTeste
    {
        [Fact]
        public void DeveCriarEventoEmDiaLetivo()
        {
            Evento evento = ObterEvento();
            Assert.True(evento != null);
            Assert.True(evento.DeveSerEmDiaLetivo());
        }

        #region RecorrenciaMensal

        [Fact]
        public void DeveCriarRecorrenciaMensalACada1MesParaSegundaQuartaFeiraDeCadaMes()
        {
            DateTime dataInicio = new DateTime(2019, 10, 04);
            DateTime? dataFinal = new DateTime(2019, 12, 30);
            IEnumerable<DayOfWeek> diasDaSemana = new List<DayOfWeek> {
                DayOfWeek.Wednesday
            };
            int repeteACada = 1;

            var evento = new Evento()
            {
                Nome = "Evento Original",
                Id = 1
            };

            var eventos = evento.ObterRecorrencia(PadraoRecorrencia.Mensal, PadraoRecorrenciaMensal.Segunda, dataInicio, dataFinal.Value, diasDaSemana, repeteACada, null);

            Assert.NotNull(eventos);
            Assert.True(eventos.Count() == 3);
            Assert.Contains(eventos, c => c.DataInicio.Day == 9 && c.DataInicio.Month == 10);
            Assert.Contains(eventos, c => c.DataInicio.Day == 13 && c.DataInicio.Month == 11);
            Assert.Contains(eventos, c => c.DataInicio.Day == 11 && c.DataInicio.Month == 12);
        }

        [Fact]
        public void DeveCriarRecorrenciaMensalACada1MesParaUltimaTercaFeiraDeCadaMes()
        {
            DateTime dataInicio = new DateTime(2019, 10, 04);
            DateTime? dataFinal = new DateTime(2019, 12, 30);
            IEnumerable<DayOfWeek> diasDaSemana = new List<DayOfWeek> {
                DayOfWeek.Tuesday
            };
            int repeteACada = 1;

            var evento = new Evento()
            {
                Nome = "Evento Original",
                Id = 1
            };

            var eventos = evento.ObterRecorrencia(PadraoRecorrencia.Mensal, PadraoRecorrenciaMensal.Ultima, dataInicio, dataFinal.Value, diasDaSemana, repeteACada, null);

            Assert.NotNull(eventos);
            Assert.True(eventos.Count() == 3);
            Assert.Contains(eventos, c => c.DataInicio.Day == 29 && c.DataInicio.Month == 10);
            Assert.Contains(eventos, c => c.DataInicio.Day == 26 && c.DataInicio.Month == 11);
            Assert.Contains(eventos, c => c.DataInicio.Day == 31 && c.DataInicio.Month == 12);
        }

        [Fact]
        public void DeveCriarRecorrenciaMensalACada2MeseParaTerceiroDomingoDeCadaMes()
        {
            DateTime dataInicio = new DateTime(2019, 01, 01);
            DateTime? dataFinal = new DateTime(2019, 12, 30);
            IEnumerable<DayOfWeek> diasDaSemana = new List<DayOfWeek> {
                DayOfWeek.Sunday
            };
            int repeteACada = 2;

            var evento = new Evento()
            {
                Nome = "Evento Original",
                Id = 1
            };

            var eventos = evento.ObterRecorrencia(PadraoRecorrencia.Mensal, PadraoRecorrenciaMensal.Terceira, dataInicio, dataFinal.Value, diasDaSemana, repeteACada, null);

            Assert.NotNull(eventos);
            Assert.True(eventos.Count() == 6);
            Assert.Contains(eventos, c => c.DataInicio.Day == 20 && c.DataInicio.Month == 1);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 17 && c.DataInicio.Month == 2);
            Assert.Contains(eventos, c => c.DataInicio.Day == 17 && c.DataInicio.Month == 3);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 21 && c.DataInicio.Month == 4);
            Assert.Contains(eventos, c => c.DataInicio.Day == 19 && c.DataInicio.Month == 5);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 16 && c.DataInicio.Month == 6);
            Assert.Contains(eventos, c => c.DataInicio.Day == 21 && c.DataInicio.Month == 7);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 18 && c.DataInicio.Month == 8);
            Assert.Contains(eventos, c => c.DataInicio.Day == 15 && c.DataInicio.Month == 9);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 20 && c.DataInicio.Month == 10);
            Assert.Contains(eventos, c => c.DataInicio.Day == 17 && c.DataInicio.Month == 11);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 15 && c.DataInicio.Month == 12);
        }

        #endregion RecorrenciaMensal

        [Fact]
        public void DeveCriarRecorrenciaSemanalACada1Semana()
        {
            DateTime dataInicio = new DateTime(2019, 11, 04);
            DateTime? dataFinal = new DateTime(2019, 11, 30);
            IEnumerable<DayOfWeek> diasDaSemana = new List<DayOfWeek> {
                DayOfWeek.Sunday,
                DayOfWeek.Wednesday,
                DayOfWeek.Friday
            };
            int repeteACada = 1;

            var evento = new Evento()
            {
                Nome = "Evento Original",
                Id = 1
            };
            var eventos = evento.ObterRecorrencia(PadraoRecorrencia.Semanal, 0, dataInicio, dataFinal.Value, diasDaSemana, repeteACada, null);

            Assert.NotNull(eventos);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 1);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 2);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 3);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 4);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 5);
            Assert.Contains(eventos, c => c.DataInicio.Day == 6);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 7);
            Assert.Contains(eventos, c => c.DataInicio.Day == 8);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 9);
            Assert.Contains(eventos, c => c.DataInicio.Day == 10);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 11);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 12);
            Assert.Contains(eventos, c => c.DataInicio.Day == 13);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 14);
            Assert.Contains(eventos, c => c.DataInicio.Day == 15);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 16);
            Assert.Contains(eventos, c => c.DataInicio.Day == 17);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 18);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 19);
            Assert.Contains(eventos, c => c.DataInicio.Day == 20);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 21);
            Assert.Contains(eventos, c => c.DataInicio.Day == 22);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 23);
            Assert.Contains(eventos, c => c.DataInicio.Day == 24);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 25);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 26);
            Assert.Contains(eventos, c => c.DataInicio.Day == 27);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 28);
            Assert.Contains(eventos, c => c.DataInicio.Day == 29);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 30);
        }

        [Fact]
        public void DeveCriarRecorrenciaSemanalACada2Semanas()
        {
            DateTime dataInicio = new DateTime(2019, 11, 04);
            DateTime? dataFinal = new DateTime(2019, 11, 30);
            IEnumerable<DayOfWeek> diasDaSemana = new List<DayOfWeek> {
                DayOfWeek.Sunday,
                DayOfWeek.Wednesday,
                DayOfWeek.Friday
            };
            int repeteACada = 2;

            var evento = new Evento()
            {
                Nome = "Evento Original",
                Id = 1
            };
            var eventos = evento.ObterRecorrencia(PadraoRecorrencia.Semanal, 0, dataInicio, dataFinal.Value, diasDaSemana, repeteACada, null);

            Assert.NotNull(eventos);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 1);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 2);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 3);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 4);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 5);
            Assert.Contains(eventos, c => c.DataInicio.Day == 6);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 7);
            Assert.Contains(eventos, c => c.DataInicio.Day == 8);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 9);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 10);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 11);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 12);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 13);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 14);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 15);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 16);
            Assert.Contains(eventos, c => c.DataInicio.Day == 17);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 18);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 19);
            Assert.Contains(eventos, c => c.DataInicio.Day == 20);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 21);
            Assert.Contains(eventos, c => c.DataInicio.Day == 22);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 23);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 24);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 25);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 26);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 27);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 28);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 29);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 30);
        }

        [Fact]
        public void DeveCriarRecorrenciaSemanalACada3Semanas()
        {
            DateTime dataInicio = new DateTime(2019, 11, 04);
            DateTime? dataFinal = new DateTime(2019, 11, 30);
            IEnumerable<DayOfWeek> diasDaSemana = new List<DayOfWeek> {
                DayOfWeek.Sunday,
                DayOfWeek.Wednesday,
                DayOfWeek.Friday
            };
            int repeteACada = 3;

            var evento = new Evento()
            {
                Nome = "Evento Original",
                Id = 1
            };
            var eventos = evento.ObterRecorrencia(PadraoRecorrencia.Semanal, 0, dataInicio, dataFinal.Value, diasDaSemana, repeteACada, null);

            Assert.NotNull(eventos);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 1);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 2);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 3);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 4);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 5);
            Assert.Contains(eventos, c => c.DataInicio.Day == 6);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 7);
            Assert.Contains(eventos, c => c.DataInicio.Day == 8);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 9);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 10);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 11);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 12);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 13);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 14);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 15);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 16);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 17);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 18);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 19);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 20);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 21);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 22);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 23);
            Assert.Contains(eventos, c => c.DataInicio.Day == 24);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 25);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 26);
            Assert.Contains(eventos, c => c.DataInicio.Day == 27);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 28);
            Assert.Contains(eventos, c => c.DataInicio.Day == 29);
            Assert.DoesNotContain(eventos, c => c.DataInicio.Day == 30);
        }

        [Fact]
        public void NaoDeveCriarEventoEmAnoLetivoDiferenteDoCalendarioEscolhido()
        {
            Evento evento = ObterEvento();
            evento.DataInicio = new DateTime(2020, 01, 01);
            Assert.Throws<NegocioException>(() => evento.EstaNoAnoLetivoDoCalendario());
        }

        private Evento ObterEvento()
        {
            return new Evento
            {
                AlteradoEm = null,
                AlteradoPor = null,
                AlteradoRF = null,
                CriadoEm = DateTime.Now,
                CriadoPor = "7777710",
                CriadoRF = "7777710",
                DataFim = null,
                DataInicio = new DateTime(2019, 01, 01),
                Descricao = "Novo evento",
                DreId = "123",
                FeriadoId = null,
                Letivo = EventoLetivo.Sim,
                Nome = "Evento letivo",
                TipoCalendarioId = 1,
                TipoCalendario = new TipoCalendario
                {
                    AnoLetivo = 2019
                },
                TipoEvento = new Entidades.EventoTipo
                {
                    Letivo = EventoLetivo.Sim,
                    LocalOcorrencia = EventoLocalOcorrencia.UE,
                    Concomitancia = false
                },
                UeId = "123"
            };
        }
    }
}