using Bogus;
using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAbandono;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandonoUltimoAnoConsolidado;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterTurmasPainelEducacional;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.SituacaoAluno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsolidarAbandonoPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidarAbandonoPainelEducacionalUseCase _useCase;
        private readonly Faker _faker;

        public ConsolidarAbandonoPainelEducacionalUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsolidarAbandonoPainelEducacionalUseCase(_mediatorMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task DadoNenhumIndicadorGerado_QuandoExecutar_DeveRetornarFalsoENaoSalvar()
        {
            // Arrange
            var anoAtual = DateTime.Now.Year;
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbandonoUltimoAnoConsolidadoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(anoAtual - 1);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasPainelEducacionalQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<TurmaPainelEducacionalDto>());

            // Act
            var resultado = await _useCase.Executar(new MensagemRabbit());

            // Assert
            resultado.Should().BeFalse();
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoAbandonoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DadoIndicadoresGerados_QuandoExecutarAPartirDoAnoLimite_DeveSalvarERetornarVerdadeiro()
        {
            // Arrange
            var anoAtual = DateTime.Now.Year;
            var anoLimite = PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE;
            var anosParaProcessar = Enumerable.Range(anoLimite, anoAtual - anoLimite + 1).ToList();

            ConfigurarMocksParaAnos(anosParaProcessar);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbandonoUltimoAnoConsolidadoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(0);

            // Act
            var resultado = await _useCase.Executar(new MensagemRabbit());

            // Assert
            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmasPainelEducacionalQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(anosParaProcessar.Count));
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoAbandonoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DadoIndicadoresGerados_QuandoExecutarAPartirDoUltimoAnoConsolidado_DeveSalvarERetornarVerdadeiro()
        {
            // Arrange
            var anoAtual = DateTime.Now.Year;
            var ultimoAnoConsolidado = anoAtual - 2;
            var anosParaProcessar = new List<int> { anoAtual - 1, anoAtual };

            ConfigurarMocksParaAnos(anosParaProcessar);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbandonoUltimoAnoConsolidadoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(ultimoAnoConsolidado);

            // Act
            var resultado = await _useCase.Executar(new MensagemRabbit());

            // Assert
            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmasPainelEducacionalQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoAbandonoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DadoJaConsolidadoNoAnoAtual_QuandoExecutar_DeveProcessarApenasAnoAtualESalvar()
        {
            // Arrange
            var anoAtual = DateTime.Now.Year;

            ConfigurarMocksParaAnos(new List<int> { anoAtual });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbandonoUltimoAnoConsolidadoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(anoAtual);

            // Act
            var resultado = await _useCase.Executar(new MensagemRabbit());

            // Assert
            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmasPainelEducacionalQuery>(q => q.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoAbandonoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DadoUmaDreSemAlunosEmAbandono_QuandoExecutar_DeveIgnorarADreEConsolidarAsDemais()
        {
            // Arrange
            var anoAtual = DateTime.Now.Year;
            var codigoDreComAlunos = "DRE-01";
            var codigoDreSemAlunos = "DRE-02";

            var turmas = GerarTurmasDto(2, anoAtual);
            turmas[0].CodigoDre = codigoDreComAlunos;
            turmas[1].CodigoDre = codigoDreSemAlunos;

            var alunos = GerarAlunosSituacao(turmas.Where(t => t.CodigoDre == codigoDreComAlunos).ToList());

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbandonoUltimoAnoConsolidadoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(anoAtual - 1);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmasPainelEducacionalQuery>(q => q.AnoLetivo == anoAtual), It.IsAny<CancellationToken>())).ReturnsAsync(turmas);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAlunosSituacaoTurmasQuery>(q => q.CodigoDre == codigoDreComAlunos), It.IsAny<CancellationToken>())).ReturnsAsync(alunos);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAlunosSituacaoTurmasQuery>(q => q.CodigoDre == codigoDreSemAlunos), It.IsAny<CancellationToken>())).ReturnsAsync(new List<AlunosSituacaoTurmas>());

            SalvarPainelEducacionalConsolidacaoAbandonoCommand comandoCapturado = null;
            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoAbandonoCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, token) => comandoCapturado = cmd as SalvarPainelEducacionalConsolidacaoAbandonoCommand)
                         .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.Executar(new MensagemRabbit());

            // Assert
            resultado.Should().BeTrue();
            comandoCapturado.Should().NotBeNull();
            comandoCapturado.IndicadoresDre.All(i => i.CodigoDre == codigoDreComAlunos).Should().BeTrue();
            comandoCapturado.IndicadoresUe.All(i => i.CodigoDre == codigoDreComAlunos).Should().BeTrue();
        }

        private void ConfigurarMocksParaAnos(IEnumerable<int> anos)
        {
            foreach (var ano in anos)
            {
                var turmas = GerarTurmasDto(3, ano);
                var alunos = GerarAlunosSituacao(turmas);
                var dresDistintas = turmas.Select(t => t.CodigoDre).Distinct();

                _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmasPainelEducacionalQuery>(q => q.AnoLetivo == ano), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(turmas);

                foreach (var dre in dresDistintas)
                {
                    _mediatorMock.Setup(m => m.Send(It.Is<ObterAlunosSituacaoTurmasQuery>(q => q.AnoLetivo == ano && q.CodigoDre == dre), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(alunos.Where(a => turmas.First(t => t.TurmaId == a.CodigoTurma).CodigoDre == dre).ToList());
                }
            }

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoAbandonoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);
        }

        private List<TurmaPainelEducacionalDto> GerarTurmasDto(int quantidade, int anoLetivo)
        {
            return new Faker<TurmaPainelEducacionalDto>("pt_BR")
                .RuleFor(t => t.TurmaId, f => f.Random.Guid().ToString())
                .RuleFor(t => t.CodigoDre, f => $"DRE-{f.Random.Number(1, 3)}")
                .RuleFor(t => t.CodigoUe, f => f.Random.AlphaNumeric(10))
                .RuleFor(t => t.Nome, f => $"Turma {f.Commerce.ProductName()}")
                .RuleFor(t => t.ModalidadeCodigo, (int)Modalidade.Fundamental)
                .RuleFor(t => t.AnoLetivo, anoLetivo)
                .RuleFor(t => t.Ano, f => f.Random.Number(1, 9).ToString())
                .Generate(quantidade);
        }

        private List<AlunosSituacaoTurmas> GerarAlunosSituacao(List<TurmaPainelEducacionalDto> turmas)
        {
            var alunos = new List<AlunosSituacaoTurmas>();
            foreach (var turma in turmas)
            {
                alunos.Add(new Faker<AlunosSituacaoTurmas>()
                    .RuleFor(a => a.AnoLetivo, turma.AnoLetivo)
                    .RuleFor(a => a.CodigoTurma, turma.TurmaId)
                    .RuleFor(a => a.QuantidadeAlunos, f => f.Random.Number(1, 5))
                    .RuleFor(a => a.CodigoSituacaoMatricula, (int)SituacaoMatriculaAluno.Desistente)
                    .Generate());
            }
            return alunos;
        }
    }
}