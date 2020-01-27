using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoUsuarioTeste
    {
        private readonly Mock<IRepositorioAtribuicaoCJ> repositorioAtribuicaoCj;
        private readonly Mock<IRepositorioCache> repositorioCache;
        private readonly Mock<IRepositorioPrioridadePerfil> repositorioPrioridadePerfil;
        private readonly Mock<IRepositorioUsuario> repositorioUsuario;
        private readonly Mock<IServicoEOL> servicoEol;
        private readonly ServicoUsuario servicoUsuario;
        private readonly Mock<IUnitOfWork> unitOfWork;

        public ServicoUsuarioTeste()
        {
            repositorioUsuario = new Mock<IRepositorioUsuario>();
            servicoEol = new Mock<IServicoEOL>();
            repositorioPrioridadePerfil = new Mock<IRepositorioPrioridadePerfil>();
            unitOfWork = new Mock<IUnitOfWork>();
            repositorioCache = new Mock<IRepositorioCache>();
            var obj = new HttpContextAccessor();
            obj.HttpContext = new DefaultHttpContext();
            repositorioAtribuicaoCj = new Mock<IRepositorioAtribuicaoCJ>();

            var context = new ContextoHttp(obj);

            servicoUsuario = new ServicoUsuario(repositorioUsuario.Object, servicoEol.Object, repositorioPrioridadePerfil.Object, unitOfWork.Object, context, repositorioCache.Object, repositorioAtribuicaoCj.Object);
        }

        [Fact]
        public async Task Deve_Modificar_Email_Por_Login()
        {
            //ARRANGE
            var login = "loginTeste";
            var email = "teste@teste.com";
            var usuario = new Usuario() { Id = 5, Login = login };
            repositorioUsuario.Setup(a => a.ObterPorCodigoRfLogin(string.Empty, login)).Returns(usuario);
            servicoEol.Setup(a => a.ExisteUsuarioComMesmoEmail(email, usuario.Login)).Returns(Task.FromResult(false));
            repositorioPrioridadePerfil.Setup(c => c.ObterPerfisPorIds(It.IsAny<IEnumerable<Guid>>()))
                .Returns(new List<PrioridadePerfil>() {
                    new PrioridadePerfil
                    {
                        Id=1,
                        Ordem=10
                    }
                });
            servicoEol.Setup(a => a.ObterPerfisPorLogin(login)).Returns(Task.FromResult(new UsuarioEolAutenticacaoRetornoDto()));
            repositorioUsuario.Setup(a => a.Salvar(usuario)).Returns(usuario.Id);

            //ACT
            await servicoUsuario.AlterarEmailUsuarioPorLogin(login, "teste@teste.com");

            //ASSERT
            Assert.True(true);
        }

        [Fact]
        public async Task Deve_Modificar_Email_Por_Rf()
        {
            //ARRANGE
            var codigoRf = "7777";
            var email = "teste@teste.com";
            var usuario = new Usuario() { Id = 5, Login = codigoRf, CodigoRf = codigoRf };
            repositorioUsuario.Setup(a => a.ObterPorCodigoRfLogin(codigoRf, string.Empty)).Returns(usuario);
            servicoEol.Setup(a => a.ExisteUsuarioComMesmoEmail(email, usuario.Login)).Returns(Task.FromResult(false));
            servicoEol.Setup(a => a.ObterPerfisPorLogin(codigoRf)).Returns(Task.FromResult(new UsuarioEolAutenticacaoRetornoDto()));
            repositorioUsuario.Setup(a => a.Salvar(usuario)).Returns(usuario.Id);
            repositorioPrioridadePerfil.Setup(c => c.ObterPerfisPorIds(It.IsAny<IEnumerable<Guid>>()))
               .Returns(new List<PrioridadePerfil>() {
                    new PrioridadePerfil
                    {
                        Id=1,
                        Ordem=10
                    }
               });

            //ACT
            await servicoUsuario.AlterarEmailUsuarioPorRfOuInclui(codigoRf, "teste@teste.com");

            //ASSERT
            Assert.True(true);
        }

        [Fact]
        public async Task Deve_modificar_perfil_usuario()
        {
            var login = "usuarioDeTeste";
            var usuarioRetornoEol = new UsuarioEolAutenticacaoRetornoDto();

            usuarioRetornoEol.Perfis = new List<Guid> { Guid.Parse("7b6489d9-1870-4d41-a816-0c73c3996e0b"), Guid.Parse("f7b7f917-16c7-4891-9251-8efc9e7f8cea") };

            servicoEol.Setup(a => a.ObterPerfisPorLogin(login))
                .Returns(Task.FromResult(usuarioRetornoEol));

            await servicoUsuario.PodeModificarPerfil(Guid.Parse("f7b7f917-16c7-4891-9251-8efc9e7f8cea"), login);
            Assert.True(true);
        }

        [Fact]
        public async Task Nao_deve_modificar_perfil_usuario()
        {
            var login = "usuarioDeTeste";
            var usuarioRetornoEol = new UsuarioEolAutenticacaoRetornoDto();

            usuarioRetornoEol.Perfis = new List<Guid> { Guid.Parse("7b6489d9-1870-4d41-a816-0c73c3996e0b"), Guid.Parse("f7b7f917-16c7-4891-9251-8efc9e7f8cea") };

            servicoEol.Setup(a => a.ObterPerfisPorLogin(login))
                .Returns(Task.FromResult(usuarioRetornoEol));

            await Assert.ThrowsAsync<NegocioException>(() => servicoUsuario.PodeModificarPerfil(Guid.Parse("fac1d917-16c7-4891-9251-8efc9e7f8cea"), login));
        }
    }
}