using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasNotasConceitosTeste
    {
        private readonly Mock<IConsultaAtividadeAvaliativa> consultaAtividadeAvaliativa;
        private readonly Mock<IConsultasFechamentoTurmaDisciplina> consultasFechamentoTurmaDisciplina;
        private readonly Mock<IConsultasDisciplina> consultasDisciplina;
        private readonly Mock<IConsultasPeriodoFechamento> consultasFechamento;
        private readonly ConsultasNotasConceitos consultasNotasConceito;
        private readonly Mock<IRepositorioAtividadeAvaliativa> repositorioAtividadeAvaliativa;
        private readonly Mock<IRepositorioFrequencia> repositorioFrequencia;
        private readonly Mock<IRepositorioNotaParametro> repositorioNotaParametro;
        private readonly Mock<IRepositorioNotasConceitos> repositorioNotasConceitos;
        private readonly Mock<IRepositorioTipoCalendario> repositorioTipoCalendario;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodo> repositorioFrequenciaAluno;
        private readonly Mock<IRepositorioConceito> repositorioConceito;
        private readonly Mock<IServicoAluno> servicoAluno;
        private readonly Mock<IServicoDeNotasConceitos> servicoDeNotasConceitos;
        private readonly Mock<IServicoEol> servicoEOL;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly Mock<IRepositorioAtividadeAvaliativaDisciplina> repositorioAtividadeAvaliativaDisciplina;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodoEscolar;
        private readonly Mock<IRepositorioParametrosSistema> repositorioParametrosSistema;
        private readonly Mock<IRepositorioTipoAvaliacao> repositorioTipoAvaliacao;
        private readonly Mock<IRepositorioTurma> repositorioTurma;
        private readonly Mock<IRepositorioUe> repositorioUe;
        private readonly Mock<IRepositorioDre> repositorioDre;
        private readonly Mock<IRepositorioEvento> repositorioEvento;
        private readonly Mock<IRepositorioAtividadeAvaliativaRegencia> repositorioAtividadeAvaliativaRegencia;

        public ConsultasNotasConceitosTeste()
        {
            servicoEOL = new Mock<IServicoEol>();
            consultaAtividadeAvaliativa = new Mock<IConsultaAtividadeAvaliativa>();
            consultasFechamentoTurmaDisciplina = new Mock<IConsultasFechamentoTurmaDisciplina>();
            consultasDisciplina = new Mock<IConsultasDisciplina>();
            consultasFechamento = new Mock<IConsultasPeriodoFechamento>();
            servicoDeNotasConceitos = new Mock<IServicoDeNotasConceitos>();
            repositorioNotasConceitos = new Mock<IRepositorioNotasConceitos>();
            repositorioFrequencia = new Mock<IRepositorioFrequencia>();
            repositorioFrequenciaAluno = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodo>();
            servicoUsuario = new Mock<IServicoUsuario>();
            servicoAluno = new Mock<IServicoAluno>();
            repositorioTipoCalendario = new Mock<IRepositorioTipoCalendario>();
            repositorioNotaParametro = new Mock<IRepositorioNotaParametro>();
            repositorioAtividadeAvaliativa = new Mock<IRepositorioAtividadeAvaliativa>();
            repositorioAtividadeAvaliativaDisciplina = new Mock<IRepositorioAtividadeAvaliativaDisciplina>();
            repositorioConceito = new Mock<IRepositorioConceito>();
            repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolar>();
            repositorioParametrosSistema = new Mock<IRepositorioParametrosSistema>();
            repositorioTipoAvaliacao = new Mock<IRepositorioTipoAvaliacao>();
            repositorioTurma = new Mock<IRepositorioTurma>();
            repositorioUe = new Mock<IRepositorioUe>();
            repositorioDre = new Mock<IRepositorioDre>();
            repositorioEvento = new Mock<IRepositorioEvento>();
            repositorioAtividadeAvaliativaRegencia = new Mock<IRepositorioAtividadeAvaliativaRegencia>();

            consultasNotasConceito = new ConsultasNotasConceitos(servicoEOL.Object,
                    consultaAtividadeAvaliativa.Object,
                    consultasFechamentoTurmaDisciplina.Object,
                    consultasDisciplina.Object,
                    consultasFechamento.Object,
                    servicoDeNotasConceitos.Object,
                    repositorioNotasConceitos.Object,
                    repositorioFrequencia.Object,
                    repositorioFrequenciaAluno.Object,
                    servicoUsuario.Object,
                    servicoAluno.Object,
                    repositorioTipoCalendario.Object,
                    repositorioNotaParametro.Object,
                    repositorioAtividadeAvaliativa.Object,
                    repositorioAtividadeAvaliativaDisciplina.Object,
                    repositorioConceito.Object,
                    repositorioPeriodoEscolar.Object,
                    repositorioParametrosSistema.Object,
                    repositorioTipoAvaliacao.Object,
                    repositorioTurma.Object,
                    repositorioUe.Object,
                    repositorioDre.Object,
                    repositorioEvento.Object,
                    repositorioAtividadeAvaliativaRegencia.Object);
        }

        [Theory]
        [InlineData(2.4, 0.5, 2.5)]
        [InlineData(2.6, 0.5, 3)]
        [InlineData(3, 0.5, 3)]
        [InlineData(3.5, 0.5, 3.5)]
        [InlineData(7.15, 0.6, 7.6)]
        [InlineData(8.05, 0.04, 9)]
        public async Task Deve_Arredondar_Nota(double nota, double arredondamento, double esperado)
        {
            repositorioAtividadeAvaliativa.Setup(a => a.ObterPorIdAsync(1)).ReturnsAsync(new Dominio.AtividadeAvaliativa());
            repositorioNotaParametro.Setup(a => a.ObterPorDataAvaliacao(It.IsAny<DateTime>())).ReturnsAsync(new Dominio.NotaParametro() { Incremento = arredondamento });

            var valorArredondado = await consultasNotasConceito.ObterValorArredondado(1, nota);

            Assert.True(esperado == valorArredondado);
        }
    }
}