using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Usuarios
{
    public class ObterHierarquiaPerfisUsuarioUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterHierarquiaPerfisUsuarioUseCase useCase;

        public ObterHierarquiaPerfisUsuarioUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterHierarquiaPerfisUsuarioUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Professor()
        {
            var perfilGuid = Perfis.PERFIL_PROFESSOR;
            var usuario = new Usuario();
            usuario.DefinirPerfilAtual(perfilGuid);
            usuario.DefinirPerfis(new List<PrioridadePerfil>());

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(usuario);

            var perfil = new PrioridadePerfil { CodigoPerfil = perfilGuid, NomePerfil = "Professor", Ordem = 1 };
            mediatorMock.Setup(m => m.Send(It.Is<ObterPerfilPorGuidQuery>(q => q.Perfil == perfilGuid), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(perfil);

            var resultado = await useCase.Executar();

            Assert.Single(resultado);
            Assert.Equal(perfilGuid, resultado.First().Key);
            Assert.Equal("Professor", resultado.First().Value);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Nao_Professor()
        {
            var perfilGuid = Perfis.PERFIL_CP;
            var usuario = new Usuario();
            usuario.DefinirPerfilAtual(perfilGuid);
            usuario.DefinirPerfis(new List<PrioridadePerfil>());

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(usuario);

            var hierarquia = new List<PrioridadePerfil>
            {
                new PrioridadePerfil { CodigoPerfil = Guid.NewGuid(), NomePerfil = "Perfil1", Ordem = 2 },
                new PrioridadePerfil { CodigoPerfil = Guid.NewGuid(), NomePerfil = "Perfil2", Ordem = 1 }
            };

            mediatorMock.Setup(m => m.Send(It.Is<ObterHierarquiaPerfisPorPerfilQuery>(q => q.PerfilUsuario == perfilGuid), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(hierarquia);

            var resultado = await useCase.Executar();

            Assert.Equal(2, resultado.Count());
            Assert.Equal("Perfil2", resultado.First().Value);
            Assert.Equal("Perfil1", resultado.Last().Value);
        }

        [Fact]
        public async Task Executar_Quando_Perfil_Professor_Nao_Localizado()
        {
            var perfilGuid = Perfis.PERFIL_PROFESSOR;
            var usuario = new Usuario();
            usuario.DefinirPerfilAtual(perfilGuid);
            usuario.DefinirPerfis(new List<PrioridadePerfil>());

            mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.Is<ObterPerfilPorGuidQuery>(q => q.Perfil == perfilGuid), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((PrioridadePerfil)null);

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar());
        }
    }
}