using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ListarDocumentosUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ListarDocumentosUseCase useCase;

        public ListarDocumentosUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ListarDocumentosUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Listar_Documentos()
        {
            //Arrange
            var mockRetorno = new List<DocumentoDto> {
                    new DocumentoDto()
                    {
                         Usuario = "USUARIO (2555567)",
                         DataUpload = DateTime.Now,
                         TipoDocumento = "Tipo de Documento",
                         Classificacao = "Classificacao de Documento",
                         UsuarioId = 1
                    }
                };

            mediator.Setup(a => a.Send(It.IsAny<ObterDocumentosPorUeETipoEClassificacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetorno);

            //Act
            var retorno = await useCase.Executar();

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ObterDocumentosPorUeETipoEClassificacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno.Any());
        }
    }
}
