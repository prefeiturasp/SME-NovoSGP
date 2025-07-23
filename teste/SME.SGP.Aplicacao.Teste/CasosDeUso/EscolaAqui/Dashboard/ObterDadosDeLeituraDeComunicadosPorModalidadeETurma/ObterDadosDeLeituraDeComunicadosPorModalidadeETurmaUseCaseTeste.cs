using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorModalidadeETurma;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorModalidadeETurma
{
    public class ObterDadosDeLeituraDeComunicadosPorModalidadeETurmaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterDadosDeLeituraDeComunicadosPorModalidadeETurmaUseCase useCase;

        public ObterDadosDeLeituraDeComunicadosPorModalidadeETurmaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterDadosDeLeituraDeComunicadosPorModalidadeETurmaUseCase(mediatorMock.Object);
        }

        private FiltroDadosDeLeituraDeComunicadosPorModalidadeDto CriarFiltro()
        {
            return new FiltroDadosDeLeituraDeComunicadosPorModalidadeDto
            {
                CodigoDre = "dre1",
                CodigoUe = "ue1",
                NotificacaoId = 123,
                Modalidades = new short[] { 1 },
                CodigosTurmas = new long[] { 999 },
                ModoVisualizacao = 1
            };
        }

        private DadosDeLeituraDoComunicadoPorModalidadeETurmaDto CriarDto()
        {
            return new DadosDeLeituraDoComunicadoPorModalidadeETurmaDto
            {
                CodigoTurma = "999",
                ModalidadeCodigo = "1",
                Modalidade = "Fundamental",
                Turma = "Turma 1",
                NomeAbreviadoDre = "DRE 1",
                NaoReceberamComunicado = 1,
                ReceberamENaoVisualizaram = 2,
                VisualizaramComunicado = 3
            };
        }
        private Turma CriarTurma() => new Turma { CodigoTurma = "999", ModalidadeCodigo = Modalidade.Fundamental };

        [Fact]
        public async Task Executar_Deve_Retornar_Dados_Com_Sigla_Modalidade()
        {
            var filtro = CriarFiltro();
            var dto = CriarDto();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto> { dto });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(CriarTurma());

            var resultado = await useCase.Executar(filtro);

            resultado.Should().HaveCount(1);
            resultado.First().SiglaModalidade.Should().Be("EF");
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Turma_Nao_For_Encontrada()
        {
            var filtro = CriarFiltro();
            var dto = CriarDto();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto> { dto });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Turma)null);

            Func<Task> act = async () => await useCase.Executar(filtro);

            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage("Não foi possível localizar a turma");
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Mediator_Falhar()
        {
            var filtro = CriarFiltro();

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Erro ao buscar dados"));

            Func<Task> act = async () => await useCase.Executar(filtro);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Erro ao buscar dados");
        }

        [Fact]
        public void Construtor_Deve_Lancar_ArgumentNullException_Quando_Mediator_For_Null()
        {
            Action act = () => new ObterDadosDeLeituraDeComunicadosPorModalidadeETurmaUseCase(null);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
