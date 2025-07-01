using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.ConselhoClasse;
using SME.SGP.Infra.Dtos.Questionario;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Testes.Controllers
{
    public class ConselhoClasseControllerTeste
    {
        private readonly ConselhoClasseController _controller;
        private readonly Faker _faker;

        public ConselhoClasseControllerTeste()
        {
            _controller = new ConselhoClasseController();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter recomendações do aluno")]
        public async Task DeveChamarUseCase_ParaObterRecomendacoesAlunoFamilia()
        {
            // Arrange
            var useCaseMock = new Mock<IConsultaConselhoClasseRecomendacaoUseCase>();
            var conselhoClasseId = _faker.Random.Long(1);
            var fechamentoTurmaId = _faker.Random.Long(1);
            var alunoCodigo = _faker.Random.AlphaNumeric(8);
            var codigoTurma = _faker.Random.AlphaNumeric(5);
            var bimestre = _faker.Random.Int(1, 4);

            var retornoDto = new ConsultasConselhoClasseRecomendacaoConsultaDto();
            useCaseMock.Setup(u => u.Executar(It.Is<ConselhoClasseRecomendacaoDto>(d =>
                d.ConselhoClasseId == conselhoClasseId &&
                d.AlunoCodigo == alunoCodigo &&
                d.Bimestre == bimestre
            ))).ReturnsAsync(retornoDto);

            // Act
            var resultado = await _controller.ObterRecomendacoesAlunoFamilia(useCaseMock.Object, conselhoClasseId, fechamentoTurmaId, alunoCodigo, codigoTurma, bimestre);

            // Assert
            useCaseMock.Verify(u => u.Executar(It.IsAny<ConselhoClasseRecomendacaoDto>()), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retornoDto, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para salvar recomendações do aluno")]
        public async Task DeveChamarUseCase_ParaSalvarRecomendacoes()
        {
            // Arrange
            var useCaseMock = new Mock<ISalvarConselhoClasseAlunoRecomendacaoUseCase>();
            var conselhoDto = new ConselhoClasseAlunoAnotacoesDto { AlunoCodigo = _faker.Random.AlphaNumeric(8) };
            var retorno = new ConselhoClasseAluno();

            useCaseMock.Setup(u => u.Executar(conselhoDto)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.SalvarRecomendacoesAlunoFamilia(conselhoDto, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(conselhoDto), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retorno, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para persistir notas")]
        public async Task DeveChamarUseCase_ParaPersistirNotas()
        {
            // Arrange
            var useCaseMock = new Mock<ISalvarConselhoClasseAlunoNotaUseCase>();
            var conselhoClasseNotaDto = new ConselhoClasseNotaDto { Justificativa = "Teste" };
            var alunoCodigo = _faker.Random.AlphaNumeric(8);
            var conselhoClasseId = _faker.Random.Long(1);
            var fechamentoTurmaId = _faker.Random.Long(1);
            var codigoTurma = _faker.Random.AlphaNumeric(5);
            var bimestre = _faker.Random.Int(1, 4);
            var retorno = new ConselhoClasseNotaRetornoDto();

            useCaseMock.Setup(u => u.Executar(It.Is<SalvarConselhoClasseAlunoNotaDto>(d =>
                d.ConselhoClasseId == conselhoClasseId &&
                d.CodigoAluno == alunoCodigo &&
                d.Bimestre == bimestre
            ))).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.PersistirNotas(useCaseMock.Object, conselhoClasseNotaDto, alunoCodigo, conselhoClasseId, fechamentoTurmaId, codigoTurma, bimestre);

            // Assert
            useCaseMock.Verify(u => u.Executar(It.IsAny<SalvarConselhoClasseAlunoNotaDto>()), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retorno, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar consulta para obter detalhamento de nota")]
        public void DeveChamarConsulta_ParaObterDetalhamentoNota()
        {
            // Arrange
            var consultaMock = new Mock<IConsultasConselhoClasseNota>();
            var id = _faker.Random.Long(1);
            var retorno = new ConselhoClasseNota();

            consultaMock.Setup(c => c.ObterPorId(id)).Returns(retorno);

            // Act
            var resultado = _controller.DetalhamentoNota(id, consultaMock.Object);

            // Assert
            consultaMock.Verify(c => c.ObterPorId(id), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retorno, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar consulta para obter conselho de classe da turma")]
        public async Task DeveChamarConsulta_ParaObterConselhoClasseTurma()
        {
            // Arrange
            var consultaMock = new Mock<IConsultasConselhoClasse>();
            var turmaCodigo = _faker.Random.AlphaNumeric(5);
            var alunoCodigo = _faker.Random.AlphaNumeric(8);
            var retornoDto = new ConselhoClasseAlunoResumoDto();

            consultaMock.Setup(c => c.ObterConselhoClasseTurma(turmaCodigo, alunoCodigo, It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
                        .ReturnsAsync(retornoDto);

            // Act
            var resultado = await _controller.ObterConselhoClasseTurma(turmaCodigo, 1, alunoCodigo, false, false, consultaMock.Object);

            // Assert
            consultaMock.Verify(c => c.ObterConselhoClasseTurma(turmaCodigo, alunoCodigo, 1, false, false), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retornoDto, okResult.Value);
        }

        [Fact(DisplayName = "Deve retornar Ok quando encontrar conselho de classe final")]
        public async Task DeveRetornarOk_QuandoEncontrarConselhoClasseFinal()
        {
            // Arrange
            var consultaMock = new Mock<IConsultasConselhoClasse>();
            var turmaCodigo = _faker.Random.AlphaNumeric(5);
            var alunoCodigo = _faker.Random.AlphaNumeric(8);
            var retornoDto = new ConselhoClasseAlunoResumoDto { AnoLetivo = DateTime.Now.Year };

            consultaMock.Setup(c => c.ObterConselhoClasseTurmaFinal(turmaCodigo, alunoCodigo, false))
                        .ReturnsAsync(retornoDto);

            // Act
            var resultado = await _controller.ObterConselhoClasseTurmaFinal(turmaCodigo, alunoCodigo, false, consultaMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<ConselhoClasseAlunoResumoDto>(okResult.Value);
            Assert.Same(retornoDto, valorRetornado);
        }

        [Fact(DisplayName = "Deve retornar NoContent quando não encontrar conselho de classe final")]
        public async Task DeveRetornarNoContent_QuandoNaoEncontrarConselhoFinal()
        {
            // Arrange
            var consultaMock = new Mock<IConsultasConselhoClasse>();
            var turmaCodigo = _faker.Random.AlphaNumeric(5);
            var alunoCodigo = _faker.Random.AlphaNumeric(8);

            consultaMock.Setup(c => c.ObterConselhoClasseTurmaFinal(turmaCodigo, alunoCodigo, false))
                        .ReturnsAsync((ConselhoClasseAlunoResumoDto)null);

            // Act
            var resultado = await _controller.ObterConselhoClasseTurmaFinal(turmaCodigo, alunoCodigo, false, consultaMock.Object);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter parecer conclusivo por turma e aluno")]
        public async Task DeveChamarUseCase_ParaObterParecerConclusivoPorTurmaAluno()
        {
            // Arrange
            var useCaseMock = new Mock<IObterParecerConclusivoAlunoTurmaUseCase>();
            var codigoTurma = _faker.Random.AlphaNumeric(5);
            var alunoCodigo = _faker.Random.AlphaNumeric(8);
            var retorno = new ParecerConclusivoDto();

            useCaseMock.Setup(u => u.Executar(codigoTurma, alunoCodigo)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterParecerConclusivoPorTurmaAluno(codigoTurma, alunoCodigo, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(codigoTurma, alunoCodigo), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para gerar parecer conclusivo do aluno")]
        public async Task DeveChamarUseCase_ParaGerarParecerConclusivoAluno()
        {
            // Arrange
            var useCaseMock = new Mock<IGerarParecerConclusivoUseCase>();
            var conselhoClasseId = _faker.Random.Long(1);
            var fechamentoTurmaId = _faker.Random.Long(1);
            var alunoCodigo = _faker.Random.AlphaNumeric(8);
            var retorno = new ParecerConclusivoDto();

            useCaseMock.Setup(u => u.Executar(It.Is<ConselhoClasseFechamentoAlunoDto>(d => d.ConselhoClasseId == conselhoClasseId && d.AlunoCodigo == alunoCodigo)))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.GerarParecerConclusivoAluno(conselhoClasseId, fechamentoTurmaId, alunoCodigo, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(It.IsAny<ConselhoClasseFechamentoAlunoDto>()), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter sínteses do conselho de classe")]
        public async Task DeveChamarUseCase_ParaObterSintesesConselhoDeClasse()
        {
            // Arrange
            var useCaseMock = new Mock<IObterSinteseConselhoDeClasseUseCase>();
            var conselhoClasseId = _faker.Random.Long(1);
            var retorno = new List<ConselhoDeClasseGrupoMatrizDto>();

            useCaseMock.Setup(u => u.Executar(It.Is<ConselhoClasseSinteseDto>(d => d.ConselhoClasseId == conselhoClasseId)))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterSintesesConselhoDeClasse(conselhoClasseId, 1, "1", "1", 1, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(It.IsAny<ConselhoClasseSinteseDto>()), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter notas do aluno")]
        public async Task DeveChamarUseCase_ParaObterNotasAluno()
        {
            // Arrange
            var useCaseMock = new Mock<IObterNotasFrequenciaUseCase>();
            var conselhoClasseId = _faker.Random.Long(1);
            var retorno = new ConselhoClasseAlunoNotasConceitosRetornoDto();

            useCaseMock.Setup(u => u.Executar(It.Is<ConselhoClasseNotasFrequenciaDto>(d => d.ConselhoClasseId == conselhoClasseId)))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterNotasAluno(conselhoClasseId, 1, "1", "1", 1, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(It.IsAny<ConselhoClasseNotasFrequenciaDto>()), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para imprimir conselho de classe da turma")]
        public async Task DeveChamarUseCase_ParaImprimirConselhoTurma()
        {
            // Arrange
            var useCaseMock = new Mock<IImpressaoConselhoClasseTurmaUseCase>();
            var conselhoClasseId = _faker.Random.Long(1);
            var fechamentoTurmaId = _faker.Random.Long(1);

            useCaseMock.Setup(u => u.Executar(It.Is<FiltroRelatorioConselhoClasseDto>(f =>
                f.ConselhoClasseId == conselhoClasseId &&
                f.FechamentoTurmaId == fechamentoTurmaId
            ))).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ImprimirConselhoTurma(conselhoClasseId, fechamentoTurmaId, useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para imprimir conselho de classe do aluno")]
        public async Task DeveChamarUseCase_ParaImprimirConselhoAluno()
        {
            // Arrange
            var useCaseMock = new Mock<IImpressaoConselhoClasseAlunoUseCase>();
            var conselhoClasseId = _faker.Random.Long(1);
            var fechamentoTurmaId = _faker.Random.Long(1);
            var alunoCodigo = _faker.Random.AlphaNumeric(8);

            useCaseMock.Setup(u => u.Executar(It.Is<FiltroRelatorioConselhoClasseAlunoDto>(f =>
                f.ConselhoClasseId == conselhoClasseId &&
                f.FechamentoTurmaId == fechamentoTurmaId &&
                f.CodigoAluno == alunoCodigo
            ))).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ImprimirConselhoAluno(conselhoClasseId, fechamentoTurmaId, alunoCodigo, useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para listar pareceres conclusivos")]
        public async Task DeveChamarUseCase_ParaListarPareceresConclusivos()
        {
            // Arrange
            var useCaseMock = new Mock<IObterPareceresConclusivosUseCase>();
            var retorno = new List<ConselhoClasseParecerConclusivoDto>();

            useCaseMock.Setup(u => u.Executar()).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ListarPareceresConclusivos(useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter bimestres com conselho de classe")]
        public async Task DeveChamarUseCase_ParaObterBimestresComConselho()
        {
            // Arrange
            var useCaseMock = new Mock<IObterBimestresComConselhoClasseTurmaUseCase>();
            var turmaId = _faker.Random.Long(1);
            var retorno = new List<BimestreComConselhoClasseTurmaDto>();

            useCaseMock.Setup(u => u.Executar(turmaId)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterBimestresComConselhoClasseTurma(turmaId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(turmaId), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para reprocessar situação do conselho")]
        public async Task DeveChamarUseCase_ParaReprocessarSituacaoConselho()
        {
            // Arrange
            var useCaseMock = new Mock<IConsolidarConselhoClasseUseCase>();
            var dreId = _faker.Random.Int(1);

            useCaseMock.Setup(u => u.Executar(dreId)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.ReprocessarSituacaoConselhoClasseAluno(dreId, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(dreId), Times.Once);
            Assert.IsType<OkResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter total de aulas por aluno")]
        public async Task DeveChamarUseCase_ParaObterTotalAulasPorAlunoTurma()
        {
            // Arrange
            var useCaseMock = new Mock<IObterTotalAulasPorAlunoTurmaUseCase>();
            var alunoCodigo = _faker.Random.AlphaNumeric(8);
            var turmaCodigo = _faker.Random.AlphaNumeric(5);
            var retorno = new List<TotalAulasPorAlunoTurmaDto>();

            useCaseMock.Setup(u => u.Executar(alunoCodigo, turmaCodigo)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterTotalAulasPorAlunoTurma(alunoCodigo, turmaCodigo, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(alunoCodigo, turmaCodigo), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter total de aulas sem frequência")]
        public async Task DeveChamarUseCase_ParaObterTotalAulasSemFrequencia()
        {
            // Arrange
            var useCaseMock = new Mock<IObterTotalAulasSemFrequenciaPorTurmaUseCase>();
            var disciplinaId = "1106"; // Valor fixo no controller
            var turmaCodigo = _faker.Random.AlphaNumeric(5);
            var retorno = new List<TotalAulasPorAlunoTurmaDto>();

            useCaseMock.Setup(u => u.Executar(disciplinaId, turmaCodigo)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterTotalAulasSemFrequenciaPorTurma(turmaCodigo, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(disciplinaId, turmaCodigo), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter total de aulas que não lançam nota")]
        public async Task DeveChamarUseCase_ParaObterTotalAulasNaoLancamNotas()
        {
            // Arrange
            var useCaseMock = new Mock<IObterTotalAulasNaoLancamNotaUseCase>();
            var turmaCodigo = _faker.Random.AlphaNumeric(5);
            var bimestre = _faker.Random.Int(1, 4);
            var alunoCodigo = _faker.Random.AlphaNumeric(8);
            var retorno = new List<TotalAulasNaoLancamNotaDto>();

            useCaseMock.Setup(u => u.Executar(turmaCodigo, bimestre, alunoCodigo)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterTotalAulasNaoLancamNotasPorTurmaBimestre(turmaCodigo, bimestre, alunoCodigo, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(turmaCodigo, bimestre, alunoCodigo), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter total de compensações")]
        public async Task DeveChamarUseCase_ParaObterTotalCompensacoes()
        {
            // Arrange
            var useCaseMock = new Mock<IObterTotalCompensacoesComponenteNaoLancaNotaUseCase>();
            var turmaCodigo = _faker.Random.AlphaNumeric(5);
            var bimestre = _faker.Random.Int(1, 4);
            var retorno = new List<TotalCompensacoesComponenteNaoLancaNotaDto>();

            useCaseMock.Setup(u => u.Executar(turmaCodigo, bimestre)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterTotalCompensacoesComponentesNaoLancamNota(turmaCodigo, bimestre, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(turmaCodigo, bimestre), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter recomendações gerais")]
        public async Task DeveChamarUseCase_ParaObterRecomendacoesGerais()
        {
            // Arrange
            var useCaseMock = new Mock<IObterRecomendacoesAlunoFamiliaUseCase>();
            var retorno = new List<RecomendacoesAlunoFamiliaDto>();

            useCaseMock.Setup(u => u.Executar()).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterRecomendacoes(useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve retornar NoContent quando não houver recomendações gerais")]
        public async Task DeveRetornarNoContent_QuandoNaoHouverRecomendacoesGerais()
        {
            // Arrange
            var useCaseMock = new Mock<IObterRecomendacoesAlunoFamiliaUseCase>();

            useCaseMock.Setup(u => u.Executar()).ReturnsAsync((IEnumerable<RecomendacoesAlunoFamiliaDto>)null);

            // Act
            var resultado = await _controller.ObterRecomendacoes(useCaseMock.Object);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(resultado);
            Assert.Equal(204, statusCodeResult.StatusCode);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter alunos sem notas e recomendações")]
        public async Task DeveChamarUseCase_ParaObterAlunosSemNotasRecomendacoes()
        {
            // Arrange
            var useCaseMock = new Mock<IObterAlunosSemNotasRecomendacoesUseCase>();
            var turmaId = _faker.Random.Long(1);
            var bimestre = _faker.Random.Int(1, 4);
            var retorno = new List<InconsistenciasAlunoFamiliaDto>();

            useCaseMock.Setup(u => u.Executar(It.Is<FiltroInconsistenciasAlunoFamiliaDto>(f => f.TurmaId == turmaId && f.Bimestre == bimestre)))
                       .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterAlunosSemNotasRecomendacoes(turmaId, bimestre, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(It.IsAny<FiltroInconsistenciasAlunoFamiliaDto>()), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter pareceres conclusivos por turma")]
        public async Task DeveChamarUseCase_ParaObterPareceresConclusivosPorTurma()
        {
            // Arrange
            var useCaseMock = new Mock<IObterPareceresConclusivosTurmaUseCase>();
            var turmaId = _faker.Random.Long(1);
            var retorno = new List<ParecerConclusivoDto>();

            useCaseMock.Setup(u => u.Executar(turmaId, false)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterPareceresConclusivosTurma(turmaId, useCaseMock.Object, false);

            // Assert
            useCaseMock.Verify(u => u.Executar(turmaId, false), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para alterar parecer conclusivo")]
        public async Task DeveChamarUseCase_ParaAlterarParecerConclusivo()
        {
            // Arrange
            var useCaseMock = new Mock<IAlterarParecerConclusivoUseCase>();
            var dto = new AlterarParecerConclusivoDto { AlunoCodigo = _faker.Random.AlphaNumeric(8) };
            var retorno = new ParecerConclusivoDto();

            useCaseMock.Setup(u => u.Executar(dto)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.AlterarParecerConclusivo(dto, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(dto), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter questionário PAP")]
        public async Task DeveChamarUseCase_ParaObterQuestionarioPAP()
        {
            // Arrange
            var useCaseMock = new Mock<IObterRelatorioPAPConselhoClasseUseCase>();
            var codigoTurma = _faker.Random.AlphaNumeric(5);
            var alunoCodigo = _faker.Random.AlphaNumeric(8);
            var bimestre = _faker.Random.Int(1, 4);
            var retorno = new List<SecaoQuestoesDTO>();

            useCaseMock.Setup(u => u.Executar(codigoTurma, alunoCodigo, bimestre)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterQuestionarioPAPConselhoClasse(codigoTurma, alunoCodigo, bimestre, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(codigoTurma, alunoCodigo, bimestre), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "Deve chamar caso de uso para obter pareceres por ano e modalidade")]
        public async Task DeveChamarUseCase_ParaObterPareceresPorAnoModalidade()
        {
            // Arrange
            var useCaseMock = new Mock<IObterPareceresConclusivosAnoLetivoModalidadeUseCase>();
            var anoLetivo = DateTime.Now.Year;
            var modalidade = Modalidade.Fundamental;
            var retorno = new List<ParecerConclusivoDto>();

            useCaseMock.Setup(u => u.Executar(anoLetivo, modalidade)).ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterPareceresConclusivosAnoLetivoModalidade(anoLetivo, modalidade, useCaseMock.Object);

            // Assert
            useCaseMock.Verify(u => u.Executar(anoLetivo, modalidade), Times.Once);
            Assert.IsType<OkObjectResult>(resultado);
        }
    }
}