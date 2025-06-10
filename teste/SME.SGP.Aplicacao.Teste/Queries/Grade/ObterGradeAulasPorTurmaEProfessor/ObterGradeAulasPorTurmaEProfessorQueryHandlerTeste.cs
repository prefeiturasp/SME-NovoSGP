using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Grade.ObterGradeAulasPorTurmaEProfessor
{
    public class ObterGradeAulasPorTurmaEProfessorQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_GradeComponenteTurmaAulasDto_Valido()
        {
            // Arrange
            var repositorioTurmaMock = new Mock<IRepositorioTurmaConsulta>();
            var repositorioAulaMock = new Mock<IRepositorioAulaConsulta>();
            var repositorioGradeMock = new Mock<IRepositorioGrade>();
            var mediatorMock = new Mock<IMediator>();

            var turma = new Turma
            {
                CodigoTurma = "123",
                Ano = "5",
                AnoLetivo = DateTime.Now.Year,
                ModalidadeCodigo = Modalidade.Medio,
                QuantidadeDuracaoAula = 50,
                Ue = new Ue { TipoEscola = TipoEscola.EMEFM },
            };

            repositorioTurmaMock.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SME.SGP.Dominio.Grade { Id = 1 });

            repositorioGradeMock.Setup(r => r.ObterHorasComponente(It.IsAny<long>(), It.IsAny<long[]>(), It.IsAny<int>()))
                .ReturnsAsync(4);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { CodigoRf = "rf123" });


            repositorioAulaMock.Setup(r => r.ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(
                    It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<bool>()))
                .ReturnsAsync(1);

            mediatorMock.Setup(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var handler = new ObterGradeAulasPorTurmaEProfessorQueryHandler(
                repositorioTurmaMock.Object,
                repositorioAulaMock.Object,
                repositorioGradeMock.Object,
                mediatorMock.Object
            );

            var query = new ObterGradeAulasPorTurmaEProfessorQuery(
                turmaCodigo: "123",
                componentesCurriculares: new long[] { 1234 },
                dataAula: DateTime.Today,
                codigoRf: null,
                ehRegencia: false,
                ehGestor: false
            );


            // Act
            var resultado = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(4, resultado.QuantidadeAulasGrade);
            Assert.Equal(3, resultado.QuantidadeAulasRestante);
            Assert.True(resultado.PodeEditar);
        }
    }
}
