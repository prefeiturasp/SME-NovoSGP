using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class DiretoriaRegionalEducacaoControllerTeste
    {
        private readonly Mock<IConsultaDres> _consultaDresMock;
        private readonly DiretoriaRegionalEducacaoController _controller;

        public DiretoriaRegionalEducacaoControllerTeste()
        {
            _consultaDresMock = new Mock<IConsultaDres>();
            _controller = new DiretoriaRegionalEducacaoController(_consultaDresMock.Object);
        }

        [Fact(DisplayName = "ObterTodos deve retornar Ok com lista")]
        public async Task ObterTodos_DeveRetornarOk()
        {
            var retorno = new List<DreConsultaDto> { new DreConsultaDto() };

            _consultaDresMock
                .Setup(s => s.ObterTodos())
                .ReturnsAsync(retorno);

            var result = await _controller.Get();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<DreConsultaDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterEscolasSemAtribuicao deve retornar Ok com lista quando houver dados")]
        public async Task ObterEscolasSemAtribuicao_DeveRetornarOk()
        {
            var retorno = new List<UnidadeEscolarDto> { new UnidadeEscolarDto() };

            _consultaDresMock.Setup(s => s.ObterEscolasSemAtribuicao("D1", 1))
                             .ReturnsAsync(retorno);

            var result = await _controller.ObterEscolasSemAtribuicao("D1", 1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<UnidadeEscolarDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterEscolasSemAtribuicao deve retornar 204 quando não houver dados")]
        public async Task ObterEscolasSemAtribuicao_DeveRetornar204()
        {
            var retorno = new List<UnidadeEscolarDto>(); // vazio

            _consultaDresMock.Setup(s => s.ObterEscolasSemAtribuicao("D1", 1))
                             .ReturnsAsync(retorno);

            var result = await _controller.ObterEscolasSemAtribuicao("D1", 1);

            var noContent = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(204, noContent.StatusCode);
        }

        [Fact(DisplayName = "ObterUesPorDre deve retornar Ok com lista quando houver dados")]
        public async Task ObterUesPorDre_DeveRetornarOk()
        {
            var retorno = new List<UnidadeEscolarDto> { new UnidadeEscolarDto() };

            _consultaDresMock.Setup(s => s.ObterEscolasPorDre("D1"))
                             .ReturnsAsync(retorno);

            var result = await _controller.ObterUesPorDre("D1");

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<UnidadeEscolarDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterUesPorDre deve retornar 204 quando não houver dados")]
        public async Task ObterUesPorDre_DeveRetornar204()
        {
            var retorno = new List<UnidadeEscolarDto>();

            _consultaDresMock.Setup(s => s.ObterEscolasPorDre("D1"))
                             .ReturnsAsync(retorno);

            var result = await _controller.ObterUesPorDre("D1");

            var noContent = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(204, noContent.StatusCode);
        }
    }

}
