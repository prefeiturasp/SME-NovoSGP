using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class AutenticacaoIntegracaoControllerTeste
    {
        private readonly AutenticacaoIntegracaoController _controller;
        private readonly Mock<IObterGuidAutenticacaoFrequencia> _obterGuidAutenticacaoFrequencia = new();
        private readonly Mock<IObterAutenticacaoFrequencia> _obterAutenticacaoFrequencia = new();
        private readonly Faker _faker;

        public AutenticacaoIntegracaoControllerTeste()
        {
            _controller = new AutenticacaoIntegracaoController();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve retornar 200 OK com GUID ao obter autenticação de frequência")]
        public async Task ObterGuidAutenticacaoFrequencia_DeveRetornarOkComGuid()
        {
            // Arrange
            var dto = new SolicitacaoGuidAutenticacaoFrequenciaDto
            {
                Rf = _faker.Random.AlphaNumeric(6),
                TurmaCodigo = _faker.Random.String2(4),
                ComponenteCurricularCodigo = _faker.Random.String2(4)
            };

            var guidEsperado = Guid.NewGuid();

            _obterGuidAutenticacaoFrequencia
                .Setup(s => s.Executar(dto))
                .ReturnsAsync(guidEsperado);

            // Act
            var resultado = await _controller.ObterGuidAutenticacaoFrequencia(dto, _obterGuidAutenticacaoFrequencia.Object);

            // Assert
            var okResult = resultado as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(guidEsperado, okResult.Value);
        }

        [Fact(DisplayName = "Deve lançar exceção quando guid é vazio")]
        public async Task ObterAutenticacaoFrequencia_GuidVazio_DeveLancarExcecao()
        {
            // Arrange
            var guidVazio = Guid.Empty;

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() =>
                _controller.ObterAutenticacaoFrequencia(guidVazio, _obterAutenticacaoFrequencia.Object));
        }

        [Fact(DisplayName = "Deve retornar 200 OK quando usuário está autenticado")]
        public async Task ObterAutenticacaoFrequencia_UsuarioAutenticado_DeveRetornarOk()
        {
            // Arrange
            var guid = Guid.NewGuid();

            var usuarioAutenticacao = new UsuarioAutenticacaoRetornoDto
            {  
                Autenticado = true,
                ModificarSenha = _faker.Random.Bool()
            };

            var turma = new TurmaUeDreDto
            {
                Id = _faker.Random.Int(),
            };

            var retorno = new UsuarioAutenticacaoFrequenciaRetornoDto(
                usuarioAutenticacao,
                turma,
                componenteCurricularCodigo: "MAT"
            );

            _obterAutenticacaoFrequencia.Setup(s => s.Executar(guid)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterAutenticacaoFrequencia(guid, _obterAutenticacaoFrequencia.Object);

            // Assert
            var okResult = resultado as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(retorno, okResult.Value);
        }

        [Fact(DisplayName = "Deve retornar 401 quando usuário não está autenticado")]
        public async Task ObterAutenticacaoFrequencia_UsuarioNaoAutenticado_DeveRetornarUnauthorized()
        {
            // Arrange
            var guid = Guid.NewGuid();

            var usuarioAutenticacao = new UsuarioAutenticacaoRetornoDto
            {
                Autenticado = false, // FORÇAR false
                ModificarSenha = _faker.Random.Bool()
            };

            var turma = new TurmaUeDreDto
            {
                Id = _faker.Random.Int(),
            };

            var retorno = new UsuarioAutenticacaoFrequenciaRetornoDto(
                usuarioAutenticacao,
                turma,
                componenteCurricularCodigo: "MAT"
            );

            _obterAutenticacaoFrequencia.Setup(s => s.Executar(guid)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterAutenticacaoFrequencia(guid, _obterAutenticacaoFrequencia.Object);

            // Assert
            var statusCodeResult = resultado as StatusCodeResult;
            Assert.NotNull(statusCodeResult);
            Assert.Equal((int)HttpStatusCode.Unauthorized, statusCodeResult.StatusCode);
        }

    }
}
