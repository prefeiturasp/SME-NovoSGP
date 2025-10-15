using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula.Pendencia
{
    public class PendenciaTurmaComponenteSemAulasUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveGerarPendenciaQuandoTodosOsCriteriosForemAtendidos()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var turmaDto = new TurmaDTO { TurmaId = 1, TurmaCodigo = "123456" };
            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(turmaDto));

            var periodoEscolar = new PeriodoEscolar
            {
                Id = 10,
                PeriodoInicio = DateTime.Today.AddDays(-30),
                PeriodoFim = DateTime.Today.AddDays(30),
                Bimestre = 2
            };

            var dre = new SME.SGP.Dominio.Dre
            {
                Id = 100,
                Nome = "DRE Z",
                Abreviacao = "DRE-Z", // necessário para Replace("-","")
                CodigoDre = "1234",
                DataAtualizacao = DateTime.Now
            };

            var ue = new Ue
            {
                Id = 200,
                Nome = "Escola X",
                TipoEscola = Dominio.TipoEscola.EMEBS, // ou o tipo correto que não seja Nenhum
                Dre = dre,
                DreId = dre.Id
            };

            var turma = new Turma
            {
                Id = 99,
                CodigoTurma = "123456",
                Nome = "Turma Teste",
                Ano = "2025",
                ModalidadeCodigo = Modalidade.Fundamental,
                Ue = ue,
                UeId = ue.Id
            };

            var componente = new ComponenteCurricularEol
            {
                Codigo = 555,
                Descricao = "Português",
                Regencia = false
            };

            var professor = new ProfessorTitularDisciplinaEol
            {
                ProfessorRf = "112233",
                ProfessorNome = "Prof. João",
                TurmaId = turma.Id,
                CodigosDisciplinas = "555"
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodoEscolarAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodoEscolar);

            mediatorMock.Setup(m => m.Send(It.IsAny<DiasAposInicioPeriodoLetivoComponenteSemAulaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasDreUePorCodigosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Turma> { turma });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEOLPorTurmasCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ComponenteCurricularEol> { componente });

            mediatorMock.Setup(m => m.Send(It.IsAny<PossuiAulaCadastradaPorPeriodoTurmaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterProfessorTitularPorTurmaEComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(professor);

            mediatorMock.Setup(m => m.Send(It.IsAny<ExistePendenciaProfessorPorTurmaEComponenteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            
            mediatorMock.Setup(m => m.Send(It.Is<SalvarPendenciaCommand>(cmd =>
                cmd.TipoPendencia == TipoPendencia.ComponenteSemAula &&
                cmd.Titulo.Contains("Português")
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(888);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioIdPorRfOuCriaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(42);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarPendenciaUsuarioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var useCase = new PendenciaTurmaComponenteSemAulasUseCase(mediatorMock.Object, unitOfWorkMock.Object);
                        
            // Act
            var resultado = await useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);
            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.Rollback(), Times.Never);
        }
    }
}
