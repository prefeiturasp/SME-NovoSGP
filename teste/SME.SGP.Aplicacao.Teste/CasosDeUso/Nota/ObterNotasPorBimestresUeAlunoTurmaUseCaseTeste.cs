using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Nota
{
    public class ObterNotasPorBimestresUeAlunoTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterNotasPorBimestresUeAlunoTurmaUseCase _useCase;

        public ObterNotasPorBimestresUeAlunoTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterNotasPorBimestresUeAlunoTurmaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Send_Com_Parametros_Corretos()
        {
            var dto = new NotaConceitoPorBimestresAlunoTurmaDto(
                ueCodigo: "UE123",
                turmaCodigo: "TURMA456",
                alunoCodigo: "ALUNO789",
                bimestres: new[] { 1, 2, 3 });

            var retornoEsperado = new List<NotaConceitoBimestreComponenteDto>
            {
                new NotaConceitoBimestreComponenteDto
                {
                    ConselhoClasseId = 1,
                    ConselhoClasseNotaId = 10,
                    ComponenteCurricularCodigo = 100,
                    ComponenteCurricularNome = "Matemática",
                    ConceitoId = null,
                    Nota = 8.5,
                    Bimestre = 1,
                    AlunoCodigo = "ALUNO789",
                    Conceito = null,
                    TurmaCodigo = "TURMA456"
                }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNotasPorBimestresUeAlunoTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _useCase.Executar(dto);

            _mediatorMock.Verify(m =>
                m.Send(It.Is<ObterNotasPorBimestresUeAlunoTurmaQuery>(q =>
                    q.UeCodigo == dto.UeCodigo &&
                    q.TurmaCodigo == dto.TurmaCodigo &&
                    q.AlunoCodigo == dto.AlunoCodigo &&
                    q.Bimestres == dto.Bimestres
                ), It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Matemática", resultado.First().ComponenteCurricularNome);
        }

        [Fact]
        public async Task Executar_Quando_Mediator_Retorna_Vazio_Deve_Retornar_Lista_Vazia()
        {
            var dto = new NotaConceitoPorBimestresAlunoTurmaDto(
                ueCodigo: "UE999",
                turmaCodigo: "TURMA999",
                alunoCodigo: "ALUNO999",
                bimestres: new int[] { });

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNotasPorBimestresUeAlunoTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<NotaConceitoBimestreComponenteDto>());

            var resultado = await _useCase.Executar(dto);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public void Constructor_Null_Mediator_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<System.ArgumentNullException>(() => new ObterNotasPorBimestresUeAlunoTurmaUseCase(null));
        }
    }
}
