using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AcompanhamentoFrequencia
{
    public class ObterInformacoesDeFrequenciaAlunosPorBimestreUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IConsultasPeriodoEscolar> consultasPeriodoEscolarMock;
        private readonly ObterInformacoesDeFrequenciaAlunosPorBimestreUseCase useCase;

        public ObterInformacoesDeFrequenciaAlunosPorBimestreUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            consultasPeriodoEscolarMock = new Mock<IConsultasPeriodoEscolar>();
            useCase = new ObterInformacoesDeFrequenciaAlunosPorBimestreUseCase(mediatorMock.Object, consultasPeriodoEscolarMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarFrequenciaAlunosPorBimestreDto_QuandoDadosForemValidos()
        {
            // Arrange
            var dto = new ObterFrequenciaAlunosPorBimestreDto
            {
                TurmaId = 123,
                ComponenteCurricularId = 123,
                Bimestre = 1
            };

            var turma = new Turma
            {
                Id = 123,
                Nome = "Turma Teste",
                Ano = "5",
                AnoLetivo = 2025,
                CodigoTurma = "TURMA1",
                ModalidadeCodigo = Modalidade.Fundamental,
                TipoTurma = TipoTurma.Regular,
                Semestre = 1,
                QuantidadeDuracaoAula = 5,
                TipoTurno = (int)TipoTurnoEOL.Manha,
                Ue = new Ue
                {
                    Id = 987,
                    Nome = "EMEF Monte Sião",
                    TipoEscola = Dominio.TipoEscola.CIEJA,
                    Dre = new SME.SGP.Dominio.Dre { Abreviacao = "DRE-CE" }
                }
            };

            var tipoCalendarioId = 100;
            var periodos = new List<PeriodoEscolar>
            {
                new PeriodoEscolar
                {
                    Id = 10,
                    Bimestre = 1,
                    TipoCalendarioId = tipoCalendarioId,
                    PeriodoInicio = new DateTime(2025, 2, 1),
                    PeriodoFim = new DateTime(2025, 4, 30)
                }
            };

            var alunosMock = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "1",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = 123,
                    NomeAluno = "João da Silva",
                    DataMatricula = new DateTime(2025, 2, 15),
                    DataSituacao = new DateTime(2025, 12, 20),
                    DataNascimento = new DateTime(2010, 4, 5),
                }
            };

            var componente = new DisciplinaDto
            {
                CodigoComponenteCurricular = 123,
                RegistraFrequencia = true
            };

            var mediatorMock = new Mock<IMediator>();
            var consultasPeriodoEscolarMock = new Mock<IConsultasPeriodoEscolar>();
            var useCase = new ObterInformacoesDeFrequenciaAlunosPorBimestreUseCase(mediatorMock.Object, consultasPeriodoEscolarMock.Object);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<DisciplinaDto> { componente });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(tipoCalendarioId);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(periodos);

            consultasPeriodoEscolarMock.Setup(c => c.ObterPeriodoEscolarPorData(tipoCalendarioId, It.IsAny<DateTime>()))
                                       .ReturnsAsync(periodos.First());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(alunosMock);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(20);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(18);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<FrequenciaAluno>
                        {
                    new FrequenciaAluno
                    {
                        CodigoAluno = "1",
                        TotalAulas = 20,
                        TotalAusencias = 2,
                        TotalCompensacoes = 1,
                        TotalPresencas = 17,
                        TotalRemotos = 0,
                        PeriodoInicio = new DateTime(2025, 2, 1),
                        PeriodoFim = new DateTime(2025, 4, 30)
                    }
                        });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMarcadorFrequenciaAlunoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new MarcadorFrequenciaDto
                        {
                            Tipo = TipoMarcadorFrequencia.Novo,
                            Descricao = "Estudante Novo: Data da matrícula 13/07/2025"
                        });

            mediatorMock.Setup(m => m.Send(It.IsAny<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosTurmaProgramaPapEolQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<AlunosTurmaProgramaPapDto>());

            // Act
            var resultado = await useCase.Executar(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(20, resultado.AulasPrevistas);
            Assert.Equal(18, resultado.AulasDadas);
            Assert.Equal(1, resultado.Bimestre);
            Assert.Single(resultado.FrequenciaAlunos);
            var aluno = resultado.FrequenciaAlunos.First();
            Assert.Equal("João da Silva", aluno.Nome);
            Assert.Equal(2, aluno.Ausencias);
            Assert.Equal(17, aluno.Presencas);
        }
    }
}
