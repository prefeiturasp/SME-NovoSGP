using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RegistroIndividual;
using SME.SGP.TesteIntegracao.RegistroIndividual.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using Xunit;
using Shouldly;
using SME.SGP.TesteIntegracao.PlanoAnualTerritorio.ServicoFake;
using SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;

namespace SME.SGP.TesteIntegracao.PlanoAnualTerritorio
{
    public class Ao_obter_plano_anual_do_territorio : TesteBaseComuns
    {
        public Ao_obter_plano_anual_do_territorio(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>),
                typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponenteCurricularPorIdQuery, DisciplinaDto>),
               typeof(ObterComponenteCurricularPorIdQueryHandlerFakePlanoAnual), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<TurmaEmPeriodoAbertoQuery, bool>),
               typeof(TurmaEmPeriodoAbertoQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioLogadoQuery, Usuario>),
             typeof(ObterUsuarioLogadoQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>),
             typeof(ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerPlanoAnualTerritorio), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessorTitularPorTurmaEComponenteCurricularQuery, ProfessorTitularDisciplinaEol>),
             typeof(ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandlerFakePlanoAnual), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Plano anual do território - obter planos conforme o perfil/componente")]
        public async Task Deve_obter_os_planos_correspondentes_ao_perfil_componente_do_professor()
        {
            await CriarDados();

            var consultaPlano = ServiceProvider.GetService<IConsultasPlanoAnualTerritorioSaber>();

            var planosAnuais = await consultaPlano.ObterPorUETurmaAnoETerritorioExperiencia("22", "1", 2026, 1111);

            planosAnuais.ShouldNotBeNull();
            planosAnuais.Count().ShouldBe(2);
            planosAnuais.Where(p => p.Id == 1).FirstOrDefault().Desenvolvimento.ShouldBe("<p>wwwww</p>");
        }

        private async Task CriarDados()
        {
            await InserirNaBase(new Dre()
            {
                Id = 1,
                Nome = "Dre Teste",
                CodigoDre = "11",
                Abreviacao = "DT"
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                Nome = "Ue Teste",
                DreId = 1,
                TipoEscola = TipoEscola.EMEF,
                CodigoUe = "22"
            });
            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = TURMA_ANO_2,
                CodigoTurma = TURMA_CODIGO_1,
                Historica = false,
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1,
                TipoTurma = TipoTurma.Regular,
                Id = 1
            });

            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Nome = "Calendário Teste Ano Atual",
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Periodo = Periodo.Anual,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 1,
                Bimestre = 1,
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 1, 1),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 6, 15),
                TipoCalendarioId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Id = 2,
                Bimestre = 2,
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 6, 16),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 12, 31),
                TipoCalendarioId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new PlanoAnualTerritorioSaber()
            {
                Id = 2,
                Bimestre = 1,
                Ano = 2026,
                TerritorioExperienciaId = 1111,
                EscolaId = "22",
                TurmaId = 1,
                Desenvolvimento = "<p>wwwww</p>",
                Reflexao = "<p>wwwww</p>",
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });
        }
    }
}
