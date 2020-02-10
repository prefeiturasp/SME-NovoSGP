using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasNotasConceitosTeste
    {
        private readonly ConsultasNotasConceitos consultasNotasConceito;
        private readonly Mock<IConsultaAtividadeAvaliativa> consultasAtividadeAvaliativa;
        private readonly Mock<IConsultasDisciplina> consultasDisciplina;
        private readonly Mock<IConsultasFechamentoTurmaDisciplina> consultasFechamentoTurmaDisciplina;
        private readonly Mock<IRepositorioTipoCalendario> repositorioTipoCalendario;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodoEscolar;
        private readonly Mock<IRepositorioAtividadeAvaliativa> repositorioAtividadeAvaliativa;
        private readonly Mock<IRepositorioAtividadeAvaliativaDisciplina> repositorioAtividadeAvaliativaDisciplina;
        private readonly Mock<IRepositorioFrequencia> repositorioFrequencia;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodo> repositorioFrequenciaAluno;
        private readonly Mock<IRepositorioNotaParametro> repositorioNotaParametro;
        private readonly Mock<IRepositorioNotasConceitos> repositorioNotasConceitos;
        private readonly Mock<IRepositorioParametrosSistema> repositorioParametrosSistema;
        private readonly Mock<IRepositorioConceito> repositorioConceito;
        private readonly Mock<IRepositorioTipoAvaliacao> repositorioTipoAvaliacao;
        private readonly Mock<IServicoAluno> servicoAluno;
        private readonly Mock<IServicoDeNotasConceitos> servicoDeNotasConceitos;
        private readonly Mock<IServicoEOL> servicoEOL;
        private readonly Mock<IServicoUsuario> servicoUsuario;


        public ConsultasNotasConceitosTeste()
        {
            servicoEOL = new Mock<IServicoEOL>();
            consultasAtividadeAvaliativa = new Mock<IConsultaAtividadeAvaliativa>();
            servicoDeNotasConceitos = new Mock<IServicoDeNotasConceitos>();
            repositorioNotasConceitos = new Mock<IRepositorioNotasConceitos>();
            repositorioFrequencia = new Mock<IRepositorioFrequencia>();
            servicoUsuario = new Mock<IServicoUsuario>();
            servicoAluno = new Mock<IServicoAluno>();
            repositorioNotaParametro = new Mock<IRepositorioNotaParametro>();
            repositorioAtividadeAvaliativa = new Mock<IRepositorioAtividadeAvaliativa>();
            repositorioAtividadeAvaliativaDisciplina = new Mock<IRepositorioAtividadeAvaliativaDisciplina>();
            repositorioTipoCalendario = new Mock<IRepositorioTipoCalendario>();

            consultasNotasConceito = new ConsultasNotasConceitos(servicoEOL.Object, consultasAtividadeAvaliativa.Object,
            consultasFechamentoTurmaDisciplina.Object, consultasDisciplina.Object,
            servicoDeNotasConceitos.Object, repositorioNotasConceitos.Object,
            repositorioFrequencia.Object, repositorioFrequenciaAluno.Object,
            servicoUsuario.Object, servicoAluno.Object, repositorioTipoCalendario.Object,
            repositorioNotaParametro.Object, repositorioAtividadeAvaliativa.Object,
            repositorioAtividadeAvaliativaDisciplina.Object, repositorioConceito.Object,
            repositorioPeriodoEscolar.Object, repositorioParametrosSistema.Object,
            repositorioTipoAvaliacao.Object);
        }

        [Theory]
        [InlineData(2.4, 0.5, 2.5)]
        [InlineData(2.6, 0.5, 3)]
        [InlineData(3, 0.5, 3)]
        [InlineData(3.5, 0.5, 3.5)]
        [InlineData(7.15, 0.6, 7.6)]
        [InlineData(8.05, 0.04, 9)]
        public void Deve_Arredondar_Nota(double nota, double arredondamento, double esperado)
        {
            repositorioAtividadeAvaliativa.Setup(a => a.ObterPorId(1)).Returns(new Dominio.AtividadeAvaliativa());
            repositorioNotaParametro.Setup(a => a.ObterPorDataAvaliacao(It.IsAny<DateTime>())).Returns(new Dominio.NotaParametro() { Incremento = arredondamento });

            var valorArredondado = consultasNotasConceito.ObterValorArredondado(1, nota);

            Assert.True(esperado == valorArredondado);
        }
    }
}