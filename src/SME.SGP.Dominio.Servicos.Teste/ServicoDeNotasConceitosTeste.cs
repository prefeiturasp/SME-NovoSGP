using Microsoft.Extensions.Configuration;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoDeNotasConceitosTeste
    {
        private readonly Mock<IConsultasAbrangencia> consultasAbrangencia;
        private readonly Mock<IRepositorioAtividadeAvaliativa> repositorioAtividadeAvaliativa;
        private readonly Mock<IRepositorioAtividadeAvaliativaDisciplina> repositorioAtividadeAvaliativaDisciplina;
        private readonly Mock<IRepositorioCiclo> repositorioCiclo;
        private readonly Mock<IRepositorioConceito> repositorioConceito;
        private readonly Mock<IRepositorioNotaParametro> repositorioNotaParametro;
        private readonly Mock<IRepositorioNotasConceitos> repositorioNotasConceitos;
        private readonly Mock<IRepositorioNotaTipoValor> repositorioNotaTipoValor;
        private readonly ServicoDeNotasConceitos servicoDeNotasConceitos;
        private readonly Mock<IServicoEOL> servicoEOL;
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<IServicoNotificacao> servicoNotificacao;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodoEscolar;
        private readonly Mock<IRepositorioAula> repositorioAula;
        private readonly Mock<IRepositorioTurma> repositorioTurma;
        private readonly Mock<IRepositorioParametrosSistema> repositorioParametrosSistema;
        private readonly Mock<IRepositorioPeriodoFechamento> repositorioPeriodoFechamento;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly Mock<IConfiguration> configuration;

        public ServicoDeNotasConceitosTeste()
        {
            consultasAbrangencia = new Mock<IConsultasAbrangencia>();
            repositorioAtividadeAvaliativa = new Mock<IRepositorioAtividadeAvaliativa>();
            repositorioCiclo = new Mock<IRepositorioCiclo>();
            repositorioConceito = new Mock<IRepositorioConceito>();
            repositorioNotaParametro = new Mock<IRepositorioNotaParametro>();
            repositorioNotaTipoValor = new Mock<IRepositorioNotaTipoValor>();
            servicoEOL = new Mock<IServicoEOL>();
            repositorioNotasConceitos = new Mock<IRepositorioNotasConceitos>();
            unitOfWork = new Mock<IUnitOfWork>();
            servicoNotificacao = new Mock<IServicoNotificacao>();
            repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolar>();
            repositorioAula = new Mock<IRepositorioAula>();
            repositorioTurma = new Mock<IRepositorioTurma>();
            repositorioParametrosSistema = new Mock<IRepositorioParametrosSistema>();
            repositorioPeriodoFechamento = new Mock<IRepositorioPeriodoFechamento>();
            configuration = new Mock<IConfiguration>();

            servicoDeNotasConceitos = new ServicoDeNotasConceitos(
                repositorioAtividadeAvaliativa.Object,
                servicoEOL.Object,
                consultasAbrangencia.Object,
                repositorioNotaTipoValor.Object,
                repositorioCiclo.Object,
                repositorioConceito.Object,
                repositorioNotaParametro.Object,
                repositorioNotasConceitos.Object,
                unitOfWork.Object,
                repositorioAtividadeAvaliativaDisciplina.Object,
                repositorioPeriodoFechamento.Object,
                servicoNotificacao.Object,
                repositorioPeriodoEscolar.Object,
                repositorioAula.Object,
                repositorioTurma.Object,
                repositorioParametrosSistema.Object,
                servicoUsuario.Object,
                configuration.Object);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Se_Instanciar_Sem_Dependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(null, servicoEOL.Object, consultasAbrangencia.Object, repositorioNotaTipoValor.Object, repositorioCiclo.Object, repositorioConceito.Object, repositorioNotaParametro.Object, repositorioNotasConceitos.Object, unitOfWork.Object, repositorioAtividadeAvaliativaDisciplina.Object, repositorioPeriodoFechamento.Object, servicoNotificacao.Object, repositorioPeriodoEscolar.Object,repositorioAula.Object, repositorioTurma.Object, repositorioParametrosSistema.Object, servicoUsuario.Object, configuration.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, null, consultasAbrangencia.Object, repositorioNotaTipoValor.Object, repositorioCiclo.Object, repositorioConceito.Object, repositorioNotaParametro.Object, repositorioNotasConceitos.Object, unitOfWork.Object, repositorioAtividadeAvaliativaDisciplina.Object, repositorioPeriodoFechamento.Object, servicoNotificacao.Object, repositorioPeriodoEscolar.Object, repositorioAula.Object, repositorioTurma.Object, repositorioParametrosSistema.Object, servicoUsuario.Object, configuration.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, servicoEOL.Object, null, repositorioNotaTipoValor.Object, repositorioCiclo.Object, repositorioConceito.Object, repositorioNotaParametro.Object, repositorioNotasConceitos.Object, unitOfWork.Object, repositorioAtividadeAvaliativaDisciplina.Object, repositorioPeriodoFechamento.Object, servicoNotificacao.Object, repositorioPeriodoEscolar.Object, repositorioAula.Object, repositorioTurma.Object, repositorioParametrosSistema.Object, servicoUsuario.Object, configuration.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, servicoEOL.Object, consultasAbrangencia.Object, null, repositorioCiclo.Object, repositorioConceito.Object, repositorioNotaParametro.Object, repositorioNotasConceitos.Object, unitOfWork.Object, repositorioAtividadeAvaliativaDisciplina.Object, repositorioPeriodoFechamento.Object, servicoNotificacao.Object, repositorioPeriodoEscolar.Object, repositorioAula.Object, repositorioTurma.Object, repositorioParametrosSistema.Object, servicoUsuario.Object, configuration.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, servicoEOL.Object, consultasAbrangencia.Object, repositorioNotaTipoValor.Object, null, repositorioConceito.Object, repositorioNotaParametro.Object, repositorioNotasConceitos.Object, unitOfWork.Object, repositorioAtividadeAvaliativaDisciplina.Object, repositorioPeriodoFechamento.Object, servicoNotificacao.Object, repositorioPeriodoEscolar.Object, repositorioAula.Object, repositorioTurma.Object, repositorioParametrosSistema.Object, servicoUsuario.Object, configuration.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, servicoEOL.Object, consultasAbrangencia.Object, repositorioNotaTipoValor.Object, repositorioCiclo.Object, null, repositorioNotaParametro.Object, repositorioNotasConceitos.Object, unitOfWork.Object, repositorioAtividadeAvaliativaDisciplina.Object, repositorioPeriodoFechamento.Object, servicoNotificacao.Object, repositorioPeriodoEscolar.Object, repositorioAula.Object, repositorioTurma.Object, repositorioParametrosSistema.Object, servicoUsuario.Object, configuration.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, servicoEOL.Object, consultasAbrangencia.Object, repositorioNotaTipoValor.Object, repositorioCiclo.Object, repositorioConceito.Object, null, repositorioNotasConceitos.Object, unitOfWork.Object, repositorioAtividadeAvaliativaDisciplina.Object, repositorioPeriodoFechamento.Object, servicoNotificacao.Object, repositorioPeriodoEscolar.Object, repositorioAula.Object, repositorioTurma.Object, repositorioParametrosSistema.Object, servicoUsuario.Object, configuration.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, servicoEOL.Object, consultasAbrangencia.Object, repositorioNotaTipoValor.Object, repositorioCiclo.Object, repositorioConceito.Object, repositorioNotaParametro.Object, null, unitOfWork.Object, repositorioAtividadeAvaliativaDisciplina.Object, repositorioPeriodoFechamento.Object, servicoNotificacao.Object, repositorioPeriodoEscolar.Object, repositorioAula.Object, repositorioTurma.Object, repositorioParametrosSistema.Object, servicoUsuario.Object, configuration.Object));

            Assert.Throws<ArgumentNullException>(() => new ServicoDeNotasConceitos(repositorioAtividadeAvaliativa.Object, servicoEOL.Object, consultasAbrangencia.Object, repositorioNotaTipoValor.Object, repositorioCiclo.Object, repositorioConceito.Object, repositorioNotaParametro.Object, repositorioNotasConceitos.Object, null, repositorioAtividadeAvaliativaDisciplina.Object, repositorioPeriodoFechamento.Object, servicoNotificacao.Object, repositorioPeriodoEscolar.Object, repositorioAula.Object, repositorioTurma.Object, repositorioParametrosSistema.Object, servicoUsuario.Object, configuration.Object));
        }
    }
}