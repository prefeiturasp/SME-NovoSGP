using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAnoMaisRecenteIdeb;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdebPorAnoSerie;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsultasIdebPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsultasIdebPainelEducacionalUseCase _useCase;

        public ConsultasIdebPainelEducacionalUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsultasIdebPainelEducacionalUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Obter_Ideb_Filtro_Nulo_Deve_Throw_NegocioException()
        {
            FiltroPainelEducacionalIdeb filtro = null;

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.ObterIdeb(filtro));
            Assert.Equal("Filtro não pode ser nulo", exception.Message);
        }

        [Theory]
        [InlineData(-99)]
        [InlineData(0)]
        [InlineData(10)]
        public async Task Obter_Ideb_Serie_Invalida_Deve_ThrowNegocioException(int serieInvalida)
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = (PainelEducacionalIdebSerie)serieInvalida,
                AnoLetivo = 2023,
                CodigoDre = "01",
                CodigoUe = "001"
            };

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.ObterIdeb(filtro));
            Assert.Equal("Série/Ano inválida", exception.Message);
        }

        [Theory]
        [InlineData(2018)]
        [InlineData(2030)]
        public async Task Obter_Ideb_Ano_Letivo_Invalido_Deve_ThrowNegocioException(int anoInvalido)
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = PainelEducacionalIdebSerie.AnosIniciais,
                AnoLetivo = anoInvalido,
                CodigoDre = "01",
                CodigoUe = "001"
            };

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.ObterIdeb(filtro));
            Assert.Equal("Ano letivo deve estar entre 2019 e o próximo ano", exception.Message);
        }

        [Fact]
        public async Task Obter_Ideb_Ano_Letivo_Menos_99_Com_Ano_Mais_Recente_Com_Dados_Deve_Retornar_Dados()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = PainelEducacionalIdebSerie.AnosIniciais,
                AnoLetivo = -99,
                CodigoDre = "01",
                CodigoUe = "001"
            };

            var anoMaisRecente = 2022;
            var dadosIdeb = new List<PainelEducacionalIdebDto>
            {
                new PainelEducacionalIdebDto
                {
                    AnoLetivo = 2022,
                    SerieAno = "1",
                    Nota = 5.5m,
                    Faixa = "5.0-6.0",
                    Quantidade = 10
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoMaisRecenteIdebQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anoMaisRecente);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdebPorAnoSerieQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosIdeb);

            var result = await _useCase.ObterIdeb(filtro);

            Assert.NotNull(result);
            Assert.Equal(-99, result.AnoSolicitado);
            Assert.Equal(2022, result.AnoUtilizado);
            Assert.True(result.AnoSolicitadoSemDados);
            Assert.Equal("1", result.Serie);
            Assert.Equal(5.5, result.MediaGeral);
            Assert.Equal("01", result.CodigoDre);
            Assert.Equal("001", result.CodigoUe);
            Assert.Single(result.Distribuicao);
        }

        [Fact]
        public async Task Obter_Ideb_Ano_Letivo_Menos_99_Com_Ano_Mais_Recente_Sem_Dados_Deve_Retornar_Ideb_Vazio()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = PainelEducacionalIdebSerie.AnosIniciais,
                AnoLetivo = -99,
                CodigoDre = "01",
                CodigoUe = "001"
            };

            var anoMaisRecente = DateTimeOffset.UtcNow.Year;
            var dadosVazios = new List<PainelEducacionalIdebDto>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoMaisRecenteIdebQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anoMaisRecente);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdebPorAnoSerieQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosVazios);

            var result = await _useCase.ObterIdeb(filtro);

            Assert.NotNull(result);
            Assert.Equal(-99, result.AnoSolicitado);
            Assert.Equal(DateTime.Now.Year, result.AnoUtilizado);
            Assert.True(result.AnoSolicitadoSemDados);
            Assert.Equal(string.Empty, result.Serie);
            Assert.Equal(0, result.MediaGeral);
            Assert.Empty(result.Distribuicao);
        }

        [Fact]
        public async Task Obter_Ideb_Ano_Letivo_Menos_99_Sem_Ano_Mais_Recente_Deve_Retornar_Ideb_Vazio()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = PainelEducacionalIdebSerie.AnosIniciais,
                AnoLetivo = -99,
                CodigoDre = "01",
                CodigoUe = "001"
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoMaisRecenteIdebQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int?)null);

            var result = await _useCase.ObterIdeb(filtro);

            Assert.NotNull(result);
            Assert.Equal(-99, result.AnoSolicitado);
            Assert.Equal(DateTime.Now.Year, result.AnoUtilizado);
            Assert.True(result.AnoSolicitadoSemDados);
            Assert.Equal(string.Empty, result.Serie);
            Assert.Equal(0, result.MediaGeral);
            Assert.Empty(result.Distribuicao);
        }

        [Fact]
        public async Task Obter_Ideb_Ano_Letivo_Null_Com_Ano_Mais_Recente_Com_Dados_Deve_Retornar_Dados()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = PainelEducacionalIdebSerie.AnosFinais,
                AnoLetivo = null,
                CodigoDre = "02",
                CodigoUe = "002"
            };

            var anoMaisRecente = 2021;
            var dadosIdeb = new List<PainelEducacionalIdebDto>
            {
                new PainelEducacionalIdebDto
                {
                    AnoLetivo = 2021,
                    SerieAno = "2",
                    Nota = 4.8m,
                    Faixa = "4.0-5.0",
                    Quantidade = 15
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnoMaisRecenteIdebQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anoMaisRecente);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdebPorAnoSerieQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosIdeb);

            var result = await _useCase.ObterIdeb(filtro);

            Assert.NotNull(result);
            Assert.Equal(-99, result.AnoSolicitado);
            Assert.Equal(2021, result.AnoUtilizado);
            Assert.True(result.AnoSolicitadoSemDados);
            Assert.Equal("2", result.Serie);
            Assert.Equal(4.8, result.MediaGeral);
        }

        [Fact]
        public async Task Obter_Ideb_Ano_Letivo_Especifico_Com_Dados_Deve_Retornar_Dados()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = PainelEducacionalIdebSerie.EnsinoMedio,
                AnoLetivo = 2023,
                CodigoDre = "03",
                CodigoUe = "003"
            };

            var dadosIdeb = new List<PainelEducacionalIdebDto>
            {
                new PainelEducacionalIdebDto
                {
                    AnoLetivo = 2023,
                    SerieAno = "3",
                    Nota = 6.2m,
                    Faixa = "6.0-7.0",
                    Quantidade = 20
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdebPorAnoSerieQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosIdeb);

            var result = await _useCase.ObterIdeb(filtro);

            Assert.NotNull(result);
            Assert.Equal(2023, result.AnoSolicitado);
            Assert.Equal(2023, result.AnoUtilizado);
            Assert.False(result.AnoSolicitadoSemDados);
            Assert.Equal("3", result.Serie);
            Assert.Equal(6.2, result.MediaGeral);
        }

        [Fact]
        public async Task Obter_Ideb_Ano_Letivo_Especifico_Sem_Dados_Deve_Procurar_Anos_Anteriores()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = PainelEducacionalIdebSerie.AnosIniciais,
                AnoLetivo = 2023,
                CodigoDre = "04",
                CodigoUe = "004"
            };

            var dadosVazios = new List<PainelEducacionalIdebDto>();
            var dadosAno2022 = new List<PainelEducacionalIdebDto>
            {
                new PainelEducacionalIdebDto
                {
                    AnoLetivo = 2022,
                    SerieAno = "1",
                    Nota = 5.0m,
                    Faixa = "5.0-6.0",
                    Quantidade = 12
                }
            };

            _mediatorMock.SetupSequence(m => m.Send(It.IsAny<ObterIdebPorAnoSerieQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosVazios)
                .ReturnsAsync(dadosAno2022);

            var result = await _useCase.ObterIdeb(filtro);

            Assert.NotNull(result);
            Assert.Equal(2023, result.AnoSolicitado);
            Assert.Equal(2022, result.AnoUtilizado);
            Assert.True(result.AnoSolicitadoSemDados);
        }

        [Fact]
        public async Task Obter_Ideb_Ano_Letivo_Especifico_Sem_Dados_Ate_Ano_Minimo_Deve_Retornar_Ideb_Vazio()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = PainelEducacionalIdebSerie.AnosIniciais,
                AnoLetivo = 2020,
                CodigoDre = "05",
                CodigoUe = "005"
            };

            var dadosVazios = new List<PainelEducacionalIdebDto>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdebPorAnoSerieQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosVazios);

            var result = await _useCase.ObterIdeb(filtro);

            Assert.NotNull(result);
            Assert.Equal(2020, result.AnoSolicitado);
            Assert.Equal(2018, result.AnoUtilizado);
            Assert.True(result.AnoSolicitadoSemDados);
            Assert.Equal(string.Empty, result.Serie);
            Assert.Equal(0, result.MediaGeral);
            Assert.Empty(result.Distribuicao);
        }

        [Fact]
        public async Task Obter_Ideb_Com_Codigos_Com_Espacos_Deve_Trim_Espacos()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = PainelEducacionalIdebSerie.AnosIniciais,
                AnoLetivo = 2023,
                CodigoDre = "  07  ",
                CodigoUe = "  007  "
            };

            var dadosIdeb = new List<PainelEducacionalIdebDto>
            {
                new PainelEducacionalIdebDto
                {
                    AnoLetivo = 2023,
                    SerieAno = "1",
                    Nota = 4.5m,
                    Faixa = "4.0-5.0",
                    Quantidade = 5
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdebPorAnoSerieQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosIdeb);

            var result = await _useCase.ObterIdeb(filtro);

            Assert.Equal("07", result.CodigoDre);
            Assert.Equal("007", result.CodigoUe);
        }

        [Fact]
        public async Task Obter_Ideb_Com_Codigos_Nulos_Deve_Manter_Nulos()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = PainelEducacionalIdebSerie.AnosIniciais,
                AnoLetivo = 2023,
                CodigoDre = null,
                CodigoUe = null
            };

            var dadosIdeb = new List<PainelEducacionalIdebDto>
            {
                new PainelEducacionalIdebDto
                {
                    AnoLetivo = 2023,
                    SerieAno = "1",
                    Nota = 3.8m,
                    Faixa = "3.0-4.0",
                    Quantidade = 3
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdebPorAnoSerieQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosIdeb);

            var result = await _useCase.ObterIdeb(filtro);

            Assert.Null(result.CodigoDre);
            Assert.Null(result.CodigoUe);
        }

        [Fact]
        public async Task Obter_Ideb_Com_Multiplos_Dados_Deve_Mapear_Corretamente()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = PainelEducacionalIdebSerie.AnosFinais,
                AnoLetivo = 2023,
                CodigoDre = "08",
                CodigoUe = "008"
            };

            var dadosIdeb = new List<PainelEducacionalIdebDto>
            {
                new PainelEducacionalIdebDto
                {
                    AnoLetivo = 2023,
                    SerieAno = "2",
                    Nota = 5.2m,
                    Faixa = "5.0-6.0",
                    Quantidade = 15
                },
                new PainelEducacionalIdebDto
                {
                    AnoLetivo = 2023,
                    SerieAno = "2",
                    Nota = 5.2m,
                    Faixa = "4.0-5.0",
                    Quantidade = 10
                },
                new PainelEducacionalIdebDto
                {
                    AnoLetivo = 2022,
                    SerieAno = "2",
                    Nota = 4.8m,
                    Faixa = "4.0-5.0",
                    Quantidade = 8
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdebPorAnoSerieQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosIdeb);

            var result = await _useCase.ObterIdeb(filtro);

            Assert.NotNull(result);
            Assert.Equal(2, result.Distribuicao.Count);
            Assert.Contains(result.Distribuicao, d => d.Faixa == "5.0-6.0" && d.Quantidade == 15);
            Assert.Contains(result.Distribuicao, d => d.Faixa == "4.0-5.0" && d.Quantidade == 10);
            Assert.DoesNotContain(result.Distribuicao, d => d.Quantidade == 8);
        }

        [Fact]
        public async Task Obter_Ideb_Dados_Vazios_Nota_Nula_Deve_Usar_Zero()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = PainelEducacionalIdebSerie.AnosIniciais,
                AnoLetivo = 2023,
                CodigoDre = "09",
                CodigoUe = "009"
            };

            var dadosIdeb = new List<PainelEducacionalIdebDto>
            {
                new PainelEducacionalIdebDto
                {
                    AnoLetivo = 2023,
                    SerieAno = "1",
                    Nota = 0,
                    Faixa = "0.0-1.0",
                    Quantidade = 1
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdebPorAnoSerieQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosIdeb);

            var result = await _useCase.ObterIdeb(filtro);

            Assert.Equal(0, result.MediaGeral);
        }

        [Fact]
        public async Task Obter_Ideb_Dados_Vazios_Sem_Primeiro_Item_Deve_Usar_String_Vazia()
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = PainelEducacionalIdebSerie.AnosIniciais,
                AnoLetivo = 2023,
                CodigoDre = "10",
                CodigoUe = "010"
            };

            var dadosIdeb = new List<PainelEducacionalIdebDto>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdebPorAnoSerieQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosIdeb);

            // Act
            var result = await _useCase.ObterIdeb(filtro);

            // Assert
            Assert.Equal(string.Empty, result.Serie);
            Assert.Equal(0, result.MediaGeral);
        }

        [Theory]
        [InlineData(2019)]
        [InlineData(2024)]
        public async Task Obter_Ideb_Anos_Validos_Limites_Deve_Passar_Validacao(int ano)
        {
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = PainelEducacionalIdebSerie.AnosIniciais,
                AnoLetivo = ano,
                CodigoDre = "11",
                CodigoUe = "011"
            };

            var dadosIdeb = new List<PainelEducacionalIdebDto>
            {
                new PainelEducacionalIdebDto
                {
                    AnoLetivo = ano,
                    SerieAno = "1",
                    Nota = 5.0m,
                    Faixa = "5.0-6.0",
                    Quantidade = 1
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdebPorAnoSerieQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosIdeb);

            var result = await _useCase.ObterIdeb(filtro);

            Assert.NotNull(result);
            Assert.Equal(ano, result.AnoSolicitado);
        }

        [Fact]
        public async Task Obter_Ideb_Ano_Letivo_Proximo_Ano_Deve_Passar_Validacao()
        {
            var proximoAno = DateTime.Now.Year + 1;
            var filtro = new FiltroPainelEducacionalIdeb
            {
                Serie = PainelEducacionalIdebSerie.AnosIniciais,
                AnoLetivo = proximoAno,
                CodigoDre = "12",
                CodigoUe = "012"
            };

            var dadosIdeb = new List<PainelEducacionalIdebDto>
            {
                new PainelEducacionalIdebDto
                {
                    AnoLetivo = proximoAno,
                    SerieAno = "1",
                    Nota = 4.5m,
                    Faixa = "4.0-5.0",
                    Quantidade = 2
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdebPorAnoSerieQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosIdeb);

            var result = await _useCase.ObterIdeb(filtro);

            Assert.NotNull(result);
            Assert.Equal(proximoAno, result.AnoSolicitado);
        }
    }
}