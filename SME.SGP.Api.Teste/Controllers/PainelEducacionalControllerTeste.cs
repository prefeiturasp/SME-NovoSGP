using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class PainelEducacionalControllerTeste
    {
        private readonly PainelEducacionalController _controller;
        private readonly Mock<IConsultasVisaoGeralPainelEducacionalUseCase> _consultasVisaoGeralPainelEducacionalUseCase = new();

        public PainelEducacionalControllerTeste()
        {
            _controller = new PainelEducacionalController();
        }

        [Fact]
        public async Task ObterVisaoGeral_DeveRetornarOkComDados()
        {
            // Arrange
            var codigoDre = "123";
            var codigoUe= "456";
            var anoLetivo = 2025;

            var retornoEsperado = new List<PainelEducacionalVisaoGeralRetornoDto>
            {
                new PainelEducacionalVisaoGeralRetornoDto
                {
                    Indicador = "IDEP",
                    Series = new List<PainelEducacionalSerieDto>
                    {
                        new PainelEducacionalSerieDto { Serie = "Anos iniciais", Valor = 53.00m },
                        new PainelEducacionalSerieDto { Serie = "Anos finais", Valor = 7.50m }
                    }
                },
                new PainelEducacionalVisaoGeralRetornoDto
                {
                    Indicador = "Frequência global",
                    Series = new List<PainelEducacionalSerieDto>
                    {
                        new PainelEducacionalSerieDto { Serie = "Geral", Valor = 72.94m }
                    }
                }
            };

            _consultasVisaoGeralPainelEducacionalUseCase
                .Setup(x => x.ObterVisaoGeralConsolidada(anoLetivo, codigoDre, codigoUe))
                .Returns(Task.FromResult<IEnumerable<PainelEducacionalVisaoGeralRetornoDto>>(retornoEsperado));

            // Act
            var result = await _controller.ObterVisaoGeral(anoLetivo, codigoDre, codigoUe, _consultasVisaoGeralPainelEducacionalUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<PainelEducacionalVisaoGeralRetornoDto>>(okResult.Value);

            Assert.Collection(retorno,
                idep =>
                {
                    Assert.Equal("IDEP", idep.Indicador);
                    Assert.Collection(idep.Series,
                        s =>
                        {
                            Assert.Equal("Anos iniciais", s.Serie);
                            Assert.Equal(53.00m, s.Valor);
                        },
                        s =>
                        {
                            Assert.Equal("Anos finais", s.Serie);
                            Assert.Equal(7.50m, s.Valor);
                        });
                },
                freq =>
                {
                    Assert.Equal("Frequência global", freq.Indicador);
                    Assert.Collection(freq.Series,
                        s =>
                        {
                            Assert.Equal("Geral", s.Serie);
                            Assert.Equal(72.94m, s.Valor);
                        });
                });
        }
    }
}
