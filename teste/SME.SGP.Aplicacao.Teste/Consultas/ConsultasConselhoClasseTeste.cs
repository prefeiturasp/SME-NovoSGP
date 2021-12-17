using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasConselhoClasseTeste
    {
        private readonly ConsultasConselhoClasse consultasConselhoClasse;
        private readonly Mock<IConsultasDisciplina> consultasDisciplinas;
        private readonly Mock<IRepositorioConselhoClasseConsulta> repositorioConselhoClasseConsulta;
        private readonly Mock<IRepositorioPeriodoEscolarConsulta> repositorioPeriodoEscolar;
        private readonly Mock<IRepositorioParametrosSistema> repositorioParametrosSistema;
        private readonly Mock<IRepositorioConselhoClasseAlunoConsulta> repositorioConselhoClasseAluno;
        private readonly Mock<IRepositorioTipoCalendario> repositorioTipoCalendario;
        private readonly Mock<IRepositorioFechamentoTurmaConsulta> repositorioFechamentoTurma;
        private readonly Mock<IRepositorioConselhoClasse> repositorioConselhoClasse;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodoEscolar;
        private readonly Mock<IRepositorioParametrosSistemaConsulta> repositorioParametrosSistema;
        private readonly Mock<IRepositorioConselhoClasseAluno> repositorioConselhoClasseAluno;
        private readonly Mock<IRepositorioTipoCalendarioConsulta> repositorioTipoCalendario;
        private readonly Mock<IRepositorioFechamentoTurma> repositorioFechamentoTurma;
        private readonly Mock<IConsultasTurma> consultasTurma;
        private readonly Mock<IConsultasPeriodoEscolar> consultasPeriodoEscolar;
        private readonly Mock<IConsultasPeriodoFechamento> consultasPeriodoFechamento;
        private readonly Mock<IConsultasFechamentoTurma> consultasFechamentoTurma;
        private readonly Mock<IServicoDeNotasConceitos> servicoDeNotasConceitos;
        private readonly Mock<IMediator> mediator;

        public ConsultasConselhoClasseTeste()
        {
            consultasDisciplinas = new Mock<IConsultasDisciplina>();
            repositorioConselhoClasseConsulta = new Mock<IRepositorioConselhoClasseConsulta>();
            repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolarConsulta>();
            repositorioParametrosSistema = new Mock<IRepositorioParametrosSistema>();
            repositorioConselhoClasseAluno = new Mock<IRepositorioConselhoClasseAlunoConsulta>();
            repositorioTipoCalendario = new Mock<IRepositorioTipoCalendario>();
            repositorioFechamentoTurma = new Mock<IRepositorioFechamentoTurmaConsulta>();
            repositorioConselhoClasse = new Mock<IRepositorioConselhoClasse>();
            repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolar>();
            repositorioParametrosSistema = new Mock<IRepositorioParametrosSistemaConsulta>();
            repositorioConselhoClasseAluno = new Mock<IRepositorioConselhoClasseAluno>();
            repositorioTipoCalendario = new Mock<IRepositorioTipoCalendarioConsulta>();
            repositorioFechamentoTurma = new Mock<IRepositorioFechamentoTurma>();
            consultasTurma = new Mock<IConsultasTurma>();
            consultasPeriodoEscolar = new Mock<IConsultasPeriodoEscolar>();
            consultasPeriodoFechamento = new Mock<IConsultasPeriodoFechamento>();
            consultasFechamentoTurma = new Mock<IConsultasFechamentoTurma>();
            servicoDeNotasConceitos = new Mock<IServicoDeNotasConceitos>();
            mediator = new Mock<IMediator>();
            consultasConselhoClasse = new ConsultasConselhoClasse(repositorioConselhoClasseConsulta.Object, 
                                       repositorioPeriodoEscolar.Object,
                                       repositorioParametrosSistema.Object,
                                       repositorioConselhoClasseAluno.Object,
                                       repositorioTipoCalendario.Object,
                                       repositorioFechamentoTurma.Object,
                                       consultasTurma.Object,
                                       consultasPeriodoEscolar.Object,
                                       consultasPeriodoFechamento.Object,
                                       consultasFechamentoTurma.Object,
                                       servicoDeNotasConceitos.Object,
                                       mediator.Object);
        }

        [Fact]
        public async Task DeveObterResultado()
        {
            consultasTurma.Setup(t => t.ObterComUeDrePorCodigo(It.IsAny<string>())).Returns(Task.FromResult(ObterTurma()));
            consultasFechamentoTurma.Setup(f => f.ObterPorTurmaCodigoBimestreAsync(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(ObterFechamentoTurma()));
            consultasPeriodoFechamento.Setup(f => f.ObterPeriodoFechamentoTurmaAsync(It.IsAny<Turma>(), It.IsAny<int>(), It.IsAny<long>())).Returns(Task.FromResult(ObterPeriodoFechamentoBimestre()));
            consultasPeriodoEscolar.Setup(p => p.ObterUltimoPeriodoAsync(It.IsAny<int>(), It.IsAny<ModalidadeTipoCalendario>(), It.IsAny<int>())).Returns(Task.FromResult(ObterPeriodoEscolar()));
            servicoDeNotasConceitos.Setup(tn => tn.ObterNotaTipo(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<bool>())).Returns(Task.FromResult(new NotaTipoValor()));
            repositorioParametrosSistema.Setup(m => m.ObterValorPorTipoEAno(It.IsAny<TipoParametroSistema>(),null)).Returns(Task.FromResult("10"));
            repositorioConselhoClasseAluno.Setup(c => c.ObterPorConselhoClasseAlunoCodigoAsync(It.IsAny<long>(), It.IsAny<string>())).Returns(Task.FromResult(new ConselhoClasseAluno()));
            Assert.NotNull(consultasConselhoClasse.ObterConselhoClasseTurma("", "", 0, true, false));
        }

        [Fact]
        public async Task DeveTerErroAoObterTurma()
        {
            await Assert.ThrowsAsync<NegocioException>(() => consultasConselhoClasse.ObterConselhoClasseTurma("", "", 0, true, false));
        }

        [Fact]
        public async Task DeveTerErroFechamentoNaoLocalizadoAnoAtual()
        {
            var turma = ObterTurma();
            turma.AnoLetivo = DateTime.Today.Year;
            consultasTurma.Setup(t => t.ObterComUeDrePorCodigo(It.IsAny<string>())).Returns(Task.FromResult(turma));
            consultasFechamentoTurma.Setup(f => f.ObterPorTurmaCodigoBimestreAsync(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(ObterFechamentoTurma()));
            await Assert.ThrowsAsync<NegocioException>(() => consultasConselhoClasse.ObterConselhoClasseTurma("", "", 0, true, false));
        }

        [Fact]
        public async Task ErroObterPeriodoUltimoBimestre()
        {
            consultasTurma.Setup(t => t.ObterComUeDrePorCodigo(It.IsAny<string>())).Returns(Task.FromResult(ObterTurma()));
            consultasFechamentoTurma.Setup(f => f.ObterPorTurmaCodigoBimestreAsync(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(ObterFechamentoTurma()));
            await Assert.ThrowsAsync<NegocioException>(() => consultasConselhoClasse.ObterConselhoClasseTurma("", "", 0, true, false));
        }

        [Fact]
        public async Task ErroObterTipoNota()
        {
            consultasTurma.Setup(t => t.ObterComUeDrePorCodigo(It.IsAny<string>())).Returns(Task.FromResult(ObterTurma()));
            consultasFechamentoTurma.Setup(f => f.ObterPorTurmaCodigoBimestreAsync(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(ObterFechamentoTurma()));
            consultasPeriodoEscolar.Setup(p => p.ObterUltimoPeriodoAsync(It.IsAny<int>(), It.IsAny<ModalidadeTipoCalendario>(), It.IsAny<int>())).Returns(Task.FromResult(ObterPeriodoEscolar()));
            await Assert.ThrowsAsync<NegocioException>(() => consultasConselhoClasse.ObterConselhoClasseTurma("", "", 0, true, false));
        }

        [Fact]
        public async Task DeveTerErroTipoCalendarioAnoAnterior()
        {
            var turma = ObterTurma();
            turma.AnoLetivo = DateTime.Today.Year-1;
            consultasTurma.Setup(t => t.ObterComUeDrePorCodigo(It.IsAny<string>())).Returns(Task.FromResult(turma));
            consultasFechamentoTurma.Setup(f => f.ObterPorTurmaCodigoBimestreAsync(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(ObterFechamentoTurma()));
            await Assert.ThrowsAsync<NegocioException>(() => consultasConselhoClasse.ObterConselhoClasseTurma("", "", 0, true, false));
        }

        [Fact]
        public async Task DeveTerErroPeriodoEscolarAnoAnterior()
        {
            var turma = ObterTurma();
            turma.AnoLetivo = DateTime.Today.Year - 1;
            consultasTurma.Setup(t => t.ObterComUeDrePorCodigo(It.IsAny<string>())).Returns(Task.FromResult(turma));
            consultasFechamentoTurma.Setup(f => f.ObterPorTurmaCodigoBimestreAsync(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(ObterFechamentoTurma()));
            repositorioTipoCalendario.Setup(t => t.BuscarPorAnoLetivoEModalidade(It.IsAny<int>(), It.IsAny<ModalidadeTipoCalendario>(), It.IsAny<int>())).Returns(Task.FromResult(new TipoCalendario()));
            await Assert.ThrowsAsync<NegocioException>(() => consultasConselhoClasse.ObterConselhoClasseTurma("", "", 0, true, false));
        }

        [Fact]
        public async Task DeveObterResultadoAnoAnterior()
        {
            var turma = ObterTurma();
            turma.AnoLetivo = DateTime.Today.Year - 1;
            consultasTurma.Setup(t => t.ObterComUeDrePorCodigo(It.IsAny<string>())).Returns(Task.FromResult(turma));
            consultasFechamentoTurma.Setup(f => f.ObterPorTurmaCodigoBimestreAsync(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(ObterFechamentoTurma()));
            repositorioTipoCalendario.Setup(t => t.BuscarPorAnoLetivoEModalidade(It.IsAny<int>(), It.IsAny<ModalidadeTipoCalendario>(), It.IsAny<int>())).Returns(Task.FromResult(new TipoCalendario()));
            repositorioPeriodoEscolar.Setup(p => p.ObterPorTipoCalendarioEBimestreAsync(It.IsAny<long>(), It.IsAny<int>())).Returns(Task.FromResult(new PeriodoEscolar()));
            consultasPeriodoEscolar.Setup(p => p.ObterUltimoPeriodoAsync(It.IsAny<int>(), It.IsAny<ModalidadeTipoCalendario>(), It.IsAny<int>())).Returns(Task.FromResult(ObterPeriodoEscolar()));
            servicoDeNotasConceitos.Setup(tn => tn.ObterNotaTipo(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<bool>())).Returns(Task.FromResult(new NotaTipoValor()));
            repositorioParametrosSistema.Setup(m => m.ObterValorPorTipoEAno(It.IsAny<TipoParametroSistema>(), null)).Returns(Task.FromResult("10"));
            repositorioConselhoClasseAluno.Setup(c => c.ObterPorConselhoClasseAlunoCodigoAsync(It.IsAny<long>(), It.IsAny<string>())).Returns(Task.FromResult(new ConselhoClasseAluno()));
            Assert.NotNull(consultasConselhoClasse.ObterConselhoClasseTurma("", "", 0, true, false));
        }

        private Turma ObterTurma()
        {
            var turma = new Turma();
            return turma;
        }

        private FechamentoTurma ObterFechamentoTurma()
        {
            return new FechamentoTurma()
            {

            };
        }

        private ConselhoClasse ObterConselhoClasse()
        {
            return new ConselhoClasse() { };
        }

        private PeriodoEscolar ObterPeriodoEscolar()
        {
            return new PeriodoEscolar() { };
        }

        private PeriodoFechamentoBimestre ObterPeriodoFechamentoBimestre()
        {
            return new PeriodoFechamentoBimestre(0, ObterPeriodoEscolar(), null, null) { };
        }

    }
}
