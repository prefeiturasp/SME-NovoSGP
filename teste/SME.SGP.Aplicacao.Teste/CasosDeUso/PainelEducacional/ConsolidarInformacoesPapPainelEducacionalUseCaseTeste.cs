using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirConsolidacaoPap;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDadosBrutosParaConsolidacaoIndicadoresPap;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.Frequencia;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesPapPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidarInformacoesPapPainelEducacionalUseCase _handler;

        public ConsolidarInformacoesPapPainelEducacionalUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _handler = new ConsolidarInformacoesPapPainelEducacionalUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_QuandoHouverDados_DeveConsolidarCorretamenteParaSmeDreEUe()
        {
            // Arrange
            var anoLetivo = DateTime.Now.Year;
            const string codigoDre = "108300";
            const string codigoUe = "094416";

            var mensagemRabbit = new MensagemRabbit();

            var dadosAlunos = GerarDadosAlunos(anoLetivo, codigoDre, codigoUe);
            var indicadores = GerarDadosIndicadores(anoLetivo, codigoDre, codigoUe);
            var frequencia = GerarDadosFrequencia();

            ConfigurarMockMediator(anoLetivo - 1, dadosAlunos, indicadores, frequencia);

            IList<PainelEducacionalConsolidacaoPapSme> consolidadoSmeCapturado = null;
            IList<PainelEducacionalConsolidacaoPapDre> consolidadoDreCapturado = null;
            IList<PainelEducacionalConsolidacaoPapUe> consolidadoUeCapturado = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarConsolidacaoPapSmeCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, ct) => consolidadoSmeCapturado = ((SalvarConsolidacaoPapSmeCommand)cmd).Consolidacao);

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarConsolidacaoPapDreCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, ct) => consolidadoDreCapturado = ((SalvarConsolidacaoPapDreCommand)cmd).Consolidacao);

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarConsolidacaoPapUeCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, ct) => consolidadoUeCapturado = ((SalvarConsolidacaoPapUeCommand)cmd).Consolidacao);


            // Act
            var resultado = await _handler.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeTrue();

            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirConsolidacaoPapCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoPapSmeCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoPapDreCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoPapUeCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());

            // Asserts SME
            consolidadoSmeCapturado.Should().NotBeNull();
            consolidadoSmeCapturado.Should().HaveCount(1);
            var sme = consolidadoSmeCapturado.First();
            sme.AnoLetivo.Should().Be(anoLetivo);
            sme.TipoPap.Should().Be(TipoPap.PapColaborativo);
            sme.TotalTurmas.Should().Be(4);
            sme.TotalAlunos.Should().Be(7);
            sme.TotalAlunosComFrequenciaInferiorLimite.Should().Be(40);
            sme.NomeDificuldadeTop1.Should().Be("Dificuldade Alta");
            sme.TotalAlunosDificuldadeTop1.Should().Be(100);
            sme.NomeDificuldadeTop2.Should().Be("Dificuldade Média");
            sme.TotalAlunosDificuldadeTop2.Should().Be(50);
            sme.TotalAlunosDificuldadeOutras.Should().Be(5);

            // Asserts DRE
            consolidadoDreCapturado.Should().NotBeNull();
            consolidadoDreCapturado.Should().HaveCount(1);
            var dre = consolidadoDreCapturado.First();
            dre.CodigoDre.Should().Be(codigoDre);
            dre.AnoLetivo.Should().Be(anoLetivo);
            dre.TipoPap.Should().Be(TipoPap.PapColaborativo);
            dre.TotalTurmas.Should().Be(3);
            dre.TotalAlunos.Should().Be(5);
            dre.TotalAlunosComFrequenciaInferiorLimite.Should().Be(30);
            dre.NomeDificuldadeTop1.Should().Be("Leitura");
            dre.TotalAlunosDificuldadeTop1.Should().Be(120);
            dre.NomeDificuldadeTop2.Should().Be("Escrita");
            dre.TotalAlunosDificuldadeTop2.Should().Be(60);
            dre.TotalAlunosDificuldadeOutras.Should().Be(10);

            // Asserts UE
            consolidadoUeCapturado.Should().NotBeNull();
            consolidadoUeCapturado.Should().HaveCount(1);
            var ue = consolidadoUeCapturado.First();
            ue.CodigoDre.Should().Be(codigoDre);
            ue.CodigoUe.Should().Be(codigoUe);
            ue.AnoLetivo.Should().Be(anoLetivo);
            ue.TipoPap.Should().Be(TipoPap.PapColaborativo);
            ue.TotalTurmas.Should().Be(2);
            ue.TotalAlunos.Should().Be(3);
            ue.TotalAlunosComFrequenciaInferiorLimite.Should().Be(15);
            ue.NomeDificuldadeTop1.Should().Be("Cálculo");
            ue.TotalAlunosDificuldadeTop1.Should().Be(80);
            ue.NomeDificuldadeTop2.Should().Be("Raciocínio");
            ue.TotalAlunosDificuldadeTop2.Should().Be(40);
            ue.TotalAlunosDificuldadeOutras.Should().Be(20);
        }

        [Fact]
        public async Task Executar_QuandoNenhumAnoConsolidado_DeveIniciarPeloAnoLimite()
        {
            // Arrange
            var anoAtual = DateTime.Now.Year;
            var mensagemRabbit = new MensagemRabbit();

            ConfigurarMockMediator(0, new List<DadosMatriculaAlunoTipoPapDto>(), new List<ContagemDificuldadeIndicadoresPapPorTipoDto>(), new List<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>());

            // Act
            await _handler.Executar(mensagemRabbit);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery>(q => q.AnoLetivo >= PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
            _mediatorMock.Verify(m => m.Send(It.Is<ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery>(q => q.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Executar_QuandoAnoCorrenteJaConsolidado_DeveReprocessarApenasAnoCorrente()
        {
            // Arrange
            var anoAtual = DateTime.Now.Year;
            var mensagemRabbit = new MensagemRabbit();

            ConfigurarMockMediator(anoAtual, new List<DadosMatriculaAlunoTipoPapDto>(), new List<ContagemDificuldadeIndicadoresPapPorTipoDto>(), new List<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>());

            // Act
            await _handler.Executar(mensagemRabbit);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery>(q => q.AnoLetivo == anoAtual), It.IsAny<CancellationToken>()), Times.Once());
            _mediatorMock.Verify(m => m.Send(It.Is<ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery>(q => q.AnoLetivo < anoAtual), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task Executar_QuandoDadosBrutosVazios_NaoDeveExcluirOuSalvar()
        {
            // Arrange
            var anoLetivo = DateTime.Now.Year;
            var mensagemRabbit = new MensagemRabbit();

            ConfigurarMockMediator(anoLetivo - 1, new List<DadosMatriculaAlunoTipoPapDto>(), new List<ContagemDificuldadeIndicadoresPapPorTipoDto>(), new List<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>());

            // Act
            var resultado = await _handler.Executar(mensagemRabbit);

            // Assert
            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirConsolidacaoPapCommand>(), It.IsAny<CancellationToken>()), Times.Never());
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoPapSmeCommand>(), It.IsAny<CancellationToken>()), Times.Never());
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoPapDreCommand>(), It.IsAny<CancellationToken>()), Times.Never());
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoPapUeCommand>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        #region Metodos Auxiliares
        private void ConfigurarMockMediator(int ultimoAnoConsolidado,
                                            IEnumerable<DadosMatriculaAlunoTipoPapDto> dadosAlunos,
                                            IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> indicadores,
                                            IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto> frequencia)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterInformacoesPapUltimoAnoConsolidadoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(ultimoAnoConsolidado);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosBrutosParaConsolidacaoIndicadoresPapQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((dadosAlunos, indicadores, frequencia));
        }

        private IEnumerable<DadosMatriculaAlunoTipoPapDto> GerarDadosAlunos(int anoLetivo, string codigoDre, string codigoUe)
        {
            return new List<DadosMatriculaAlunoTipoPapDto>
            {
                // Cenário UE
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = anoLetivo, CodigoDre = codigoDre, CodigoUe = codigoUe, CodigoTurma = 1, CodigoAluno = 101, TipoPap = TipoPap.PapColaborativo },
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = anoLetivo, CodigoDre = codigoDre, CodigoUe = codigoUe, CodigoTurma = 1, CodigoAluno = 102, TipoPap = TipoPap.PapColaborativo },
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = anoLetivo, CodigoDre = codigoDre, CodigoUe = codigoUe, CodigoTurma = 2, CodigoAluno = 103, TipoPap = TipoPap.PapColaborativo },
                // Cenário DRE (mesmo aluno da UE para testar distinct)
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = anoLetivo, CodigoDre = codigoDre, CodigoUe = "OUTRA_UE", CodigoTurma = 3, CodigoAluno = 201, TipoPap = TipoPap.PapColaborativo },
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = anoLetivo, CodigoDre = codigoDre, CodigoUe = "OUTRA_UE", CodigoTurma = 3, CodigoAluno = 202, TipoPap = TipoPap.PapColaborativo },
                 // Cenário SME (mesmo aluno da UE para testar distinct)
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = anoLetivo, CodigoDre = "OUTRA_DRE", CodigoUe = "OUTRA_UE_2", CodigoTurma = 4, CodigoAluno = 301, TipoPap = TipoPap.PapColaborativo },
                new DadosMatriculaAlunoTipoPapDto { AnoLetivo = anoLetivo, CodigoDre = "OUTRA_DRE", CodigoUe = "OUTRA_UE_2", CodigoTurma = 4, CodigoAluno = 302, TipoPap = TipoPap.PapColaborativo },
            };
        }

        private IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto> GerarDadosIndicadores(int anoLetivo, string codigoDre, string codigoUe)
        {
            return new List<ContagemDificuldadeIndicadoresPapPorTipoDto>
            {
                // Indicadores UE
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = anoLetivo, CodigoDre = codigoDre, CodigoUe = codigoUe, TipoPap = TipoPap.PapColaborativo, NomeDificuldade = "Cálculo", Quantidade = 80, RespostaId = 1 },
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = anoLetivo, CodigoDre = codigoDre, CodigoUe = codigoUe, TipoPap = TipoPap.PapColaborativo, NomeDificuldade = "Raciocínio", Quantidade = 40, RespostaId = 2 },
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = anoLetivo, CodigoDre = codigoDre, CodigoUe = codigoUe, TipoPap = TipoPap.PapColaborativo, NomeDificuldade = "Problemas", Quantidade = 10, RespostaId = 3 },
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = anoLetivo, CodigoDre = codigoDre, CodigoUe = codigoUe, TipoPap = TipoPap.PapColaborativo, NomeDificuldade = PainelEducacionalConstants.NOME_OUTRAS_DIFICULDADES_PAP, Quantidade = 20, RespostaId = PainelEducacionalConstants.ID_OUTRAS_DIFICULDADES_PAP },
                
                // Indicadores DRE
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = anoLetivo, CodigoDre = codigoDre, CodigoUe = null, TipoPap = TipoPap.PapColaborativo, NomeDificuldade = "Leitura", Quantidade = 120, RespostaId = 4 },
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = anoLetivo, CodigoDre = codigoDre, CodigoUe = null, TipoPap = TipoPap.PapColaborativo, NomeDificuldade = "Escrita", Quantidade = 60, RespostaId = 5 },
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = anoLetivo, CodigoDre = codigoDre, CodigoUe = null, TipoPap = TipoPap.PapColaborativo, NomeDificuldade = PainelEducacionalConstants.NOME_OUTRAS_DIFICULDADES_PAP, Quantidade = 10, RespostaId = PainelEducacionalConstants.ID_OUTRAS_DIFICULDADES_PAP },
                
                // Indicadores SME
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = anoLetivo, CodigoDre = null, CodigoUe = null, TipoPap = TipoPap.PapColaborativo, NomeDificuldade = "Dificuldade Alta", Quantidade = 100, RespostaId = 6 },
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = anoLetivo, CodigoDre = null, CodigoUe = null, TipoPap = TipoPap.PapColaborativo, NomeDificuldade = "Dificuldade Média", Quantidade = 50, RespostaId = 7 },
                new ContagemDificuldadeIndicadoresPapPorTipoDto { AnoLetivo = anoLetivo, CodigoDre = null, CodigoUe = null, TipoPap = TipoPap.PapColaborativo, NomeDificuldade = PainelEducacionalConstants.NOME_OUTRAS_DIFICULDADES_PAP, Quantidade = 5, RespostaId = PainelEducacionalConstants.ID_OUTRAS_DIFICULDADES_PAP },
            };
        }

        private IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto> GerarDadosFrequencia()
        {
            return new List<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>
            {
                new QuantitativoAlunosFrequenciaBaixaPorTurmaDto { CodigoTurma = 1, QuantidadeAbaixoMinimoFrequencia = 10 },
                new QuantitativoAlunosFrequenciaBaixaPorTurmaDto { CodigoTurma = 2, QuantidadeAbaixoMinimoFrequencia = 5 },
                new QuantitativoAlunosFrequenciaBaixaPorTurmaDto { CodigoTurma = 3, QuantidadeAbaixoMinimoFrequencia = 15 },
                new QuantitativoAlunosFrequenciaBaixaPorTurmaDto { CodigoTurma = 4, QuantidadeAbaixoMinimoFrequencia = 10 },
            };
        }
        #endregion
    }
}
