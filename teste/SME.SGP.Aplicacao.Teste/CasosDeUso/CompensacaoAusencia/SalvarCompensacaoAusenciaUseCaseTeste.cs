using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.CompensacaoAusencia
{
    public class SalvarCompensacaoAusenciaUseCaseTeste
    {
        [Fact]
        public async Task Executar_DevePersistirCompensacaoAusencia_ComFluxoCompleto()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var configMock = new Mock<IOptions<ConfiguracaoArmazenamentoOptions>>();
            configMock.Setup(c => c.Value).Returns(new ConfiguracaoArmazenamentoOptions
            {
                Port = 9000,
                BucketTemp = "temp",
                BucketArquivos = "arquivos"
            });

            var turma = new Turma
            {
                Id = 10,
                AnoLetivo = 2025,
                Semestre = 1,
                ModalidadeCodigo = Modalidade.EJA,
                CodigoTurma = "TURMA1"
            };

            var periodoEscolar = new PeriodoEscolar
            {
                Id = 99,
                Bimestre = 2,
                PeriodoInicio = new DateTime(2025, 2, 1),
                PeriodoFim = new DateTime(2025, 4, 30)
            };

            List<PrioridadePerfil> Perfis = new List<PrioridadePerfil>();

            Usuario usuario = new Usuario { CodigoRf = "RF123", Nome = "Gestor" };

            usuario.DefinirPerfis(Perfis);

            var compensacaoDto = new CompensacaoAusenciaDto
            {
                Id = 0,
                TurmaId = "TURMA1",
                Bimestre = 2,
                DisciplinaId = "20",
                Atividade = "Atividade Teste",
                Descricao = "temp/arquivo.pdf",
                DisciplinasRegenciaIds = new List<string> { "21" }, // nunca null
                Alunos = new List<CompensacaoAusenciaAlunoDto>
                {
                    new CompensacaoAusenciaAlunoDto
                    {
                        Id = "A1",
                        QtdFaltasCompensadas = 2,
                        CompensacaoAusenciaAlunoAula = new List<CompensacaoAusenciaAlunoAulaDto>
                        {
                            new CompensacaoAusenciaAlunoAulaDto { 
                                Id = 1, CompensacaoAusenciaAlunoId = 100,
                                RegistroFrequenciaAlunoId = 1 
                            }
                        }
                    }
                }
            };

            // Mocks dos métodos privados e queries/comandos
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar> { periodoEscolar });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = true });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaPorAnoTurmaENomeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SME.SGP.Dominio.CompensacaoAusencia)null);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteRegistraFrequenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto>
                {
                    new DisciplinaDto
                    {
                        Id = 20,
                        Nome = "Matemática",
                        CodigoComponenteCurricular = 20,
                        Regencia = false,
                        RegistraFrequencia = true
                    }
                    // Adicione outros DisciplinaDto conforme necessário para o teste
                });

            var ListaFrequenciaAluno = new List<FrequenciaAluno>();

            var itemFrequenciaAluno = new FrequenciaAluno
            {
                CodigoAluno = "A1",
                DisciplinaId = "20",
                PeriodoFim = periodoEscolar.PeriodoFim,
                TurmaId = turma.CodigoTurma,
                TotalCompensacoes = 2
            };

            ListaFrequenciaAluno.Add(itemFrequenciaAluno);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);

            mediatorMock.Setup(m => m.Send(It.IsAny<InserirVariosCompensacaoAusenciaRegenciaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ListaFrequenciaAluno);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(100L);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAusenciaParaCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<RegistroFaltasNaoCompensadaDto>
                {
                    new RegistroFaltasNaoCompensadaDto { Descricao = "Falta 1", Sugestao = true, CodigoAluno = "A1", RegistroFrequenciaAlunoId = 1, DataAula = DateTime.Today, NumeroAula = 1 },
                    new RegistroFaltasNaoCompensadaDto { Descricao = "Falta 2", Sugestao = true, CodigoAluno = "A1", RegistroFrequenciaAlunoId = 2, DataAula = DateTime.Today, NumeroAula = 2 }

                });

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoAulaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);

            mediatorMock.Setup(m => m.Send(It.IsAny<MoverArquivosTemporariosCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("TextoEditorNovo");

            mediatorMock.Setup(m => m.Send(It.IsAny<RemoverArquivosExcluidosCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<IncluirFilaCalcularFrequenciaPorTurmaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<VerificaSeExisteParametroSistemaPorTipoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<ProfessorPodePersistirTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);


            unitOfWorkMock.Setup(u => u.IniciarTransacao());
            unitOfWorkMock.Setup(u => u.PersistirTransacao());
            unitOfWorkMock.Setup(u => u.Rollback());

            var useCase = new SalvarCompensacaoAusenciaUseCase(mediatorMock.Object, unitOfWorkMock.Object, configMock.Object);

            // Act
            await useCase.Executar(0, compensacaoDto);

            // Assert
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterCompensacaoAusenciaPorAnoTurmaENomeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterComponenteRegistraFrequenciaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<InserirVariosCompensacaoAusenciaRegenciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterFrequenciaPorListaDeAlunosDisciplinaDataQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAusenciaParaCompensacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarCompensacaoAusenciaAlunoAulaCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            mediatorMock.Verify(m => m.Send(It.IsAny<MoverArquivosTemporariosCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaCalcularFrequenciaPorTurmaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<VerificaSeExisteParametroSistemaPorTipoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
        }
    }
}
