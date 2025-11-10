using MediatR;
using Moq;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class ObterAlunosSemNotasRecomendacoesUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterAlunosSemNotasRecomendacoesUseCase useCase;

        public ObterAlunosSemNotasRecomendacoesUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterAlunosSemNotasRecomendacoesUseCase(mediator.Object);
        }

        [Fact(DisplayName = "Executar_Quando_Mediator_Nulo_Deve_LancarArgumentNullException")]
        public void Executar_Quando_Mediator_Nulo_Deve_LancarArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterAlunosSemNotasRecomendacoesUseCase(null));
        }

        [Fact(DisplayName = "Executar_Quando_Param_Nulo_Deve_LancarNullReferenceException")]
        public async Task Executar_Quando_Param_Nulo_Deve_LancarNullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() => useCase.Executar(null));
        }

        [Fact(DisplayName = "Executar_Quando_TurmaNaoEncontrada_Deve_LancarNegocioException")]
        public async Task Executar_Quando_TurmaNaoEncontrada_Deve_LancarNegocioException()
        {
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Turma)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(new FiltroInconsistenciasAlunoFamiliaDto(1, 1)));
            Assert.Equal("Turma não encontrada", exception.Message);
        }

        [Fact(DisplayName = "Executar_Quando_PeriodoEscolarNaoEncontrado_TurmaRegular_Deve_LancarNullReferenceException")]
        public async Task Executar_Quando_PeriodoEscolarNaoEncontrado_TurmaRegular_Deve_LancarNullReferenceException()
        {
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "1", ModalidadeCodigo = Modalidade.Fundamental });

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarPorTurmaBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PeriodoEscolar)null);

            await Assert.ThrowsAsync<NullReferenceException>(() => useCase.Executar(new FiltroInconsistenciasAlunoFamiliaDto(1, 1)));
        }

        [Fact(DisplayName = "Executar_Quando_TurmaEJA_TipoCalendarioNaoEncontrado_Deve_LancarNegocioException")]
        public async Task Executar_Quando_TurmaEJA_TipoCalendarioNaoEncontrado_Deve_LancarNegocioException()
        {
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "1", ModalidadeCodigo = Modalidade.EJA });

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(new FiltroInconsistenciasAlunoFamiliaDto(1, 1)));
            Assert.Contains("Tipo de calendário não encontrado", exception.Message);
        }

        [Fact(DisplayName = "Executar_Quando_FechamentoTurmaNaoEncontrado_TurmaAnoCorrente_Deve_LancarNegocioException")]
        public async Task Executar_Quando_FechamentoTurmaNaoEncontrado_TurmaAnoCorrente_Deve_LancarNegocioException()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "1", ModalidadeCodigo = Modalidade.Fundamental, AnoLetivo = anoAtual });

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarPorTurmaBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoEscolar { PeriodoInicio = DateTime.Now, PeriodoFim = DateTime.Now.AddDays(10) });

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PeriodoFechamentoBimestre)null);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta> { new() { CodigoAluno = "1" } });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasComplementaresPorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaComplementarDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaItinerarioEnsinoMedioDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            mediator.Setup(x => x.Send(It.IsAny<ObterPerfilAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            mediator.Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesPorTurmasCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DisciplinaDto[] { new() { LancaNota = true } });

            mediator.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((FechamentoTurma)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(new FiltroInconsistenciasAlunoFamiliaDto(1, 1)));
            Assert.Contains("Fechamento da turma não localizado", exception.Message);
        }

        [Fact(DisplayName = "Executar_Quando_FechamentoTurmaNaoEncontrado_TurmaAnoAnterior_Deve_LancarNegocioException")]
        public async Task Executar_Quando_FechamentoTurmaNaoEncontrado_TurmaAnoAnterior_Deve_LancarNegocioException()
        {
            var anoAnterior = DateTimeExtension.HorarioBrasilia().Year - 1;
            
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "1", ModalidadeCodigo = Modalidade.Fundamental, AnoLetivo = anoAnterior });

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarPorTurmaBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoEscolar { PeriodoInicio = DateTime.Now, PeriodoFim = DateTime.Now.AddDays(10) });

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PeriodoFechamentoBimestre)null);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta> { new() { CodigoAluno = "1" } });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasComplementaresPorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaComplementarDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaItinerarioEnsinoMedioDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            mediator.Setup(x => x.Send(It.IsAny<ObterPerfilAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            mediator.Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesPorTurmasCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DisciplinaDto[] { new() { LancaNota = true } });

            mediator.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((FechamentoTurma)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(new FiltroInconsistenciasAlunoFamiliaDto(1, 1)));
            Assert.Equal("Fechamento da turma não localizado", exception.Message);
        }

        [Fact(DisplayName = "Executar_Quando_NaoExisteConselhoClasseParaTurma_Deve_LancarNegocioException")]
        public async Task Executar_Quando_NaoExisteConselhoClasseParaTurma_Deve_LancarNegocioException()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "1", ModalidadeCodigo = Modalidade.Fundamental, AnoLetivo = anoAtual });

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarPorTurmaBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoEscolar { PeriodoInicio = DateTime.Now, PeriodoFim = DateTime.Now.AddDays(10) });

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PeriodoFechamentoBimestre)null);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta> { new() { CodigoAluno = "1" } });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasComplementaresPorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaComplementarDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaItinerarioEnsinoMedioDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            mediator.Setup(x => x.Send(It.IsAny<ObterPerfilAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            mediator.Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesPorTurmasCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DisciplinaDto[] { new() { LancaNota = true } });

            mediator.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FechamentoTurma());

            mediator.Setup(x => x.Send(It.IsAny<ExisteConselhoClasseParaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(new FiltroInconsistenciasAlunoFamiliaDto(1, 1)));
            Assert.Contains("Não foi encontrado registro de conselho de classe", exception.Message);
        }

        [Fact(DisplayName = "Executar_Quando_TurmaEnsinoMedio_SemTurmasAluno_Deve_LancarNegocioException")]
        public async Task Executar_Quando_TurmaEnsinoMedio_SemTurmasAluno_Deve_LancarNegocioException()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "1", ModalidadeCodigo = Modalidade.Medio, AnoLetivo = anoAtual });

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarPorTurmaBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoEscolar { PeriodoInicio = DateTime.Now, PeriodoFim = DateTime.Now.AddDays(10) });

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PeriodoFechamentoBimestre)null);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta> { new() { CodigoAluno = "1" } });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasComplementaresPorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaComplementarDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaItinerarioEnsinoMedioDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasFechamentoConselhoPorAlunosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaAlunoDto>());

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(new FiltroInconsistenciasAlunoFamiliaDto(1, 1)));
            Assert.Equal("Turma não encontrada", exception.Message);
        }

        [Fact(DisplayName = "Executar_Quando_CenarioCompleto_ComInconsistencias_Deve_RetornarListaComInconsistencias")]
        public async Task Executar_Quando_CenarioCompleto_ComInconsistencias_Deve_RetornarListaComInconsistencias()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var periodoInicio = new DateTime(anoAtual, 02, 05);
            var periodoFim = new DateTime(anoAtual, 04, 30);

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "1", ModalidadeCodigo = Modalidade.Fundamental, AnoLetivo = anoAtual });

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarPorTurmaBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoEscolar { PeriodoInicio = periodoInicio, PeriodoFim = periodoFim });

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PeriodoFechamentoBimestre)null);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta> { new() { CodigoAluno = "1", NomeAluno = "Aluno 1", NumeroAlunoChamada = 1 } });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasComplementaresPorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaComplementarDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaItinerarioEnsinoMedioDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { CodigoRf = "1234567" });

            mediator.Setup(x => x.Send(It.IsAny<ObterPerfilAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            mediator.Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesPorTurmasCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DisciplinaDto[] { new() { LancaNota = true, Nome = "Matemática", CodigoComponenteCurricular = 1, TurmaCodigo = "1" } });

            mediator.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FechamentoTurma());

            mediator.Setup(x => x.Send(It.IsAny<ExisteConselhoClasseParaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(x => x.Send(It.IsAny<VerificarSeExisteRecomendacaoPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoTemRecomandacaoDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterConselhoClasseAlunoNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ConselhoClasseAlunoNotaDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasComMatriculaValidasParaValidarConselhoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new string[] { "1" });

            var resultado = await useCase.Executar(new FiltroInconsistenciasAlunoFamiliaDto(1, 1));

            Assert.NotEmpty(resultado);
            var primeiroAluno = resultado.First();
            Assert.Equal("1", primeiroAluno.AlunoCodigo);
            Assert.Equal("Aluno 1", primeiroAluno.AlunoNome);
            Assert.Equal(1, primeiroAluno.NumeroChamada);
            Assert.Contains("Matemática", primeiroAluno.Inconsistencias.First());
            Assert.Contains("Ausência das recomendações a família e ao estudante", primeiroAluno.Inconsistencias.Last());
        }

        [Fact(DisplayName = "Executar_Quando_BimestreZero_TurmaRegular_Deve_UsarBimestre4")]
        public async Task Executar_Quando_BimestreZero_TurmaRegular_Deve_UsarBimestre4()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "1", ModalidadeCodigo = Modalidade.Fundamental, AnoLetivo = anoAtual });

            mediator.Setup(x => x.Send(It.Is<ObterPeriodoEscolarPorTurmaBimestreQuery>(q => q.Bimestre == 4), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoEscolar { PeriodoInicio = DateTime.Now, PeriodoFim = DateTime.Now.AddDays(10) });

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PeriodoFechamentoBimestre)null);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasComplementaresPorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaComplementarDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaItinerarioEnsinoMedioDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { CodigoRf = "1234567" });

            mediator.Setup(x => x.Send(It.IsAny<ObterPerfilAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            mediator.Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesPorTurmasCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DisciplinaDto[] { new() { LancaNota = true } });

            mediator.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FechamentoTurma());

            mediator.Setup(x => x.Send(It.IsAny<ExisteConselhoClasseParaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(x => x.Send(It.IsAny<VerificarSeExisteRecomendacaoPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoTemRecomandacaoDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterConselhoClasseAlunoNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ConselhoClasseAlunoNotaDto>());

            await useCase.Executar(new FiltroInconsistenciasAlunoFamiliaDto(1, 0));

            mediator.Verify(x => x.Send(It.Is<ObterPeriodoEscolarPorTurmaBimestreQuery>(q => q.Bimestre == 4), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Executar_Quando_BimestreZero_TurmaEJA_Deve_UsarBimestre2")]
        public async Task Executar_Quando_BimestreZero_TurmaEJA_Deve_UsarBimestre2()
        {
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "1", ModalidadeCodigo = Modalidade.EJA, AnoLetivo = DateTimeExtension.HorarioBrasilia().Year });

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar> { new() { Bimestre = 2, PeriodoInicio = DateTime.Now, PeriodoFim = DateTime.Now.AddDays(10) } });

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PeriodoFechamentoBimestre)null);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasComplementaresPorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaComplementarDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaItinerarioEnsinoMedioDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { CodigoRf = "1234567" });

            mediator.Setup(x => x.Send(It.IsAny<ObterPerfilAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            mediator.Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesPorTurmasCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DisciplinaDto[] { new() { LancaNota = true } });

            mediator.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FechamentoTurma());

            mediator.Setup(x => x.Send(It.IsAny<ExisteConselhoClasseParaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(x => x.Send(It.IsAny<VerificarSeExisteRecomendacaoPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoTemRecomandacaoDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterConselhoClasseAlunoNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ConselhoClasseAlunoNotaDto>());

            await useCase.Executar(new FiltroInconsistenciasAlunoFamiliaDto(1, 0));

            mediator.Verify(x => x.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Executar_Quando_ComTurmasComplementares_Deve_IncluirTurmasComplementares")]
        public async Task Executar_Quando_ComTurmasComplementares_Deve_IncluirTurmasComplementares()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "1", ModalidadeCodigo = Modalidade.Fundamental, AnoLetivo = anoAtual, Semestre = 1 });

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarPorTurmaBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoEscolar { PeriodoInicio = DateTime.Now, PeriodoFim = DateTime.Now.AddDays(10) });

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta> { new() { CodigoAluno = "1" } });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasComplementaresPorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TurmaComplementarDto> { new() { CodigoTurma = "2", TurmaRegularCodigo = "1", Semestre = 1 } });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaItinerarioEnsinoMedioDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            mediator.Setup(x => x.Send(It.IsAny<ObterPerfilAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            mediator.Setup(x => x.Send(It.Is<ObterComponentesCurricularesPorTurmasCodigoQuery>(q => q.TurmasCodigo.Contains("2")), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DisciplinaDto[] { new() { LancaNota = true } });

            mediator.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FechamentoTurma());

            mediator.Setup(x => x.Send(It.IsAny<ExisteConselhoClasseParaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(x => x.Send(It.IsAny<VerificarSeExisteRecomendacaoPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoTemRecomandacaoDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterConselhoClasseAlunoNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ConselhoClasseAlunoNotaDto>());

            await useCase.Executar(new FiltroInconsistenciasAlunoFamiliaDto(1, 1));

            mediator.Verify(x => x.Send(It.Is<ObterComponentesCurricularesPorTurmasCodigoQuery>(q => q.TurmasCodigo.Contains("2")), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Executar_Quando_ComTurmasItinerarioEnsinoMedio_Deve_IncluirTurmasItinerario")]
        public async Task Executar_Quando_ComTurmasItinerarioEnsinoMedio_Deve_IncluirTurmasItinerario()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "1", ModalidadeCodigo = Modalidade.Medio, AnoLetivo = anoAtual, TipoTurma = TipoTurma.Regular, UeId = 1 });

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarPorTurmaBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoEscolar { PeriodoInicio = DateTime.Now, PeriodoFim = DateTime.Now.AddDays(10) });

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta> { new() { CodigoAluno = "1" } });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasComplementaresPorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaComplementarDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TurmaItinerarioEnsinoMedioDto> { new() { Id = (int)TipoTurma.Regular } });

            mediator.Setup(x => x.Send(It.IsAny<ObterUePorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Ue { CodigoUe = "123456" });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaCodigosAlunoPorAnoLetivoUeTipoTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new string[] { "1", "2" });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasFechamentoConselhoPorAlunosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TurmaAlunoDto> { new() { TurmaCodigo = "1", TipoTurma = TipoTurma.Regular } });

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            mediator.Setup(x => x.Send(It.IsAny<ObterPerfilAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            mediator.Setup(x => x.Send(It.Is<ObterComponentesCurricularesPorTurmasCodigoQuery>(q => q.TurmasCodigo.Contains("2")), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DisciplinaDto[] { new() { LancaNota = true } });

            mediator.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FechamentoTurma());

            mediator.Setup(x => x.Send(It.IsAny<ExisteConselhoClasseParaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(x => x.Send(It.IsAny<VerificarSeExisteRecomendacaoPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoTemRecomandacaoDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterConselhoClasseAlunoNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ConselhoClasseAlunoNotaDto>());

            await useCase.Executar(new FiltroInconsistenciasAlunoFamiliaDto(1, 1));

            mediator.Verify(x => x.Send(It.Is<ObterComponentesCurricularesPorTurmasCodigoQuery>(q => q.TurmasCodigo.Contains("2")), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Executar_Quando_AlunoComRecomendacao_Deve_NaoAdicionarInconsistenciaRecomendacao")]
        public async Task Executar_Quando_AlunoComRecomendacao_Deve_NaoAdicionarInconsistenciaRecomendacao()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { CodigoTurma = "1", ModalidadeCodigo = Modalidade.Fundamental, AnoLetivo = anoAtual });

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoEscolarPorTurmaBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoEscolar { PeriodoInicio = DateTime.Now, PeriodoFim = DateTime.Now.AddDays(10) });

            mediator.Setup(x => x.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta> { new() { CodigoAluno = "1", NomeAluno = "Aluno 1" } });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasComplementaresPorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaComplementarDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaItinerarioEnsinoMedioDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            mediator.Setup(x => x.Send(It.IsAny<ObterPerfilAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            mediator.Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesPorTurmasCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DisciplinaDto[] { new() { LancaNota = true, Nome = "Matemática", CodigoComponenteCurricular = 1, TurmaCodigo = "1" } });

            mediator.Setup(x => x.Send(It.IsAny<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FechamentoTurma());

            mediator.Setup(x => x.Send(It.IsAny<ExisteConselhoClasseParaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(x => x.Send(It.IsAny<VerificarSeExisteRecomendacaoPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoTemRecomandacaoDto> { new() { AluncoCodigo = "1", TemRecomendacao = true } });

            mediator.Setup(x => x.Send(It.IsAny<ObterConselhoClasseAlunoNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ConselhoClasseAlunoNotaDto> { new() { AlunoCodigo = "1", ComponenteCurricularId = 1, Nota = "10" } });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasComMatriculaValidasParaValidarConselhoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new string[] { "1" });

            var resultado = await useCase.Executar(new FiltroInconsistenciasAlunoFamiliaDto(1, 1));

            Assert.Empty(resultado);
        }

        [Fact(DisplayName = "ObterAlunosSemNotasRecomendacoesUseCase - Deve considerar a verificação de alunos dentro do período escolar")]
        public async Task DeveConsiderarVerificacaoAlunosDentroPeriodoEscolar()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var periodoInicio = new DateTime(anoAtual, 02, 05);
            var periodoFim = new DateTime(anoAtual, 04, 30);

            mediator.Setup(x => x.Send(It.Is<ObterTurmaPorIdQuery>(y => y.TurmaId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { CodigoTurma = "1", ModalidadeCodigo = Modalidade.Fundamental, AnoLetivo = anoAtual });

            mediator.Setup(x => x.Send(It.Is<ObterPeriodoEscolarPorTurmaBimestreQuery>(y => y.Turma.CodigoTurma == "1" && y.Bimestre == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoEscolar() { PeriodoInicio = periodoInicio, PeriodoFim = periodoFim });

            mediator.Setup(x => x.Send(It.Is<ObterPeriodosEscolaresPorTipoCalendarioQuery>(y => y.TipoCalendarioId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar>() { new() { PeriodoInicio = periodoInicio, PeriodoFim = periodoFim } });

            mediator.Setup(x => x.Send(It.Is<ObterAlunosDentroPeriodoQuery>(y => y.CodigoTurma == "1" && y.Periodo.dataInicio == periodoInicio && y.Periodo.dataFim == periodoFim), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>() { new() { CodigoAluno = "1" }, new() { CodigoAluno = "2" } });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasComplementaresPorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaComplementarDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaItinerarioEnsinoMedioDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            mediator.Setup(x => x.Send(It.IsAny<ObterPerfilAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            mediator.Setup(x => x.Send(It.Is<ObterComponentesCurricularesPorTurmasCodigoQuery>(y => y.TurmasCodigo.Single() == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DisciplinaDto[] { new() { LancaNota = true } });

            mediator.Setup(x => x.Send(It.Is<ObterTipoCalendarioIdPorTurmaQuery>(y => y.Turma.CodigoTurma == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.Is<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery>(y => y.Bimestre == 1 &&
                                                                                                                                    y.CodigoTurma == "1" &&
                                                                                                                                    y.AnoLetivoTurma == anoAtual &&
                                                                                                                                    y.TipoCalendario == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FechamentoTurma());

            mediator.Setup(x => x.Send(It.Is<ExisteConselhoClasseParaTurmaQuery>(y => y.CodigosTurmas.Single() == "1" && y.Bimestre == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(x => x.Send(It.Is<VerificarSeExisteRecomendacaoPorTurmaQuery>(y => y.TurmasCodigo.Single() == "1" && y.Bimestre == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoTemRecomandacaoDto>() { new() });

            var resultado = await useCase.Executar(new Interfaces.FiltroInconsistenciasAlunoFamiliaDto(1, 1));

            Assert.NotNull(resultado);

            mediator.Verify(x => x.Send(It.Is<ObterAlunosDentroPeriodoQuery>(y => y.CodigoTurma == "1" &&
                                                                                  y.Periodo.dataInicio == periodoInicio &&
                                                                                  y.Periodo.dataFim == periodoFim &&
                                                                                  !y.ConsideraSomenteAtivos), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "ObterAlunosSemNotasRecomendacoesUseCase - Deve considerar a verificação de alunos dentro do período fechamento")]
        public async Task DeveConsiderarVerificacaoAlunosDentroPeriodoFechamento()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var periodoInicio = new DateTime(anoAtual, 02, 05);
            var periodoFim = new DateTime(anoAtual, 04, 30);
            var periodoFechamentoInicio = new DateTime(anoAtual, 04, 01);
            var periodoFechamentoFim = new DateTime(anoAtual, 05, 31);

            mediator.Setup(x => x.Send(It.Is<ObterTurmaPorIdQuery>(y => y.TurmaId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { CodigoTurma = "1", ModalidadeCodigo = Modalidade.Fundamental, AnoLetivo = anoAtual });

            mediator.Setup(x => x.Send(It.Is<ObterPeriodoEscolarPorTurmaBimestreQuery>(y => y.Turma.CodigoTurma == "1" && y.Bimestre == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoEscolar() { PeriodoInicio = periodoInicio, PeriodoFim = periodoFim });

            mediator.Setup(x => x.Send(It.Is<ObterPeriodosEscolaresPorTipoCalendarioQuery>(y => y.TipoCalendarioId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoEscolar>() { new() { PeriodoInicio = periodoInicio, PeriodoFim = periodoFim } });

            mediator.Setup(x => x.Send(It.Is<ObterTipoCalendarioIdPorTurmaQuery>(y => y.Turma.CodigoTurma == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(x => x.Send(It.Is<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery>(y => y.TipoCandarioId == 1 && y.Bimestre == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoFechamentoBimestre() { InicioDoFechamento = periodoFechamentoInicio, FinalDoFechamento = periodoFechamentoFim });

            mediator.Setup(x => x.Send(It.Is<ObterAlunosDentroPeriodoQuery>(y => y.CodigoTurma == "1" && y.Periodo.dataInicio == periodoInicio && y.Periodo.dataFim == periodoFim), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>() { new() { CodigoAluno = "1" }, new() { CodigoAluno = "2" } });

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasComplementaresPorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaComplementarDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaItinerarioEnsinoMedioDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            mediator.Setup(x => x.Send(It.IsAny<ObterPerfilAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            mediator.Setup(x => x.Send(It.Is<ObterComponentesCurricularesPorTurmasCodigoQuery>(y => y.TurmasCodigo.Single() == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DisciplinaDto[] { new() { LancaNota = true } });            

            mediator.Setup(x => x.Send(It.Is<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery>(y => y.Bimestre == 1 &&
                                                                                                                                    y.CodigoTurma == "1" &&
                                                                                                                                    y.AnoLetivoTurma == anoAtual &&
                                                                                                                                    y.TipoCalendario == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FechamentoTurma());

            mediator.Setup(x => x.Send(It.Is<ExisteConselhoClasseParaTurmaQuery>(y => y.CodigosTurmas.Single() == "1" && y.Bimestre == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(x => x.Send(It.Is<VerificarSeExisteRecomendacaoPorTurmaQuery>(y => y.TurmasCodigo.Single() == "1" && y.Bimestre == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoTemRecomandacaoDto>() { new() });

            var resultado = await useCase.Executar(new Interfaces.FiltroInconsistenciasAlunoFamiliaDto(1, 1));

            Assert.NotNull(resultado);

            mediator.Verify(x => x.Send(It.Is<ObterAlunosDentroPeriodoQuery>(y => y.CodigoTurma == "1" &&
                                                                                  y.Periodo.dataInicio == periodoFechamentoInicio &&
                                                                                  y.Periodo.dataFim == periodoFechamentoFim &&
                                                                                  !y.ConsideraSomenteAtivos), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
