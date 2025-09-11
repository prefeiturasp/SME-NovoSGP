using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class ComponentesCurricularesControllerTeste
    {
        private readonly ComponentesCurricularesController _controller;
        private readonly Mock<IObterComponentesCurricularesPorAnoEscolarUseCase> _obterComponentesCurricularesPorAnoEscolarUseCase = new();
        private readonly Mock<IObterComponentesCurricularesRegenciaPorTurmaUseCase> _obterComponentesCurricularesRegenciaPorTurmaUseCase = new();
        private readonly Mock<IObterComponentesCurricularesPorTurmaECodigoUeUseCase> _obterComponentesCurricularesPorTurmaECodigoUeUseCase = new();

        public ComponentesCurricularesControllerTeste()
        {
            _controller = new ComponentesCurricularesController();
        }

        [Fact]
        public async Task ObterDresAtribuicoes_DeveRetornarOkComComponentes()
        {
            // Arrange
            var codigoUe = "UE123";
            var modalidade = Modalidade.Fundamental;
            var anoLetivo = 2025;
            var anosEscolares = new[] { "1", "2" };
            var turmasPrograma = true;

            var componentesMock = new List<ComponenteCurricularEol>
            {
                new ComponenteCurricularEol { Codigo = 123, Descricao = "Matemática" },
                new ComponenteCurricularEol { Codigo = 456, Descricao = "Português" }
            };

            _obterComponentesCurricularesPorAnoEscolarUseCase
                .Setup(x => x.Executar(codigoUe, modalidade, anoLetivo, anosEscolares, turmasPrograma))
                .ReturnsAsync(componentesMock);

            // Act
            var resultado = await _controller.ObterDresAtribuicoes(
                codigoUe, modalidade, anosEscolares, anoLetivo, turmasPrograma, _obterComponentesCurricularesPorAnoEscolarUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode ?? 200);
            Assert.IsAssignableFrom<IEnumerable<ComponenteCurricularEol>>(okResult.Value);
        }

        [Fact]
        public async Task Obter_DeveRetornarOkComBooleano()
        {
            // Arrange
            var ueId = "UE456";
            var turmas = new[] { "T1", "T2" };

            var retornoEsperado = new List<ComponenteCurricularDto>
            {
                new ComponenteCurricularDto { Codigo = "1", Descricao = "Matemática" },
                new ComponenteCurricularDto { Codigo = "2", Descricao = "Português" }
            };

            _obterComponentesCurricularesPorTurmaECodigoUeUseCase
                .Setup(x => x.Executar(It.Is<FiltroComponentesCurricularesPorTurmaECodigoUeDto>(
                    f => f.CodigoUe == ueId && f.CodigosDeTurmas.SequenceEqual(turmas))))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _controller.Obter(ueId, turmas, _obterComponentesCurricularesPorTurmaECodigoUeUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode ?? 200);
            Assert.IsAssignableFrom<IEnumerable<ComponenteCurricularDto>>(okResult.Value);
            var retorno = okResult.Value as IEnumerable<ComponenteCurricularDto>;
            Assert.Equal(2, retorno.Count());
        }

        [Fact]
        public async Task ObterComponentesCurricularesRegencia_DeveRetornarOkComDisciplinas()
        {
            // Arrange
            long turmaId = 789;
            var disciplinas = new List<DisciplinaDto>
            {
                new DisciplinaDto { Id = 1, Nome = "História" },
                new DisciplinaDto { Id = 2, Nome = "Geografia" }
            };

            _obterComponentesCurricularesRegenciaPorTurmaUseCase
                .Setup(x => x.Executar(turmaId))
                .ReturnsAsync(disciplinas);

            // Act
            var resultado = await _controller.ObterComponentesCurricularesRegencia(turmaId, _obterComponentesCurricularesRegenciaPorTurmaUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode ?? 200);
            Assert.IsAssignableFrom<IEnumerable<DisciplinaDto>>(okResult.Value);
        }



    }
}
