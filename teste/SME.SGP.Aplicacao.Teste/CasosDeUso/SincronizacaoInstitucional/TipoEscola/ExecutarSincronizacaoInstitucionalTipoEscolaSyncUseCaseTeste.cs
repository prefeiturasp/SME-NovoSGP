using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.SincronizacaoInstitucional.TipoEscola
{
    public class ExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCase useCase;

        public ExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_Quando_Nao_Encontrar_Tipos__Escola()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTiposEscolaEolQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((IEnumerable<TipoEscolaRetornoDto>)null);

            var mensagem = new MensagemRabbit { CodigoCorrelacao = Guid.NewGuid() };

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(mensagem));
            Assert.Contains("Não foi possível localizar tipos de escolas", ex.Message);
        }

        [Fact]
        public async Task Executar_Deve_Publicar_Com_Sucesso_Quando_Existirem_TiposEscola()
        {
            var tiposEscola = new List<TipoEscolaRetornoDto>
            {
                new TipoEscolaRetornoDto { Codigo = 1, DescricaoSigla = "E.M.", DtAtualizacao = DateTime.Now }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTiposEscolaEolQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(tiposEscola);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var mensagem = new MensagemRabbit { CodigoCorrelacao = Guid.NewGuid() };

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Salvar_Log_Quando_Publicacao_Falhar()
        {
            var tiposEscola = new List<TipoEscolaRetornoDto>
            {
                new TipoEscolaRetornoDto { Codigo = 99, DescricaoSigla = "E.E.", DtAtualizacao = DateTime.Now }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTiposEscolaEolQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(tiposEscola);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var mensagem = new MensagemRabbit { CodigoCorrelacao = Guid.NewGuid() };

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
