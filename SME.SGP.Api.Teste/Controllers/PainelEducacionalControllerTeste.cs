using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class PainelEducacionalControllerTeste
    {
        private readonly PainelEducacionalController _controller;
        private readonly Mock<IConsultasVisaoGeralPainelEducacionalUseCase> _consultasVisaoGeralPainelEducacionalUseCase = new();
        private readonly Mock<IConsultasIdebPainelEducacionalUseCase> _consultasIdebPainelEducacionalUseCase = new();
        private readonly Mock<IConsultasPainelEducacionalFluenciaLeitoraUseCase> _consultasFluenciaLeitoraUseCase = new();
        private readonly Mock<IConsultasProficienciaIdebPainelEducacionalUseCase> _consultasProficienciaIdebUseCase = new();

        public PainelEducacionalControllerTeste()
        {
            _controller = new PainelEducacionalController();
        }

        [Fact]
        public async Task Obter_Visao_Geral_Deve_Retornar_Ok_Com_Dados()
        {
            var codigoDre = "123";
            var codigoUe = "456";
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

            var result = await _controller.ObterVisaoGeral(anoLetivo, codigoDre, codigoUe, _consultasVisaoGeralPainelEducacionalUseCase.Object);

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

        [Fact]
        public async Task Obter_Ideb_Deve_Retornar_Ok_Com_Dados()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                AnoLetivo = 2023,
                Serie = PainelEducacionalIdebSerie.AnosIniciais,
                CodigoDre = "123",
                CodigoUe = "456"
            };

            var retornoEsperado = new PainelEducacionalIdebAgrupamentoDto
            {
                AnoSolicitado = 2023,
                AnoUtilizado = 2023,
                AnoSolicitadoSemDados = false,
                Serie = "1",
                MediaGeral = 5.5,
                CodigoDre = "123",
                CodigoUe = "456",
                Distribuicao = new List<FaixaQuantidadeIdeb>
                {
                    new FaixaQuantidadeIdeb { Faixa = "5.0-6.0", Quantidade = 10 },
                    new FaixaQuantidadeIdeb { Faixa = "4.0-5.0", Quantidade = 8 }
                }
            };

            _consultasIdebPainelEducacionalUseCase
                .Setup(x => x.ObterIdeb(It.IsAny<FiltroPainelEducacionalIdeb>()))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterIdeb(filtro, _consultasIdebPainelEducacionalUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsType<PainelEducacionalIdebAgrupamentoDto>(okResult.Value);

            Assert.Equal(2023, retorno.AnoSolicitado);
            Assert.Equal(2023, retorno.AnoUtilizado);
            Assert.False(retorno.AnoSolicitadoSemDados);
            Assert.Equal("1", retorno.Serie);
            Assert.Equal(5.5, retorno.MediaGeral);
            Assert.Equal("123", retorno.CodigoDre);
            Assert.Equal("456", retorno.CodigoUe);
            Assert.Equal(2, retorno.Distribuicao.Count);

            Assert.Collection(retorno.Distribuicao,
                item =>
                {
                    Assert.Equal("5.0-6.0", item.Faixa);
                    Assert.Equal(10, item.Quantidade);
                },
                item =>
                {
                    Assert.Equal("4.0-5.0", item.Faixa);
                    Assert.Equal(8, item.Quantidade);
                });

            _consultasIdebPainelEducacionalUseCase.Verify(x => x.ObterIdeb(It.Is<FiltroPainelEducacionalIdeb>(f =>
                f.AnoLetivo == 2023 &&
                f.Serie == PainelEducacionalIdebSerie.AnosIniciais &&
                f.CodigoDre == "123" &&
                f.CodigoUe == "456"
            )), Times.Once);
        }

        [Fact]
        public async Task Obter_Ideb_Com_Ano_Letivo_Null_Deve_Retornar_Ok_Com_Dados()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                AnoLetivo = null,
                Serie = PainelEducacionalIdebSerie.AnosFinais,
                CodigoDre = "789",
                CodigoUe = "012"
            };

            var retornoEsperado = new PainelEducacionalIdebAgrupamentoDto
            {
                AnoSolicitado = -99,
                AnoUtilizado = 2022,
                AnoSolicitadoSemDados = true,
                Serie = "2",
                MediaGeral = 4.8,
                CodigoDre = "789",
                CodigoUe = "012",
                Distribuicao = new List<FaixaQuantidadeIdeb>
                {
                    new FaixaQuantidadeIdeb { Faixa = "4.0-5.0", Quantidade = 15 }
                }
            };

            _consultasIdebPainelEducacionalUseCase
                .Setup(x => x.ObterIdeb(It.IsAny<FiltroPainelEducacionalIdeb>()))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterIdeb(filtro, _consultasIdebPainelEducacionalUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsType<PainelEducacionalIdebAgrupamentoDto>(okResult.Value);

            Assert.Equal(-99, retorno.AnoSolicitado);
            Assert.Equal(2022, retorno.AnoUtilizado);
            Assert.True(retorno.AnoSolicitadoSemDados);
            Assert.Equal("2", retorno.Serie);
            Assert.Equal(4.8, retorno.MediaGeral);
            Assert.Equal("789", retorno.CodigoDre);
            Assert.Equal("012", retorno.CodigoUe);
            Assert.Single(retorno.Distribuicao);
            Assert.Equal("4.0-5.0", retorno.Distribuicao[0].Faixa);
            Assert.Equal(15, retorno.Distribuicao[0].Quantidade);
        }

        [Fact]
        public async Task Obter_Ideb_Com_Ensino_Medio_Deve_Retornar_Ok_Com_Dados()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                AnoLetivo = 2023,
                Serie = PainelEducacionalIdebSerie.EnsinoMedio,
                CodigoDre = "345",
                CodigoUe = "678"
            };

            var retornoEsperado = new PainelEducacionalIdebAgrupamentoDto
            {
                AnoSolicitado = 2023,
                AnoUtilizado = 2023,
                AnoSolicitadoSemDados = false,
                Serie = "3",
                MediaGeral = 6.2,
                CodigoDre = "345",
                CodigoUe = "678",
                Distribuicao = new List<FaixaQuantidadeIdeb>
                {
                    new FaixaQuantidadeIdeb { Faixa = "6.0-7.0", Quantidade = 20 },
                    new FaixaQuantidadeIdeb { Faixa = "5.0-6.0", Quantidade = 12 },
                    new FaixaQuantidadeIdeb { Faixa = "4.0-5.0", Quantidade = 5 }
                }
            };

            _consultasIdebPainelEducacionalUseCase
                .Setup(x => x.ObterIdeb(It.IsAny<FiltroPainelEducacionalIdeb>()))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterIdeb(filtro, _consultasIdebPainelEducacionalUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsType<PainelEducacionalIdebAgrupamentoDto>(okResult.Value);

            Assert.Equal(PainelEducacionalIdebSerie.EnsinoMedio, filtro.Serie);
            Assert.Equal(6.2, retorno.MediaGeral);
            Assert.Equal(3, retorno.Distribuicao.Count);
        }

        [Fact]
        public async Task Obter_Ideb_Sem_Dados_Deve_Retornar_Ok_Com_Dados_Vazios()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                AnoLetivo = 2020,
                Serie = PainelEducacionalIdebSerie.AnosIniciais,
                CodigoDre = "999",
                CodigoUe = "000"
            };

            var retornoEsperado = new PainelEducacionalIdebAgrupamentoDto
            {
                AnoSolicitado = 2020,
                AnoUtilizado = 2018,
                AnoSolicitadoSemDados = true,
                Serie = string.Empty,
                MediaGeral = 0,
                CodigoDre = "999",
                CodigoUe = "000",
                Distribuicao = new List<FaixaQuantidadeIdeb>()
            };

            _consultasIdebPainelEducacionalUseCase
                .Setup(x => x.ObterIdeb(It.IsAny<FiltroPainelEducacionalIdeb>()))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterIdeb(filtro, _consultasIdebPainelEducacionalUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsType<PainelEducacionalIdebAgrupamentoDto>(okResult.Value);

            Assert.Equal(2020, retorno.AnoSolicitado);
            Assert.Equal(2018, retorno.AnoUtilizado);
            Assert.True(retorno.AnoSolicitadoSemDados);
            Assert.Equal(string.Empty, retorno.Serie);
            Assert.Equal(0, retorno.MediaGeral);
            Assert.Empty(retorno.Distribuicao);
        }

        [Fact]
        public async Task Obter_Ideb_Com_Codigos_Nulos_Deve_Retornar_Ok_Com_Dados()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                AnoLetivo = 2023,
                Serie = PainelEducacionalIdebSerie.AnosIniciais,
                CodigoDre = null,
                CodigoUe = null
            };

            var retornoEsperado = new PainelEducacionalIdebAgrupamentoDto
            {
                AnoSolicitado = 2023,
                AnoUtilizado = 2023,
                AnoSolicitadoSemDados = false,
                Serie = "1",
                MediaGeral = 3.8,
                CodigoDre = null,
                CodigoUe = null,
                Distribuicao = new List<FaixaQuantidadeIdeb>
                {
                    new FaixaQuantidadeIdeb { Faixa = "3.0-4.0", Quantidade = 3 }
                }
            };

            _consultasIdebPainelEducacionalUseCase
                .Setup(x => x.ObterIdeb(It.IsAny<FiltroPainelEducacionalIdeb>()))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterIdeb(filtro, _consultasIdebPainelEducacionalUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsType<PainelEducacionalIdebAgrupamentoDto>(okResult.Value);

            Assert.Null(retorno.CodigoDre);
            Assert.Null(retorno.CodigoUe);
        }

        [Fact]
        public async Task Obter_Ideb_Deve_Passar_Filtro_Correto_Para_Use_Case()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                AnoLetivo = 2024,
                Serie = PainelEducacionalIdebSerie.AnosFinais,
                CodigoDre = "111",
                CodigoUe = "222"
            };

            var retornoEsperado = new PainelEducacionalIdebAgrupamentoDto
            {
                AnoSolicitado = 2024,
                AnoUtilizado = 2024,
                AnoSolicitadoSemDados = false,
                Serie = "2",
                MediaGeral = 5.0,
                CodigoDre = "111",
                CodigoUe = "222",
                Distribuicao = new List<FaixaQuantidadeIdeb>()
            };

            _consultasIdebPainelEducacionalUseCase
                .Setup(x => x.ObterIdeb(It.IsAny<FiltroPainelEducacionalIdeb>()))
                .ReturnsAsync(retornoEsperado);

            await _controller.ObterIdeb(filtro, _consultasIdebPainelEducacionalUseCase.Object);

            _consultasIdebPainelEducacionalUseCase.Verify(x => x.ObterIdeb(It.Is<FiltroPainelEducacionalIdeb>(f =>
                f == filtro
            )), Times.Once);
        }

        [Fact]
        public async Task Obter_Fluencia_Leitora_Deve_Retornar_Ok_Com_Dados()
        {
            var filtro = new FiltroPainelEducacionalAnoLetivoPeriodo
            {
                AnoLetivo = 2023,
                Periodo = 1,
                CodigoDre = "123",
                CodigoUe = "456"
            };

            var retornoEsperado = new List<PainelEducacionalFluenciaLeitoraDto>
            {
                new PainelEducacionalFluenciaLeitoraDto
                {
                    NomeFluencia = "Fluência 1",
                    DescricaoFluencia = "Não leu",
                    DreCodigo = "123",
                    Percentual = 15.75m,
                    QuantidadeAlunos = 50,
                    Ano = 2023,
                    Periodo = "1"
                },
                new PainelEducacionalFluenciaLeitoraDto
                {
                    NomeFluencia = "Fluência 2",
                    DescricaoFluencia = "Soletrou",
                    DreCodigo = "123",
                    Percentual = 25.30m,
                    QuantidadeAlunos = 80,
                    Ano = 2023,
                    Periodo = "1"
                }
            };

            _consultasFluenciaLeitoraUseCase
                .Setup(x => x.ObterFluenciaLeitora(filtro.Periodo, filtro.AnoLetivo, filtro.CodigoDre))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterFluenciaLeitora(filtro, _consultasFluenciaLeitoraUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<PainelEducacionalFluenciaLeitoraDto>>(okResult.Value);

            Assert.Collection(retorno,
                item =>
                {
                    Assert.Equal("Fluência 1", item.NomeFluencia);
                    Assert.Equal("Não leu", item.DescricaoFluencia);
                    Assert.Equal("123", item.DreCodigo);
                    Assert.Equal(15.75m, item.Percentual);
                    Assert.Equal(50, item.QuantidadeAlunos);
                    Assert.Equal(2023, item.Ano);
                    Assert.Equal("1", item.Periodo);
                },
                item =>
                {
                    Assert.Equal("Fluência 2", item.NomeFluencia);
                    Assert.Equal("Soletrou", item.DescricaoFluencia);
                    Assert.Equal("123", item.DreCodigo);
                    Assert.Equal(25.30m, item.Percentual);
                    Assert.Equal(80, item.QuantidadeAlunos);
                    Assert.Equal(2023, item.Ano);
                    Assert.Equal("1", item.Periodo);
                });

            _consultasFluenciaLeitoraUseCase.Verify(x => x.ObterFluenciaLeitora(
                It.Is<int>(p => p == 1),
                It.Is<int>(a => a == 2023),
                It.Is<string>(d => d == "123")
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Fluencia_Leitora_Sem_Codigo_Dre_Deve_Retornar_Ok_Com_Dados()
        {
            var filtro = new FiltroPainelEducacionalAnoLetivoPeriodo
            {
                AnoLetivo = 2023,
                Periodo = 2,
                CodigoDre = null,
                CodigoUe = null
            };

            var retornoEsperado = new List<PainelEducacionalFluenciaLeitoraDto>
            {
                new PainelEducacionalFluenciaLeitoraDto
                {
                    NomeFluencia = "Fluência 3",
                    DescricaoFluencia = "Silabou",
                    DreCodigo = "",
                    Percentual = 35.80m,
                    QuantidadeAlunos = 120,
                    Ano = 2023,
                    Periodo = "2"
                }
            };

            _consultasFluenciaLeitoraUseCase
                .Setup(x => x.ObterFluenciaLeitora(filtro.Periodo, filtro.AnoLetivo, filtro.CodigoDre))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterFluenciaLeitora(filtro, _consultasFluenciaLeitoraUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<PainelEducacionalFluenciaLeitoraDto>>(okResult.Value);

            Assert.Single(retorno);
            var item = retorno.First();
            Assert.Equal("Fluência 3", item.NomeFluencia);
            Assert.Equal("Silabou", item.DescricaoFluencia);
            Assert.Equal("", item.DreCodigo);
            Assert.Equal(35.80m, item.Percentual);
            Assert.Equal(120, item.QuantidadeAlunos);

            _consultasFluenciaLeitoraUseCase.Verify(x => x.ObterFluenciaLeitora(
                It.Is<int>(p => p == 2),
                It.Is<int>(a => a == 2023),
                It.Is<string>(d => d == null)
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Fluencia_Leitora_Com_Lista_Vazia_Deve_Retornar_Ok()
        {
            var filtro = new FiltroPainelEducacionalAnoLetivoPeriodo
            {
                AnoLetivo = 2020,
                Periodo = 3,
                CodigoDre = "999",
                CodigoUe = "000"
            };

            var retornoEsperado = new List<PainelEducacionalFluenciaLeitoraDto>();

            _consultasFluenciaLeitoraUseCase
                .Setup(x => x.ObterFluenciaLeitora(filtro.Periodo, filtro.AnoLetivo, filtro.CodigoDre))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterFluenciaLeitora(filtro, _consultasFluenciaLeitoraUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<PainelEducacionalFluenciaLeitoraDto>>(okResult.Value);

            Assert.Empty(retorno);

            _consultasFluenciaLeitoraUseCase.Verify(x => x.ObterFluenciaLeitora(
                It.Is<int>(p => p == 3),
                It.Is<int>(a => a == 2020),
                It.Is<string>(d => d == "999")
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Proficiencia_Ideb_Quando_Use_Case_Retorna_Dados_Deve_Retornar_Ok_Com_Conteudo_Correto()
        {
            var anoLetivo = 2025;
            var codigoUe = "ue-123";
            var dadosEsperados = new List<PainelEducacionalProficienciaIdepDto>
            {
                new PainelEducacionalProficienciaIdepDto
                {
                    AnoLetivo = anoLetivo,
                    PercentualInicial = 75,
                    PercentualFinal = 25,
                    Proficiencia = new ProficienciaIdebResumidoDto
                    {
                        AnosIniciais = new List<ComponenteCurricularIdebResumidoDto> { new ComponenteCurricularIdebResumidoDto { ComponenteCurricular = 1 } },
                        AnosFinais = new List<ComponenteCurricularIdebResumidoDto> { new ComponenteCurricularIdebResumidoDto { ComponenteCurricular = 2 } }
                    }
                }
            };

            _consultasProficienciaIdebUseCase.Setup(u => u.ObterProficienciaIdep(anoLetivo, codigoUe))
                .ReturnsAsync(dadosEsperados);

            var result = await _controller.ObterProficienciaIdep(anoLetivo, codigoUe, _consultasProficienciaIdebUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<PainelEducacionalProficienciaIdepDto>>(okResult.Value);
            var primeiroItem = retorno.First();

            Assert.NotNull(retorno);
            Assert.Single(retorno);
            Assert.Equal(anoLetivo, primeiroItem.AnoLetivo);
            Assert.Equal(75, primeiroItem.PercentualInicial);
            Assert.Equal(25, primeiroItem.PercentualFinal);
        }

        [Fact]
        public async Task Obter_Proficiencia_Ideb_Quando_Use_Case_Retorna_Lista_Vazia_Deve_Retornar_Ok_Com_Lista_Vazia()
        {
            var anoLetivo = 2025;
            var codigoUe = "ue-123";
            var dadosEsperados = new List<PainelEducacionalProficienciaIdepDto>();

            _consultasProficienciaIdebUseCase.Setup(u => u.ObterProficienciaIdep(anoLetivo, codigoUe))
                .ReturnsAsync(dadosEsperados);

            var result = await _controller.ObterProficienciaIdep(anoLetivo, codigoUe, _consultasProficienciaIdebUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<PainelEducacionalProficienciaIdepDto>>(okResult.Value);

            Assert.Empty(retorno);
        }

        [Fact]
        public async Task Obter_Proficiencia_Escola_Dados_Deve_Retornar_Ok_Com_Dados()
        {
            var codigoUe = "ue-456";
            var retornoEsperado = new PainelEducacionalProficienciaEscolaDadosDto
            {
                NomeUe = "Escola Exemplo",
                Diretor = "Diretor Exemplo",
                Telefone = "(11) 1234-5678",
                Email = "diretor@escolaexemplo.com",
                CodigoEol = "123456",
                CodigoInep = "654321"
            };

            var mockUseCase = new Mock<IConsultasProficienciaEscolaDadosUseCase>();
            mockUseCase
                .Setup(x => x.ObterProficienciaEscolaDados(codigoUe))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterProficienciaEscolaDados(codigoUe, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsType<PainelEducacionalProficienciaEscolaDadosDto>(okResult.Value);

            Assert.Equal("Escola Exemplo", retorno.NomeUe);
            Assert.Equal("Diretor Exemplo", retorno.Diretor);
            Assert.Equal("(11) 1234-5678", retorno.Telefone);
            Assert.Equal("diretor@escolaexemplo.com", retorno.Email);
            Assert.Equal("123456", retorno.CodigoEol);
            Assert.Equal("654321", retorno.CodigoInep);

            mockUseCase.Verify(x => x.ObterProficienciaEscolaDados(It.Is<string>(c => c == codigoUe)), Times.Once);
        }
    }
}