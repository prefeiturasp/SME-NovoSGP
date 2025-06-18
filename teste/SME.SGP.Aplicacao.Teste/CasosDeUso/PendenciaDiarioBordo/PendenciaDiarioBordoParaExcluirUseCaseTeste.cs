using Bogus;
using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaDiarioBordo
{
    public class PendenciaDiarioBordoParaExcluirUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PendenciaDiarioBordoParaExcluirUseCase _useCase;
        private readonly Faker _faker;

        public PendenciaDiarioBordoParaExcluirUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new PendenciaDiarioBordoParaExcluirUseCase(_mediatorMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Theory(DisplayName = "Deve enviar o comando para excluir pendências e retornar o resultado")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Executar_DeveEnviarComandoParaExcluir_E_RetornarResultado(bool resultadoEsperado)
        {
            // Organização
            var listaPendencias = new List<PendenciaDiarioBordoParaExcluirDto>
            {
                new PendenciaDiarioBordoParaExcluirDto { AulaId = _faker.Random.Long(1, 100), ComponenteCurricularId = _faker.Random.Long(1, 1000) },
                new PendenciaDiarioBordoParaExcluirDto { AulaId = _faker.Random.Long(101, 200), ComponenteCurricularId = _faker.Random.Long(1001, 2000) }
            };

            var filtro = new FiltroListaAulaIdComponenteCurricularIdDto(listaPendencias);
            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            PendenciaDiarioBordoParaExcluirCommand comandoCapturado = null;

            // Configura o mock para capturar o comando enviado e retornar o resultado esperado
            _mediatorMock.Setup(m => m.Send(It.IsAny<PendenciaDiarioBordoParaExcluirCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, _) => comandoCapturado = cmd as PendenciaDiarioBordoParaExcluirCommand)
                         .ReturnsAsync(resultadoEsperado);

            // Ação
            var resultado = await _useCase.Executar(mensagemRabbit);

            // Verificação
            Assert.Equal(resultadoEsperado, resultado);

            _mediatorMock.Verify(m => m.Send(It.IsAny<PendenciaDiarioBordoParaExcluirCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(comandoCapturado);
            comandoCapturado
                .PendenciaDiariosBordoParaExcluirDto
                .Should()
                .BeEquivalentTo(listaPendencias, options => options.WithStrictOrdering());
        }
    }
}
