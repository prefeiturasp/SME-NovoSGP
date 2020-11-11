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
    public class SalvarDocumentoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly SalvarDocumentoUseCase useCase;

        public SalvarDocumentoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new SalvarDocumentoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Salvar_Documento()
        {
            var codigo = Guid.NewGuid();
            //Arrange
            var param = new SalvarDocumentoDto()
            {
                ArquivoCodigo = codigo,
                ClassificacaoId = 2,
                TipoDocumentoId = 1,
                UsuarioId = 1,
                UeId = 1
            };

            Usuario usuario = new Usuario()
            {
                Id = 1,
                CodigoRf = "7938128"
            };

            Arquivo arquivo = new Arquivo()
            {
                Nome = "",
                Codigo = codigo,
                TipoConteudo = "",
                Tipo = TipoArquivo.Geral
            };

            usuario.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil() { NomePerfil = "PAP" } });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(usuario);

            mediator.Setup(a => a.Send(It.IsAny<ObterArquivoPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(arquivo);


            mediator.Setup(a => a.Send(It.IsAny<VerificaUsuarioPossuiArquivoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<SalvarDocumentoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            //Act
            var retorno = await useCase.Executar(param);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<SalvarDocumentoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno);
        }
    }
}
