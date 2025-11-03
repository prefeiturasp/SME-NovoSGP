using FluentAssertions;
using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdeb;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional.ProficienciaIdeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class ObterProficienciaIdebQueryHandlerTestes
    {
        private readonly Mock<IRepositorioPainelEducacionalProficienciaIdeb> _repositorioMock;
        private readonly ObterProficienciaIdebQueryHandler _handler;

        public ObterProficienciaIdebQueryHandlerTestes()
        {
            _repositorioMock = new Mock<IRepositorioPainelEducacionalProficienciaIdeb>();
            _handler = new ObterProficienciaIdebQueryHandler(_repositorioMock.Object);
        }

        [Fact]
        public async Task DadoAnoLetivoIgualZero_QuandoChamarExecutar_DeveChamarRepositorioComAnoLetivoZero()
        {
            // Arrange
            var query = new ObterProficienciaIdebQuery(0, "123456");
            _repositorioMock
                .Setup(r => r.ObterConsolidacaoPorAnoVisaoUeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new List<PainelEducacionalConsolidacaoProficienciaIdebUe>());
            // Act
            await _handler.Handle(query, default);
            // Assert
            _repositorioMock.Verify(r => r.ObterConsolidacaoPorAnoVisaoUeAsync(
                It.IsAny<int>(),
                0,
                "123456"), Times.Once);
        }

        [Fact]
        public async Task DadoConsolidacaoInexistenteParaAnoInformado_QuandoExecutarHandler_DeveChamarRepositorioParaAnosAnteriores()
        {
            // Arrange
            var anoLetivoInformado = PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE + 1;
            var codigoUe = "123456";
            var query = new ObterProficienciaIdebQuery(anoLetivoInformado, codigoUe);
            _repositorioMock
                .SetupSequence(r => r.ObterConsolidacaoPorAnoVisaoUeAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new List<PainelEducacionalConsolidacaoProficienciaIdebUe>())
                .ReturnsAsync(new List<PainelEducacionalConsolidacaoProficienciaIdebUe>());

            // Act
            await _handler.Handle(query, default);

            // Assert
            _repositorioMock.Verify(r => r.ObterConsolidacaoPorAnoVisaoUeAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    codigoUe), Times.Exactly(2));
        }

        [Fact]
        public async Task DadoConsolidacaoExistenteParaAnoInformado_QuandoExecutarHandler_DeveRetornarResultadosCorretos()
        {
            // Arrange
            var anoLetivoInformado = DateTime.Now.Year;
            var codigoUe = "123456";
            var resultadoEsperado = new List<PainelEducacionalProficienciaIdebDto>
            {
                new PainelEducacionalProficienciaIdebDto
                {
                    AnoLetivo = anoLetivoInformado,
                    Boletim = "Boletim Exemplo",
                    NotaFinal = 75.5m,
                    NotaInicial = 65.0m,
                    NotaEnsinoMedio = 99.99m,
                    Proficiencia = new ProficienciaIdebResumidoDto
                    {
                        AnosIniciais = new List<ComponenteCurricularIdebResumidoDto>
                        {
                            new ComponenteCurricularIdebResumidoDto
                            {
                                ComponenteCurricular = ComponenteCurricularEnum.Portugues,
                                Percentual = 70.0m
                            },
                            new ComponenteCurricularIdebResumidoDto
                            {
                                ComponenteCurricular = ComponenteCurricularEnum.Matematica,
                                Percentual = 60.0m
                            },
                            new ComponenteCurricularIdebResumidoDto
                            {
                                ComponenteCurricular = ComponenteCurricularEnum.CienciasNatureza,
                                Percentual = 75.0m
                            }
                        },
                        AnosFinais = new List<ComponenteCurricularIdebResumidoDto>
                        {
                            new ComponenteCurricularIdebResumidoDto
                            {
                                ComponenteCurricular = ComponenteCurricularEnum.Portugues,
                                Percentual = 76.54m
                            },
                            new ComponenteCurricularIdebResumidoDto
                            {
                                ComponenteCurricular = ComponenteCurricularEnum.Matematica,
                                Percentual = 6.08m
                            },
                            new ComponenteCurricularIdebResumidoDto
                            {
                                ComponenteCurricular = ComponenteCurricularEnum.CienciasNatureza
                            }
                        },
                        EnsinoMedio  = new List<ComponenteCurricularIdebResumidoDto>
                        {
                            new ComponenteCurricularIdebResumidoDto
                            {
                                ComponenteCurricular = ComponenteCurricularEnum.Portugues
                            },
                            new ComponenteCurricularIdebResumidoDto
                            {
                                ComponenteCurricular = ComponenteCurricularEnum.Matematica
                            },
                            new ComponenteCurricularIdebResumidoDto
                            {
                                ComponenteCurricular = ComponenteCurricularEnum.CienciasNatureza,
                                Percentual = 6.8m
                            }
                        }
                    }
                }
            };

            var proficiencias = new List<PainelEducacionalConsolidacaoProficienciaIdebUe>
            {
                new PainelEducacionalConsolidacaoProficienciaIdebUe
                {
                    AnoLetivo = anoLetivoInformado,
                    SerieAno = SerieAnoIndiceDesenvolvimentoEnum.AnosIniciais,
                    ComponenteCurricular = ComponenteCurricularEnum.Portugues,
                    Proficiencia = 70.0m,
                    Nota = 65.0m
                },
                new PainelEducacionalConsolidacaoProficienciaIdebUe
                {
                    AnoLetivo = anoLetivoInformado,
                    SerieAno = SerieAnoIndiceDesenvolvimentoEnum.AnosIniciais,
                    ComponenteCurricular = ComponenteCurricularEnum.Matematica,
                    Proficiencia = 60.0m,
                    Nota = 65.0m
                },
                new PainelEducacionalConsolidacaoProficienciaIdebUe
                {
                    AnoLetivo = anoLetivoInformado,
                    SerieAno = SerieAnoIndiceDesenvolvimentoEnum.AnosIniciais,
                    ComponenteCurricular = ComponenteCurricularEnum.CienciasNatureza,
                    Proficiencia = 75.0m,
                    Nota = 65.0m
                },
                new PainelEducacionalConsolidacaoProficienciaIdebUe
                {
                    AnoLetivo = anoLetivoInformado,
                    SerieAno = SerieAnoIndiceDesenvolvimentoEnum.AnosFinais,
                    ComponenteCurricular = ComponenteCurricularEnum.Portugues,
                    Proficiencia = 76.54m,
                    Nota = 75.5m,
                    Boletim = "Boletim Exemplo"
                },
                new PainelEducacionalConsolidacaoProficienciaIdebUe
                {
                    AnoLetivo = anoLetivoInformado,
                    SerieAno = SerieAnoIndiceDesenvolvimentoEnum.AnosFinais,
                    ComponenteCurricular = ComponenteCurricularEnum.Matematica,
                    Proficiencia = 6.08m,
                    Nota = 75.5m
                },
                new PainelEducacionalConsolidacaoProficienciaIdebUe
                {
                    AnoLetivo = anoLetivoInformado,
                    SerieAno = SerieAnoIndiceDesenvolvimentoEnum.EnsinoMedio,
                    ComponenteCurricular = ComponenteCurricularEnum.CienciasNatureza,
                    Proficiencia = 6.8m,
                    Nota = 99.99m
                }
            };

            var query = new ObterProficienciaIdebQuery(anoLetivoInformado, codigoUe);
            _repositorioMock
                .Setup(r => r.ObterConsolidacaoPorAnoVisaoUeAsync(It.IsAny<int>(), anoLetivoInformado, codigoUe))
                .ReturnsAsync(proficiencias);

            // Act
            var resultado = await _handler.Handle(query, default);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEquivalentTo(resultadoEsperado);
        }

        [Fact]
        public async Task DadoConsolidacaoInexistente_QuandoExecutarHandler_DeveRetornarListaVazia()
        {
            // Arrange
            var anoLetivoInformado = DateTime.Now.Year;
            var codigoUe = "123456";
            var query = new ObterProficienciaIdebQuery(anoLetivoInformado, codigoUe);
            _repositorioMock
                .Setup(r => r.ObterConsolidacaoPorAnoVisaoUeAsync(It.IsAny<int>(), anoLetivoInformado, codigoUe))
                .ReturnsAsync(new List<PainelEducacionalConsolidacaoProficienciaIdebUe>());
            // Act
            var resultado = await _handler.Handle(query, default);
            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
        }

        [Fact]
        public async Task DadoConsolidacaoNula_QuandoExecutarHandler_DeveRetornarListaVazia()
        {
            // Arrange
            var anoLetivoInformado = DateTime.Now.Year;
            var codigoUe = "123456";
            var query = new ObterProficienciaIdebQuery(anoLetivoInformado, codigoUe);
            _repositorioMock
                .Setup(r => r.ObterConsolidacaoPorAnoVisaoUeAsync(It.IsAny<int>(), anoLetivoInformado, codigoUe))
                .ReturnsAsync((IEnumerable<PainelEducacionalConsolidacaoProficienciaIdebUe>)null);
            // Act
            var resultado = await _handler.Handle(query, default);
            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
        }

        [Fact]
        public async Task DadoAnoLetivoMenorQueLimite_QuandoExecutarHandler_NaoDeveChamarRepositorio()
        {
            // Arrange
            var anoLetivoInformado = PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE - 1;
            var codigoUe = "123456";
            var query = new ObterProficienciaIdebQuery(anoLetivoInformado, codigoUe);
            // Act
            await _handler.Handle(query, default);
            // Assert
            _repositorioMock.Verify(r => r.ObterConsolidacaoPorAnoVisaoUeAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DadoConsolidacaoExistenteEAnoLetivoMenorQueZero_QuandoExecutarHandle_DeveRetornarDadosDeTodosOsAnosDisponiveis()
        {
            // Arrange
            var anoLetivoInformado = -1;
            var codigoUe = "123456";
            var proficiencias = new List<PainelEducacionalConsolidacaoProficienciaIdebUe>
            {
                new PainelEducacionalConsolidacaoProficienciaIdebUe
                {
                    AnoLetivo = 2023,
                    SerieAno = SerieAnoIndiceDesenvolvimentoEnum.AnosIniciais,
                    ComponenteCurricular = ComponenteCurricularEnum.Portugues,
                    Proficiencia = 70.0m,
                    Nota = 65.0m,
                    Boletim = "Boletim Exemplo"
                },
                new PainelEducacionalConsolidacaoProficienciaIdebUe
                {
                    AnoLetivo = 2022,
                    SerieAno = SerieAnoIndiceDesenvolvimentoEnum.AnosFinais,
                    ComponenteCurricular = ComponenteCurricularEnum.Matematica,
                    Proficiencia = 6.08m,
                    Nota = 75.5m
                }
            };

            var query = new ObterProficienciaIdebQuery(anoLetivoInformado, codigoUe);
            _repositorioMock
                .Setup(r => r.ObterConsolidacaoPorAnoVisaoUeAsync(It.IsAny<int>(), anoLetivoInformado, codigoUe))
                .ReturnsAsync(proficiencias);

            // Act
            var resultado = await _handler.Handle(query, default);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Count().Should().Be(2);
        }
    }
}