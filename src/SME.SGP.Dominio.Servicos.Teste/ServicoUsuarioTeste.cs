using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoUsuarioTeste
    {
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
            var context = new DefaultHttpContext();
            var obj = new HttpContextAccessor();
            obj.HttpContext = context;

            servicoUsuario = new ServicoUsuario(repositorioUsuario.Object, servicoEol.Object, repositorioPrioridadePerfil.Object, unitOfWork.Object, obj);
        }

        [Fact]
        public async void Deve_Modificar_Email_Por_Login()
        {
            //ARRANGE
            var login = "loginTeste";
            var email = "teste@teste.com";
            var usuario = new Usuario() { Id = 5, Login = login };
            repositorioUsuario.Setup(a => a.ObterPorCodigoRfLogin(string.Empty, login)).Returns(usuario);
            repositorioUsuario.Setup(a => a.ExisteUsuarioComMesmoEmail(email, usuario.Id)).Returns(false);
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
        public async void Deve_Modificar_Email_Por_Rf()
        {
            //ARRANGE
            var codigoRf = "7777";
            var email = "teste@teste.com";
            var usuario = new Usuario() { Id = 5, Login = codigoRf, CodigoRf = codigoRf };
            repositorioUsuario.Setup(a => a.ObterPorCodigoRfLogin(codigoRf, string.Empty)).Returns(usuario);
            repositorioUsuario.Setup(a => a.ExisteUsuarioComMesmoEmail(email, usuario.Id)).Returns(false);
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
        public async void Deve_modificar_perfil_usuario()
        {
            var login = "usuarioDeTeste";
            var usuarioRetornoEol = new UsuarioEolAutenticacaoRetornoDto();

            usuarioRetornoEol.Perfis = new List<Guid> { Guid.Parse("7b6489d9-1870-4d41-a816-0c73c3996e0b"), Guid.Parse("f7b7f917-16c7-4891-9251-8efc9e7f8cea") };

            servicoEol.Setup(a => a.ObterPerfisPorLogin(login))
                .Returns(Task.FromResult(usuarioRetornoEol));

            await servicoUsuario.PodeModificarPerfil("f7b7f917-16c7-4891-9251-8efc9e7f8cea", login);
            Assert.True(true);
        }

        [Fact]
        public async void Nao_deve_modificar_perfil_usuario()
        {
            var login = "usuarioDeTeste";
            var usuarioRetornoEol = new UsuarioEolAutenticacaoRetornoDto();

            usuarioRetornoEol.Perfis = new List<Guid> { Guid.Parse("7b6489d9-1870-4d41-a816-0c73c3996e0b"), Guid.Parse("f7b7f917-16c7-4891-9251-8efc9e7f8cea") };

            servicoEol.Setup(a => a.ObterPerfisPorLogin(login))
                .Returns(Task.FromResult(usuarioRetornoEol));

            await Assert.ThrowsAsync<NegocioException>(() => servicoUsuario.PodeModificarPerfil("fac1d917-16c7-4891-9251-8efc9e7f8cea", login));
        }
    }
}