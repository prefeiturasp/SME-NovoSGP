using System;
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