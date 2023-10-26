using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasDisciplinasTeste
    {
        private readonly Mock<IConsultasObjetivoAprendizagem> consultasObjetivoAprendizagem;
        private readonly Mock<IRepositorioAtribuicaoCJ> repositorioAtribuicaoCJ;
        private readonly Mock<IRepositorioCache> repositorioCache;
        private readonly Mock<IRepositorioComponenteCurricularJurema> repositorioComponenteCurricularJurema;
        private readonly Mock<IRepositorioComponenteCurricularConsulta> repositorioComponenteCurricularConsulta;
        private readonly Mock<IMediator> mediator;

        public ConsultasDisciplinasTeste()
        {
            consultasObjetivoAprendizagem = new Mock<IConsultasObjetivoAprendizagem>();
            repositorioAtribuicaoCJ = new Mock<IRepositorioAtribuicaoCJ>();
            repositorioCache = new Mock<IRepositorioCache>();
            repositorioComponenteCurricularJurema = new Mock<IRepositorioComponenteCurricularJurema>();
            repositorioComponenteCurricularConsulta = new Mock<IRepositorioComponenteCurricularConsulta>();
            mediator = new Mock<IMediator>();
        }

        [Fact(DisplayName = "Consultas Disciplina - Obter considerando componentes do infantil agrupados")]
        public async Task ObterComponentesCurricularesPorProfessorETurmaConsiderandoComponentesInfantilAgrupados()
        {
            var usuario = new Usuario() { Login = "1", PerfilAtual = Perfis.PERFIL_PROFESSOR_INFANTIL };
            usuario.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil() { CodigoPerfil = Perfis.PERFIL_PROFESSOR_INFANTIL } });

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("2020");

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { ModalidadeCodigo = Modalidade.EducacaoInfantil });

            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema() { Valor = string.Empty });

            mediator.Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ComponenteCurricularEol>());

            var consulta = new ConsultasDisciplina(repositorioCache.Object,
                                                   consultasObjetivoAprendizagem.Object,
                                                   repositorioComponenteCurricularJurema.Object,
                                                   repositorioAtribuicaoCJ.Object,
                                                   repositorioComponenteCurricularConsulta.Object,
                                                   mediator.Object);

            var resultado = await consulta.ObterComponentesCurricularesPorProfessorETurma("1", false);

            mediator.Verify(x =>
                x.Send(It.Is<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(x => x.RealizarAgrupamentoComponente), It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(resultado);
        }
    }
}