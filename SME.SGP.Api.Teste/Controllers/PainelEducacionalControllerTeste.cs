using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaDiaria;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using SME.SGP.Infra.Dtos.PainelEducacional.Reclassificacao;
using SME.SGP.Infra.Dtos.PainelEducacional.SondagemEscrita;
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
        private readonly Mock<IConsultasDistorcaoIdadeUseCase> _consultasDistorcaoIdadeUseCase = new();
        private readonly Mock<IConsultasProficienciaIdepPainelEducacionalUseCase> _consultasProficienciaIdepUseCase = new();
        private readonly Mock<IConsultasRegistroFrequenciaDiariaDreUseCase> _consultasRegistroFrequenciaDiariaDreUseCase = new();
        private readonly Mock<IConsultasRegistroFrequenciaDiariaUeUseCase> _consultasRegistroFrequenciaDiariaUeUseCase = new();

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
                    Proficiencia = new ProficienciaIdepResumidoDto
                    {
                        AnosIniciais = new List<ComponenteCurricularIdepResumidoDto> { new ComponenteCurricularIdepResumidoDto { ComponenteCurricular = Dominio.Enumerados.ComponenteCurricular.Portugues.GetDisplayName() } },
                        AnosFinais = new List<ComponenteCurricularIdepResumidoDto> { new ComponenteCurricularIdepResumidoDto { ComponenteCurricular = Dominio.Enumerados.ComponenteCurricular.Matematica.GetDisplayName() } }
                    }
                }
            };

            _consultasProficienciaIdepUseCase.Setup(u => u.ObterProficienciaIdep(anoLetivo, codigoUe))
                .ReturnsAsync(dadosEsperados);

            var result = await _controller.ObterProficienciaIdep(anoLetivo, codigoUe, _consultasProficienciaIdepUseCase.Object);

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

            _consultasProficienciaIdepUseCase.Setup(u => u.ObterProficienciaIdep(anoLetivo, codigoUe))
                .ReturnsAsync(dadosEsperados);

            var result = await _controller.ObterProficienciaIdep(anoLetivo, codigoUe, _consultasProficienciaIdepUseCase.Object);

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

        [Fact]
        public async Task Obter_Sondagem_Escrita_Deve_Retornar_Ok_Com_Dados()
        {
            var filtro = new FiltroPainelEducacionalAnoLetivoBimestre
            {
                AnoLetivo = 2025,
                Bimestre = 1,
                CodigoDre = "123",
                CodigoUe = "456",
                SerieAno = 3
            };

            var retornoEsperado = new List<SondagemEscritaDto>
            {
                new SondagemEscritaDto
                {
                    CodigoDre = "123",
                    CodigoUe = "456",
                    AnoLetivo = 2025,
                    Bimestre = 1,
                    SerieAno = 3,
                    QuantidadeAlunos = 30
                },
                new SondagemEscritaDto
                {
                    CodigoDre = "123",
                    CodigoUe = "456",
                    AnoLetivo = 2025,
                    Bimestre = 1,
                    SerieAno = 3,
                    QuantidadeAlunos = 25
                }
            };

            var mockUseCase = new Mock<IConsultasSondagemEscritaUseCase>();
            mockUseCase
                .Setup(x => x.ObterSondagemEscrita(filtro.CodigoDre, filtro.CodigoUe, filtro.AnoLetivo, filtro.Bimestre, filtro.SerieAno))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterSondagemEscrita(filtro, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<SondagemEscritaDto>>(okResult.Value);

            Assert.Collection(retorno,
                item =>
                {
                    Assert.Equal("123", item.CodigoDre);
                    Assert.Equal("456", item.CodigoUe);
                    Assert.Equal(2025, item.AnoLetivo);
                    Assert.Equal(1, item.Bimestre);
                    Assert.Equal(3, item.SerieAno);
                    Assert.Equal(30, item.QuantidadeAlunos);
                },
                item =>
                {
                    Assert.Equal("123", item.CodigoDre);
                    Assert.Equal("456", item.CodigoUe);
                    Assert.Equal(2025, item.AnoLetivo);
                    Assert.Equal(1, item.Bimestre);
                    Assert.Equal(3, item.SerieAno);
                    Assert.Equal(25, item.QuantidadeAlunos);
                });

            mockUseCase.Verify(x => x.ObterSondagemEscrita(
                It.Is<string>(d => d == filtro.CodigoDre),
                It.Is<string>(u => u == filtro.CodigoUe),
                It.Is<int>(a => a == filtro.AnoLetivo),
                It.Is<int>(p => p == filtro.Bimestre),
                It.Is<int>(s => s == filtro.SerieAno)
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Indicadores_Pap_Deve_Retornar_Ok_Com_Dados()
        {
            // Arrange
            var anoLetivo = 2024;
            var codigoDre = "DRE123";
            var codigoUe = "UE456";
            var retornoEsperado = new IndicadoresPapDto
            {
                NomeDificuldadeTop1 = "Dificuldade 1",
                NomeDificuldadeTop2 = "Dificuldade 2",
                QuantidadesPorTipoPap = new List<IndicadoresPapQuantidadesPorTipoDto>
                {
                    new IndicadoresPapQuantidadesPorTipoDto
                    {
                        TipoPap = TipoPap.Pap2Ano,
                        TotalAlunos = 10,
                        TotalAlunosComFrequenciaInferiorLimite = 2,
                        TotalAlunosDificuldadeOutras = 1,
                        TotalAlunosDificuldadeTop1 = 5,
                        TotalAlunosDificuldadeTop2 = 2,
                        TotalTurmas = 3
                    },
                    new IndicadoresPapQuantidadesPorTipoDto
                    {
                        TipoPap = TipoPap.PapColaborativo,
                        TotalAlunos = 20,
                        TotalAlunosComFrequenciaInferiorLimite = 12,
                        TotalAlunosDificuldadeOutras = 11,
                        TotalAlunosDificuldadeTop1 = 5,
                        TotalAlunosDificuldadeTop2 = 11,
                        TotalTurmas = 13
                    },
                    new IndicadoresPapQuantidadesPorTipoDto
                    {
                        TipoPap = TipoPap.RecuperacaoAprendizagens,
                        TotalAlunos = 22,
                        TotalAlunosComFrequenciaInferiorLimite = 10,
                        TotalAlunosDificuldadeOutras = 10,
                        TotalAlunosDificuldadeTop1 = 5,
                        TotalAlunosDificuldadeTop2 = 1,
                        TotalTurmas = 14
                    }
                },
            };
            var mockUseCase = new Mock<IConsultasInformacoesPapUseCase>();
            mockUseCase
                .Setup(x => x.ObterInformacoesPap(anoLetivo, codigoDre, codigoUe))
                .ReturnsAsync(retornoEsperado);

            // Act
            var result = await _controller.ObterIndicadoresPap(mockUseCase.Object, anoLetivo, codigoDre, codigoUe);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsType<IndicadoresPapDto>(okResult.Value);

            // Assert
            mockUseCase.Verify(x => x.ObterInformacoesPap(
                It.Is<int>(a => a == anoLetivo),
                It.Is<string>(d => d == codigoDre),
                It.Is<string>(u => u == codigoUe)
            ), Times.Once);

            Assert.Equal("Dificuldade 1", retorno.NomeDificuldadeTop1);
            Assert.Equal("Dificuldade 2", retorno.NomeDificuldadeTop2);
            Assert.Equal(3, retorno.QuantidadesPorTipoPap.Count);
            Assert.Collection(retorno.QuantidadesPorTipoPap,
                item =>
                {
                    Assert.Equal(TipoPap.Pap2Ano, item.TipoPap);
                    Assert.Equal(10, item.TotalAlunos);
                    Assert.Equal(2, item.TotalAlunosComFrequenciaInferiorLimite);
                    Assert.Equal(1, item.TotalAlunosDificuldadeOutras);
                    Assert.Equal(5, item.TotalAlunosDificuldadeTop1);
                    Assert.Equal(2, item.TotalAlunosDificuldadeTop2);
                    Assert.Equal(3, item.TotalTurmas);
                },
                item =>
                {
                    Assert.Equal(TipoPap.PapColaborativo, item.TipoPap);
                    Assert.Equal(20, item.TotalAlunos);
                    Assert.Equal(12, item.TotalAlunosComFrequenciaInferiorLimite);
                    Assert.Equal(11, item.TotalAlunosDificuldadeOutras);
                    Assert.Equal(5, item.TotalAlunosDificuldadeTop1);
                    Assert.Equal(11, item.TotalAlunosDificuldadeTop2);
                    Assert.Equal(13, item.TotalTurmas);
                },
                item =>
                {
                    Assert.Equal(TipoPap.RecuperacaoAprendizagens, item.TipoPap);
                    Assert.Equal(22, item.TotalAlunos);
                    Assert.Equal(10, item.TotalAlunosComFrequenciaInferiorLimite);
                    Assert.Equal(10, item.TotalAlunosDificuldadeOutras);
                    Assert.Equal(5, item.TotalAlunosDificuldadeTop1);
                    Assert.Equal(1, item.TotalAlunosDificuldadeTop2);
                    Assert.Equal(14, item.TotalTurmas);
                });
        }

        [Fact]
        public async Task Obter_Notas_Visao_Sme_Dre_Deve_Retornar_Ok_Com_Dados()
        {
            var filtro = new FiltroPainelEducacionalNotasVisaoSmeDre
            {
                AnoLetivo = 2024,
                Bimestre = 2,
                CodigoDre = "DRE123",
                SerieAno = "5"
            };

            var retornoEsperado = new List<PainelEducacionalNotasVisaoSmeDreDto>
            {
                new PainelEducacionalNotasVisaoSmeDreDto
                {
                    Modalidades = new List<ModalidadeNotasVisaoSmeDreDto>
                    {
                        new ModalidadeNotasVisaoSmeDreDto
                        {
                            Nome = "Fundamental",
                            SerieAno = new List<SerieAnoNotasVisaoSmeDreDto>
                            {
                                new SerieAnoNotasVisaoSmeDreDto
                                {
                                    Nome = "5º",
                                    ComponentesCurriculares = new List<ComponenteCurricularNotasDto>
                                    {
                                        new ComponenteCurricularNotasDto
                                        {
                                            Nome = "Português",
                                            AbaixoDaMedia = 15,
                                            AcimaDaMedia = 35
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var mockUseCase = new Mock<IConsultasNotasVisaoSmeDreUseCase>();
            mockUseCase
                .Setup(x => x.ObterNotasVisaoSmeDre(filtro.CodigoDre, filtro.AnoLetivo, filtro.Bimestre, filtro.SerieAno))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterNotasVisaoSmeDre(filtro, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>>(okResult.Value);

            Assert.Single(retorno);
            var item = retorno.First();
            Assert.NotNull(item.Modalidades);
            Assert.Single(item.Modalidades);

            var modalidade = item.Modalidades.First();
            Assert.Equal("Fundamental", modalidade.Nome);
            Assert.NotNull(modalidade.SerieAno);
            Assert.Single(modalidade.SerieAno);

            mockUseCase.Verify(x => x.ObterNotasVisaoSmeDre(
                It.Is<string>(d => d == filtro.CodigoDre),
                It.Is<int>(a => a == filtro.AnoLetivo),
                It.Is<int>(b => b == filtro.Bimestre),
                It.Is<string>(s => s == filtro.SerieAno)
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Notas_Visao_Sme_Dre_Com_Lista_Vazia_Deve_Retornar_Ok()
        {
            var filtro = new FiltroPainelEducacionalNotasVisaoSmeDre
            {
                AnoLetivo = 2023,
                Bimestre = 1,
                CodigoDre = "DRE999",
                SerieAno = "3"
            };

            var retornoEsperado = new List<PainelEducacionalNotasVisaoSmeDreDto>();

            var mockUseCase = new Mock<IConsultasNotasVisaoSmeDreUseCase>();
            mockUseCase
                .Setup(x => x.ObterNotasVisaoSmeDre(filtro.CodigoDre, filtro.AnoLetivo, filtro.Bimestre, filtro.SerieAno))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterNotasVisaoSmeDre(filtro, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>>(okResult.Value);

            Assert.Empty(retorno);

            mockUseCase.Verify(x => x.ObterNotasVisaoSmeDre(
                It.Is<string>(d => d == filtro.CodigoDre),
                It.Is<int>(a => a == filtro.AnoLetivo),
                It.Is<int>(b => b == filtro.Bimestre),
                It.Is<string>(s => s == filtro.SerieAno)
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Notas_Visao_Sme_Dre_Com_Codigo_Dre_Nulo_Deve_Retornar_Ok_Com_Dados()
        {
            var filtro = new FiltroPainelEducacionalNotasVisaoSmeDre
            {
                AnoLetivo = 2024,
                Bimestre = 3,
                CodigoDre = null,
                SerieAno = "7"
            };

            var retornoEsperado = new List<PainelEducacionalNotasVisaoSmeDreDto>
            {
                new PainelEducacionalNotasVisaoSmeDreDto
                {
                    Modalidades = new List<ModalidadeNotasVisaoSmeDreDto>
                    {
                        new ModalidadeNotasVisaoSmeDreDto
                        {
                            Nome = "Fundamental",
                            SerieAno = new List<SerieAnoNotasVisaoSmeDreDto>
                            {
                                new SerieAnoNotasVisaoSmeDreDto
                                {
                                    Nome = "7º",
                                    ComponentesCurriculares = new List<ComponenteCurricularNotasDto>
                                    {
                                        new ComponenteCurricularNotasDto
                                        {
                                            Nome = "Português",
                                            AbaixoDaMedia = 25,
                                            AcimaDaMedia = 75
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var mockUseCase = new Mock<IConsultasNotasVisaoSmeDreUseCase>();
            mockUseCase
                .Setup(x => x.ObterNotasVisaoSmeDre(filtro.CodigoDre, filtro.AnoLetivo, filtro.Bimestre, filtro.SerieAno))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterNotasVisaoSmeDre(filtro, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>>(okResult.Value);

            Assert.Single(retorno);
            var item = retorno.First();
            Assert.NotNull(item.Modalidades);
            Assert.Single(item.Modalidades);

            var modalidade = item.Modalidades.First();
            Assert.Equal("Fundamental", modalidade.Nome);
            Assert.NotNull(modalidade.SerieAno);
            Assert.Single(modalidade.SerieAno);

            var serieAno = modalidade.SerieAno.First();
            Assert.Equal("7º", serieAno.Nome);

            mockUseCase.Verify(x => x.ObterNotasVisaoSmeDre(
                It.Is<string>(d => d == null),
                It.Is<int>(a => a == 2024),
                It.Is<int>(b => b == 3),
                It.Is<string>(s => s == "7")
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Notas_Visao_Ue_Deve_Retornar_Ok_Com_Dados_Paginados()
        {
            var filtro = new FiltroPainelEducacionalNotasVisaoUe
            {
                AnoLetivo = 2024,
                Bimestre = 2,
                CodigoUe = "UE123",
                Modalidade = Modalidade.Fundamental
            };

            var retornoEsperado = new PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>
            {
                Items = new List<PainelEducacionalNotasVisaoUeDto>
                {
                    new PainelEducacionalNotasVisaoUeDto
                    {
                        Modalidades = new List<ModalidadeNotasVisaoUeDto>
                        {
                            new ModalidadeNotasVisaoUeDto
                            {
                                Nome = "Ensino Fundamental",
                                Turmas = new List<TurmaNotasVisaoUeDto>
                                {
                                    new TurmaNotasVisaoUeDto
                                    {
                                        Nome = "5º Ano",
                                        ComponentesCurriculares = new List<ComponenteCurricularNotasDto>
                                        {
                                            new ComponenteCurricularNotasDto
                                            {
                                                Nome = "Português",
                                                AbaixoDaMedia = 25,
                                                AcimaDaMedia = 75
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                TotalRegistros = 3,
                TotalPaginas = 1
            };

            var mockUseCase = new Mock<IConsultasNotasVisaoUeUseCase>();
            mockUseCase
                .Setup(x => x.ObterNotasVisaoUe(filtro.CodigoUe, filtro.AnoLetivo, filtro.Bimestre, filtro.Modalidade))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterNotasVisaoUe(filtro, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsType<PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>>(okResult.Value);

            Assert.Equal(3, retorno.TotalRegistros);
            Assert.Equal(1, retorno.TotalPaginas);
            Assert.Single(retorno.Items);

            var item = retorno.Items.First();
            Assert.NotNull(item.Modalidades);
            Assert.Single(item.Modalidades);

            mockUseCase.Verify(x => x.ObterNotasVisaoUe(
                It.Is<string>(c => c == filtro.CodigoUe),
                It.Is<int>(a => a == filtro.AnoLetivo),
                It.Is<int>(b => b == filtro.Bimestre),
                It.Is<Modalidade>(m => m == filtro.Modalidade)
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Notas_Visao_Ue_Com_Multiplas_Modalidades_Deve_Retornar_Ok_Com_Paginacao()
        {
            var filtro = new FiltroPainelEducacionalNotasVisaoUe
            {
                AnoLetivo = 2024,
                Bimestre = 1,
                CodigoUe = "UE456",
                Modalidade = Modalidade.Fundamental
            };

            var retornoEsperado = new PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>
            {
                Items = new List<PainelEducacionalNotasVisaoUeDto>
                {
                    new PainelEducacionalNotasVisaoUeDto
                    {
                        Modalidades = new List<ModalidadeNotasVisaoUeDto>
                        {
                            new ModalidadeNotasVisaoUeDto
                            {
                                Nome = "Ensino Fundamental",
                                Turmas = new List<TurmaNotasVisaoUeDto>
                                {
                                    new TurmaNotasVisaoUeDto
                                    {
                                        Nome = "1º Ano",
                                        ComponentesCurriculares = new List<ComponenteCurricularNotasDto>
                                        {
                                            new ComponenteCurricularNotasDto
                                            {
                                                Nome = "Português",
                                                AbaixoDaMedia = 15,
                                                AcimaDaMedia = 85
                                            },
                                            new ComponenteCurricularNotasDto
                                            {
                                                Nome = "Matemática",
                                                AbaixoDaMedia = 18,
                                                AcimaDaMedia = 82
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new PainelEducacionalNotasVisaoUeDto
                    {
                        Modalidades = new List<ModalidadeNotasVisaoUeDto>
                        {
                            new ModalidadeNotasVisaoUeDto
                            {
                                Nome = "Ensino Médio",
                                Turmas = new List<TurmaNotasVisaoUeDto>
                                {
                                    new TurmaNotasVisaoUeDto
                                    {
                                        Nome = "1ª Série",
                                        ComponentesCurriculares = new List<ComponenteCurricularNotasDto>
                                        {
                                            new ComponenteCurricularNotasDto
                                            {
                                                Nome = "Português",
                                                AbaixoDaMedia = 45,
                                                AcimaDaMedia = 55
                                            },
                                            new ComponenteCurricularNotasDto
                                            {
                                                Nome = "Matemática",
                                                AbaixoDaMedia = 50,
                                                AcimaDaMedia = 50
                                            },
                                            new ComponenteCurricularNotasDto
                                            {
                                                Nome = "Ciências",
                                                AbaixoDaMedia = 42,
                                                AcimaDaMedia = 58
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                TotalRegistros = 2,
                TotalPaginas = 1
            };

            var mockUseCase = new Mock<IConsultasNotasVisaoUeUseCase>();
            mockUseCase
                .Setup(x => x.ObterNotasVisaoUe(filtro.CodigoUe, filtro.AnoLetivo, filtro.Bimestre, filtro.Modalidade))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterNotasVisaoUe(filtro, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsType<PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>>(okResult.Value);

            Assert.Equal(2, retorno.TotalRegistros);
            Assert.Equal(1, retorno.TotalPaginas);
            Assert.Equal(2, retorno.Items.Count());

            var primeiroItem = retorno.Items.First();
            var segundoItem = retorno.Items.Last();

            Assert.Equal("Ensino Fundamental", primeiroItem.Modalidades.First().Nome);
            Assert.Equal("Ensino Médio", segundoItem.Modalidades.First().Nome);

            mockUseCase.Verify(x => x.ObterNotasVisaoUe(
                It.Is<string>(c => c == "UE456"),
                It.Is<int>(a => a == 2024),
                It.Is<int>(b => b == 1),
                It.Is<Modalidade>(m => m == Modalidade.Fundamental)
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Notas_Visao_Ue_Com_Lista_Vazia_Deve_Retornar_Ok_Com_Paginacao_Vazia()
        {
            var filtro = new FiltroPainelEducacionalNotasVisaoUe
            {
                AnoLetivo = 2023,
                Bimestre = 4,
                CodigoUe = "UE999",
                Modalidade = Modalidade.EJA
            };

            var retornoEsperado = new PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>
            {
                Items = new List<PainelEducacionalNotasVisaoUeDto>(),
                TotalRegistros = 0,
                TotalPaginas = 0
            };

            var mockUseCase = new Mock<IConsultasNotasVisaoUeUseCase>();
            mockUseCase
                .Setup(x => x.ObterNotasVisaoUe(filtro.CodigoUe, filtro.AnoLetivo, filtro.Bimestre, filtro.Modalidade))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterNotasVisaoUe(filtro, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsType<PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>>(okResult.Value);

            Assert.Equal(0, retorno.TotalRegistros);
            Assert.Equal(0, retorno.TotalPaginas);
            Assert.Empty(retorno.Items);

            mockUseCase.Verify(x => x.ObterNotasVisaoUe(
                It.Is<string>(c => c == "UE999"),
                It.Is<int>(a => a == 2023),
                It.Is<int>(b => b == 4),
                It.Is<Modalidade>(m => m == Modalidade.EJA)
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Notas_Visao_Ue_Com_Codigo_Ue_Nulo_Deve_Retornar_Ok_Com_Dados()
        {
            var filtro = new FiltroPainelEducacionalNotasVisaoUe
            {
                AnoLetivo = 2024,
                Bimestre = 3,
                CodigoUe = null,
                Modalidade = Modalidade.Medio
            };

            var retornoEsperado = new PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>
            {
                Items = new List<PainelEducacionalNotasVisaoUeDto>
                {
                    new PainelEducacionalNotasVisaoUeDto
                    {
                        Modalidades = new List<ModalidadeNotasVisaoUeDto>
                        {
                            new ModalidadeNotasVisaoUeDto
                            {
                                Nome = "Ensino Médio",
                                Turmas = new List<TurmaNotasVisaoUeDto>
                                {
                                    new TurmaNotasVisaoUeDto
                                    {
                                        Nome = "3ª Série",
                                        ComponentesCurriculares = new List<ComponenteCurricularNotasDto>
                                        {
                                            new ComponenteCurricularNotasDto
                                            {
                                                Nome = "Português",
                                                AbaixoDaMedia = 60,
                                                AcimaDaMedia = 40
                                            },
                                            new ComponenteCurricularNotasDto
                                            {
                                                Nome = "Matemática",
                                                AbaixoDaMedia = 65,
                                                AcimaDaMedia = 35
                                            },
                                            new ComponenteCurricularNotasDto
                                            {
                                                Nome = "Ciências",
                                                AbaixoDaMedia = 55,
                                                AcimaDaMedia = 45
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                TotalRegistros = 1,
                TotalPaginas = 1
            };

            var mockUseCase = new Mock<IConsultasNotasVisaoUeUseCase>();
            mockUseCase
                .Setup(x => x.ObterNotasVisaoUe(filtro.CodigoUe, filtro.AnoLetivo, filtro.Bimestre, filtro.Modalidade))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterNotasVisaoUe(filtro, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsType<PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>>(okResult.Value);

            Assert.Equal(1, retorno.TotalRegistros);
            Assert.Equal(1, retorno.TotalPaginas);
            Assert.Single(retorno.Items);

            var item = retorno.Items.First();
            var modalidade = item.Modalidades.First();
            Assert.Equal("Ensino Médio", modalidade.Nome);

            var turma = modalidade.Turmas.First();
            Assert.Equal("3ª Série", turma.Nome);

            mockUseCase.Verify(x => x.ObterNotasVisaoUe(
                It.Is<string>(c => c == null),
                It.Is<int>(a => a == 2024),
                It.Is<int>(b => b == 3),
                It.Is<Modalidade>(m => m == Modalidade.Medio)
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Notas_Visao_Ue_Com_Paginacao_Multiplas_Paginas_Deve_Retornar_Ok()
        {
            var filtro = new FiltroPainelEducacionalNotasVisaoUe
            {
                AnoLetivo = 2024,
                Bimestre = 2,
                CodigoUe = "UE123",
                Modalidade = Modalidade.Fundamental
            };

            var retornoEsperado = new PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>
            {
                Items = new List<PainelEducacionalNotasVisaoUeDto>
                {
                    new PainelEducacionalNotasVisaoUeDto
                    {
                        Modalidades = new List<ModalidadeNotasVisaoUeDto>
                        {
                            new ModalidadeNotasVisaoUeDto
                            {
                                Nome = "Ensino Fundamental",
                                Turmas = new List<TurmaNotasVisaoUeDto>
                                {
                                    new TurmaNotasVisaoUeDto
                                    {
                                        Nome = "2º Ano",
                                        ComponentesCurriculares = new List<ComponenteCurricularNotasDto>
                                        {
                                            new ComponenteCurricularNotasDto
                                            {
                                                Nome = "Português",
                                                AbaixoDaMedia = 22,
                                                AcimaDaMedia = 78
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                TotalRegistros = 15,
                TotalPaginas = 5
            };

            var mockUseCase = new Mock<IConsultasNotasVisaoUeUseCase>();
            mockUseCase
                .Setup(x => x.ObterNotasVisaoUe(filtro.CodigoUe, filtro.AnoLetivo, filtro.Bimestre, filtro.Modalidade))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterNotasVisaoUe(filtro, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsType<PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>>(okResult.Value);

            Assert.Equal(15, retorno.TotalRegistros);
            Assert.Equal(5, retorno.TotalPaginas);
            Assert.Single(retorno.Items);

            mockUseCase.Verify(x => x.ObterNotasVisaoUe(
                It.Is<string>(c => c == filtro.CodigoUe),
                It.Is<int>(a => a == filtro.AnoLetivo),
                It.Is<int>(b => b == filtro.Bimestre),
                It.Is<Modalidade>(m => m == filtro.Modalidade)
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Reclassificacao_Deve_Retornar_Ok_Com_Dados()
        {
            var filtro = new FiltroPainelEducacionalReclassificacao
            {
                AnoLetivo = 2024,
                CodigoDre = "DRE123",
                CodigoUe = "UE456",
                AnoTurma = 5
            };

            var retornoEsperado = new List<PainelEducacionalReclassificacaoDto>
     {
         new PainelEducacionalReclassificacaoDto
         {
             Modalidade = new List<ModalidadeReclassificacaoDto>
             {
                 new ModalidadeReclassificacaoDto
                 {
                     Nome = "Ensino Fundamental",
                     AnoTurma = 5,
                     QuantidadeAlunos = 15
                 },
                 new ModalidadeReclassificacaoDto
                 {
                     Nome = "Ensino Fundamental",
                     AnoTurma = 6,
                     QuantidadeAlunos = 8
                 }
             }
         }
     };

            var mockUseCase = new Mock<IConsultasReclassificacaoPainelEducacionalUseCase>();
            mockUseCase
                .Setup(x => x.ObterReclassificacao(filtro.CodigoDre, filtro.CodigoUe, filtro.AnoLetivo, filtro.AnoTurma))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterReclassificacao(filtro, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<PainelEducacionalReclassificacaoDto>>(okResult.Value);

            Assert.Single(retorno);
            var item = retorno.First();
            Assert.NotNull(item.Modalidade);
            Assert.Equal(2, item.Modalidade.Count());

            Assert.Collection(item.Modalidade,
                modalidade =>
                {
                    Assert.Equal("Ensino Fundamental", modalidade.Nome);
                    Assert.Equal(5, modalidade.AnoTurma);
                    Assert.Equal(15, modalidade.QuantidadeAlunos);
                },
                modalidade =>
                {
                    Assert.Equal("Ensino Fundamental", modalidade.Nome);
                    Assert.Equal(6, modalidade.AnoTurma);
                    Assert.Equal(8, modalidade.QuantidadeAlunos);
                });

            mockUseCase.Verify(x => x.ObterReclassificacao(
                It.Is<string>(d => d == filtro.CodigoDre),
                It.Is<string>(u => u == filtro.CodigoUe),
                It.Is<int>(a => a == filtro.AnoLetivo),
                It.Is<int>(t => t == filtro.AnoTurma)
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Reclassificacao_Com_Lista_Vazia_Deve_Retornar_Ok()
        {
            var filtro = new FiltroPainelEducacionalReclassificacao
            {
                AnoLetivo = 2023,
                CodigoDre = "DRE999",
                CodigoUe = "UE000",
                AnoTurma = 3
            };

            var retornoEsperado = new List<PainelEducacionalReclassificacaoDto>();

            var mockUseCase = new Mock<IConsultasReclassificacaoPainelEducacionalUseCase>();
            mockUseCase
                .Setup(x => x.ObterReclassificacao(filtro.CodigoDre, filtro.CodigoUe, filtro.AnoLetivo, filtro.AnoTurma))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterReclassificacao(filtro, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<PainelEducacionalReclassificacaoDto>>(okResult.Value);

            Assert.Empty(retorno);

            mockUseCase.Verify(x => x.ObterReclassificacao(
                It.Is<string>(d => d == filtro.CodigoDre),
                It.Is<string>(u => u == filtro.CodigoUe),
                It.Is<int>(a => a == filtro.AnoLetivo),
                It.Is<int>(t => t == filtro.AnoTurma)
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Reclassificacao_Com_Codigos_Nulos_Deve_Retornar_Ok_Com_Dados()
        {
            var filtro = new FiltroPainelEducacionalReclassificacao
            {
                AnoLetivo = 2024,
                CodigoDre = null,
                CodigoUe = null,
                AnoTurma = 7
            };

            var retornoEsperado = new List<PainelEducacionalReclassificacaoDto>
      {
          new PainelEducacionalReclassificacaoDto
          {
              Modalidade = new List<ModalidadeReclassificacaoDto>
              {
                  new ModalidadeReclassificacaoDto
                  {
                      Nome = "Ensino Fundamental",
                      AnoTurma = 7,
                      QuantidadeAlunos = 25
                  }
              }
          }
      };

            var mockUseCase = new Mock<IConsultasReclassificacaoPainelEducacionalUseCase>();
            mockUseCase
                .Setup(x => x.ObterReclassificacao(filtro.CodigoDre, filtro.CodigoUe, filtro.AnoLetivo, filtro.AnoTurma))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterReclassificacao(filtro, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<PainelEducacionalReclassificacaoDto>>(okResult.Value);

            Assert.Single(retorno);
            var item = retorno.First();
            Assert.NotNull(item.Modalidade);
            Assert.Single(item.Modalidade);

            var modalidade = item.Modalidade.First();
            Assert.Equal("Ensino Fundamental", modalidade.Nome);
            Assert.Equal(7, modalidade.AnoTurma);
            Assert.Equal(25, modalidade.QuantidadeAlunos);

            mockUseCase.Verify(x => x.ObterReclassificacao(
                It.Is<string>(d => d == null),
                It.Is<string>(u => u == null),
                It.Is<int>(a => a == 2024),
                It.Is<int>(t => t == 7)
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Reclassificacao_Com_Multiplas_Modalidades_Deve_Retornar_Ok_Com_Dados()
        {
            var filtro = new FiltroPainelEducacionalReclassificacao
            {
                AnoLetivo = 2024,
                CodigoDre = "DRE789",
                CodigoUe = "UE321",
                AnoTurma = 9
            };

            var retornoEsperado = new List<PainelEducacionalReclassificacaoDto>
      {
          new PainelEducacionalReclassificacaoDto
          {
              Modalidade = new List<ModalidadeReclassificacaoDto>
              {
                  new ModalidadeReclassificacaoDto
                  {
                      Nome = "Ensino Fundamental",
                      AnoTurma = 9,
                      QuantidadeAlunos = 32
                  },
                  new ModalidadeReclassificacaoDto
                  {
                      Nome = "EJA",
                      AnoTurma = 9,
                      QuantidadeAlunos = 12
                  },
                  new ModalidadeReclassificacaoDto
                  {
                      Nome = "Ensino Médio",
                      AnoTurma = 1,
                      QuantidadeAlunos = 5
                  }
              }
          }
      };

            var mockUseCase = new Mock<IConsultasReclassificacaoPainelEducacionalUseCase>();
            mockUseCase
                .Setup(x => x.ObterReclassificacao(filtro.CodigoDre, filtro.CodigoUe, filtro.AnoLetivo, filtro.AnoTurma))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterReclassificacao(filtro, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<PainelEducacionalReclassificacaoDto>>(okResult.Value);

            Assert.Single(retorno);
            var item = retorno.First();
            Assert.NotNull(item.Modalidade);
            Assert.Equal(3, item.Modalidade.Count());

            Assert.Collection(item.Modalidade,
                modalidade =>
                {
                    Assert.Equal("Ensino Fundamental", modalidade.Nome);
                    Assert.Equal(9, modalidade.AnoTurma);
                    Assert.Equal(32, modalidade.QuantidadeAlunos);
                },
                modalidade =>
                {
                    Assert.Equal("EJA", modalidade.Nome);
                    Assert.Equal(9, modalidade.AnoTurma);
                    Assert.Equal(12, modalidade.QuantidadeAlunos);
                },
                modalidade =>
                {
                    Assert.Equal("Ensino Médio", modalidade.Nome);
                    Assert.Equal(1, modalidade.AnoTurma);
                    Assert.Equal(5, modalidade.QuantidadeAlunos);
                });

            mockUseCase.Verify(x => x.ObterReclassificacao(
                It.Is<string>(d => d == "DRE789"),
                It.Is<string>(u => u == "UE321"),
                It.Is<int>(a => a == 2024),
                It.Is<int>(t => t == 9)
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Reclassificacao_Deve_Passar_Filtro_Correto_Para_Use_Case()
        {
            var filtro = new FiltroPainelEducacionalReclassificacao
            {
                AnoLetivo = 2025,
                CodigoDre = "DRE456",
                CodigoUe = "UE789",
                AnoTurma = 1
            };

            var retornoEsperado = new List<PainelEducacionalReclassificacaoDto>
      {
          new PainelEducacionalReclassificacaoDto
          {
              Modalidade = new List<ModalidadeReclassificacaoDto>
              {
                  new ModalidadeReclassificacaoDto
                  {
                      Nome = "Ensino Fundamental",
                      AnoTurma = 1,
                      QuantidadeAlunos = 28
                  }
              }
          }
      };

            var mockUseCase = new Mock<IConsultasReclassificacaoPainelEducacionalUseCase>();
            mockUseCase
                .Setup(x => x.ObterReclassificacao(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(retornoEsperado);

            await _controller.ObterReclassificacao(filtro, mockUseCase.Object);

            mockUseCase.Verify(x => x.ObterReclassificacao(
                It.Is<string>(d => d == filtro.CodigoDre),
                It.Is<string>(u => u == filtro.CodigoUe),
                It.Is<int>(a => a == filtro.AnoLetivo),
                It.Is<int>(t => t == filtro.AnoTurma)
            ), Times.Once);
        }

        [Fact]
        public async Task Obter_Reclassificacao_Com_Ano_Turma_Nulo_Deve_Retornar_Ok_Com_Dados()
        {
            var filtro = new FiltroPainelEducacionalReclassificacao
            {
                AnoLetivo = 2024,
                CodigoDre = "DRE111",
                CodigoUe = "UE222",
                AnoTurma = 0
            };

            var retornoEsperado = new List<PainelEducacionalReclassificacaoDto>
      {
          new PainelEducacionalReclassificacaoDto
          {
              Modalidade = new List<ModalidadeReclassificacaoDto>
              {
                  new ModalidadeReclassificacaoDto
                  {
                      Nome = "Ensino Fundamental",
                      AnoTurma = 0,
                      QuantidadeAlunos = 150
                  }
              }
          }
      };

            var mockUseCase = new Mock<IConsultasReclassificacaoPainelEducacionalUseCase>();
            mockUseCase
                .Setup(x => x.ObterReclassificacao(filtro.CodigoDre, filtro.CodigoUe, filtro.AnoLetivo, filtro.AnoTurma))
                .ReturnsAsync(retornoEsperado);

            var result = await _controller.ObterReclassificacao(filtro, mockUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<PainelEducacionalReclassificacaoDto>>(okResult.Value);

            Assert.Single(retorno);
            var item = retorno.First();
            Assert.NotNull(item.Modalidade);
            Assert.Single(item.Modalidade);

            var modalidade = item.Modalidade.First();
            Assert.Equal("Ensino Fundamental", modalidade.Nome);
            Assert.Equal(0, modalidade.AnoTurma);
            Assert.Equal(150, modalidade.QuantidadeAlunos);

            mockUseCase.Verify(x => x.ObterReclassificacao(
                It.Is<string>(d => d == "DRE111"),
                It.Is<string>(u => u == "UE222"),
                It.Is<int>(a => a == 2024),
                It.Is<int>(t => t == 0)
            ), Times.Once);
        }


        [Fact(DisplayName = "Deve retornar 200 OK com os dados de distorção idade/série")]
        public async Task ObterDistorcaoSerieIdade_DeveRetornarOkComDados()
        {
            // Arrange
            var filtro = new FiltroPainelEducacionalDistorcaoIdade
            {
                AnoLetivo = 2025,
                CodigoDre = "110000",
                CodigoUe = "110001"
            };

            var retorno = new List<PainelEducacionalDistorcaoIdadeDto>
            {
                new PainelEducacionalDistorcaoIdadeDto
                {
                    Modalidade = "Ensino Fundamental",
                    SerieAno = new List<SerieAnoDistorcaoIdadeDto>
                    {
                        new SerieAnoDistorcaoIdadeDto
                        {
                            Ano = "1º",
                            QuantidadeAlunos = 5,
                        },
                        new SerieAnoDistorcaoIdadeDto
                        {
                            Ano = "2º",
                            QuantidadeAlunos = 3,
                        }
                    }
                }
            };

            _consultasDistorcaoIdadeUseCase
                .Setup(x => x.ObterDistorcaoIdade(It.IsAny<FiltroPainelEducacionalDistorcaoIdade>()))
                .Returns(Task.FromResult<IEnumerable<PainelEducacionalDistorcaoIdadeDto>>(retorno));

            // Act
            var resultado = await _controller.ObterDistorcaoSerieIdade(filtro, _consultasDistorcaoIdadeUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var dto = Assert.IsAssignableFrom<IEnumerable<PainelEducacionalDistorcaoIdadeDto>>(okResult.Value);

            Assert.Single(dto);
            Assert.Equal("Ensino Fundamental", dto.First().Modalidade);

            _consultasDistorcaoIdadeUseCase.Verify(x => x.ObterDistorcaoIdade(It.IsAny<FiltroPainelEducacionalDistorcaoIdade>()), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar Ok com frequência diária para DRE")]
        public async Task ObterFrequenciaDiariaDre_DeveRetornarOkComDados()
        {
            // Arrange
            var filtro = new FiltroFrequenciaDiariaDreDto
            {
                AnoLetivo = 2025,
                CodigoDre = "123456",
                DataFrequencia = "2025-10-20"
            };

            var resultadoEsperado = new FrequenciaDiariaDreDto
            {
                Ues = new List<RegistroFrequenciaDiariaUeDto>(),
                TotalPaginas = 1,
                TotalRegistros = 10
            };

            _consultasRegistroFrequenciaDiariaDreUseCase
                .Setup(x => x.ObterFrequenciaDiariaPorDre(filtro))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var result = await _controller.ObterFrequenciaDiariaDre(filtro, _consultasRegistroFrequenciaDiariaDreUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<FrequenciaDiariaDreDto>(okResult.Value);
            Assert.Equal(1, dto.TotalPaginas);
            Assert.Equal(10, dto.TotalRegistros);

            _consultasRegistroFrequenciaDiariaDreUseCase.Verify(x => x.ObterFrequenciaDiariaPorDre(filtro), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar Ok com frequência diária para UE")]
        public async Task ObterFrequenciaDiariaUe_DeveRetornarOkComDados()
        {
            // Arrange
            var filtro = new FiltroFrequenciaDiariaUeDto
            {
                AnoLetivo = 2025,
                CodigoUe = "123456",
                DataFrequencia = "2025-10-20"
            };

            var resultadoEsperado = new FrequenciaDiariaUeDto
            {
                Turmas = new List<RegistroFrequenciaDiariaTurmaDto>(),
                TotalPaginas = 1,
                TotalRegistros = 10
            };

            _consultasRegistroFrequenciaDiariaUeUseCase
                .Setup(x => x.ObterFrequenciaDiariaPorUe(filtro))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var result = await _controller.ObterFrequenciaDiariaUe(filtro, _consultasRegistroFrequenciaDiariaUeUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<FrequenciaDiariaUeDto>(okResult.Value);
            Assert.Equal(1, dto.TotalPaginas);
            Assert.Equal(10, dto.TotalRegistros);

            _consultasRegistroFrequenciaDiariaUeUseCase.Verify(x => x.ObterFrequenciaDiariaPorUe(filtro), Times.Once);
        }
    }
}