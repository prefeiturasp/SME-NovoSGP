using Microsoft.Extensions.Configuration;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoAulaTeste
    {
        #region Mocks

        private readonly Mock<IComandosPlanoAula> comandosPlanoAula;
        private readonly Mock<IComandosNotificacaoAula> comandosNotificacaoAula;
        private readonly Mock<IComandosWorkflowAprovacao> comandosWorkflowAprovacao;
        private readonly Mock<IConfiguration> configuration;
        private readonly Mock<IConsultasFrequencia> consultasFrequencia;
        private readonly Mock<IConsultasGrade> consultasGrade;
        private readonly Mock<IConsultasPeriodoEscolar> consultasPeriodoEscolar;
        private readonly Mock<IConsultasPlanoAula> consultasPlanoAula;
        private readonly Mock<IRepositorioAbrangencia> repositorioAbrangencia;
        private readonly Mock<IRepositorioAtividadeAvaliativa> repositorioAtividadeAvaliativa;
        private readonly Mock<IRepositorioAtribuicaoCJ> repositorioAtribuicaoCJ;
        private readonly Mock<IRepositorioAula> repositorioAula;
        private readonly Mock<IRepositorioTipoCalendario> repositorioTipoCalendario;
        private readonly Mock<IRepositorioTurma> repositorioTurma;
        private readonly IServicoAula servicoAula;
        private readonly Mock<IServicoDiaLetivo> servicoDiaLetivo;
        private readonly Mock<IServicoEOL> servicoEol;
        private readonly Mock<IServicoFrequencia> servicoFrequencia;
        private readonly Mock<IServicoLog> servicoLog;
        private readonly Mock<IServicoNotificacao> servicoNotificacao;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly Mock<IServicoWorkflowAprovacao> servicoWorkflowAprovacao;
        private readonly Mock<IUnitOfWork> unitOfWork;

        #endregion Mocks

        private Aula aula;
        private Usuario usuario;

        public ServicoAulaTeste()
        {
            consultasPeriodoEscolar = new Mock<IConsultasPeriodoEscolar>();
            servicoDiaLetivo = new Mock<IServicoDiaLetivo>();
            repositorioAula = new Mock<IRepositorioAula>();
            repositorioTipoCalendario = new Mock<IRepositorioTipoCalendario>();
            servicoLog = new Mock<IServicoLog>();
            servicoEol = new Mock<IServicoEOL>();
            consultasGrade = new Mock<IConsultasGrade>();
            repositorioAbrangencia = new Mock<IRepositorioAbrangencia>();
            servicoNotificacao = new Mock<IServicoNotificacao>();
            comandosWorkflowAprovacao = new Mock<IComandosWorkflowAprovacao>();
            comandosNotificacaoAula = new Mock<IComandosNotificacaoAula>();
            servicoNotificacao = new Mock<IServicoNotificacao>();
            comandosPlanoAula = new Mock<IComandosPlanoAula>();
            consultasPeriodoEscolar = new Mock<IConsultasPeriodoEscolar>();
            consultasFrequencia = new Mock<IConsultasFrequencia>();
            consultasPlanoAula = new Mock<IConsultasPlanoAula>();
            servicoFrequencia = new Mock<IServicoFrequencia>();
            servicoUsuario = new Mock<IServicoUsuario>();
            repositorioTurma = new Mock<IRepositorioTurma>();
            configuration = new Mock<IConfiguration>();
            repositorioAtividadeAvaliativa = new Mock<IRepositorioAtividadeAvaliativa>();
            repositorioTurma = new Mock<IRepositorioTurma>();
            repositorioAtribuicaoCJ = new Mock<IRepositorioAtribuicaoCJ>();
            consultasFrequencia = new Mock<IConsultasFrequencia>();
            consultasPlanoAula = new Mock<IConsultasPlanoAula>();
            servicoWorkflowAprovacao = new Mock<IServicoWorkflowAprovacao>();
            unitOfWork = new Mock<IUnitOfWork>();

            servicoAula = new ServicoAula(repositorioAula.Object,
                                          servicoEol.Object,
                                          repositorioTipoCalendario.Object,
                                          servicoDiaLetivo.Object,
                                          consultasGrade.Object,
                                          consultasPeriodoEscolar.Object,
                                          consultasFrequencia.Object,
                                          consultasPlanoAula.Object,
                                          servicoLog.Object,
                                          servicoNotificacao.Object,
                                          comandosWorkflowAprovacao.Object,
                                          comandosPlanoAula.Object,
                                          comandosNotificacaoAula.Object,
                                          servicoFrequencia.Object,
                                          configuration.Object,
                                          repositorioAtividadeAvaliativa.Object,
                                          repositorioAtribuicaoCJ.Object,
                                          repositorioTurma.Object,
                                          servicoWorkflowAprovacao.Object,
                                          servicoUsuario.Object,
                                          unitOfWork.Object);

            Setup();
        }

        [Fact]
        public async void Deve_Alterar_Aula_Com_Recorrencia()
        {
            aula.Id = 1;
            aula.DataAula = aula.DataAula.AddDays(2);

            var msg = servicoAula.Salvar(aula, usuario, RecorrenciaAula.RepetirBimestreAtual, aula.Quantidade);

            // ASSERT
            Assert.False(msg == "");
            repositorioAula.Verify(c => c.Salvar(It.IsAny<Aula>()), Times.Exactly(3));
            servicoNotificacao.Verify(c => c.Salvar(It.IsAny<Notificacao>()), Times.Once());
        }

        [Fact]
        public async void Deve_Consistir_Dia_Letivo()
        {
            servicoDiaLetivo.Setup(a => a.ValidarSeEhDiaLetivo(It.IsAny<DateTime>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            Assert.Throws<NegocioException>(() => servicoAula.Salvar(aula, usuario, aula.RecorrenciaAula));
        }

        [Fact]
        public async void Deve_Consistir_Disciplina()
        {
            aula.DisciplinaId = "2";

            Assert.Throws<NegocioException>(() => servicoAula.Salvar(aula, usuario, aula.RecorrenciaAula));
        }

        [Fact]
        public async void Deve_Consistir_Grade()
        {
            aula.Quantidade = 2;

            Assert.Throws<NegocioException>(() => servicoAula.Salvar(aula, usuario, aula.RecorrenciaAula));
        }

        [Fact]
        public async void Deve_Consistir_Recorrencia_Reposicao()
        {
            aula.TipoAula = TipoAula.Reposicao;
            aula.RecorrenciaAula = RecorrenciaAula.RepetirBimestreAtual;

            Assert.Throws<NegocioException>(() => servicoAula.Salvar(aula, usuario, aula.RecorrenciaAula));
        }

        [Fact]
        public async void Deve_Excluir_Aula_Com_Recorrencia()
        {
            aula.Id = 1;

            var msg = await servicoAula.Excluir(aula, RecorrenciaAula.RepetirBimestreAtual, usuario);

            // ASSERT
            Assert.False(msg == "");
            repositorioAula.Verify(c => c.SalvarAsync(It.IsAny<Aula>()), Times.Exactly(3));
            servicoNotificacao.Verify(c => c.Salvar(It.IsAny<Notificacao>()), Times.Once());
        }

        [Fact]
        public async void Deve_Incluir_Aula()
        {
            //ACT
            servicoAula.Salvar(aula, usuario, RecorrenciaAula.AulaUnica);

            //ASSERT
            repositorioAula.Verify(c => c.Salvar(aula), Times.Once);
        }

        [Fact]
        public async void Deve_Incluir_Aula_Recorrencia()
        {
            //ARRANGE
            aula = new Aula()
            {
                DisciplinaId = "1",
                UeId = "1",
                DataAula = new DateTime(2019, 1, 1),
                TurmaId = "1",
                Quantidade = 1,
                RecorrenciaAula = RecorrenciaAula.RepetirBimestreAtual
            };

            consultasPeriodoEscolar.Setup(a => a.ObterFimPeriodoRecorrencia(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<RecorrenciaAula>())).Returns(new DateTime(2019, 3, 31));

            //ACT
            servicoAula.Salvar(aula, usuario, aula.RecorrenciaAula);

            //ASSERT
            repositorioAula.Verify(c => c.Salvar(aula), Times.Exactly(1));
        }

        private void Setup()
        {
            aula = new Aula()
            {
                DisciplinaId = "1",
                UeId = "1",
                DataAula = new DateTime(2019, 12, 2),
                TurmaId = "1",
                Quantidade = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica
            };

            IEnumerable<Aula> aulasRecorrentes = new List<Aula>()
            {
                new Aula() { Id = 2, DataAula = DateTime.Parse("2019-12-09"), UeId = "1", TurmaId = "1", Quantidade = 1, DisciplinaId = "1", RecorrenciaAula = RecorrenciaAula.AulaUnica },
                new Aula() { Id = 2, DataAula = DateTime.Parse("2019-12-16"), UeId = "1", TurmaId = "1", Quantidade = 1, DisciplinaId = "1", RecorrenciaAula = RecorrenciaAula.AulaUnica },
            };

            repositorioAula.Setup(a => a.UsuarioPodeCriarAulaNaUeTurmaEModalidade(It.IsAny<Aula>(), It.IsAny<ModalidadeTipoCalendario>())).Returns(true);
            repositorioAula.Setup(a => a.ObterAulasRecorrencia(It.IsAny<long>(), It.IsAny<long?>(), It.IsAny<DateTime?>())).Returns(Task.FromResult(aulasRecorrentes));

            usuario = new Usuario();
            usuario.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil() { CodigoPerfil = Guid.Parse("40E1E074-37D6-E911-ABD6-F81654FE895D") } });

            var tipoCalendario = new TipoCalendario();
            IEnumerable<DisciplinaResposta> disciplinaRespotas = new List<DisciplinaResposta>() { new DisciplinaResposta() { CodigoComponenteCurricular = 1 } };

            repositorioTipoCalendario.Setup(a => a.ObterPorId(It.IsAny<long>())).Returns(tipoCalendario);

            servicoEol.Setup(a => a.ObterDisciplinasPorCodigoTurmaLoginEPerfil(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(disciplinaRespotas));

            //repositorioPeriodoEscolar.Setup(a => a.ObterPorTipoCalendarioData(aula.TipoCalendarioId, aula.DataAula)).Returns(new PeriodoEscolar());
            consultasGrade.Setup(a => a.ObterGradeAulasTurmaProfessor(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new GradeComponenteTurmaAulasDto() { QuantidadeAulasGrade = 1, QuantidadeAulasRestante = 1 }));

            servicoDiaLetivo.Setup(a => a.ValidarSeEhDiaLetivo(It.IsAny<DateTime>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            ///////
            var periodoEscolar = new PeriodoEscolar() { PeriodoInicio = new DateTime(2019, 1, 1), PeriodoFim = new DateTime(2019, 1, 31) };

            //repositorioPeriodoEscolar.Setup(a => a.ObterPorTipoCalendarioData(aula.TipoCalendarioId, aula.DataAula)).Returns(periodoEscolar);
            //repositorioPeriodoEscolar.Setup(a => a.ObterPorTipoCalendario(aula.TipoCalendarioId)).Returns(new List<PeriodoEscolar>() { periodoEscolar });
            repositorioAbrangencia.Setup(a => a.ObterAbrangenciaTurma(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AbrangenciaFiltroRetorno() { NomeDre = "Dre 1", NomeUe = "Ue 1", NomeTurma = "Turma 1A" }));

            consultasFrequencia.Setup(a => a.FrequenciaAulaRegistrada(It.IsAny<long>()))
                .Returns(Task.FromResult(false));
        }
    }
}