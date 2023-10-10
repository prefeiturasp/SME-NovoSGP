using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasAulaPrevistaTeste
    {
        private readonly Mock<IRepositorioPeriodoEscolarConsulta> repositorioPeriodoEscolar;
        private readonly Mock<IRepositorioAulaPrevistaConsulta> repositorioAulaPrevistaConsulta;
        private readonly Mock<IRepositorioAulaPrevistaBimestreConsulta> repositorioBimestre;
        private readonly Mock<IRepositorioTipoCalendarioConsulta> repositorioTipoCalendario;
        private readonly Mock<IConsultasTurma> consultasTurma;
        private readonly Mock<IMediator> mediator;

        public ConsultasAulaPrevistaTeste()
        {
            repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolarConsulta>();
            repositorioAulaPrevistaConsulta = new Mock<IRepositorioAulaPrevistaConsulta>();
            repositorioBimestre = new Mock<IRepositorioAulaPrevistaBimestreConsulta>();
            repositorioTipoCalendario = new Mock<IRepositorioTipoCalendarioConsulta>();
            consultasTurma = new Mock<IConsultasTurma>();
            mediator = new Mock<IMediator>();
        }

        [Fact(DisplayName = "Consultas Aula Prevista - Deve obter aula prevista dada considerando agrupamento dos componentes do infantil")]
        public async Task DeveObterAulaPrevistaDadaConsiderandoAgrupamentoComponentesDoInfantil()
        {
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { ModalidadeCodigo = Modalidade.EducacaoInfantil });

            repositorioTipoCalendario.Setup(x => x.BuscarPorAnoLetivoEModalidade(It.IsAny<int>(), ModalidadeTipoCalendario.Infantil, It.IsAny<int>()))
                .ReturnsAsync(new TipoCalendario());

            mediator.Setup(x => x.Send(It.IsAny<ObterAulasPrevistasPorCodigoUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AulaPrevista>().AsEnumerable());

            var usuario = new Usuario() { PerfilAtual = Perfis.PERFIL_PROFESSOR_INFANTIL };
            usuario.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil() { CodigoPerfil = Perfis.PERFIL_PROFESSOR_INFANTIL } });

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            var consulta = new ConsultasAulaPrevista(repositorioAulaPrevistaConsulta.Object,
                                                     repositorioBimestre.Object,
                                                     repositorioPeriodoEscolar.Object,                                                                                                          
                                                     repositorioTipoCalendario.Object,
                                                     consultasTurma.Object,
                                                     mediator.Object);

            await consulta.ObterAulaPrevistaDada(Modalidade.EducacaoInfantil, "1", "1");

            mediator.Verify(x => 
                x.Send(It.Is<ObterComponentesCurricularesDoProfessorNaTurmaQuery>(x => x.RealizarAgrupamentoComponente), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
