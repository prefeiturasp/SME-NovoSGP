using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.TipoCalendario
{
    public class BuscarTiposCalendarioPorDescricaoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IConsultasAbrangencia> consultasAbrangencia;
        private readonly BuscarTiposCalendarioPorDescricaoUseCase buscarTiposCalendarioPorDescricaoUseCaseTeste;

        public BuscarTiposCalendarioPorDescricaoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            consultasAbrangencia = new Mock<IConsultasAbrangencia>();
            buscarTiposCalendarioPorDescricaoUseCaseTeste = new BuscarTiposCalendarioPorDescricaoUseCase(mediator.Object, consultasAbrangencia.Object);
        }

        [Fact]
        public async Task Deve_Buscar_Lista_Tipo_Calendario_Pela_Busca()
        {
            //Arrange
            var mockRetorno = new List<TipoCalendarioBuscaDto> {
                    new TipoCalendarioBuscaDto{
                        AnoLetivo = 2020,
                        Descricao = "2020 - Calendário Fundamental",
                        Id = 1,
                        Nome = "Calendário Fundamental",
                        Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                        Periodo = Periodo.Anual,
                        Migrado = false,
                        Situacao = true
                    },
                    new TipoCalendarioBuscaDto{
                        AnoLetivo = 2020,
                        Descricao = "2020 - Calendário Médio",
                        Id = 1,
                        Nome = "Calendário Médio",
                        Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                        Periodo = Periodo.Anual,
                        Migrado = false,
                        Situacao = true
                    },
                    new TipoCalendarioBuscaDto{
                        AnoLetivo = 2020,
                        Descricao = "2020 - Calendário EJA",
                        Id = 1,
                        Nome = "Calendário EJA",
                        Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                        Periodo = Periodo.Anual,
                        Migrado = false,
                        Situacao = true
                    }
                };

            mediator.Setup(a => a.Send(It.IsAny<ObterTipoCalendarioPorBuscaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetorno);

            var usuario = new Usuario();
            usuario.DefinirPerfis(new List<PrioridadePerfil>());
            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(usuario));
            //Act
            var retorno = await buscarTiposCalendarioPorDescricaoUseCaseTeste.Executar("2020");

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ObterTipoCalendarioPorBuscaQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno.Any());
        }
    }
}
