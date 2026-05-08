using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Grade.ObterGradeAulasPorTurmaEProfessor
{
    public class ObterGradeAulasPorTurmaEProfessorQueryHandlerTeste
    {
        private readonly Mock<IRepositorioTurmaConsulta> mockRepositorioTurma;
        private readonly Mock<IRepositorioAulaConsulta> mockRepositorioAula;
        private readonly Mock<IRepositorioGrade> mockRepositorioGrade;
        private readonly Mock<IMediator> mockMediator;
        private readonly ObterGradeAulasPorTurmaEProfessorQueryHandler handler;

        public ObterGradeAulasPorTurmaEProfessorQueryHandlerTeste()
        {
            mockRepositorioTurma = new Mock<IRepositorioTurmaConsulta>();
            mockRepositorioAula = new Mock<IRepositorioAulaConsulta>();
            mockRepositorioGrade = new Mock<IRepositorioGrade>();
            mockMediator = new Mock<IMediator>();

            handler = new ObterGradeAulasPorTurmaEProfessorQueryHandler(
                mockRepositorioTurma.Object,
                mockRepositorioAula.Object,
                mockRepositorioGrade.Object,
                mockMediator.Object
            );
        }

        [Fact]
        public void Construtor_DeveLancarExcecao_QuandoRepositorioTurmaForNulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterGradeAulasPorTurmaEProfessorQueryHandler(
                null,
                mockRepositorioAula.Object,
                mockRepositorioGrade.Object,
                mockMediator.Object
            ));
        }

        [Fact]
        public void Construtor_DeveLancarExcecao_QuandoRepositorioAulaForNulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterGradeAulasPorTurmaEProfessorQueryHandler(
                mockRepositorioTurma.Object,
                null,
                mockRepositorioGrade.Object,
                mockMediator.Object
            ));
        }

        [Fact]
        public void Construtor_DeveLancarExcecao_QuandoRepositorioGradeForNulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterGradeAulasPorTurmaEProfessorQueryHandler(
                mockRepositorioTurma.Object,
                mockRepositorioAula.Object,
                null,
                mockMediator.Object
            ));
        }

        [Fact]
        public void Construtor_DeveLancarExcecao_QuandoMediatorForNulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterGradeAulasPorTurmaEProfessorQueryHandler(
                mockRepositorioTurma.Object,
                mockRepositorioAula.Object,
                mockRepositorioGrade.Object,
                null
            ));
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoTurmaNaoForLocalizada()
        {
            var turmaCodigo = "123456";
            var dataAula = DateTime.Now;
            var componentesCurriculares = new long[] { 1 };
            var request = new ObterGradeAulasPorTurmaEProfessorQuery(turmaCodigo, componentesCurriculares, dataAula);
            mockRepositorioTurma.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync((Turma)null);

            await Assert.ThrowsAsync<NegocioException>(() =>
                handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeveRetornarNull_QuandoHorasGradeForZero()
        {
            var turma = CriarTurma();
            var dataAula = DateTime.Now;
            var componentesCurriculares = new long[] { 1 };
            var request = new ObterGradeAulasPorTurmaEProfessorQuery(turma.CodigoTurma, componentesCurriculares, dataAula, "1234567", false);

            mockRepositorioTurma.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            mockMediator.Setup(m => m.Send(It.IsAny<ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Dominio.Grade)null);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Handle_DeveObterUsuarioLogado_QuandoCodigoRfForVazio()
        {
            var turma = CriarTurma();
            var usuario = new Usuario { CodigoRf = "7654321" };
            var grade = new Dominio.Grade { Id = 1 };
            var request = new ObterGradeAulasPorTurmaEProfessorQuery(turma.CodigoTurma, new long[] { 1 }, DateTime.Now, string.Empty, false);

            mockRepositorioTurma.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            mockMediator.Setup(m => m.Send(It.Is<ObterUsuarioLogadoQuery>(q => q == ObterUsuarioLogadoQuery.Instance), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mockMediator.Setup(m => m.Send(It.IsAny<ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(grade);

            mockRepositorioGrade.Setup(r => r.ObterHorasComponente(It.IsAny<long>(), It.IsAny<long[]>(), It.IsAny<int>()))
                .ReturnsAsync(5);

            mockMediator.Setup(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mockRepositorioAula.Setup(r => r.ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(
                It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<bool>()))
                .ReturnsAsync(2);

            var resultado = await handler.Handle(request, CancellationToken.None);

            mockMediator.Verify(m => m.Send(It.Is<ObterUsuarioLogadoQuery>(q => q == ObterUsuarioLogadoQuery.Instance), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task Handle_DeveRetornarDtoCorreto_QuandoEhRegenciaETemHorasDisponiveis()
        {
            var turma = CriarTurmaComRegencia();
            var request = new ObterGradeAulasPorTurmaEProfessorQuery(turma.CodigoTurma, new long[] { 1 }, DateTime.Now, "1234567", true);

            mockRepositorioTurma.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            mockMediator.Setup(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mockRepositorioAula.Setup(r => r.ObterQuantidadeAulasTurmaComponenteCurricularDiaProfessor(
                It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(2);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.True(resultado.QuantidadeAulasGrade > 0);
            Assert.True(resultado.QuantidadeAulasRestante >= 0);
            Assert.False(resultado.PodeEditar);
        }

        [Fact]
        public async Task Handle_DeveRetornarDtoCorreto_QuandoNaoEhRegenciaETemGrade()
        {
            var turma = CriarTurma();
            var grade = new Dominio.Grade { Id = 1 };
            var request = new ObterGradeAulasPorTurmaEProfessorQuery(turma.CodigoTurma, new long[] { 1 }, DateTime.Now, "1234567", false, false);

            mockRepositorioTurma.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            mockMediator.Setup(m => m.Send(It.IsAny<ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(grade);

            mockRepositorioGrade.Setup(r => r.ObterHorasComponente(It.IsAny<long>(), It.IsAny<long[]>(), It.IsAny<int>()))
                .ReturnsAsync(5);

            mockMediator.Setup(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mockRepositorioAula.Setup(r => r.ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(
                It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<bool>()))
                .ReturnsAsync(2);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(5, resultado.QuantidadeAulasGrade);
            Assert.Equal(3, resultado.QuantidadeAulasRestante);
            Assert.True(resultado.PodeEditar);
        }

        [Fact]
        public async Task Handle_DeveRetornarAulasRestantesZero_QuandoHorasCadastradasMaiorQueGrade()
        {
            var turma = CriarTurma();
            var grade = new Dominio.Grade { Id = 1 };
            var request = new ObterGradeAulasPorTurmaEProfessorQuery(turma.CodigoTurma, new long[] { 1 }, DateTime.Now, "1234567", false, false);

            mockRepositorioTurma.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            mockMediator.Setup(m => m.Send(It.IsAny<ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(grade);

            mockRepositorioGrade.Setup(r => r.ObterHorasComponente(It.IsAny<long>(), It.IsAny<long[]>(), It.IsAny<int>()))
                .ReturnsAsync(5);

            mockMediator.Setup(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mockRepositorioAula.Setup(r => r.ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(
                It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<bool>()))
                .ReturnsAsync(10);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(5, resultado.QuantidadeAulasGrade);
            Assert.Equal(0, resultado.QuantidadeAulasRestante);
            Assert.False(resultado.PodeEditar);
        }

        [Fact]
        public async Task Handle_DeveRetornar4Horas_QuandoComponenteCurricularFor1030()
        {
            var turma = CriarTurma();
            var request = new ObterGradeAulasPorTurmaEProfessorQuery(turma.CodigoTurma, new long[] { 1030 }, DateTime.Now, "1234567", false, false);

            mockRepositorioTurma.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            mockMediator.Setup(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mockRepositorioAula.Setup(r => r.ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(
                It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<bool>()))
                .ReturnsAsync(1);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(4, resultado.QuantidadeAulasGrade);
            Assert.Equal(3, resultado.QuantidadeAulasRestante);
        }

        [Fact]
        public async Task Handle_DeveUsarMetodoCorreto_QuandoEhExperienciaPedagogicaERegencia()
        {
            var turma = CriarTurmaComRegencia();
            var request = new ObterGradeAulasPorTurmaEProfessorQuery(turma.CodigoTurma, new long[] { 1 }, DateTime.Now, "1234567", true, false);

            mockRepositorioTurma.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            mockMediator.Setup(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mockRepositorioAula.Setup(r => r.ObterQuantidadeAulasTurmaExperienciasPedagogicasDia(
                It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(2);

            var resultado = await handler.Handle(request, CancellationToken.None);

            mockRepositorioAula.Verify(r => r.ObterQuantidadeAulasTurmaExperienciasPedagogicasDia(
                It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task Handle_DeveUsarMetodoCorreto_QuandoEhExperienciaPedagogicaENaoRegencia()
        {
            var turma = CriarTurma();
            var grade = new Dominio.Grade { Id = 1 };
            var request = new ObterGradeAulasPorTurmaEProfessorQuery(turma.CodigoTurma, new long[] { 1 }, DateTime.Now, "1234567", false, false);

            mockRepositorioTurma.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            mockMediator.Setup(m => m.Send(It.IsAny<ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(grade);

            mockRepositorioGrade.Setup(r => r.ObterHorasComponente(It.IsAny<long>(), It.IsAny<long[]>(), It.IsAny<int>()))
                .ReturnsAsync(5);

            mockMediator.Setup(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mockRepositorioAula.Setup(r => r.ObterQuantidadeAulasTurmaExperienciasPedagogicasSemana(
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string[]>()))
                .ReturnsAsync(2);

            var resultado = await handler.Handle(request, CancellationToken.None);

            mockRepositorioAula.Verify(r => r.ObterQuantidadeAulasTurmaExperienciasPedagogicasSemana(
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string[]>()), Times.Once);
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task Handle_DevePararNoProximo_QuandoEncontrarExperienciaPedagogica()
        {
            var turma = CriarTurma();
            var grade = new Dominio.Grade { Id = 1 };
            var request = new ObterGradeAulasPorTurmaEProfessorQuery(turma.CodigoTurma, new long[] { 1, 2, 3 }, DateTime.Now, "1234567", false, false);

            mockRepositorioTurma.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            mockMediator.Setup(m => m.Send(It.IsAny<ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(grade);

            mockRepositorioGrade.Setup(r => r.ObterHorasComponente(It.IsAny<long>(), It.IsAny<long[]>(), It.IsAny<int>()))
                .ReturnsAsync(5);

            mockMediator.SetupSequence(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false)
                .ReturnsAsync(true);

            mockRepositorioAula.Setup(r => r.ObterQuantidadeAulasTurmaExperienciasPedagogicasSemana(
                It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string[]>()))
                .ReturnsAsync(2);

            var resultado = await handler.Handle(request, CancellationToken.None);

            mockMediator.Verify(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task Handle_DeveDefinirEhGestor_QuandoUsuarioForGestorEscolar()
        {
            var turma = CriarTurma();
            var usuario = CriarUsuarioGestor();
            var grade = new Dominio.Grade { Id = 1 };
            var request = new ObterGradeAulasPorTurmaEProfessorQuery(turma.CodigoTurma, new long[] { 1 }, DateTime.Now, string.Empty, false, false);

            mockRepositorioTurma.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            mockMediator.Setup(m => m.Send(It.Is<ObterUsuarioLogadoQuery>(q => q == ObterUsuarioLogadoQuery.Instance), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mockMediator.Setup(m => m.Send(It.IsAny<ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(grade);

            mockRepositorioGrade.Setup(r => r.ObterHorasComponente(It.IsAny<long>(), It.IsAny<long[]>(), It.IsAny<int>()))
                .ReturnsAsync(5);

            mockMediator.Setup(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mockRepositorioAula.Setup(r => r.ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(
                It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<bool>()))
                .ReturnsAsync(2);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.True(request.EhGestor);
        }

        [Fact]
        public async Task Handle_DevePodeEditarFalse_QuandoAulasRestantesMenorOuIgualUm()
        {
            var turma = CriarTurma();
            var grade = new Dominio.Grade { Id = 1 };
            var request = new ObterGradeAulasPorTurmaEProfessorQuery(turma.CodigoTurma, new long[] { 1 }, DateTime.Now, "1234567", false, false);

            mockRepositorioTurma.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            mockMediator.Setup(m => m.Send(It.IsAny<ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(grade);

            mockRepositorioGrade.Setup(r => r.ObterHorasComponente(It.IsAny<long>(), It.IsAny<long[]>(), It.IsAny<int>()))
                .ReturnsAsync(5);

            mockMediator.Setup(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mockRepositorioAula.Setup(r => r.ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(
                It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<bool>()))
                .ReturnsAsync(4);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.QuantidadeAulasRestante);
            Assert.False(resultado.PodeEditar);
        }

        [Fact]
        public async Task Handle_DevePodeEditarFalse_QuandoRegenciaETurmaEJA()
        {
            var turma = CriarTurmaEJAComRegencia();
            var request = new ObterGradeAulasPorTurmaEProfessorQuery(turma.CodigoTurma, new long[] { 1 }, DateTime.Now, "1234567", true, false);

            mockRepositorioTurma.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            mockMediator.Setup(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mockRepositorioAula.Setup(r => r.ObterQuantidadeAulasTurmaComponenteCurricularDiaProfessor(
                It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(2);

            var resultado = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.False(resultado.PodeEditar);
        }

        [Fact]
        public async Task Handle_DeveCalcularSemanaISO_CorretamenteParaDataAula()
        {
            var turma = CriarTurma();
            var grade = new Dominio.Grade { Id = 1 };
            var dataAula = new DateTime(2024, 1, 15);
            var semanaEsperada = UtilData.ObterSemanaDoAnoISO(dataAula);
            var request = new ObterGradeAulasPorTurmaEProfessorQuery(turma.CodigoTurma, new long[] { 1 }, dataAula, "1234567", false, false);

            mockRepositorioTurma.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            mockMediator.Setup(m => m.Send(It.IsAny<ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(grade);

            mockRepositorioGrade.Setup(r => r.ObterHorasComponente(It.IsAny<long>(), It.IsAny<long[]>(), It.IsAny<int>()))
                .ReturnsAsync(5);

            mockMediator.Setup(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mockRepositorioAula.Setup(r => r.ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(
                It.IsAny<string>(), It.IsAny<string[]>(), It.Is<int>(s => s == semanaEsperada), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<bool>()))
                .ReturnsAsync(2)
                .Verifiable();

            var resultado = await handler.Handle(request, CancellationToken.None);

            mockRepositorioAula.Verify(r => r.ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(
                It.IsAny<string>(),
                It.IsAny<string[]>(),
                It.Is<int>(s => s == semanaEsperada),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<bool>()), Times.Once);

            Assert.NotNull(resultado);
        }

        [Theory]
        [InlineData("2024-01-01", 1)]
        [InlineData("2024-01-08", 2)]
        [InlineData("2024-12-30", 1)]
        [InlineData("2024-07-15", 29)]
        [InlineData("2026-02-10", 7)]
        [InlineData("2026-02-12", 7)]
        [InlineData("2026-02-18", 8)]
        public async Task Handle_DeveUsarSemanaISOCorreta_ParaDiferentesDatas(string dataString, int semanaEsperada)
        {
            var turma = CriarTurma();
            var grade = new Dominio.Grade { Id = 1 };
            var dataAula = DateTime.Parse(dataString);
            var request = new ObterGradeAulasPorTurmaEProfessorQuery(turma.CodigoTurma, new long[] { 1 }, dataAula, "1234567", false, false);

            mockRepositorioTurma.Setup(r => r.ObterTurmaComUeEDrePorCodigo(It.IsAny<string>()))
                .ReturnsAsync(turma);

            mockMediator.Setup(m => m.Send(It.IsAny<ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(grade);

            mockRepositorioGrade.Setup(r => r.ObterHorasComponente(It.IsAny<long>(), It.IsAny<long[]>(), It.IsAny<int>()))
                .ReturnsAsync(5);

            mockMediator.Setup(m => m.Send(It.IsAny<AulaDeExperienciaPedagogicaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mockRepositorioAula.Setup(r => r.ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(
                It.IsAny<string>(), It.IsAny<string[]>(), semanaEsperada, It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<bool>()))
                .ReturnsAsync(2);

            var resultado = await handler.Handle(request, CancellationToken.None);

            mockRepositorioAula.Verify(r => r.ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(
                It.IsAny<string>(),
                It.IsAny<string[]>(),
                semanaEsperada,
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<bool>()), Times.Once);

            Assert.NotNull(resultado);
        }

        private Turma CriarTurma()
        {
            return new Turma
            {
                CodigoTurma = "123456",
                Ano = "5",
                AnoLetivo = 2024,
                ModalidadeCodigo = Modalidade.Fundamental,
                QuantidadeDuracaoAula = 45,
                Ue = new Ue
                {
                    TipoEscola = TipoEscola.EMEF
                }
            };
        }

        private Turma CriarTurmaComRegencia()
        {
            var turma = CriarTurma();
            turma.TipoTurma = TipoTurma.Regular;
            return turma;
        }

        private Turma CriarTurmaEJAComRegencia()
        {
            return new Turma
            {
                CodigoTurma = "123456",
                Ano = "1",
                AnoLetivo = 2024,
                ModalidadeCodigo = Modalidade.EJA,
                QuantidadeDuracaoAula = 45,
                TipoTurma = TipoTurma.Regular,
                Ue = new Ue
                {
                    TipoEscola = TipoEscola.EMEF
                }
            };
        }

        private Usuario CriarUsuarioGestor()
        {
            var perfis = new List<PrioridadePerfil>
            {
                new PrioridadePerfil
                {
                    CodigoPerfil = Perfis.PERFIL_DIRETOR,
                    NomePerfil = "Diretor"
                }
            };

            var usuario = new Usuario
            {
                CodigoRf = "1234567",
                PerfilAtual = Perfis.PERFIL_DIRETOR
            };
            usuario.DefinirPerfis(perfis);

            return usuario;
        }
    }
}
