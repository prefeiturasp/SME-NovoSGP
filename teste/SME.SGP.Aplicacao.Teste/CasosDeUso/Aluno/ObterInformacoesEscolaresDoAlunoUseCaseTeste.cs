using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste
{
    public class ObterInformacoesEscolaresDoAlunoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediator;
        private readonly ObterInformacoesEscolaresDoAlunoUseCase _useCase;

        public ObterInformacoesEscolaresDoAlunoUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _useCase = new ObterInformacoesEscolaresDoAlunoUseCase(_mediator.Object);
        }

        [Fact]
        public async Task Deve_Chamar_Query_Com_Parametros_Corretos()
        {
            var codigoAluno = "123456";
            var turmaId = "789";
            var resultadoEsperado = new InformacoesEscolaresAlunoDto();

            _mediator.Setup(m => m.Send(It.Is<ObterNecessidadesEspeciaisAlunoQuery>(
                q => q.CodigoAluno == codigoAluno && q.TurmaCodigo == turmaId),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            await _useCase.Executar(codigoAluno, turmaId);

            _mediator.Verify(m => m.Send(It.Is<ObterNecessidadesEspeciaisAlunoQuery>(
                q => q.CodigoAluno == codigoAluno && q.TurmaCodigo == turmaId),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Resultado_Da_Query()
        {
            var codigoAluno = "123456";
            var turmaId = "789";
            var resultadoEsperado = new InformacoesEscolaresAlunoDto
            {
                DescricaoRecurso = "Aluno Teste",
                TipoNecessidadeEspecial = 1,
            };

            _mediator.Setup(m => m.Send(It.IsAny<ObterNecessidadesEspeciaisAlunoQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await _useCase.Executar(codigoAluno, turmaId);

            Assert.NotNull(resultado);
            Assert.Equal(resultadoEsperado.DescricaoRecurso, resultado.DescricaoRecurso);
            Assert.Equal(resultadoEsperado.TipoNecessidadeEspecial, resultado.TipoNecessidadeEspecial);
        }
    }
}