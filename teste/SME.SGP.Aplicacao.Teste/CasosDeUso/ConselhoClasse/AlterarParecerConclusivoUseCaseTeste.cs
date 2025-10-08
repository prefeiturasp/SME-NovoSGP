using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.ConselhoClasse;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class AlterarParecerConclusivoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IAlterarParecerConclusivoUseCase _alterarParecerConclusivoUseCase;

        public AlterarParecerConclusivoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _alterarParecerConclusivoUseCase = new AlterarParecerConclusivoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_QuandoEnviarComandoValido_DeveMapearParaComandoERetornarDto()
        {
            var alterarParecerDto = new AlterarParecerConclusivoDto
            {
                ConselhoClasseId = 10,
                FechamentoTurmaId = 20,
                AlunoCodigo = "123456",
                ParecerConclusivoId = 5
            };

            var parecerRetornoDto = new ParecerConclusivoDto
            {
                Id = 5,
                Nome = "Aprovado"
            };

            _mediatorMock.Setup(m => m.Send(It.Is<AlterarManualParecerConclusivoCommand>(c =>
                    c.ConselhoClasseId == alterarParecerDto.ConselhoClasseId &&
                    c.FechamentoTurmaId == alterarParecerDto.FechamentoTurmaId &&
                    c.AlunoCodigo == alterarParecerDto.AlunoCodigo &&
                    c.ParecerConclusivoId == alterarParecerDto.ParecerConclusivoId),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(parecerRetornoDto);

            var resultado = await _alterarParecerConclusivoUseCase.Executar(alterarParecerDto);

            Assert.NotNull(resultado);
            Assert.Equal(parecerRetornoDto.Id, resultado.Id);
            Assert.Equal(parecerRetornoDto.Nome, resultado.Nome);

            _mediatorMock.Verify(m => m.Send(It.Is<AlterarManualParecerConclusivoCommand>(c =>
                    c.ConselhoClasseId == alterarParecerDto.ConselhoClasseId &&
                    c.FechamentoTurmaId == alterarParecerDto.FechamentoTurmaId &&
                    c.AlunoCodigo == alterarParecerDto.AlunoCodigo &&
                    c.ParecerConclusivoId == alterarParecerDto.ParecerConclusivoId),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
