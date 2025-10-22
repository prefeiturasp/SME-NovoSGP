using MediatR;
using Moq;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.SincronizacaoInstitucional.TipoEscola
{
    public class ExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCase _useCase;

        public ExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Tipos_Escola_Nulo_Deve_Lancar_Negocio_Exception()
        {
            var mensagemRabbit = new MensagemRabbit();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTiposEscolaEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<TipoEscolaRetornoDto>)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal("Não foi possível localizar tipos de escolas para a sincronização instituicional", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Tipos_Escola_Vazio_Deve_Lancar_Negocio_Exception()
        {
            var mensagemRabbit = new MensagemRabbit();
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTiposEscolaEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TipoEscolaRetornoDto>());

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(mensagemRabbit));

            Assert.Equal("Não foi possível localizar tipos de escolas para a sincronização instituicional", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Publicacao_Falha_Deve_Salvar_Log_E_Retornar_Verdadeiro()
        {
            var mensagemRabbit = new MensagemRabbit { CodigoCorrelacao = Guid.NewGuid() };
            var tiposEscola = new List<TipoEscolaRetornoDto>
            {
                new TipoEscolaRetornoDto { Codigo = 1, DescricaoSigla = "EMEF" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTiposEscolaEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tiposEscola);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c =>
                c.Rota == RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalTipoEscolaTratar &&
                c.Filtros == tiposEscola.First()), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(c =>
                c.Mensagem.Contains("Não foi possível inserir o Tipo de Escola") &&
                c.Contexto == LogContexto.SincronizacaoInstitucional &&
                c.Nivel == LogNivel.Negocio), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Publicacao_Sucesso_Deve_Retornar_Verdadeiro()
        {
            var mensagemRabbit = new MensagemRabbit { CodigoCorrelacao = Guid.NewGuid() };
            var tiposEscola = new List<TipoEscolaRetornoDto>
            {
                new TipoEscolaRetornoDto { Codigo = 1, DescricaoSigla = "EMEF" },
                new TipoEscolaRetornoDto { Codigo = 2, DescricaoSigla = "EMEI" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTiposEscolaEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tiposEscola);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(tiposEscola.Count));

            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
