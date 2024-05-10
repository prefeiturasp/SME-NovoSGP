using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.AtribuicaoCJs.ServicosFake;
using SME.SGP.TesteIntegracao.ConsultaDisciplina.ServicosFake;
using SME.SGP.TesteIntegracao.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConsultaDisciplina
{
    public class Ao_consultar_disciplina : TesteBase
    {
        private readonly Mock<IConsultasObjetivoAprendizagem> consultasObjetivoAprendizagem;
        private readonly Mock<IRepositorioAtribuicaoCJ> repositorioAtribuicaoCJ;
        private readonly Mock<IRepositorioCache> repositorioCache;
        private readonly Mock<IRepositorioComponenteCurricularJurema> repositorioComponenteCurricularJurema;
        private readonly Mock<IRepositorioComponenteCurricularConsulta> repositorioComponenteCurricularConsulta;
        private readonly Mock<IMediator> mediator;
        public Ao_consultar_disciplina(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            consultasObjetivoAprendizagem = new Mock<IConsultasObjetivoAprendizagem>();
            repositorioAtribuicaoCJ = new Mock<IRepositorioAtribuicaoCJ>();
            repositorioCache = new Mock<IRepositorioCache>();
            repositorioComponenteCurricularJurema = new Mock<IRepositorioComponenteCurricularJurema>();
            repositorioComponenteCurricularConsulta = new Mock<IRepositorioComponenteCurricularConsulta>();
            mediator = new Mock<IMediator>();
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFake), ServiceLifetime.Scoped));
            
        }

        [Fact]
        public async Task Nao_Considera_Como_TurmaInfantil_Para_Retornar_Componentes_Desagrupados()
        {
            var consulta = new ConsultasDisciplina(repositorioCache.Object,
                                                   consultasObjetivoAprendizagem.Object,
                                                   repositorioComponenteCurricularJurema.Object,
                                                   repositorioAtribuicaoCJ.Object,
                                                   mediator.Object);

            var usuario = new Usuario() { Login = "1", PerfilAtual = Perfis.PERFIL_PROFESSOR_INFANTIL };
            var disciplinaDto = RetornaDisciplinaDto();
            usuario.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil() { CodigoPerfil = Perfis.PERFIL_PROFESSOR_INFANTIL } });
            var componenteCurricular = await RetornaComponentesCurricularesEOL();

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            repositorioComponenteCurricularConsulta.Setup(x => x.ObterDisciplinasPorIds(new long[] { 1, 2 })).Returns(disciplinaDto);

            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("2020");

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SME.SGP.Dominio.Turma() { CodigoTurma = "1", ModalidadeCodigo = Modalidade.EducacaoInfantil });

            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema() { Valor = string.Empty });

            mediator.Setup(x => x.Send(It.IsAny<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componenteCurricular);

            await InserirNaBase("componente_curricular_area_conhecimento", "1", "'Área de conhecimento 1'");
            await InserirNaBase("componente_curricular_grupo_matriz", "1", "'Grupo matriz 1'");

            await InserirNaBase("componente_curricular", "2", "513", "1", "1", "'PORTUGUES'", "false", "false", "true", "false", "false", "true", "' '", "''");


            var resultado = await consulta.ObterComponentesCurricularesPorProfessorETurma("1", false, false, false);

            resultado.ShouldNotBeNull();
        }
        private async Task<IEnumerable<ComponenteCurricularEol>> RetornaComponentesCurricularesEOL()
        {
            var listaComponenteCurricularEOL = new List<ComponenteCurricularEol>();

            var componentesCurriculares1 = new ComponenteCurricularEol
            {
                CodigoComponenteCurricularPai = 1,
                Codigo = 1,
                TurmaCodigo = "1",
                Descricao = "MATEMATICA"
            };
            var componentesCurriculares2 = new ComponenteCurricularEol
            {
                CodigoComponenteCurricularPai = 1,
                Codigo = 2,
                TurmaCodigo = "1",
                Descricao = "PORTUGUES"
            };

            listaComponenteCurricularEOL.Add(componentesCurriculares1);
            listaComponenteCurricularEOL.Add(componentesCurriculares2);

            return await Task.FromResult(listaComponenteCurricularEOL);
        }
        private async Task<IEnumerable<DisciplinaDto>> RetornaDisciplinaDto()
        {
            var listaDisciplinaDto = new List<DisciplinaDto>();
            var disciplinaDto = new DisciplinaDto() { Id = 1, CodigoComponenteCurricular = 1, CdComponenteCurricularPai = 1, Nome = "Matematica", TurmaCodigo = "1" };
            var disciplinaDto2 = new DisciplinaDto() { Id = 1, CodigoComponenteCurricular = 2, CdComponenteCurricularPai = 1, Nome = "Portugues", TurmaCodigo = "1" };

            listaDisciplinaDto.Add(disciplinaDto);
            listaDisciplinaDto.Add(disciplinaDto2);

            return await Task.FromResult(listaDisciplinaDto);
        }
    }
}
