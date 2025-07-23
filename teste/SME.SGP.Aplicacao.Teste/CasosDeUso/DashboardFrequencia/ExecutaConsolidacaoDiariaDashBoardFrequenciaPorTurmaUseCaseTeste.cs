using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardFrequencia
{
    public class ExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioConsolidacaoFrequenciaTurma> _repositorioMock;
        private readonly ExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase _useCase;

        public ExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioMock = new Mock<IRepositorioConsolidacaoFrequenciaTurma>();
            _useCase = new ExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase(_mediatorMock.Object, _repositorioMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarTrue_ERealizarConsolidacao_WhenFrequenciasExistem()
        {
            // Arrange
            var filtro = new ConsolidacaoPorTurmaDashBoardFrequencia
            {
                AnoLetivo = 2025,
                Mes = 7,
                TurmaId = 12345,
                DataAula = new DateTime(2025, 07, 10)
            };

            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            // Mock da turma (com Ue e Dre completos)
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma
                {
                    Id = 12345,
                    AnoLetivo = 2025,
                    Semestre = 1,
                    ModalidadeCodigo = Modalidade.Medio,
                    CodigoTurma = "TURMA001",
                    Nome = "Turma Teste",
                    UeId = 123,
                    Ue = new Ue
                    {
                        Id = 123,
                        DreId = 1,
                        Dre = new Dre
                        {
                            CodigoDre = "DRE01",
                            Abreviacao = "DRE"
                        }
                    },
                });

            // Mock das frequências
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosParaConsolidacaoDashBoardFrequenciaPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DadosParaConsolidacaoDashBoardFrequenciaDto>
                {
                    new DadosParaConsolidacaoDashBoardFrequenciaDto
                    {
                        Presentes = 20,
                        Remotos = 5,
                        Ausentes = 5,
                        TotalAulas = 30,
                        TotalFrequencias = 30,
                        DataAula = new DateTime(2025, 07, 10),
                        CodigoAluno = "123"
                    }
                });

            // Mock dos alunos
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>
                {
                    new AlunoPorTurmaResposta
                    {
                        CodigoAluno = "123",
                        DataMatricula = new DateTime(2024, 02, 20),
                        CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                        NomeAluno = "Aluno Teste",
                        DataSituacao = new DateTime(2024, 06, 15)
                    }
                });

            // Mock da consolidação existente (retorna nulo para criar nova)
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConsolidacaoDashboardPorTurmaAulaModalidadeAnoLetivoDreUeTipoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ConsolidacaoDashBoardFrequencia)null);

            //// Mock do comando de salvar consolidação
            //ConsolidacaoDashBoardFrequencia consolidacaoSalva = null;
            //_mediatorMock.Setup(m => m.Send(It.IsAny<SalvarConsolidacaoDashBoardFrequenciaCommand>(), It.IsAny<CancellationToken>()))
            //    .Callback<SalvarConsolidacaoDashBoardFrequenciaCommand, CancellationToken>((cmd, token) =>
            //    {
            //        consolidacaoSalva = cmd.ConsolidacaoDashBoardFrequencia;
            //    })
            //    .ReturnsAsync(true);

            // Mock do comando de salvar consolidação
            ConsolidacaoDashBoardFrequencia consolidacaoSalva = null;
            _mediatorMock.Setup(m => m.Send(
                It.IsAny<SalvarConsolidacaoDashBoardFrequenciaCommand>(), 
                It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((cmd, token) =>
                {
                    var salvarCmd = cmd as SalvarConsolidacaoDashBoardFrequenciaCommand;
                    if (salvarCmd != null)
                        consolidacaoSalva = salvarCmd.ConsolidacaoDashBoardFrequencia;
                })
                .ReturnsAsync(true);


            // Act           
            var resultado = await _useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);
            Assert.NotNull(consolidacaoSalva);
            Assert.Equal(2025, consolidacaoSalva.AnoLetivo);
            Assert.Equal(12345, consolidacaoSalva.TurmaId);
            Assert.Equal("EM-Turma Teste", consolidacaoSalva.TurmaNome);
            Assert.Equal(12345, consolidacaoSalva.TurmaId);
            Assert.Equal("DRE01", consolidacaoSalva.DreCodigo);
            Assert.Equal(20, consolidacaoSalva.QuantidadePresencas);
            Assert.Equal(5, consolidacaoSalva.QuantidadeRemotos);
            Assert.Equal(5, consolidacaoSalva.QuantidadeAusentes);
            Assert.Equal(30, consolidacaoSalva.TotalAulas);
            Assert.Equal(30, consolidacaoSalva.TotalFrequencias);

            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoDashBoardFrequenciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);


        }

    }
}

