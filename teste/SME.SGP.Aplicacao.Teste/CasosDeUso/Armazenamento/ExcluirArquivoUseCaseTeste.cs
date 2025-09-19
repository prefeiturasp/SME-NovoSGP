using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Armazenamento
{
    public class ExcluirArquivoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExcluirArquivoUseCase _useCase;

        public ExcluirArquivoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExcluirArquivoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Arquivo_Nao_Encontrado_Deve_Lancar_Negocio_Exception()
        {
            var codigoArquivo = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterArquivoPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Arquivo)null);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(codigoArquivo));
            Assert.Equal(MensagemNegocioComuns.ARQUIVO_INF0RMADO_NAO_ENCONTRADO, ex.Message);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterArquivoPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Arquivo_Encontrado_Deve_Excluir_E_Publicar_Na_Fila()
        {
            var codigoArquivo = Guid.NewGuid();
            var arquivo = new Arquivo
            {
                Id = 123,
                Codigo = codigoArquivo,
                Nome = "documento.pdf"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterArquivoPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(arquivo);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ExcluirArquivoRepositorioPorIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(codigoArquivo);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterArquivoPorCodigoQuery>(q => q.Codigo == codigoArquivo), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirArquivoRepositorioPorIdCommand>(c => c.Id == arquivo.Id), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                cmd.Rota == RotasRabbitSgp.RemoverArquivoArmazenamento &&
                ((FiltroExcluirArquivoArmazenamentoDto)cmd.Filtros).ArquivoNome == codigoArquivo + Path.GetExtension(arquivo.Nome)
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
