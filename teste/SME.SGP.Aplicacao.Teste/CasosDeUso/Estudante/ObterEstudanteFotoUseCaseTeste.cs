using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Estudante
{
    public class ObterEstudanteFotoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterEstudanteFotoUseCase useCase;

        public ObterEstudanteFotoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterEstudanteFotoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_AlunoCodigoVazio_Deve_LancarExcecao_Teste()
        {
            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(""));
        }

        [Fact]
        public async Task Executar_Quando_MiniaturaNula_Deve_RetornarNull_Teste()
        {
            var alunoCodigo = "12345";
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMiniaturaFotoEstudantePorAlunoCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((MiniaturaFotoDto)null);

            var resultado = await useCase.Executar(alunoCodigo);

            Assert.Null(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterMiniaturaFotoEstudantePorAlunoCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_MiniaturaValidaEArquivoVazio_Deve_RetornarNull_Teste()
        {
            var alunoCodigo = "12345";
            var miniatura = new MiniaturaFotoDto
            {
                Codigo = Guid.NewGuid(),
                Nome = "foto.jpg",
                Tipo = TipoArquivo.FotoAluno,
                CodigoFotoOriginal = Guid.NewGuid(),
                TipoConteudo = "image/jpeg"
            };
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMiniaturaFotoEstudantePorAlunoCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(miniatura);
            mediatorMock.Setup(m => m.Send(It.IsAny<DownloadArquivoCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(Array.Empty<byte>());

            var resultado = await useCase.Executar(alunoCodigo);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Executar_Quando_MiniaturaValidaEArquivoValido_Deve_RetornarArquivoDto_Teste()
        {
            var alunoCodigo = "12345";
            var miniatura = new MiniaturaFotoDto
            {
                Codigo = Guid.NewGuid(),
                Nome = "foto.jpg",
                Tipo = TipoArquivo.FotoAluno,
                CodigoFotoOriginal = Guid.NewGuid(),
                TipoConteudo = "image/jpeg",
                CriadoRf = "RF123"
            };
            var arquivoBytes = new byte[] { 1, 2, 3 };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMiniaturaFotoEstudantePorAlunoCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(miniatura);
            mediatorMock.Setup(m => m.Send(It.IsAny<DownloadArquivoCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(arquivoBytes);

            var resultado = await useCase.Executar(alunoCodigo);

            Assert.NotNull(resultado);
            Assert.Equal(miniatura.CodigoFotoOriginal, resultado.Codigo);
            Assert.Equal(miniatura.Nome, resultado.Nome);
            Assert.Equal(arquivoBytes, resultado.Download.Item1);
            Assert.Equal(miniatura.TipoConteudo, resultado.Download.Item2);
            Assert.Equal(miniatura.Nome, resultado.Download.Item3);
            Assert.Equal(miniatura.CriadoRf, resultado.CriadoRf);
        }
    }
}
