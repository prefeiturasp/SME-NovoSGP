using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.SincronizacaoInstitucional.TipoEscola
{
    public class ExecutarSincronizacaoInstitucionalTipoEscolaTratarUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarSincronizacaoInstitucionalTipoEscolaTratarUseCase _useCase;

        public ExecutarSincronizacaoInstitucionalTipoEscolaTratarUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarSincronizacaoInstitucionalTipoEscolaTratarUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Tipo_Escola_Eol_Nulo_Deve_Lancar_Negocio_Exception()
        {
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(null),
                CodigoCorrelacao = Guid.NewGuid()
            };

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal($"Não foi possível fazer o tratamento do tipo de escola da mensagem {mensagemRabbit.CodigoCorrelacao}", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Tratar_Sincronizacao_E_Retornar_Verdadeiro()
        {
            var tipoEscolaEol = new TipoEscolaRetornoDto { Codigo = 4, DescricaoSigla = "EMEF" };
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(tipoEscolaEol)
            };
            var tipoEscolaSgp = new TipoEscolaEol { Id = 1, CodEol = 4 };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTipoEscolaPorCodigoQuery>(q => q.Codigo == tipoEscolaEol.Codigo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoEscolaSgp);

            _mediatorMock.Setup(m => m.Send(It.IsAny<TrataSincronizacaoInstitucionalTipoEscolaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterTipoEscolaPorCodigoQuery>(q => q.Codigo == tipoEscolaEol.Codigo), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<TrataSincronizacaoInstitucionalTipoEscolaCommand>(
                c => c.TipoEscolaEol.Codigo == tipoEscolaEol.Codigo && c.TipoEscolaSGP.Id == tipoEscolaSgp.Id),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
