using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class AulaPrevistaControllerTeste
    {
        private readonly AulaPrevistaController _controller;
        private readonly Mock<IConsultasAulaPrevista> _consultasAulaPrevista = new();
        private readonly Mock<IComandosAulaPrevista> _comandosAulaPrevista = new();
        private readonly Faker _faker;

        public AulaPrevistaControllerTeste()
        {
            _controller = new AulaPrevistaController();
            _faker = new Faker("pt_BR");
        }

        #region AulaPrevistaDada

        [Fact(DisplayName = "Deve chamar o caso de uso que consulta aula prevista dada")]
        public async Task DeveRetornarOk_QuandoObterAulaPrevistaDada()
        {
            // Arrange
            var modalidade = Modalidade.EducacaoInfantil;
            var turmaId = _faker.Random.AlphaNumeric(6);
            var disciplinaId = _faker.Random.AlphaNumeric(6);
            var semestre = _faker.Random.Int(1, 2);

            var aulasPrevistasDadasAuditoriaDto = new AulasPrevistasDadasAuditoriaDto
            {
                AulasPrevistasPorBimestre = new List<AulasPrevistasDadasDto>
                {
                    new AulasPrevistasDadasDto
                    {
                        Bimestre = 1,
                        Inicio = DateTime.Now.AddMonths(-1),
                        Fim = DateTime.Now,
                        Cumpridas = 10,
                        Previstas = new AulasPrevistasDto(),
                        Criadas = new AulasQuantidadePorProfessorDto(),
                        Reposicoes = 2,
                        PodeEditar = true
                    }
                }
            };

            _consultasAulaPrevista
                .Setup(c => c.ObterAulaPrevistaDada(modalidade, turmaId, disciplinaId, semestre))
                .ReturnsAsync(aulasPrevistasDadasAuditoriaDto);

            // Act
            var resultado = await _controller.ObterAulaPrevistaDada(modalidade, turmaId, disciplinaId, semestre, _consultasAulaPrevista.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<AulasPrevistasDadasAuditoriaDto>(okResult.Value);

            Assert.NotNull(retorno.AulasPrevistasPorBimestre);
            Assert.Single(retorno.AulasPrevistasPorBimestre);
            Assert.Equal(1, retorno.AulasPrevistasPorBimestre.First().Bimestre);
            Assert.Equal(10, retorno.AulasPrevistasPorBimestre.First().Cumpridas);
        }
        #endregion

        #region BuscarAulaPrevistaPorID
        [Fact(DisplayName = "Deve chamar a consulta que retorna aula prevista por ID")]
        public async Task DeveRetornarOk_QuandoBuscarPorId()
        {
            // Arrange
            var id = 1L;

            var retornoEsperado = new AulasPrevistasDadasAuditoriaDto
            {
                AulasPrevistasPorBimestre = new List<AulasPrevistasDadasDto>()
                {
                    new AulasPrevistasDadasDto { Bimestre = 1 }
                }
            };

            _consultasAulaPrevista.Setup(c => c.BuscarPorId(id)).ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _controller.BuscarPorId(id, _consultasAulaPrevista.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var dto = Assert.IsType<AulasPrevistasDadasAuditoriaDto>(okResult.Value);
            Assert.NotNull(dto);
        }
        #endregion

        #region InserirAulaPrevista
        [Fact(DisplayName = "Deve chamar o comando que insere aula prevista")]
        public async Task DeveRetornarOk_QuandoInserirAulaPrevista()
        {
            // Arrange
            var dto = new AulaPrevistaDto
            {
                // Inicialize as propriedades necessárias do dto aqui,
                // por exemplo:
                // Propriedade1 = valor1,
                // Propriedade2 = valor2,
            };

            var retornoEsperado = new AulasPrevistasDadasAuditoriaDto
            {
                AulasPrevistasPorBimestre = new List<AulasPrevistasDadasDto>()
                {
                    new AulasPrevistasDadasDto { Bimestre = 1 }
                }
            };

            _comandosAulaPrevista.Setup(c => c.Inserir(dto)).ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _controller.Inserir(dto, _comandosAulaPrevista.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<AulasPrevistasDadasAuditoriaDto>(okResult.Value);
            Assert.NotNull(retorno);
            Assert.NotEmpty(retorno.AulasPrevistasPorBimestre);
        }
        #endregion

        #region AlterarAulaPrevista
        [Fact(DisplayName = "Deve chamar o comando que altera aula prevista")]
        public async Task DeveRetornarOk_QuandoAlterarAulaPrevistaComSucesso()
        {
            // Arrange
            var id = 1L;
            var dto = new AulaPrevistaDto
            {
                Id = _faker.Random.Int(1),
                DisciplinaId = _faker.Random.AlphaNumeric(6),
                Modalidade = Modalidade.Fundamental,
                TurmaId = _faker.Random.AlphaNumeric(6),
            };

            var mensagemEsperada = "Aula prevista alterada com sucesso";

            var comandosMock = new Mock<IComandosAulaPrevista>();
            comandosMock.Setup(c => c.Alterar(dto, id)).ReturnsAsync(mensagemEsperada);

            var controller = new AulaPrevistaController();

            // Act
            var resultado = await controller.Alterar(dto, id, comandosMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<RetornoBaseDto>(okResult.Value);
            Assert.Contains(mensagemEsperada, retorno.Mensagens);
        }
        #endregion
    }
}
