
using MediatR;
using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasConselhoClasseAlunoTeste
    {
        private readonly ConsultasConselhoClasseAluno consultasConselhoClasseAluno;
        private readonly Mock<IConsultasAulaPrevista> consultasAulaPrevista;
        private readonly Mock<IConsultasConselhoClasseNota> consultasConselhoClasseNota;
        private readonly Mock<IConsultasDisciplina> consultasDisciplina;
        private readonly Mock<IConsultasFechamentoNota> consultasFechamentoNota;
        private readonly Mock<IConsultasFechamentoTurma> consultasFechamentoTurma;
        private readonly Mock<IConsultasFrequencia> consultasFrequencia;
        private readonly Mock<IConsultasPeriodoEscolar> consultasPeriodoEscolar;
        private readonly Mock<IConsultasTipoCalendario> consultasTipoCalendario;
        private readonly Mock<IRepositorioConselhoClasseAlunoConsulta> repositorioConselhoClasseAluno;
        private readonly Mock<IRepositorioTurma> repositorioTurma;
        private readonly Mock<IRepositorioTipoCalendario> repositorioTipoCalendario;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodo> repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly Mock<IServicoConselhoClasse> servicoConselhoClasse;
        private readonly Mock<IServicoEol> servicoEOL;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodoEscolar;
        private readonly Mock<IConsultasPeriodoFechamento> consultasPeriodoFechamento;

        public ConsultasConselhoClasseAlunoTeste()
        {
            repositorioConselhoClasseAluno = new Mock<IRepositorioConselhoClasseAlunoConsulta>();
            repositorioTipoCalendario = new Mock<IRepositorioTipoCalendario>();
            consultasPeriodoEscolar = new Mock<IConsultasPeriodoEscolar>();
            consultasDisciplina = new Mock<IConsultasDisciplina>();
            consultasTipoCalendario = new Mock<IConsultasTipoCalendario>();
            consultasFechamentoTurma = new Mock<IConsultasFechamentoTurma>();
            consultasAulaPrevista = new Mock<IConsultasAulaPrevista>();
            consultasConselhoClasseNota = new Mock<IConsultasConselhoClasseNota>();
            consultasFechamentoNota = new Mock<IConsultasFechamentoNota>();
            servicoEOL = new Mock<IServicoEol>();
            servicoUsuario = new Mock<IServicoUsuario>();
            repositorioFrequenciaAlunoDisciplinaPeriodo = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodo>();
            consultasFrequencia = new Mock<IConsultasFrequencia>();
            servicoConselhoClasse = new Mock<IServicoConselhoClasse>();
            mediator = new Mock<IMediator>();
            repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolar>();
            repositorioTurma = new Mock<IRepositorioTurma>();
            consultasPeriodoFechamento = new Mock<IConsultasPeriodoFechamento>();

            consultasConselhoClasseAluno = new ConsultasConselhoClasseAluno(repositorioConselhoClasseAluno.Object,
                                             repositorioTurma.Object,
                                             consultasDisciplina.Object,
                                             repositorioTipoCalendario.Object,
                                             consultasPeriodoEscolar.Object,
                                             consultasConselhoClasseNota.Object,
                                             servicoEOL.Object,
                                             servicoUsuario.Object,
                                             repositorioFrequenciaAlunoDisciplinaPeriodo.Object,
                                             consultasFrequencia.Object,
                                             servicoConselhoClasse.Object,
                                             consultasPeriodoFechamento.Object,
                                             mediator.Object);
        }

        [Fact]
        public void ObtemSintesesAluno()
        {
            consultasFechamentoTurma.Setup(a => a.ObterCompletoPorIdAsync(It.IsAny<long>())).Returns(Task.FromResult(new FechamentoTurma() { Turma = new Turma() { CodigoTurma = "1234" } }));
            consultasPeriodoEscolar.Setup(a => a.ObterUltimoPeriodoAsync(It.IsAny<int>(), It.IsAny<ModalidadeTipoCalendario>(), It.IsAny<int>())).Returns(Task.FromResult(new PeriodoEscolar()));
            repositorioConselhoClasseAluno.Setup(a => a.ObterPorPeriodoAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(new ConselhoClasseAluno()));
            servicoConselhoClasse.Setup(a => a.VerificaNotasTodosComponentesCurriculares(It.IsAny<string>(), It.IsAny<Turma>(), It.IsAny<long>())).Returns(Task.FromResult(true));
            servicoUsuario.Setup(a => a.ObterUsuarioLogado()).Returns(Task.FromResult(new Usuario()));
            servicoEOL.Setup(a => a.ObterDisciplinasPorCodigoTurma(It.IsAny<string>())).Returns(Task.FromResult(Enumerable.Empty<DisciplinaResposta>()));
            repositorioFrequenciaAlunoDisciplinaPeriodo.Setup(a => a.ObterFrequenciaBimestresAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<TipoFrequenciaAluno>())).Returns(Task.FromResult(Enumerable.Empty<FrequenciaAluno>()));
            Assert.NotNull(consultasConselhoClasseAluno.ObterListagemDeSinteses(0, 0, "", "", 0));
        }

        [Fact]
        public async Task ErroSemFechamentoTurmaObtemSintesesAluno()
        {
            await Assert.ThrowsAsync<NegocioException>(() => consultasConselhoClasseAluno.ObterListagemDeSinteses(0, 0, "", "", 0));
        }
        [Fact]
        public async Task ErroAlunoNaoPossuiConselhoUltimoBimestreAnoAtual()
        {
            if (DateTime.Today.Year == 2020)
                return;

            consultasPeriodoEscolar.Setup(a => a.ObterUltimoPeriodoAsync(It.IsAny<int>(), It.IsAny<ModalidadeTipoCalendario>(), It.IsAny<int>())).Returns(Task.FromResult(new PeriodoEscolar()));
            repositorioConselhoClasseAluno.Setup(a => a.ObterPorPeriodoAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(new ConselhoClasseAluno()));
            servicoConselhoClasse.Setup(a => a.VerificaNotasTodosComponentesCurriculares(It.IsAny<string>(), It.IsAny<Turma>(), It.IsAny<long>())).Returns(Task.FromResult(false));
            consultasFechamentoTurma.Setup(a => a.ObterCompletoPorIdAsync(It.IsAny<long>())).Returns(Task.FromResult(
                new FechamentoTurma()
                {
                    Turma = new Turma()
                    {
                        CodigoTurma = "1234",
                        AnoLetivo = DateTime.Today.Year
                    }
                })
            );
            await Assert.ThrowsAsync<NegocioException>(() => consultasConselhoClasseAluno.ObterListagemDeSinteses(0, 0, "", "", 0));
        }

        [Fact]
        public void ObterParecerConclusivo()
        {
            consultasFechamentoTurma.Setup(a => a.ObterCompletoPorIdAsync(It.IsAny<long>())).Returns(Task.FromResult(new FechamentoTurma()
            {
                Turma = new Turma()
                {
                    AnoLetivo = DateTime.Today.Year
                }
            }));
            consultasPeriodoEscolar.Setup(a => a.ObterUltimoPeriodoAsync(It.IsAny<int>(), It.IsAny<ModalidadeTipoCalendario>(), It.IsAny<int>())).Returns(Task.FromResult(new PeriodoEscolar()));
            repositorioConselhoClasseAluno.Setup(a => a.ObterPorPeriodoAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(new ConselhoClasseAluno()));
            repositorioConselhoClasseAluno.Setup(a => a.ObterPorConselhoClasseAlunoCodigoAsync(It.IsAny<long>(), It.IsAny<string>())).Returns(Task.FromResult(new ConselhoClasseAluno()));
            Assert.NotNull(consultasConselhoClasseAluno.ObterParecerConclusivo(0, 0, "", ""));
        }

        [Fact]
        public async Task ObterParecerConclusivoErroAoObterFechamentoTurma()
        {
            await Assert.ThrowsAsync<NegocioException>(() => consultasConselhoClasseAluno.ObterParecerConclusivo(0, 0, "", ""));
        }


        [Fact]
        public async Task ObterParecerConclusivoErroAlunoNaoPossuiConselhoClasse()
        {
            if (DateTime.Today.Year == 2020)
                return;

            consultasFechamentoTurma.Setup(a => a.ObterCompletoPorIdAsync(It.IsAny<long>())).Returns(Task.FromResult(new FechamentoTurma()
            {
                Turma = new Turma()
                {
                    AnoLetivo = DateTime.Today.Year
                }
            }));
            consultasPeriodoEscolar.Setup(a => a.ObterUltimoPeriodoAsync(It.IsAny<int>(), It.IsAny<ModalidadeTipoCalendario>(), It.IsAny<int>())).Returns(Task.FromResult(new PeriodoEscolar()));
            repositorioConselhoClasseAluno.Setup(a => a.ObterPorPeriodoAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<long>())).Returns(Task.FromResult(new ConselhoClasseAluno()));
            servicoConselhoClasse.Setup(a => a.VerificaNotasTodosComponentesCurriculares(It.IsAny<string>(), It.IsAny<Turma>(), It.IsAny<long>())).Returns(Task.FromResult(false));
            await Assert.ThrowsAsync<NegocioException>(() => consultasConselhoClasseAluno.ObterParecerConclusivo(0, 0, "", ""));
        }
    }
}
