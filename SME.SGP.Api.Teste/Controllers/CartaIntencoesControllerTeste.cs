using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class CartaIntencoesControllerTeste
    {
        private readonly CartaIntencoesController _controller;
        private readonly Mock<ICartaIntencoesPersistenciaUseCase> _cartaIntencoesPersistenciaUseCase = new();
        private readonly Mock<IObterCartasDeIntencoesPorTurmaEComponenteUseCase> _obterCartasDeIntencoesPorTurmaEComponenteUseCase = new();
        private readonly Mock<IListarCartaIntencoesObservacoesPorTurmaEComponenteUseCase> _listarCartaIntencoesObservacoesPorTurmaEComponenteUseCase = new();
        private readonly Mock<ISalvarCartaIntencoesObservacaoUseCase> _salvarCartaIntencoesObservacaoUseCase = new();
        private readonly Mock<IAlterarCartaIntencoesObservacaoUseCase> _alterarCartaIntencoesObservacaoUseCase = new();
        private readonly Mock<IExcluirCartaIntencoesObservacaoUseCase> _excluirCartaIntencoesObservacaoUseCase = new();
        private readonly Mock<IObterUsuarioNotificarCartaIntencoesObservacaoUseCase> _obterUsuarioNotificarCartaIntencoesObservacaoUseCase = new();
        private readonly Faker _faker;

        public CartaIntencoesControllerTeste()
        {
            _controller = new CartaIntencoesController();
            _faker = new Faker("pt_BR");
        }

        #region Salvar
        [Fact]
        public async Task Salvar_DeveRetornarOkComListaDeRetorno()
        {
            // Arrange
            var dtoEntrada = new CartaIntencoesPersistenciaDto { };

            var retornoEsperado = new List<CartaIntencoesRetornoPersistenciaDto>
        {
            new CartaIntencoesRetornoPersistenciaDto { PeriodoEscolarId = 1,  },
            new CartaIntencoesRetornoPersistenciaDto { PeriodoEscolarId = 2, }
        };

            _cartaIntencoesPersistenciaUseCase
                .Setup(u => u.Executar(dtoEntrada))
                .ReturnsAsync(retornoEsperado);

            var controller = new CartaIntencoesController();

            // Act
            var resultado = await controller.Salvar(_cartaIntencoesPersistenciaUseCase.Object, dtoEntrada);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retornoLista = Assert.IsType<List<CartaIntencoesRetornoPersistenciaDto>>(okResult.Value);

            Assert.Equal(2, retornoLista.Count);
        }
        #endregion

        #region ListaDeCartas
        [Fact]
        public async Task Obter_DeveRetornarOkComListaDeCartas()
        {
            // Arrange
            var turmaCodigo = "TURMA123";
            var componenteCurricularId = 456;

            var retornoEsperado = new List<CartaIntencoesRetornoDto>
            {
                new CartaIntencoesRetornoDto
                {
                    Id = 1,
                    Planejamento = "Planejamento1",
                    Auditoria = new AuditoriaDto { CriadoPor = "usuario1", CriadoEm = DateTime.Today }
                },
                new CartaIntencoesRetornoDto
                {
                    Id = 2,
                    Planejamento = "Planejamento2",
                    Auditoria = new AuditoriaDto { CriadoPor = "usuario2", CriadoEm = DateTime.Today }
                }
            };

            _obterCartasDeIntencoesPorTurmaEComponenteUseCase
                .Setup(u => u.Executar(It.Is<ObterCartaIntencoesDto>(dto =>
                    dto.TurmaCodigo == turmaCodigo &&
                    dto.ComponenteCurricularId == componenteCurricularId)))
                .ReturnsAsync(retornoEsperado);

            var controller = new CartaIntencoesController();

            // Act
            var resultado = await controller.Obter(_obterCartasDeIntencoesPorTurmaEComponenteUseCase.Object, turmaCodigo, componenteCurricularId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retornoLista = Assert.IsAssignableFrom<IEnumerable<CartaIntencoesRetornoDto>>(okResult.Value);
            Assert.Collection(retornoLista,
                item => Assert.Equal("Planejamento1", item.Planejamento),
                item => Assert.Equal("Planejamento2", item.Planejamento));
        }
        #endregion

        #region ListaObservacoes
        [Fact]
        public async Task ListarObservacoes_DeveRetornarOkComListaDeObservacoes()
        {
            // Arrange
            var turmaCodigo = "TURMA123";
            var componenteCurricularId = 456;

            var retornoEsperado = new List<CartaIntencoesObservacaoDto>
            {
                new CartaIntencoesObservacaoDto
                {
                    Id = 1,

                },
                new CartaIntencoesObservacaoDto
                {
                    Id = 2,
                }
            };

            _listarCartaIntencoesObservacoesPorTurmaEComponenteUseCase
                .Setup(u => u.Executar(It.Is<BuscaCartaIntencoesObservacaoDto>(dto =>
                    dto.TurmaCodigo == turmaCodigo &&
                    dto.ComponenteCurricularId == componenteCurricularId)))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _controller.ListarObservacoes(_listarCartaIntencoesObservacoesPorTurmaEComponenteUseCase.Object, turmaCodigo, componenteCurricularId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retornoLista = Assert.IsAssignableFrom<IEnumerable<CartaIntencoesObservacaoDto>>(okResult.Value);
            Assert.Equal(2, retornoLista.Count());

            Assert.Collection(retornoLista,
                item => {
                    Assert.Equal(1, item.Id);
                },
                item => {
                    Assert.Equal(2, item.Id);
                });
        }
        #endregion

        #region SalvarObservacao
        [Fact]
        public async Task SalvarObservacao_DeveRetornarOkComAuditoria()
        {
            // Arrange
            var turmaCodigo = "TURMA123";
            var componenteCurricularId = 789;

            var dto = new SalvarCartaIntencoesObservacaoDto
            {
                Observacao = "Esta é uma observação de teste"
            };

            var auditoriaEsperada = new AuditoriaDto
            {
                CriadoPor = "usuario.testador",
                CriadoEm = new DateTime(2025, 7, 22)
            };

            _salvarCartaIntencoesObservacaoUseCase
                .Setup(u => u.Executar(turmaCodigo, componenteCurricularId, dto))
                .ReturnsAsync(auditoriaEsperada);

            // Act
            var resultado = await _controller.SalvarObservacao(dto, _salvarCartaIntencoesObservacaoUseCase.Object, turmaCodigo, componenteCurricularId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<AuditoriaDto>(okResult.Value);

            Assert.Equal("usuario.testador", retorno.CriadoPor);
            Assert.Equal(new DateTime(2025, 7, 22), retorno.CriadoEm);
        }
        #endregion

        #region AlterarObservacao
        [Fact]
        public async Task AlterarObservacao_DeveRetornarOkComAuditoria()
        {
            // Arrange
            var observacaoId = 123;

            var dto = new AlterarCartaIntencoesObservacaoDto
            {
                Observacao = "Texto atualizado da observação"
            };

            var auditoriaEsperada = new AuditoriaDto
            {
                CriadoPor = "usuario.alterador",
                CriadoEm = new DateTime(2025, 7, 22)
            };

            _alterarCartaIntencoesObservacaoUseCase
                .Setup(u => u.Executar(observacaoId, dto))
                .ReturnsAsync(auditoriaEsperada);

            // Act
            var resultado = await _controller.AlterarObservacao(dto, _alterarCartaIntencoesObservacaoUseCase.Object, observacaoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<AuditoriaDto>(okResult.Value);

            Assert.Equal("usuario.alterador", retorno.CriadoPor);
            Assert.Equal(new DateTime(2025, 7, 22), retorno.CriadoEm);
        }
        #endregion

        #region ExcluirObservacao
        [Fact]
        public async Task ExcluirObservacao_DeveRetornarOkComAuditoria()
        {
            // Arrange
            var observacaoId = 321;

            var retornoEsperado = true;

            _excluirCartaIntencoesObservacaoUseCase
                .Setup(u => u.Executar(observacaoId))
                .ReturnsAsync(retornoEsperado);

            var controller = new CartaIntencoesController();

            // Act
            var resultado = await controller.ExcluirObservacao(_excluirCartaIntencoesObservacaoUseCase.Object, observacaoId);

            // Assert
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(retornoEsperado, okResult.Value);
        }
        #endregion

        #region ObterUsuarioParaNotificar
        [Fact]
        public async Task ObterUsuariosParaNotificar_DeveRetornarOkComUsuarios()
        {
            // Arrange
            var dto = new ObterUsuarioNotificarCartaIntencoesObservacaoDto { };

            var retornoEsperado = new List<UsuarioNotificarCartaIntencoesObservacaoDto>
            {
                new UsuarioNotificarCartaIntencoesObservacaoDto { UsuarioId = 1, Nome = "João" },
                new UsuarioNotificarCartaIntencoesObservacaoDto { UsuarioId = 2, Nome = "Maria" }
            };

            _obterUsuarioNotificarCartaIntencoesObservacaoUseCase
                .Setup(u => u.Executar(dto))
                .ReturnsAsync(retornoEsperado);

            var controller = new CartaIntencoesController();

            // Act
            var resultado = await controller.ObterUsuariosParaNotificar(dto, _obterUsuarioNotificarCartaIntencoesObservacaoUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsAssignableFrom<IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>>(okResult.Value);

            var lista = retorno.ToList();
            Assert.Equal(2, lista.Count);
            Assert.Equal(1, lista[0].UsuarioId);
            Assert.Equal("João", lista[0].Nome);
            Assert.Equal(2, lista[1].UsuarioId);
            Assert.Equal("Maria", lista[1].Nome);
        }
        #endregion
    }
}
