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
        private readonly Mock<IConsultasGrade> consultasGrade;
        private readonly Mock<IRepositorioAbrangencia> repositorioAbrangencia;
        private readonly Mock<IRepositorioAula> repositorioAula;
        private readonly Mock<IConsultasPeriodoEscolar> consultasPeriodoEscolar;
        private readonly Mock<IRepositorioTipoCalendario> repositorioTipoCalendario;
        private readonly IServicoAula servicoAula;
        private readonly Mock<IServicoDiaLetivo> servicoDiaLetivo;
        private readonly Mock<IServicoEOL> servicoEol;
        private readonly Mock<IServicoLog> servicoLog;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly Mock<IComandosWorkflowAprovacao> comandosWorkflowAprovacao;
        private readonly Mock<IConsultasAbrangencia> consultaAbrangencia;
        private readonly Mock<IConfiguration> configuration;
        private readonly Mock<IServicoNotificacao> servicoNotificacao;
        private readonly Mock<IComandosPlanoAula> comandosPlanoAula;
        private readonly Mock<IServicoFrequencia> servicoFrequencia;

        #endregion
        Usuario usuario;
        Aula aula;

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
            consultaAbrangencia = new Mock<IConsultasAbrangencia>();
            servicoNotificacao = new Mock<IServicoNotificacao>();
            comandosPlanoAula = new Mock<IComandosPlanoAula>();
            servicoFrequencia = new Mock<IServicoFrequencia>();
            servicoUsuario = new Mock<IServicoUsuario>();
            configuration = new Mock<IConfiguration>();

            servicoAula = new ServicoAula(repositorioAula.Object, servicoEol.Object,
                                         repositorioTipoCalendario.Object, servicoDiaLetivo.Object, 
                                         consultasGrade.Object, consultasPeriodoEscolar.Object, 
                                         servicoLog.Object, repositorioAbrangencia.Object,
                                         servicoNotificacao.Object, consultaAbrangencia.Object , 
                                         servicoUsuario.Object, comandosWorkflowAprovacao.Object,
                                         comandosPlanoAula.Object, servicoFrequencia.Object,
                                         configuration.Object);

            Setup();
        }

        private void Setup()
        {
            aula = new Aula()
            {
                DisciplinaId = "1",
                UeId = "1",
                DataAula = new DateTime(2019,12,2),
                TurmaId = "1",
                Quantidade = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica
            };

            usuario = new Usuario();
            usuario.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil() { CodigoPerfil = Guid.Parse("40E1E074-37D6-E911-ABD6-F81654FE895D") } });

            var tipoCalendario = new TipoCalendario();
            IEnumerable<DisciplinaResposta> disciplinaRespotas = new List<DisciplinaResposta>() { new DisciplinaResposta() { CodigoComponenteCurricular = 1 } };

            repositorioAula.Setup(a => a.UsuarioPodeCriarAulaNaUeTurmaEModalidade(It.IsAny<Aula>(), It.IsAny<ModalidadeTipoCalendario>())).Returns(true);

            repositorioTipoCalendario.Setup(a => a.ObterPorId(It.IsAny<long>())).Returns(tipoCalendario);

            servicoEol.Setup(a => a.ObterDisciplinasPorCodigoTurmaLoginEPerfil(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(disciplinaRespotas));

            //repositorioPeriodoEscolar.Setup(a => a.ObterPorTipoCalendarioData(aula.TipoCalendarioId, aula.DataAula)).Returns(new PeriodoEscolar());
            consultasGrade.Setup(a => a.ObterGradeAulasTurma(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new GradeComponenteTurmaAulasDto() { QuantidadeAulasGrade = 1, QuantidadeAulasRestante = 1 }));

            servicoDiaLetivo.Setup(a => a.ValidarSeEhDiaLetivo(It.IsAny<DateTime>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            ///////
            var periodoEscolar = new PeriodoEscolar() { PeriodoInicio = new DateTime(2019, 1, 1), PeriodoFim = new DateTime(2019, 1, 31) };

            //repositorioPeriodoEscolar.Setup(a => a.ObterPorTipoCalendarioData(aula.TipoCalendarioId, aula.DataAula)).Returns(periodoEscolar);
            //repositorioPeriodoEscolar.Setup(a => a.ObterPorTipoCalendario(aula.TipoCalendarioId)).Returns(new List<PeriodoEscolar>() { periodoEscolar });
            repositorioAbrangencia.Setup(a => a.ObterAbrangenciaTurma(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(new AbrangenciaFiltroRetorno() { NomeDre = "Dre 1", NomeUe = "Ue 1", NomeTurma = "Turma 1A" }));
        }

        [Fact]
        public async void Deve_Incluir_Aula()
        {
            //ACT
            await servicoAula.Salvar(aula, usuario, RecorrenciaAula.AulaUnica);

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
            await servicoAula.Salvar(aula, usuario, aula.RecorrenciaAula);

            //ASSERT
            repositorioAula.Verify(c => c.Salvar(aula), Times.Exactly(1));
        }
    }
}  