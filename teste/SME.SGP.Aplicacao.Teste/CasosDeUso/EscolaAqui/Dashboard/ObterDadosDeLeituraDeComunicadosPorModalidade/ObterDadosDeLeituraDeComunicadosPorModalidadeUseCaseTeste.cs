using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorModalidade;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorModalidade
{
    public class ObterDadosDeLeituraDeComunicadosPorModalidadeUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterDadosDeLeituraDeComunicadosPorModalidadeUseCase useCase;

        public ObterDadosDeLeituraDeComunicadosPorModalidadeUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterDadosDeLeituraDeComunicadosPorModalidadeUseCase(mediatorMock.Object);
        }

        private static ObterDadosDeLeituraDeComunicadosDto CriarFiltroValido()
        {
            return new ObterDadosDeLeituraDeComunicadosDto
            {
                CodigoDre = "01",
                CodigoUe = "000123",
                NotificacaoId = 9876,
                ModoVisualizacao = 1
            };
        }
        private static List<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto> CriarListaRetorno()
        {
            return new List<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>
            {
                new DadosDeLeituraDoComunicadoPorModalidadeETurmaDto
                {
                    NomeAbreviadoDre = "DRE1",
                    ModalidadeCodigo = "1",
                    Modalidade = "Fundamental",
                    SiglaModalidade = "FUND",
                    CodigoTurma = "123",
                    Turma = "1A",
                    NaoReceberamComunicado = 5,
                    ReceberamENaoVisualizaram = 3,
                    VisualizaramComunicado = 12
                }
            };
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Lista_De_Dados_Quando_Mediator_Retornar_Dados()
        {
            var filtro = CriarFiltroValido();
            var dados = CriarListaRetorno();

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDadosDeLeituraDeComunicadosPorModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dados);

            var resultado = await useCase.Executar(filtro);

            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(1);
            resultado.First().Turma.Should().Be("1A");

            mediatorMock.Verify(m => m.Send(It.Is<ObterDadosDeLeituraDeComunicadosPorModalidadeQuery>(
                q => q.CodigoDre == filtro.CodigoDre &&
                     q.CodigoUe == filtro.CodigoUe &&
                     q.ComunicadoId == filtro.NotificacaoId &&
                     q.ModoVisualizacao == filtro.ModoVisualizacao), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Lista_Vazia_Quando_Mediator_Retornar_Lista_Vazia()
        {
            var filtro = CriarFiltroValido();

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDadosDeLeituraDeComunicadosPorModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>());

            var resultado = await useCase.Executar(filtro);

            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterDadosDeLeituraDeComunicadosPorModalidadeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Mediator_Lancar_Excecao()
        {
            var filtro = CriarFiltroValido();
            var excecaoEsperada = new InvalidOperationException("Erro ao consultar dados");

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDadosDeLeituraDeComunicadosPorModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(excecaoEsperada);

            Func<Task> acao = async () => await useCase.Executar(filtro);

            await acao.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Erro ao consultar dados");

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterDadosDeLeituraDeComunicadosPorModalidadeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
