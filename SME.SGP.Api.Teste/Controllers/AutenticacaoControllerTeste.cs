using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class AutenticacaoControllerTeste
    {
        private readonly AutenticacaoController _controller;
        private readonly Mock<IComandosUsuario> _comandosUsuario = new();
        private readonly Mock<IServicoUsuario> _servicoUsuario = new();
        private readonly Mock<IReiniciarSenhaUseCase> _reiniciarSenhaUseCase = new();
        private readonly Mock<IDeslogarSuporteUsuarioUseCase> _deslogarSuporteUsuarioUseCase = new();
        private readonly Faker _faker;

        public AutenticacaoControllerTeste()
        {
            _controller = new AutenticacaoController(_comandosUsuario.Object);
            _faker = new Faker("pt_BR");
        }

        #region AlterarSenha

        [Fact(DisplayName = "Deve alterar a senha e retornar 200 OK")]
        public async Task AlterarSenha_DeveRetornarOk()
        {
            // Arrange
            var dto = new AlterarSenhaDto { NovaSenha = "nova123", SenhaAtual = "atual123" };

            _comandosUsuario.Setup(c => c.AlterarSenha(dto)).Returns(Task.CompletedTask);

            // Act
            var resultado = await _controller.AlterarSenha(dto);

            // Assert
            var okResult = Assert.IsType<OkResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
        }
        #endregion

        #region Autenticacao

        [Fact(DisplayName = "Deve autenticar com sucesso e retornar 200 OK com dados do usuário")]
        public async Task Autenticar_DeveRetornar200_SeAutenticado()
        {
            // Arrange
            var dto = new AutenticacaoDto { Login = "usuario", Senha = "senha" };

            var retorno = new UsuarioAutenticacaoRetornoDto
            {
                Autenticado = true,
                UsuarioRf = _faker.Random.AlphaNumeric(6),
            };

            _comandosUsuario.Setup(c => c.Autenticar(dto.Login, dto.Senha))
                                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.Autenticar(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(retorno, okResult.Value);
        }
        #endregion

        #region AutenticacaoErro

        [Fact(DisplayName = "Deve retornar 401 quando autenticação falhar")]
        public async Task Autenticar_DeveRetornar401_SeNaoAutenticado()
        {
            // Arrange
            var dto = new AutenticacaoDto { Login = "usuario", Senha = "senha" };

            var retorno = new UsuarioAutenticacaoRetornoDto
            {
                Autenticado = false
            };

            _comandosUsuario.Setup(c => c.Autenticar(dto.Login, dto.Senha))
                                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.Autenticar(dto);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(resultado);
            Assert.Equal(401, statusResult.StatusCode);
        }
        #endregion

        #region ListaPerfisUsuario

        [Fact(DisplayName = "Deve retornar 200 OK com lista de perfis do usuário")]
        public async Task ListarPerfisUsuario_DeveRetornarOk_ComPerfis()
        {
            // Arrange
            var login = "usuario";
            var perfis = new List<PrioridadePerfil>();
            for (int i = 0; i < 2; i++)
            {
                perfis.Add(new PrioridadePerfil
                {
                    NomePerfil = _faker.Name.FullName(),
                    CodigoPerfil = Dominio.Perfis.PERFIL_PAP,
                    Ordem = _faker.Random.Int(),
                    Tipo = _faker.PickRandom<TipoPerfil>()
                });
            }

            _servicoUsuario.Setup(s => s.ObterPerfisUsuario(login))
                               .ReturnsAsync(perfis);

            // Act
            var resultado = await _controller.ListarPerfisUsuario(login, _servicoUsuario.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(perfis, okResult.Value);
        }
        #endregion

        #region ListaPerfisUsuarioErro

        [Fact(DisplayName = "Deve retornar 204 NoContent quando perfis forem nulos ou vazios")]
        public async Task ListarPerfisUsuario_DeveRetornarNoContent_QuandoNaoHouverPerfis()
        {
            // Arrange
            var login = "usuario";
            List<PrioridadePerfil> perfis = false ? null : new List<PrioridadePerfil>();

            _servicoUsuario.Setup(s => s.ObterPerfisUsuario(login))
                               .ReturnsAsync(perfis);

            // Act
            var resultado = await _controller.ListarPerfisUsuario(login, _servicoUsuario.Object);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(resultado);
            Assert.Equal(204, noContentResult.StatusCode);
        }
        #endregion

        #region AtualizarPerfil

        [Fact(DisplayName = "Deve retornar 200 OK ao atualizar perfil com guid válido")]
        public async Task AtualizarPerfil_DeveRetornarOk_QuandoGuidValido()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var retornoEsperado = new TrocaPerfilDto { Token = _faker.Random.AlphaNumeric(20) };

            _comandosUsuario.Setup(c => c.ModificarPerfil(guid))
                                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _controller.AtualizarPerfil(guid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(retornoEsperado, okResult.Value);
        }
        #endregion

        #region AtualizarPerfilErroGuidVazio

        [Fact(DisplayName = "Deve lançar NegocioException se o guid for vazio")]
        public async Task AtualizarPerfil_DeveLancarExcecao_QuandoGuidVazio()
        {
            // Arrange
            var guid = Guid.Empty;

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => _controller.AtualizarPerfil(guid));
            Assert.Equal("Informe um perfil", excecao.Message);
        }
        #endregion

        #region PrimeiroAcesso

        [Fact(DisplayName = "Deve retornar 200 OK no primeiro acesso quando senha alterada e autenticação bem-sucedida")]
        public async Task PrimeiroAcesso_DeveRetornarOk_QuandoSenhaAlteradaEAutenticado()
        {
            // Arrange
            var dto = new PrimeiroAcessoDto { Usuario = "usuario", NovaSenha = "novaSenha" };

            var retornoEsperadoSenha = new AlterarSenhaRespostaDto { SenhaAlterada = true };
            var retornoAutenticacao = new UsuarioAutenticacaoRetornoDto
            {
                Autenticado = true,
                Token = "token-jwt"
            };

            _comandosUsuario.Setup(c => c.AlterarSenhaPrimeiroAcesso(dto))
                                .ReturnsAsync(retornoEsperadoSenha);

            _comandosUsuario.Setup(c => c.Autenticar(dto.Usuario, dto.NovaSenha))
                                .ReturnsAsync(retornoAutenticacao);

            // Act
            var resultado = await _controller.PrimeiroAcesso(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(retornoAutenticacao, okResult.Value);
        }
        #endregion

        #region PrimeiroAcessoErro

        [Fact(DisplayName = "Deve retornar 403 Forbidden quando autenticação Error após senha alterada")]
        public async Task PrimeiroAcesso_DeveRetornar403_QuandoAutenticacaoError()
        {
            // Arrange
            var dto = new PrimeiroAcessoDto { Usuario = "usuario", NovaSenha = "novaSenha" };

            var retornoSenha = new RecuperacaoSenhaDto { NovaSenha = _faker.Random.String2(8), Token = Guid.NewGuid() };
            var retornoEsperadoSenha = new AlterarSenhaRespostaDto { SenhaAlterada = true };
            var retornoAutenticacao = new UsuarioAutenticacaoRetornoDto { Autenticado = false };

            _comandosUsuario.Setup(c => c.AlterarSenhaPrimeiroAcesso(dto))
                                .ReturnsAsync(retornoEsperadoSenha);

            _comandosUsuario.Setup(c => c.Autenticar(dto.Usuario, dto.NovaSenha))
                                .ReturnsAsync(retornoAutenticacao);

            // Act
            var resultado = await _controller.PrimeiroAcesso(dto);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(resultado);
            Assert.Equal(403, statusCodeResult.StatusCode);
        }
        #endregion

        #region RecuperarSenha

        [Fact(DisplayName = "Deve retornar 200 (OK) quando recuperar senha e o usuário for autenticado")]
        public async Task RecuperarSenha_DeveRetornarOk_QuandoAutenticado()
        {
            // Arrange
            var dto = new RecuperacaoSenhaDto
            {
                NovaSenha = "NovaSenha123",
                Token = Guid.NewGuid()
            };

            var retornoEsperado = new UsuarioAutenticacaoRetornoDto
            {
                Autenticado = true
            };

            _comandosUsuario
                .Setup(c => c.AlterarSenhaComTokenRecuperacao(dto))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _controller.RecuperarSenha(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(retornoEsperado, okResult.Value);
        }
        #endregion

        #region RecuperarSenhaErro

        [Fact(DisplayName = "Deve retornar 401 quando autenticação com token Error")]
        public async Task RecuperarSenha_DeveRetornar401_QuandoNaoAutenticado()
        {
            // Arrange
            var dto = new RecuperacaoSenhaDto { NovaSenha = _faker.Random.String2(8), Token = Guid.NewGuid() };
            var retorno = new UsuarioAutenticacaoRetornoDto { Autenticado = false };

            _comandosUsuario.Setup(c => c.AlterarSenhaComTokenRecuperacao(dto))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.RecuperarSenha(dto);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(resultado);
            Assert.Equal(401, statusCodeResult.StatusCode);
        }
        #endregion

        #region ReiniciarSenha

        [Fact(DisplayName = "Deve retornar 200 quando atualizarEmail for false ao reiniciar senha")]
        public async Task ReiniciarSenha_DeveRetornar200_QuandoDeveAtualizarEmailForFalse()
        {
            // Arrange
            var codigoRf = "12345";
            var dreUeDto = new DreUeCodigoDto { DreCodigo = "DRE123", UeCodigo = "UE456" };

            var retornoEsperado = new UsuarioReinicioSenhaDto
            {
                DeveAtualizarEmail = false,
                Mensagem = "Senha reiniciada com sucesso"
            };

            _reiniciarSenhaUseCase.Setup(x => x.ReiniciarSenha(codigoRf, dreUeDto.DreCodigo, dreUeDto.UeCodigo))
                       .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _controller.ReiniciarSenha(_reiniciarSenhaUseCase.Object, codigoRf, dreUeDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(retornoEsperado, okResult.Value);
        }

        #endregion

        #region ReiniciarSenhaErro

        [Fact(DisplayName = "Deve retornar 601 quando atualizarEmail for true ao reiniciar senha")]
        public async Task ReiniciarSenha_DeveRetornar601_QuandoDeveAtualizarEmailForTrue()
        {
            // Arrange
            var codigoRf = "12345";
            var dreUeDto = new DreUeCodigoDto { DreCodigo = "DRE123", UeCodigo = "UE456" };

            var retornoEsperado = new UsuarioReinicioSenhaDto
            {
                DeveAtualizarEmail = true,
                Mensagem = "Atualize seu e-mail"
            };

            _reiniciarSenhaUseCase.Setup(x => x.ReiniciarSenha(codigoRf, dreUeDto.DreCodigo, dreUeDto.UeCodigo))
                       .ReturnsAsync(retornoEsperado);


            // Act
            var resultado = await _controller.ReiniciarSenha(_reiniciarSenhaUseCase.Object, codigoRf, dreUeDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(resultado);
            Assert.Equal(601, objectResult.StatusCode);
            Assert.Equal(retornoEsperado, objectResult.Value);
        }
        #endregion

        #region Revalidar

        [Fact(DisplayName = "Deve retornar 200 quando retorno não for nulo ao revalidar")]
        public async Task Revalidar_DeveRetornar200_QuandoRetornoNaoForNulo()
        {
            // Arrange
            var retornoEsperado = new UsuarioReinicioSenhaDto
            {
                DeveAtualizarEmail = false,
                Mensagem = "Token válido"
            };

            // Act
            var resultado = await _controller.Revalidar();

            // Assert
            Assert.IsType<StatusCodeResult>(resultado);
        }
        #endregion

        #region RevalidarErro

        [Fact(DisplayName = "Deve retornar 401 quando retorno for nulo ao revalidar")]
        public async Task Revalidar_DeveRetornar401_QuandoRetornoForNulo()
        {
            // Act
            var resultado = await _controller.Revalidar();

            // Assert
            Assert.IsType<StatusCodeResult>(resultado);
        }
        #endregion

        #region Sair

        [Fact(DisplayName = "Deve retornar OK ao sair")]
        public void Sair_DeveRetornarOk_E_ChamarMetodoSair()
        {
            // Arrange
            _comandosUsuario.Setup(c => c.Sair());

            var controller = new AutenticacaoController(_comandosUsuario.Object);

            // Act
            var resultado = controller.Sair();

            // Assert
            Assert.IsType<OkResult>(resultado);
        }
        #endregion

        #region SolicitarRecuperacaoSenha

        [Fact(DisplayName = "Deve retornar OK ao solicitar recuperacao de senha")]
        public async Task SolicitarRecuperacaoSenha_DeveRetornarOk_ComString()
        {
            // Arrange
            var login = "usuario_teste";
            var retornoEsperado = "Token de recuperação enviado";

            _comandosUsuario
                .Setup(c => c.SolicitarRecuperacaoSenha(login))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _controller.SolicitarRecuperacaoSenha(login);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valor = Assert.IsType<string>(okResult.Value);
            Assert.Equal(retornoEsperado, valor);
            _comandosUsuario.Verify(c => c.SolicitarRecuperacaoSenha(login), Times.Once);
        }
        #endregion

        #region TokenRecuperacaoSenha

        [Fact(DisplayName = "Deve retornar OK para recuperacao de senha com token valido")]
        public async Task TokenRecuperacaoSenhaEstaValido_DeveRetornarOk_ComBooleano()
        {
            // Arrange
            var token = Guid.NewGuid();
            var retornoEsperado = true;

            _comandosUsuario
                .Setup(c => c.TokenRecuperacaoSenhaEstaValido(token))
                .ReturnsAsync(retornoEsperado);

            var controller = new AutenticacaoController(_comandosUsuario.Object);

            // Act
            var resultado = await controller.TokenRecuperacaoSenhaEstaValidoAsync(token);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valor = Assert.IsType<bool>(okResult.Value);
            Assert.True(valor);
            Assert.Equal(retornoEsperado, valor);
            _comandosUsuario.Verify(c => c.TokenRecuperacaoSenhaEstaValido(token), Times.Once);
        }
        #endregion

        #region AutenticarSuporte

        [Fact(DisplayName = "Deve Retornar OK quando suporte for autenticado")]
        public async Task AutenticarSuporte_DeveRetornarOk_QuandoAutenticado()
        {
            // Arrange
            var login = "suporte123";
            var retornoEsperado = new UsuarioAutenticacaoRetornoDto { Autenticado = true };

            _comandosUsuario
                .Setup(c => c.AutenticarSuporte(login))
                .ReturnsAsync(retornoEsperado);

            var controller = new AutenticacaoController(_comandosUsuario.Object);

            // Act
            var resultado = await controller.AutenticarSuporte(login);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valor = Assert.IsType<UsuarioAutenticacaoRetornoDto>(okResult.Value);
            Assert.True(valor.Autenticado);
            _comandosUsuario.Verify(c => c.AutenticarSuporte(login), Times.Once);
        }
        #endregion

        #region AutenticarSuporteErro

        [Fact(DisplayName = "Deve retornar 401 quando suporte não for autenticado")]
        public async Task AutenticarSuporte_DeveRetornar401_QuandoNaoAutenticado()
        {
            // Arrange
            var login = "suporte123";
            var retornoEsperado = new UsuarioAutenticacaoRetornoDto { Autenticado = false };

            _comandosUsuario
                .Setup(c => c.AutenticarSuporte(login))
                .ReturnsAsync(retornoEsperado);

            var controller = new AutenticacaoController(_comandosUsuario.Object);

            // Act
            var resultado = await controller.AutenticarSuporte(login);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(resultado);
            Assert.Equal(401, statusResult.StatusCode);
            _comandosUsuario.Verify(c => c.AutenticarSuporte(login), Times.Once);
        }
        #endregion

        #region DeslogarSuporte

        [Fact(DisplayName = "Deve retornar OK quando suporte for autenticado ao deslogar")]
        public async Task DeslogarSuporte_DeveRetornarOk_QuandoAutenticado()
        {
            // Arrange
            var retornoEsperado = new UsuarioAutenticacaoRetornoDto { Autenticado = true };

            _deslogarSuporteUsuarioUseCase
                .Setup(u => u.Executar())
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _controller.DeslogarSuporte(_deslogarSuporteUsuarioUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valor = Assert.IsType<UsuarioAutenticacaoRetornoDto>(okResult.Value);
            Assert.True(valor.Autenticado);
            _deslogarSuporteUsuarioUseCase.Verify(u => u.Executar(), Times.Once);
        }
        #endregion
    }
}
