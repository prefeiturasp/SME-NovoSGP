using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula
{
    public class ObterAulaPorIdUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IConsultasDisciplina> consultasDisciplinaMock;
        private readonly ObterAulaPorIdUseCase useCase;

        public ObterAulaPorIdUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            consultasDisciplinaMock = new Mock<IConsultasDisciplina>();
            useCase = new ObterAulaPorIdUseCase(mediatorMock.Object, consultasDisciplinaMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarAulaDto_QuandoAulaExiste()
        {
            // Arrange
            var aulaId = 123L;
            var turmaId = "TURMA-1";
            var dataAula = DateTime.Today;

            var componenteCurricularEol = new ComponenteCurricularEol
            {
                Codigo = 999,
                CodigoComponenteTerritorioSaber = 0,
                Compartilhada = false,
                Descricao = "Matemática",
                RegistraFrequencia = true,
                Regencia = false,
                TurmaCodigo = turmaId
            };

            var aula = new SME.SGP.Dominio.Aula
            {
                Id = aulaId,
                DataAula = dataAula,
                DisciplinaId = "999",
                ComponenteCurricularEol = componenteCurricularEol,
                Turma = new Turma { CodigoTurma = turmaId },
                ProfessorRf = "123456",
                CriadoRF = "123456",
                TurmaId = turmaId
            };

            var turma = new Turma
            {
                CodigoTurma = turmaId,
                Ue = new Ue
                {
                    CodigoUe = "UE1",
                    Dre = new SME.SGP.Dominio.Dre { CodigoDre = "DRE1" }
                },
                TipoTurma = TipoTurma.Regular
            };

            var perfilProfessor = new PrioridadePerfil
            {
                CodigoPerfil = Guid.NewGuid(),
                NomePerfil = "Professor",
                Ordem = 1,
                Tipo = TipoPerfil.UE
            };

            var usuario = new Usuario
            {
                CodigoRf = "123456",
                PerfilAtual = perfilProfessor.CodigoPerfil,
                Login = "123456",
                Nome = "Professor Exemplo",
                UltimoLogin = DateTime.Now
            };

            // Setup dos mocks do mediator
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aula);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterEventosCalendarioProfessorPorMesDiaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SME.SGP.Dominio.Evento>());

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ComponenteCurricularEol> { new ComponenteCurricularEol { Codigo = 999 } });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCJQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ComponenteCurricularEol>());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaEmManutencaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterBimestreAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediatorMock.Setup(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CompensacaoAusenciaAlunoAulaSimplificadoDto>());

            // *** CORREÇÃO: MOCK do método com todos os parâmetros obrigatórios e tipo correto ***
            consultasDisciplinaMock.Setup(x => x.ObterComponentesCurricularesPorProfessorETurma(
                    It.IsAny<string>(),       // codigoTurma
                    It.IsAny<bool>(),         // turmaPrograma
                    false,                   // realizarAgrupamentoComponente (explicitamente passado)
                    true                     // consideraTurmaInfantil (explicitamente passado)
                ))
                .ReturnsAsync(new List<DisciplinaDto>
                {
                    new DisciplinaDto
                    {
                        CodigoComponenteCurricular = 999,
                        CodigoComponenteCurricularTerritorioSaber = 0,
                        CdComponenteCurricularPai = null,
                        Compartilhada = false,
                        Nome = "Matemática"
                    }
                });

            // Act
            var resultado = await useCase.Executar(aulaId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(aula.Id, resultado.Id);
            Assert.Equal(aula.TurmaId, resultado.TurmaId);
            Assert.Equal(aula.DisciplinaId, resultado.DisciplinaId);
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoAulaNaoExiste()
        {
            // Arrange
            var aulaId = 999L;

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((SME.SGP.Dominio.Aula)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(aulaId));
            Assert.Equal($"Aula de id {aulaId} não encontrada", ex.Message);
        }
    }
}
