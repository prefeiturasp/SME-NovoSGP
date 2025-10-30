using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ParecerConclusivo.GerarParecerConclusivo
{
    public class GerarParecerConclusivoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IGerarParecerConclusivoUseCase _useCase;

        public GerarParecerConclusivoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new GerarParecerConclusivoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Dto_Valido_Deve_Enviar_Comando_E_Retornar_Resultado()
        {
            var dtoEntrada = new ConselhoClasseFechamentoAlunoDto
            {
                ConselhoClasseId = 10,
                FechamentoTurmaId = 20,
                AlunoCodigo = "30"
            };

            var dtoRetorno = new ParecerConclusivoDto
            {
                Id = 1,
                Nome = "Aprovado"
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GerarParecerConclusivoPorConselhoFechamentoAlunoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dtoRetorno);

            var resultado = await _useCase.Executar(dtoEntrada);

            resultado.Should().BeEquivalentTo(dtoRetorno);

            _mediatorMock.Verify(m => m.Send(
                    It.Is<GerarParecerConclusivoPorConselhoFechamentoAlunoCommand>(c =>
                        c.ConselhoClasseId == dtoEntrada.ConselhoClasseId &&
                        c.FechamentoTurmaId == dtoEntrada.FechamentoTurmaId &&
                        c.AlunoCodigo == dtoEntrada.AlunoCodigo),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
