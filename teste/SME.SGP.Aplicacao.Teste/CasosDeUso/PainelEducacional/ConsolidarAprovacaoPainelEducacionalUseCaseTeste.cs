using MediatR;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Commands.PainelEducacional.LimparConsolidacaoAprovacao;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAprovacao;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoParaConsolidacao;
using SME.SGP.Aplicacao.Queries.UE.ObterTodasUes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAprovacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Testes.Aplicacao.PainelEducacional
{
    public class ConsolidarAprovacaoPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidarAprovacaoPainelEducacionalUseCase _useCase;

        public ConsolidarAprovacaoPainelEducacionalUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsolidarAprovacaoPainelEducacionalUseCase(_mediatorMock.Object);
        }

        [Fact(DisplayName = "Executar deve limpar consolidação e processar com sucesso")]
        public async Task Deve_Executar_Com_Sucesso()
        {
            // Arrange
            var dres = new List<Dre>
            {
                new Dre { Id = 1, CodigoDre = "D1", Nome = "DRE 1" }
            };

            var ues = new List<Ue>
            {
                new Ue { Id = 10, CodigoUe = "U1", DreId = 1, TipoEscola = TipoEscola.EMEF }
            };

            var turmas = new List<TurmaModalidadeSerieAnoDto>
            {
                new TurmaModalidadeSerieAnoDto
                {
                    TurmaId = 100,
                    CodigoDre = "D1",
                    CodigoUe = "U1",
                    Turma = "1A",
                    SerieAno = "1º",
                    Modalidade = Modalidade.Fundamental,
                    AnoLetivo = DateTime.Now.Year
                }
            };

            var aprovacoes = new List<DadosParaConsolidarAprovacao>
            {
                new DadosParaConsolidarAprovacao
                {
                    TurmaId = 100,
                    ParecerDescricao = "Promovido"
                },
                new DadosParaConsolidarAprovacao
                {
                    TurmaId = 100,
                    ParecerDescricao = "Retido"
                }
            };

            // Setup mocks
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasDresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasUesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ues);

            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparConsolidacaoAprovacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparConsolidacaoAprovacaoUeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasComModalidadePorModalidadeAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmas);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAprovacaoParaConsolidacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aprovacoes);

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarConsolidacaoAprovacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarConsolidacaoAprovacaoUeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var mensagem = new MensagemRabbit("Teste");

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.IsAny<LimparConsolidacaoAprovacaoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoAprovacaoCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoAprovacaoUeCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Executar deve registrar log em caso de exceção")]
        public async Task Deve_Registrar_Log_Quando_Houver_Excecao()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasDresQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Falha na consulta de DREs"));

            var mensagem = new MensagemRabbit("Teste");

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.False(resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<SalvarLogViaRabbitCommand>(c =>
                    c.Mensagem.Contains("Painel Educacional - Consolidação de Aprovação") &&
                    c.Nivel == LogNivel.Critico),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact(DisplayName = "MesclarConselhosTurmas deve combinar dados corretamente")]
        public void Deve_Mesclar_Conselhos_Com_Turmas()
        {
            // Arrange
            var turmas = new List<TurmaModalidadeSerieAnoDto>
            {
                new TurmaModalidadeSerieAnoDto
                {
                    TurmaId = 1,
                    CodigoDre = "D1",
                    CodigoUe = "U1",
                    Turma = "1A",
                    SerieAno = "1º",
                    Modalidade = Modalidade.Fundamental,
                    AnoLetivo = 2024
                }
            };

            var aprovacoes = new List<DadosParaConsolidarAprovacao>
            {
                new DadosParaConsolidarAprovacao
                {
                    TurmaId = 1,
                    ParecerConclusivoId = 10,
                    ParecerDescricao = "Promovido"
                }
            };

            // Act
            var resultado = InvokePrivateStaticMethod<IEnumerable<ConsolidacaoAprovacaoDto>>(
                typeof(ConsolidarAprovacaoPainelEducacionalUseCase),
                "MesclarConselhosTurmas",
                aprovacoes,
                turmas
            );

            // Assert
            var item = resultado.First();
            Assert.Equal("D1", item.CodigoDre);
            Assert.Equal("U1", item.CodigoUe);
            Assert.Equal("Promovido", item.ParecerDescricao);
        }

        private static T InvokePrivateStaticMethod<T>(Type type, string methodName, params object[] parameters)
        {
            var method = type.GetMethod(methodName, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            return (T)method.Invoke(null, parameters);
        }
    }
}
