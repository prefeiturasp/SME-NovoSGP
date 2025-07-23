using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Dashboard.ObterUltimaAtualizacaoPorProcesso
{
    public class ObterUltimaAtualizacaoPorProcessoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterUltimaAtualizacaoPorProcessoUseCase useCase;

        public ObterUltimaAtualizacaoPorProcessoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterUltimaAtualizacaoPorProcessoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Resultado_Corretamente()
        {
            var nomeProcesso = "ImportacaoGSA";
            var resultadoEsperado = new UltimaAtualizaoWorkerPorProcessoResultado
            {
                NomeProcesso = nomeProcesso,
                DataUltimaAtualizacao = new DateTime(2025, 7, 21, 10, 30, 0)
            };

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterUltimaAtualizacaoPorProcessoQuery>(q => q.NomeProcesso == nomeProcesso), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar(nomeProcesso);

            resultado.Should().NotBeNull();
            resultado.NomeProcesso.Should().Be(nomeProcesso);
            resultado.DataUltimaAtualizacao.Should().Be(resultadoEsperado.DataUltimaAtualizacao);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Mediator_Retornar_Nulo()
        {
            var nomeProcesso = "ProcessoInexistente";

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUltimaAtualizacaoPorProcessoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((UltimaAtualizaoWorkerPorProcessoResultado)null);

            var resultado = await useCase.Executar(nomeProcesso);

            resultado.Should().BeNull();
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Se_Mediator_For_Nulo()
        {
            Action acao = () => new ObterUltimaAtualizacaoPorProcessoUseCase(null);

            acao.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'mediator')");
        }
    }
}
