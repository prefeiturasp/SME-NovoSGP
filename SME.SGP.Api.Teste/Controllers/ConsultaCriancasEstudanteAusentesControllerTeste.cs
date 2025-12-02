using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra.Dtos.ConsultaCriancasEstudantesAusentes;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class ConsultaCriancasEstudantesAusentesControllerTeste
    {
        private readonly Mock<IObterTurmasAlunosAusentesUseCase> _obterTurmasAlunosAusentesUseCase = new();
        private readonly ConsultaCriancasEstudanteAusentesController _controller;

        public ConsultaCriancasEstudantesAusentesControllerTeste()
        {
            _controller = new ConsultaCriancasEstudanteAusentesController();
        }

        [Fact(DisplayName = "ObterTurmasAlunosAusentes deve retornar Ok com lista de alunos ausentes")]
        public async Task ObterTurmasAlunosAusentes_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroObterAlunosAusentesDto();

            var retorno = new List<AlunosAusentesDto>
            {
                new AlunosAusentesDto()
            };

            _obterTurmasAlunosAusentesUseCase
                .Setup(x => x.Executar(filtro))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterTurmasAlunosAusentes(
                filtro,
                _obterTurmasAlunosAusentesUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var valor = Assert.IsType<List<AlunosAusentesDto>>(ok.Value);
            Assert.Single(valor);
        }
    }
}
