using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.AnotacaoFrequenciaAluno;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Controllers
{
    public class AnotacaoFrequenciaAlunoControllerTeste
    {
        private readonly AnotacaoFrequenciaAlunoController _controller;
        private readonly Mock<IObterAnotacaoFrequenciaAlunoPorIdUseCase> _obterAnotacaoFrequenciaAlunoPorIdUseCase = new();
        private readonly Mock<IObterAnotacaoFrequenciaAlunoUseCase> _obterAnotacaoFrequenciaAlunoUseCase = new();
        private readonly Mock<IObterAnotacaoFrequenciaAlunoPorPeriodoUseCase> _obterAnotacaoFrequenciaAlunoPorPeriodoUseCase = new();
        private readonly Mock<ISalvarAnotacaoFrequenciaAlunoUseCase> _salvarAnotacaoFrequenciaAlunoUseCase = new();
        private readonly Mock<IAlterarAnotacaoFrequenciaAlunoUseCase> _alterarAnotacaoFrequenciaAlunoUseCase = new();
        private readonly Mock<IObterMotivosAusenciaUseCase> _obterMotivosAusenciaUseCase = new();
        private readonly Mock<IExcluirAnotacaoFrequenciaAlunoUseCase> _excluirAnotacaoFrequenciaAlunoUseCase = new();
        private readonly Faker _faker;

        public AnotacaoFrequenciaAlunoControllerTeste()
        {
            _controller = new AnotacaoFrequenciaAlunoController();
            _faker = new Faker("pt_BR");
        }

        #region BuscarAnotacaoFrequenciaAlunoPorID

        [Fact(DisplayName = "Deve retornar anotação de frequência completa por ID")]
        public async Task DeveRetornar_AnotacaoFrequenciaAluno_BuscarPorID()
        {
            var id = _faker.Random.Long(1);
            var anotacaoEsperada = new AnotacaoFrequenciaAlunoCompletoDto { Id = id };
            _obterAnotacaoFrequenciaAlunoPorIdUseCase.Setup(x => x.Executar(id)).ReturnsAsync(anotacaoEsperada);

            var resultado = await _controller.ObterJustificativaCompleto(id, _obterAnotacaoFrequenciaAlunoPorIdUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var dto = Assert.IsType<AnotacaoFrequenciaAlunoCompletoDto>(okResult.Value);
            Assert.Equal(anotacaoEsperada.Id, dto.Id);
        }
        #endregion

        #region BuscarAnotacaoFrequenciaPorAlunoEAula

        [Fact(DisplayName = "Deve retornar anotação de frequência por aluno e aula")]
        public async Task DeveRetornarAnotacaoFrequenciaAluno_BuscaPorAlunoEAula()
        {
            var codigoAluno = _faker.Random.AlphaNumeric(8);
            var aulaId = _faker.Random.Long(1);
            var anotacaoEsperada = new AnotacaoFrequenciaAlunoDto();

            _obterAnotacaoFrequenciaAlunoUseCase.Setup(x => x.Executar(It.Is<FiltroAnotacaoFrequenciaAlunoDto>(
                f => f.CodigoAluno == codigoAluno && f.AulaId == aulaId)))
                .ReturnsAsync(anotacaoEsperada);

            var resultado = await _controller.BuscarPorId(codigoAluno, aulaId, _obterAnotacaoFrequenciaAlunoUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.IsType<AnotacaoFrequenciaAlunoDto>(okResult.Value);
        }
        #endregion

        #region BuscaAnotacaoFrequenciaAlunoNaoExiste

        [Fact(DisplayName = "Deve retornar NoContent quando anotação de frequência por aluno e aula não existir")]
        public async Task DeveRetornarNoContent_AnotacaoFrequenciaPorAlunoEAula_SeNaoEncontrar()
        {
            var codigoAluno = _faker.Random.AlphaNumeric(8);
            var aulaId = _faker.Random.Long(1);

            _obterAnotacaoFrequenciaAlunoUseCase.Setup(x => x.Executar(It.IsAny<FiltroAnotacaoFrequenciaAlunoDto>()))
                   .ReturnsAsync((AnotacaoFrequenciaAlunoDto)null);

            var resultado = await _controller.BuscarPorId(codigoAluno, aulaId, _obterAnotacaoFrequenciaAlunoUseCase.Object);

            Assert.IsType<NoContentResult>(resultado);
        }
        #endregion

        #region BuscaAnotacaoFrequenciaAlunoPorPeriodo

        [Fact(DisplayName = "Deve retornar anotação de frequência por período")]
        public async Task DeveRetornarAnotacaoFrequenciaAluno_PorAlunoEPeriodo()
        {
            var codigoAluno = _faker.Random.AlphaNumeric(8);
            var dataInicio = DateTime.Today.AddDays(-10);
            var dataFim = DateTime.Today;
            var anotacaoEsperada = new List<AnotacaoAlunoAulaPorPeriodoDto>
            {
                new AnotacaoAlunoAulaPorPeriodoDto()
            };

            _obterAnotacaoFrequenciaAlunoPorPeriodoUseCase
                .Setup(x => x.Executar(It.IsAny<FiltroAnotacaoFrequenciaAlunoPorPeriodoDto>()))
                .ReturnsAsync(anotacaoEsperada);

            var resultado = await _controller.ObterPorAlunoPorPeriodo(codigoAluno, dataInicio, dataFim, _obterAnotacaoFrequenciaAlunoPorPeriodoUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var resultadoDto = Assert.IsAssignableFrom<IEnumerable<AnotacaoAlunoAulaPorPeriodoDto>>(okResult.Value);
            Assert.Single(resultadoDto);
        }
        #endregion

        #region BuscaAnotacaoFrequenciaAlunoPorPeriodoNaoExiste

        [Fact(DisplayName = "Deve retornar NoContent quando anotação de frequência por período não existir")]
        public async Task DeveRetornarNoContent_ListaAnotacaoFrequenciaAluno_SeNaoEncontrar()
        {
            _obterAnotacaoFrequenciaAlunoPorPeriodoUseCase
                .Setup(x => x.Executar(It.IsAny<FiltroAnotacaoFrequenciaAlunoPorPeriodoDto>()))
                .ReturnsAsync((IEnumerable<AnotacaoAlunoAulaPorPeriodoDto>)null);

            var resultado = await _controller.ObterPorAlunoPorPeriodo("123456", DateTime.Now, DateTime.Now, _obterAnotacaoFrequenciaAlunoPorPeriodoUseCase.Object);

            Assert.IsType<NoContentResult>(resultado);
        }
        #endregion

        #region IncluirAnotacaoFrequenciaAluno

        [Fact(DisplayName = "Deve incluir anotação de frequência e retornar AuditoriaDto")]
        public async Task DeveIncluirAnotacaoFrequenciaAluno_RetornarAuditoriaDto_SeSucesso()
        {
            // Arrange
            var dto = new SalvarAnotacaoFrequenciaAlunoDto
            {
                AulaId = 1,
                CodigoAluno = _faker.Random.AlphaNumeric(8),
                ComponenteCurricularId = 10,
                EhInfantil = false,
                MotivoAusenciaId = 1,
            };

            var auditoriaEsperada = new AuditoriaDto
            {
                Id = _faker.Random.Long(1),
                CriadoEm = DateTime.Now,
                CriadoPor = "NomeUsuario",
                CriadoRF = _faker.Random.AlphaNumeric(8),
                AlteradoPor = "NomeUsuario",
                AlteradoRF = _faker.Random.AlphaNumeric(8),
            };

            _salvarAnotacaoFrequenciaAlunoUseCase
                .Setup(x => x.Executar(dto))
                .ReturnsAsync(auditoriaEsperada);

            // Act
            var resultado = await _controller.Salvar(dto, _salvarAnotacaoFrequenciaAlunoUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var auditoria = Assert.IsType<AuditoriaDto>(okResult.Value);

            Assert.Equal(auditoriaEsperada.CriadoRF, auditoria.CriadoRF);
        }
        #endregion

        #region AlterarAnotacaoFrequenciaAluno

        [Fact(DisplayName = "Deve alterar anotação de frequência")]
        public async Task DeveAlterar_AnotacaoFrequencia_RetornarTrue_SeSucesso()
        {
            // Arrange
            var id = _faker.Random.Long(1);
            var dto = new AlterarAnotacaoFrequenciaAlunoDto
            {
                Id = 1,
                Anotacao = "Teste"
            };

            _alterarAnotacaoFrequenciaAlunoUseCase
                .Setup(x => x.Executar(It.Is<AlterarAnotacaoFrequenciaAlunoDto>(x => x.Id == id)))
                .ReturnsAsync(true);

            // Act
            var resultado = await _controller.Alterar(id, dto, _alterarAnotacaoFrequenciaAlunoUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<bool>(okResult.Value);
            Assert.True(retorno);
        }
        #endregion

        #region ListarMotivosAusencia

        [Fact(DisplayName = "Deve listar motivos de ausência")]
        public async Task DeveRetornarLista_MotivosAusencia()
        {
            var motivos = new List<OpcaoDropdownDto>
            {
                new OpcaoDropdownDto("1", "Doença"),
                new OpcaoDropdownDto("2", "Compromisso familiar")
            };

            _obterMotivosAusenciaUseCase.Setup(x => x.Executar()).ReturnsAsync(motivos);

            var resultado = await _controller.ListarMotivos(_obterMotivosAusenciaUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsAssignableFrom<IEnumerable<OpcaoDropdownDto>>(okResult.Value);
            foreach (var esperado in motivos)
            {
                Assert.Contains(retorno, r => r.Valor == esperado.Valor && r.Descricao == esperado.Descricao);
            }
        }
        #endregion

        #region ExcluirAnotacaoFrequenciaAluno

        [Fact(DisplayName = "Deve excluir anotação de frequência")]
        public async Task DeveExcluir_AnotacaoFrequenciaComSucesso()
        {
            var id = _faker.Random.Long(1);

            _excluirAnotacaoFrequenciaAlunoUseCase.Setup(x => x.Executar(id)).ReturnsAsync(true);

            var resultado = await _controller.Excluir(id, _excluirAnotacaoFrequenciaAlunoUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var retorno = Assert.IsType<bool>(okResult.Value);
            Assert.True(retorno);
        }
        #endregion
    }
}
