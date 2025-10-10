using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirConsolidacaoPap;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDadosBrutosParaConsolidacaoIndicadoresPap;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterInformacoesPapUltimoAnoConsolidado;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Servicos;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.Aluno;
using SME.SGP.Infra.Dtos.ConsolidacaoFrequenciaTurma;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesPapPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IServicoPainelEducacionalConsolidacaoIndicadoresPap> _servicoConsolidacaoMock;
        private readonly ConsolidarInformacoesPapPainelEducacionalUseCase _useCase;

        public ConsolidarInformacoesPapPainelEducacionalUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _servicoConsolidacaoMock = new Mock<IServicoPainelEducacionalConsolidacaoIndicadoresPap>();
            _useCase = new ConsolidarInformacoesPapPainelEducacionalUseCase(_mediatorMock.Object, _servicoConsolidacaoMock.Object);
        }

        [Fact]
        public async Task Executar_QuandoHouverDados_DeveChamarServicoESalvarResultado()
        {
            // Arrange
            var anoLetivo = DateTime.Now.Year;
            var mensagemRabbit = new MensagemRabbit();

            var dadosAlunos = new List<DadosMatriculaAlunoTipoPapDto>() { new DadosMatriculaAlunoTipoPapDto() };
            var indicadores = new List<ContagemDificuldadeIndicadoresPapPorTipoDto>();
            var frequencia = new List<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>();

            var consolidadoSme = new List<PainelEducacionalConsolidacaoPapSme>() { new PainelEducacionalConsolidacaoPapSme() };
            var consolidadoDre = new List<PainelEducacionalConsolidacaoPapDre>() { new PainelEducacionalConsolidacaoPapDre() };
            var consolidadoUe = new List<PainelEducacionalConsolidacaoPapUe>() { new PainelEducacionalConsolidacaoPapUe() };

            ConfigurarMockMediator(anoLetivo - 1, (dadosAlunos, indicadores, frequencia));

            _servicoConsolidacaoMock
                .Setup(s => s.ConsolidarDados(dadosAlunos, indicadores, frequencia))
                .Returns((consolidadoSme, consolidadoDre, consolidadoUe));

            // Act
            var resultado = await _useCase.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeTrue();

            _servicoConsolidacaoMock.Verify(s => s.ConsolidarDados(dadosAlunos, indicadores, frequencia), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirConsolidacaoPapCommand>(c => c.AnoLetivo == anoLetivo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarConsolidacaoPapSmeCommand>(c => c.Consolidacao == consolidadoSme), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarConsolidacaoPapDreCommand>(c => c.Consolidacao == consolidadoDre), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarConsolidacaoPapUeCommand>(c => c.Consolidacao == consolidadoUe), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_QuandoDadosBrutosVazios_NaoDeveChamarServicoOuComandosDePersistencia()
        {
            // Arrange
            var anoLetivo = DateTime.Now.Year;
            var mensagemRabbit = new MensagemRabbit();

            ConfigurarMockMediator(anoLetivo - 1, (new List<DadosMatriculaAlunoTipoPapDto>(), null, null));

            // Act
            var resultado = await _useCase.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeTrue();

            _servicoConsolidacaoMock.Verify(s => s.ConsolidarDados(It.IsAny<IEnumerable<DadosMatriculaAlunoTipoPapDto>>(), It.IsAny<IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto>>(), It.IsAny<IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirConsolidacaoPapCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoPapSmeCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_QuandoNenhumAnoConsolidado_DeveProcessarDesdeAnoLimite()
        {
            // Arrange
            var anoAtual = DateTime.Now.Year;
            var mensagemRabbit = new MensagemRabbit();
            var totalAnosEsperado = anoAtual - PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE + 1;

            ConfigurarMockMediator(0, (new List<DadosMatriculaAlunoTipoPapDto> { new DadosMatriculaAlunoTipoPapDto() }, null, null));

            // Act
            await _useCase.Executar(mensagemRabbit);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(totalAnosEsperado));
        }

        [Fact]
        public async Task Executar_QuandoAnoCorrenteJaConsolidado_DeveReprocessarApenasAnoCorrente()
        {
            // Arrange
            var anoAtual = DateTime.Now.Year;
            var mensagemRabbit = new MensagemRabbit();

            ConfigurarMockMediator(anoAtual, (new List<DadosMatriculaAlunoTipoPapDto> { new DadosMatriculaAlunoTipoPapDto() }, null, null));

            // Act
            await _useCase.Executar(mensagemRabbit);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery>(q => q.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()), Times.Once);
        }

        private void ConfigurarMockMediator(int ultimoAnoConsolidado, (IEnumerable<DadosMatriculaAlunoTipoPapDto>, IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto>, IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>) dadosBrutos)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterInformacoesPapUltimoAnoConsolidadoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(ultimoAnoConsolidado);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dadosBrutos);
        }
    }
}