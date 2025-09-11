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
    public class ListarTiposDeDocumentosUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ListarTiposDeDocumentosUseCase useCase;

        public ListarTiposDeDocumentosUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ListarTiposDeDocumentosUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Listar_Tipos_De_Documento()
        {
            //Arrange
            var mockRetorno = new List<TipoDocumentoDto> {
                    new TipoDocumentoDto{
                        TipoDocumento = "Tipo 1",
                        Id = 1,
                        Classificacoes = new List<ClassificacaoDocumentoDto>()
                        {
                            new ClassificacaoDocumentoDto()
                            {
                                Id = 1,
                                Classificacao = "PAP",
                                TipoDocumentoId = 1
                            }
                        }
                    }
                };

            Usuario usuario = new Usuario()
            {
                Id = 1,
                CodigoRf = "7938128"
            };

            usuario.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil() { NomePerfil = "PAP", Tipo = Dominio.TipoPerfil.UE } });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(usuario);

            mediator.Setup(a => a.Send(It.IsAny<ObterTipoDocumentoClassificacaoPorPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRetorno);

            //Act
            var retorno = await useCase.Executar();

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ObterTipoDocumentoClassificacaoPorPerfilUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno.Any());
        }

        [Theory]
        [InlineData(new string[] { "POA Alfabetização", "POEI Lingua Portuguesa" }, new string[] { "POA", "POEI" })]
        [InlineData(new string[] { "POA Portugues", "PAP Matematica" }, new string[] { "POA", "PAP" })]
        [InlineData(new string[] { "PAP" }, new string[] { "PAP" })]
        [InlineData(new string[] { "Alteracao de Calendario" }, new string[] { "Alteracao de Calendario" })]
        [InlineData(new string[] { "SPI - Anexo IV" }, new string[] { "SPI - Anexo IV" })]
        public void Quando_Perfil_Contem_Acronimo_Deve_Retornar_Apenas_Acronimo(string[] perfis, string[] resultadoEsperado)
        {
            string[] perfisNormalizados = ListarTiposDeDocumentosUseCase.NormalizarPerfisComAcronimo(perfis);

            Assert.Equal(resultadoEsperado, perfisNormalizados);
        }
    }
}
