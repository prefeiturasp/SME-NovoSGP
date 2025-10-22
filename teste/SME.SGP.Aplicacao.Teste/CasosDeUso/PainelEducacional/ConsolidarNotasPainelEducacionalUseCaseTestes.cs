using Bogus;
using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoNota;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotasParaConsolidacao;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotaUltimoAnoConsolidado;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Consts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsolidarNotasPainelEducacionalUseCaseTestes
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidarNotasPainelEducacionalUseCase _consolidarNotasPainelEducacionalUseCase;
        private readonly Faker _faker;

        public ConsolidarNotasPainelEducacionalUseCaseTestes()
        {
            _faker = new();
            _mediatorMock = new Mock<IMediator>();
            _consolidarNotasPainelEducacionalUseCase = new ConsolidarNotasPainelEducacionalUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task DadoNotasConsolidacaoDreInexistente_QuandoExecutar_DeveRetornarFalseENaoChamarSalvarPainel()
        {
            // Arrange

            // Act
            var resultado = await _consolidarNotasPainelEducacionalUseCase.Executar(new Infra.MensagemRabbit());

            // Assert
            resultado.Should().BeFalse();

            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoNotaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DadoUltimoAnoConsolidadoIgualZero_QuandoExecutar_DeveConsolidarTodosOsAnosApartirDoMinimo()
        {
            // Arrange
            var anoLetivoAtual = DateTime.Now.Year;
            var anoLetivoMinimo = PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE;

            // Act
            var resultado = await _consolidarNotasPainelEducacionalUseCase.Executar(new Infra.MensagemRabbit());

            // Assert
            resultado.Should().BeFalse();
            for (int ano = anoLetivoMinimo; ano <= anoLetivoAtual; ano++)
            {
                _mediatorMock.Verify(m => m.Send(It.Is<ObterNotasParaConsolidacaoQuery>(q => q.AnoLetivo == ano), It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Fact]
        public async Task DadoUltimoAnoConsolidadoMaiorQueMinimoEMenorQueAtual_QuandoExecutar_DeveConsolidarTodosOsAnosSeguintes()
        {
            // Arrange
            var anoLetivoAtual = DateTime.Now.Year;
            var ultimoAnoConsolidado = _faker.Random.Int(PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE, anoLetivoAtual - 1);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNotaUltimoAnoConsolidadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ultimoAnoConsolidado);
            // Act
            var resultado = await _consolidarNotasPainelEducacionalUseCase.Executar(new Infra.MensagemRabbit());
            // Assert
            resultado.Should().BeFalse();
            for (int ano = ultimoAnoConsolidado + 1; ano <= anoLetivoAtual; ano++)
            {
                _mediatorMock.Verify(m => m.Send(It.Is<ObterNotasParaConsolidacaoQuery>(q => q.AnoLetivo == ano), It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Fact]
        public async Task DadoDadosBrutosExistentes_QuandoExecutar_DeveSalvarConsolidacaoDreCorretamente()
        {
            // Arrange
            var anoLetivoAtual = (short)DateTime.Now.Year;
            var dadosBrutos = GerarDadosBrutos(anoLetivoAtual);
            var consolidadoDreEsperado = GerarConsolidacaoDreEsperado(anoLetivoAtual);
            SalvarPainelEducacionalConsolidacaoNotaCommand comandoCapturado = null;

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNotasParaConsolidacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosBrutos);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNotaUltimoAnoConsolidadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anoLetivoAtual);

            // Captura o comando enviado para verificação posterior
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoNotaCommand>(), It.IsAny<CancellationToken>()))
                .Callback((IRequest<bool> cmd, CancellationToken token) =>
                {
                    comandoCapturado = cmd as SalvarPainelEducacionalConsolidacaoNotaCommand;
                })
                .ReturnsAsync(true);

            // Act
            var resultado = await _consolidarNotasPainelEducacionalUseCase.Executar(new Infra.MensagemRabbit());

            // Assert
            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoNotaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            comandoCapturado.Should().NotBeNull();
            comandoCapturado.NotasConsolidadasDre.Should().BeEquivalentTo(consolidadoDreEsperado, options =>
            options.Excluding(info => info.Path.EndsWith("CriadoEm")));
        }

        [Fact]
        public async Task DadoDadosBrutosExistentes_QuandoExecutar_DeveSalvarConsolidacaoUeCorretamente()
        {
            // Arrange
            var anoLetivoAtual = (short)DateTime.Now.Year;
            var dadosBrutos = GerarDadosBrutos(anoLetivoAtual);
            var consolidadoUeEsperado = GerarConsolidacaoUeEsperado(anoLetivoAtual);
            SalvarPainelEducacionalConsolidacaoNotaCommand comandoCapturado = null;
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNotasParaConsolidacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosBrutos);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNotaUltimoAnoConsolidadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anoLetivoAtual);
            // Captura o comando enviado para verificação posterior
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoNotaCommand>(), It.IsAny<CancellationToken>()))
                .Callback((IRequest<bool> cmd, CancellationToken token) =>
                {
                    comandoCapturado = cmd as SalvarPainelEducacionalConsolidacaoNotaCommand;
                })
                .ReturnsAsync(true);

            // Act
            var resultado = await _consolidarNotasPainelEducacionalUseCase.Executar(new Infra.MensagemRabbit());

            // Assert
            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoNotaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            comandoCapturado.Should().NotBeNull();
            comandoCapturado.NotasConsolidadasUe.Should().BeEquivalentTo(consolidadoUeEsperado, options =>
            options.Excluding(info => info.Path.EndsWith("CriadoEm")));

        }

        private static List<PainelEducacionalConsolidacaoNotaDadosBrutos> GerarDadosBrutos(short anoLetivo) =>
            new List<PainelEducacionalConsolidacaoNotaDadosBrutos>
            {
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE001",
                    AnoTurma = '1',
                    CodigoUe = "UE001",
                    ConceitoDeAprovado = false,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_PORTUGUES,
                    Nota = 50,
                    TurmaNome = "Turma 001",
                    ValorConceito = null,
                    ValorMedioNota = 60
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE001",
                    AnoTurma = '1',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = false,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_PORTUGUES,
                    Nota = 59,
                    TurmaNome = "Turma 001",
                    ValorConceito = null,
                    ValorMedioNota = 60
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE001",
                    AnoTurma = '1',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = false,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_PORTUGUES,
                    Nota = 60,
                    TurmaNome = "Turma 002",
                    ValorConceito = null,
                    ValorMedioNota = 60
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE001",
                    AnoTurma = '1',
                    CodigoUe = "UE001",
                    ConceitoDeAprovado = true,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_MATEMATICA,
                    Nota = 70,
                    TurmaNome = "Turma 001",
                    ValorConceito = null,
                    ValorMedioNota = 65
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE001",
                    AnoTurma = '1',
                    CodigoUe = "UE001",
                    ConceitoDeAprovado = true,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_CIENCIAS,
                    Nota = 80,
                    TurmaNome = "Turma 001",
                    ValorConceito = null,
                    ValorMedioNota = 75
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE001",
                    AnoTurma = '1',
                    CodigoUe = "UE001",
                    ConceitoDeAprovado = false,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_CIENCIAS,
                    Nota = 60,
                    TurmaNome = "Turma 001",
                    ValorConceito = null,
                    ValorMedioNota = 75
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = true,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_PORTUGUES,
                    Nota = 85,
                    TurmaNome = "Turma 002",
                    ValorConceito = null,
                    ValorMedioNota = 70
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = true,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_MATEMATICA,
                    Nota = 90,
                    TurmaNome = "Turma 002",
                    ValorConceito = null,
                    ValorMedioNota = 75
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = true,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_CIENCIAS,
                    Nota = 95,
                    TurmaNome = "Turma 002",
                    ValorConceito = null,
                    ValorMedioNota = 80
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = false,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_CIENCIAS,
                    Nota = 70,
                    TurmaNome = "Turma 002",
                    ValorConceito = null,
                    ValorMedioNota = 80
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = true,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_MATEMATICA,
                    Nota = null,
                    TurmaNome = "Turma 002",
                    ValorConceito = "A",
                    ValorMedioNota = 75
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = false,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_PORTUGUES,
                    Nota = null,
                    TurmaNome = "Turma 002",
                    ValorConceito = "C",
                    ValorMedioNota = 70
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = true,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_CIENCIAS,
                    Nota = null,
                    TurmaNome = "Turma 002",
                    ValorConceito = "B",
                    ValorMedioNota = 80
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = false,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_CIENCIAS,
                    Nota = null,
                    TurmaNome = "Turma 002",
                    ValorConceito = "C",
                    ValorMedioNota = 80
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = true,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_MATEMATICA,
                    Nota = 88,
                    TurmaNome = "Turma 002",
                    ValorConceito = null,
                    ValorMedioNota = 75
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = true,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_PORTUGUES,
                    Nota = 78,
                    TurmaNome = "Turma 002",
                    ValorConceito = null,
                    ValorMedioNota = 70
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Medio,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = false,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_CIENCIAS,
                    Nota = 65,
                    TurmaNome = "Turma 002",
                    ValorConceito = null,
                    ValorMedioNota = 80
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Medio,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = true,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_CIENCIAS,
                    Nota = 85,
                    TurmaNome = "Turma 002",
                    ValorConceito = null,
                    ValorMedioNota = 80
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 2,
                    Modalidade = Modalidade.Medio,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = true,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_CIENCIAS,
                    Nota = 90,
                    TurmaNome = "Turma 002",
                    ValorConceito = null,
                    ValorMedioNota = 80
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 2,
                    Modalidade = Modalidade.Medio,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = false,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_CIENCIAS,
                    Nota = 70,
                    TurmaNome = "Turma 002",
                    ValorConceito = null,
                    ValorMedioNota = 80
                },
                new PainelEducacionalConsolidacaoNotaDadosBrutos
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 2,
                    Modalidade = Modalidade.Medio,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    CodigoUe = "UE002",
                    ConceitoDeAprovado = true,
                    IdComponenteCurricular = ComponentesCurricularesConstants.CODIGO_CIENCIAS,
                    Nota = 95,
                    TurmaNome = "Turma 003",
                    ValorConceito = null,
                    ValorMedioNota = 80
                }
            };

        private static List<PainelEducacionalConsolidacaoNota> GerarConsolidacaoDreEsperado(short anoLetivo) =>
            new List<PainelEducacionalConsolidacaoNota>
            {
                new PainelEducacionalConsolidacaoNota
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE001",
                    AnoTurma = '1',
                    QuantidadeAbaixoMediaPortugues = 2,
                    QuantidadeAcimaMediaPortugues = 1,
                    QuantidadeAbaixoMediaMatematica = 0,
                    QuantidadeAcimaMediaMatematica = 1,
                    QuantidadeAbaixoMediaCiencias = 1,
                    QuantidadeAcimaMediaCiencias = 1
                },
                new PainelEducacionalConsolidacaoNota
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    QuantidadeAbaixoMediaPortugues = 1,
                    QuantidadeAcimaMediaPortugues = 2,
                    QuantidadeAbaixoMediaMatematica = 0,
                    QuantidadeAcimaMediaMatematica = 3,
                    QuantidadeAbaixoMediaCiencias = 2,
                    QuantidadeAcimaMediaCiencias = 2
                },
                new PainelEducacionalConsolidacaoNota
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Medio,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    QuantidadeAbaixoMediaPortugues = 0,
                    QuantidadeAcimaMediaPortugues = 0,
                    QuantidadeAbaixoMediaMatematica = 0,
                    QuantidadeAcimaMediaMatematica = 0,
                    QuantidadeAbaixoMediaCiencias = 1,
                    QuantidadeAcimaMediaCiencias = 1
                },
                new PainelEducacionalConsolidacaoNota
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 2,
                    Modalidade = Modalidade.Medio,
                    CodigoDre = "DRE002",
                    AnoTurma = '2',
                    QuantidadeAbaixoMediaPortugues = 0,
                    QuantidadeAcimaMediaPortugues = 0,
                    QuantidadeAbaixoMediaMatematica = 0,
                    QuantidadeAcimaMediaMatematica = 0,
                    QuantidadeAbaixoMediaCiencias = 1,
                    QuantidadeAcimaMediaCiencias = 2
                }
            };

        private static List<PainelEducacionalConsolidacaoNotaUe> GerarConsolidacaoUeEsperado(short anoLetivo) =>
            new List<PainelEducacionalConsolidacaoNotaUe>
            {
                new PainelEducacionalConsolidacaoNotaUe
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE001",
                    CodigoUe = "UE001",
                    SerieTurma = "Turma 001",
                    QuantidadeAbaixoMediaPortugues = 1,
                    QuantidadeAcimaMediaPortugues = 0,
                    QuantidadeAbaixoMediaMatematica = 0,
                    QuantidadeAcimaMediaMatematica = 1,
                    QuantidadeAbaixoMediaCiencias = 1,
                    QuantidadeAcimaMediaCiencias = 1
                },
                new PainelEducacionalConsolidacaoNotaUe
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE001",
                    CodigoUe = "UE002",
                    SerieTurma = "Turma 001",
                    QuantidadeAbaixoMediaPortugues = 1,
                    QuantidadeAcimaMediaPortugues = 0,
                    QuantidadeAbaixoMediaMatematica = 0,
                    QuantidadeAcimaMediaMatematica = 0,
                    QuantidadeAbaixoMediaCiencias = 0,
                    QuantidadeAcimaMediaCiencias = 0
                },
                new PainelEducacionalConsolidacaoNotaUe
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE001",
                    CodigoUe = "UE002",
                    SerieTurma = "Turma 002",
                    QuantidadeAbaixoMediaPortugues = 0,
                    QuantidadeAcimaMediaPortugues = 1,
                    QuantidadeAbaixoMediaMatematica = 0,
                    QuantidadeAcimaMediaMatematica = 0,
                    QuantidadeAbaixoMediaCiencias = 0,
                    QuantidadeAcimaMediaCiencias = 0
                },
                new PainelEducacionalConsolidacaoNotaUe
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Fundamental,
                    CodigoDre = "DRE002",
                    CodigoUe = "UE002",
                    SerieTurma = "Turma 002",
                    QuantidadeAbaixoMediaPortugues = 1,
                    QuantidadeAcimaMediaPortugues = 2,
                    QuantidadeAbaixoMediaMatematica = 0,
                    QuantidadeAcimaMediaMatematica = 3,
                    QuantidadeAbaixoMediaCiencias = 2,
                    QuantidadeAcimaMediaCiencias = 2
                },
                new PainelEducacionalConsolidacaoNotaUe
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 1,
                    Modalidade = Modalidade.Medio,
                    CodigoDre = "DRE002",
                    CodigoUe = "UE002",
                    SerieTurma = "Turma 002",
                    QuantidadeAbaixoMediaPortugues = 0,
                    QuantidadeAcimaMediaPortugues = 0,
                    QuantidadeAbaixoMediaMatematica = 0,
                    QuantidadeAcimaMediaMatematica = 0,
                    QuantidadeAbaixoMediaCiencias = 1,
                    QuantidadeAcimaMediaCiencias = 1
                },
                new PainelEducacionalConsolidacaoNotaUe
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 2,
                    Modalidade = Modalidade.Medio,
                    CodigoDre = "DRE002",
                    CodigoUe = "UE002",
                    SerieTurma = "Turma 002",
                    QuantidadeAbaixoMediaPortugues = 0,
                    QuantidadeAcimaMediaPortugues = 0,
                    QuantidadeAbaixoMediaMatematica = 0,
                    QuantidadeAcimaMediaMatematica = 0,
                    QuantidadeAbaixoMediaCiencias = 1,
                    QuantidadeAcimaMediaCiencias = 1
                },
                new PainelEducacionalConsolidacaoNotaUe
                {
                    AnoLetivo = anoLetivo,
                    Bimestre = 2,
                    Modalidade = Modalidade.Medio,
                    CodigoDre = "DRE002",
                    CodigoUe = "UE002",
                    SerieTurma = "Turma 003",
                    QuantidadeAbaixoMediaPortugues = 0,
                    QuantidadeAcimaMediaPortugues = 0,
                    QuantidadeAbaixoMediaMatematica = 0,
                    QuantidadeAcimaMediaMatematica = 0,
                    QuantidadeAbaixoMediaCiencias = 0,
                    QuantidadeAcimaMediaCiencias = 1
                }
            };
    }
}
