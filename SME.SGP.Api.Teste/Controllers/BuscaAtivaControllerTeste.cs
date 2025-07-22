using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class BuscaAtivaControllerTeste
    {
        private readonly BuscaAtivaController _controller;
        private readonly Mock<IObterRegistrosAcaoUseCase> _obterRegistrosAcaoUseCase = new();
        private readonly Mock<IRegistrarRegistroAcaoUseCase> _registrarRegistroAcaoUseCase = new();
        private readonly Mock<IObterSecoesRegistroAcaoSecaoUseCase> _obterSecoesRegistroAcaoSecaoUseCase = new();
        private readonly Mock<IObterQuestionarioRegistroAcaoUseCase> _obterQuestionarioRegistroAcaoUseCase = new();
        private readonly Mock<IExcluirRegistroAcaoUseCase> _excluirRegistroAcaoUseCase = new();
        private readonly Mock<IObterRegistrosAcaoCriancaEstudanteAusenteUseCase> _obterRegistrosAcaoCriancaEstudanteAusenteUseCase = new();
        private readonly Mock<IObterRegistroAcaoPorIdUseCase> _obterRegistroAcaoPorIdUseCase = new();
        private readonly Mock<IAtualizarDadosResponsaveisUseCase> _atualizarDadosResponsaveisUseCase = new();
        private readonly Mock<IObterOpcoesRespostaMotivoAusenciaBuscaAtivaUseCase> _obterOpcoesRespostaMotivoAusenciaBuscaAtivaUseCase = new();
        private readonly Faker _faker;

        public BuscaAtivaControllerTeste()
        {
            _controller = new BuscaAtivaController();
            _faker = new Faker("pt_BR");
        }

        #region RegistrarRegistroAcao
        [Fact]
        public async Task RegistrarRegistroAcao_DeveRetornarOkComResultado()
        {
            // Arrange
            var registroAcaoDto = new RegistroAcaoBuscaAtivaDto { };
            var resultadoEsperado = new ResultadoRegistroAcaoBuscaAtivaDto { };

            _registrarRegistroAcaoUseCase
                .Setup(u => u.Executar(registroAcaoDto))
                .ReturnsAsync(resultadoEsperado);


            // Act
            var resultado = await _controller.RegistrarRegistroAcao(registroAcaoDto, _registrarRegistroAcaoUseCase.Object);

            // Assert
            var okResult = resultado as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }
        #endregion

        #region ObterSecoesDeRegistroAcao
        [Fact]
        public async Task ObterSecoesDeRegistroAcao_DeveRetornarOkComSecoes()
        {
            // Arrange
            var filtro = new FiltroSecoesDeRegistroAcao { };

            var secoesEsperadas = new List<SecaoQuestionarioDto>
            {
                new SecaoQuestionarioDto { Id = 1, Nome = "Seção 1" },
                new SecaoQuestionarioDto { Id = 2, Nome = "Seção 2" }
            };

            _obterSecoesRegistroAcaoSecaoUseCase
                .Setup(u => u.Executar(filtro))
                .ReturnsAsync(secoesEsperadas);

            // Act
            var resultado = await _controller.ObterSecoesDeRegistroAcao(filtro, _obterSecoesRegistroAcaoSecaoUseCase.Object);

            // Assert
            var okResult = resultado as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(secoesEsperadas, okResult.Value);
        }
        #endregion

        #region ObterQuestionario
        [Fact]
        public async Task ObterQuestionario_DeveRetornarOkComQuestoes()
        {
            // Arrange
            long questionarioId = 1;
            long? registroAcaoId = 2;

            var questoesEsperadas = new List<QuestaoDto>
            {
                new QuestaoDto { Id = 101, Nome = "Pergunta 1" },
                new QuestaoDto { Id = 102, Nome = "Pergunta 2" }
            };

            _obterQuestionarioRegistroAcaoUseCase
                .Setup(u => u.Executar(questionarioId, registroAcaoId))
                .ReturnsAsync(questoesEsperadas);

            // Act
            var resultado = await _controller.ObterQuestionario(questionarioId, registroAcaoId, _obterQuestionarioRegistroAcaoUseCase.Object);

            // Assert
            var okResult = resultado as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(questoesEsperadas, okResult.Value);
        }
        #endregion

        #region ExcluirRegistroAcao
        [Fact]
        public async Task ExcluirRegistroAcao_DeveRetornarOkComTrue()
        {
            // Arrange
            long registroAcaoId = 123;
            bool resultadoEsperado = true;

            _excluirRegistroAcaoUseCase
                .Setup(u => u.Executar(registroAcaoId))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _controller.ExcluirRegistroAcao(registroAcaoId, _excluirRegistroAcaoUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }
        #endregion

        #region ObterRegistrosAcaoCriancaEstudanteAusente
        [Fact]
        public async Task ObterRegistrosAcaoCriancaEstudanteAusente_DeveRetornarOkComPaginacao()
        {
            // Arrange
            var filtro = new FiltroRegistrosAcaoCriancasEstudantesAusentesDto
            {
                // Preencha com dados simulados se necessário
            };

            var resultadoEsperado = new PaginacaoResultadoDto<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>
            {
                TotalRegistros = 1,
                TotalPaginas = 1,
                Items = new List<RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto>
                {
                    new RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto
                    {
                        Id = 1,
                        Usuario = "Usuario1"
                    }
                }
            };

            _obterRegistrosAcaoCriancaEstudanteAusenteUseCase
                .Setup(u => u.Executar(filtro))
                .ReturnsAsync(resultadoEsperado);


            // Act
            var resultado = await _controller.ObterRegistrosAcaoCriancaEstudanteAusente(filtro, _obterRegistrosAcaoCriancaEstudanteAusenteUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }
        #endregion

        #region ObterRegistroAcao
        [Fact]
        public async Task ObterRegistroAcao_DeveRetornarOkComRegistro()
        {
            // Arrange
            long registroAcaoId = 123;

            var registroEsperado = new RegistroAcaoBuscaAtivaRespostaDto
            {
                DreId = registroAcaoId,
                DreNome = "João da Silva",
                DreCodigo = "123",
            };

            _obterRegistroAcaoPorIdUseCase
                .Setup(u => u.Executar(registroAcaoId))
                .ReturnsAsync(registroEsperado);

            var controller = new BuscaAtivaController();

            // Act
            var resultado = await controller.ObterRegistroAcao(registroAcaoId, _obterRegistroAcaoPorIdUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(registroEsperado, okResult.Value);
        }
        #endregion

        #region AtualizarCriancasEstudantesDadosResponsaveis
        [Fact]
        public async Task AtualizarCriancasEstudantesDadosResponsaveis_DeveRetornarOkComResultado()
        {
            // Arrange
            var usuarioDto = new AtualizarDadosResponsavelDto{ };

            var resultadoEsperado = true;

            _atualizarDadosResponsaveisUseCase
                .Setup(u => u.Executar(usuarioDto))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _controller.AtualizarDadosResponsaveis(usuarioDto, _atualizarDadosResponsaveisUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }
        #endregion

        #region ObterMotivosAusencia
        [Fact]
        public async Task ObterMotivosAusencia_DeveRetornarOkComLista()
        {
            // Arrange
            var opcoesEsperadas = new List<OpcaoRespostaSimplesDto>
            {
                new OpcaoRespostaSimplesDto { Id = 1, Nome = "Teste1" },
                new OpcaoRespostaSimplesDto { Id = 2, Nome = "Teste2" }
            };

            _obterOpcoesRespostaMotivoAusenciaBuscaAtivaUseCase
                .Setup(u => u.Executar())
                .ReturnsAsync(opcoesEsperadas);

            // Act
            var resultado = await _controller.ObterMotivosAusencia(_obterOpcoesRespostaMotivoAusenciaBuscaAtivaUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(opcoesEsperadas, okResult.Value);
        }
        #endregion

        #region ListaRegistrosAcao

        [Fact]
        public async Task ObterRegistrosAcao_DeveRetornarOkComResultado()
        {
            var filtro = new FiltroRegistrosAcaoDto();
            var resultadoEsperado = new PaginacaoResultadoDto<RegistroAcaoBuscaAtivaListagemDto>
            {
            };

            _obterRegistrosAcaoUseCase
                .Setup(u => u.Executar(filtro))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await _controller.ObterRegistrosAcao(filtro, _obterRegistrosAcaoUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.NotNull(okResult.Value);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }
        #endregion
    }
}