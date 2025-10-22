using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.SincronizacaoInstitucional.Ciclo
{
    public class ExecutarSincronizacaoInstitucionalCicloTratarUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarSincronizacaoInstitucionalCicloTratarUseCase _useCase;

        public ExecutarSincronizacaoInstitucionalCicloTratarUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarSincronizacaoInstitucionalCicloTratarUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Invalida_Deve_Lancar_Negocio_Exception()
        {
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(null)
            };

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal("Não foi possível inserir o ciclo. A mensagem enviada é inválida.", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Tratar_Sincronizacao_E_Retornar_Verdadeiro()
        {
            var cicloEol = new CicloRetornoDto { Codigo = 123, Descricao = "Ciclo Teste" };
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(cicloEol)
            };
            var cicloSgp = new CicloEnsino { Id = 1, CodEol = 123 };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterCicloPorCodigoQuery>(q => q.CodigoEol == cicloEol.Codigo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cicloSgp);

            _mediatorMock.Setup(m => m.Send(It.IsAny<TrataSincronizacaoInstitucionalCicloEnsinoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterCicloPorCodigoQuery>(q => q.CodigoEol == cicloEol.Codigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<TrataSincronizacaoInstitucionalCicloEnsinoCommand>(c => c.CicloEol.Codigo == cicloEol.Codigo && c.CicloSgp.Id == cicloSgp.Id), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
