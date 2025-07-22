using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Comunicados
{
    public class ObterAnosLetivosComunicadoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterAnosLetivosComunicadoUseCase useCase;

        public ObterAnosLetivosComunicadoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterAnosLetivosComunicadoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Negocio_Exception_Quando_Ano_Minimo_Maior_Ano_Atual()
        {
            var anoMinimo = DateTime.Now.Year + 1;

            Func<Task> act = async () => await useCase.Executar(anoMinimo);

            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage("O ano letivo não pode ser maior que o atual");
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Sem_Historico_Quando_Nao_Houver_Anos_Letivos_Historico()
        {
            var anoMinimo = 2000;

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAnosLetivosHistoricoDeComunicadosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<int>()); // Lista vazia = sem histórico

            var resultado = await useCase.Executar(anoMinimo);

            resultado.TemHistorico.Should().BeFalse();
            resultado.AnosLetivosHistorico.Should().BeNullOrEmpty();
            resultado.AnoLetivoAtual.Should().Be(DateTime.Now.Year);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Com_Historico_Quando_Houver_Anos_Letivos_Historico()
        {
            var anoMinimo = 2010;

            var anosHistorico = new List<int> { 2010, 2011, 2012 };

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterAnosLetivosHistoricoDeComunicadosQuery>(q =>
                        q.DataInicio == new DateTime(anoMinimo, 1, 1) &&
                        q.DataFim.Date == DateTime.Now.Date),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(anosHistorico);

            var resultado = await useCase.Executar(anoMinimo);

            resultado.TemHistorico.Should().BeTrue();
            resultado.AnosLetivosHistorico.Should().BeEquivalentTo(anosHistorico);
            resultado.AnoLetivoAtual.Should().Be(DateTime.Now.Year);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_Data_Inicio_Null_Quando_Ano_Minimo_Zero_Ou_Menor()
        {
            int anoMinimo = 0;

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterAnosLetivosHistoricoDeComunicadosQuery>(q =>
                    q.DataInicio == null &&
                    q.DataFim.Date == DateTime.Now.Date), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<int>());

            var resultado = await useCase.Executar(anoMinimo);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAnosLetivosHistoricoDeComunicadosQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            resultado.TemHistorico.Should().BeFalse();
        }
    }
}
