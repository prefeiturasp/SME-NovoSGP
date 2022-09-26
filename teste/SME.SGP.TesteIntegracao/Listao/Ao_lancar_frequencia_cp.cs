using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;
using System.Linq;

namespace SME.SGP.TesteIntegracao.Listao
{
    public class Ao_lancar_frequencia_cp : ListaoTesteBase
    {
        private const int AULA_NORMAL_ID = 1;
        private const int AULA_REPOSICAO_ID = 2;
        public Ao_lancar_frequencia_cp(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificaPodePersistirTurmaDisciplinaEOLQuery, bool>),
                typeof(VerificaPodePersistirTurmaDisciplinaEOLQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_lancar_frequencia_para_mais_de_uma_aula_no_mesmo_dia()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 3,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CriarAula = false
            };

            await CriarDadosBasicos(filtroListao);
            await CrieAulaNoMesmoDia();

            var frequenciasSalvar = new List<FrequenciaSalvarAulaAlunosDto>();
            frequenciasSalvar.Add(new FrequenciaSalvarAulaAlunosDto { AulaId = AULA_NORMAL_ID, Alunos = ObterListaFrequenciaSalvarAluno() });
            frequenciasSalvar.Add(new FrequenciaSalvarAulaAlunosDto { AulaId = AULA_REPOSICAO_ID, Alunos = ObterListaFrequenciaSalvarAluno() });

            var useCaseSalvar = ServiceProvider.GetService<IInserirFrequenciaListaoUseCase>();
            useCaseSalvar.ShouldNotBeNull();
            await useCaseSalvar.Executar(frequenciasSalvar);

            var useCaseObterFrequencia = ServiceProvider.GetService<IObterFrequenciasPorPeriodoUseCase>();
            useCaseObterFrequencia.ShouldNotBeNull();

            var filtroFrequenciaPeriodo = new FiltroFrequenciaPorPeriodoDto
            {
                TurmaId = TURMA_CODIGO_1,
                DisciplinaId = filtroListao.ComponenteCurricularId.ToString(),
                ComponenteCurricularId = filtroListao.ComponenteCurricularId.ToString(),
                DataInicio = DATA_25_07_INICIO_BIMESTRE_3,
                DataFim = DATA_30_09_FIM_BIMESTRE_3
            };
            var frequencias = await useCaseObterFrequencia.Executar(filtroFrequenciaPeriodo);
            frequencias.ShouldNotBeNull();
            frequencias.Aulas.ShouldNotBeNull();
            frequencias.Aulas.ToList().Exists(aula => aula.EhReposicao && aula.Data.Date == DATA_25_07_INICIO_BIMESTRE_3.Date).ShouldBeTrue();
            frequencias.Aulas.ToList().Exists(aula => !aula.EhReposicao && aula.Data.Date == DATA_25_07_INICIO_BIMESTRE_3.Date).ShouldBeTrue();
        }

        [Theory]
        [InlineData(PerfilUsuario.CP)]
        [InlineData(PerfilUsuario.DIRETOR)]
        public async Task Ao_lancar_frequencia_cp_diretor(PerfilUsuario perfil)
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 3,
                Modalidade = Modalidade.Fundamental,
                Perfil = perfil.Name(),
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };

            await CriarDadosBasicos(filtroListao);

            var listaAulaId = ObterTodos<Dominio.Aula>().Select(c => c.Id).Distinct().ToList();
            listaAulaId.ShouldNotBeNull();

            var frequenciasSalvar = listaAulaId.Select(aulaId => new FrequenciaSalvarAulaAlunosDto
            { AulaId = aulaId, Alunos = ObterListaFrequenciaSalvarAluno() }).ToList();

            //-> Salvar a frequencia
            var useCaseSalvar = ServiceProvider.GetService<IInserirFrequenciaListaoUseCase>();
            useCaseSalvar.ShouldNotBeNull();
            await useCaseSalvar.Executar(frequenciasSalvar);

            var useCasePeriodos = ServiceProvider.GetService<IObterPeriodosPorComponenteUseCase>();
            useCasePeriodos.ShouldNotBeNull();
            var listaPeriodo = (await useCasePeriodos.Executar(TURMA_CODIGO_1, filtroListao.ComponenteCurricularId, false,
                filtroListao.Bimestre)).ToList();

            var useCaseObterFrequencia = ServiceProvider.GetService<IObterFrequenciasPorPeriodoUseCase>();
            useCaseObterFrequencia.ShouldNotBeNull();

            foreach (var filtroFrequenciaPorPeriodoDto in listaPeriodo.Select(periodo =>
                         new FiltroFrequenciaPorPeriodoDto
                         {
                             TurmaId = TURMA_CODIGO_1,
                             DisciplinaId = filtroListao.ComponenteCurricularId.ToString(),
                             ComponenteCurricularId = filtroListao.ComponenteCurricularId.ToString(),
                             DataInicio = periodo.DataInicio,
                             DataFim = periodo.DataFim
                         }))
            {
                var frequenciasPorPeriodo = await useCaseObterFrequencia.Executar(filtroFrequenciaPorPeriodoDto);
                frequenciasPorPeriodo.ShouldNotBeNull();

                var listaAluno = frequenciasPorPeriodo.Alunos;
                listaAluno.ShouldNotBeNull();

                var listaAula = listaAluno.SelectMany(c => c.Aulas).ToList();
                listaAula.ShouldNotBeNull();

                var listaDetalheFrequencia = listaAula.SelectMany(c => c.DetalheFrequencia).ToList();
                listaDetalheFrequencia.ShouldNotBeNull();

                listaDetalheFrequencia.Any(c => TIPOS_FREQUENCIAS_SIGLA.Contains(c.TipoFrequencia)).ShouldBeTrue();
            }
        }

        private async Task CrieAulaNoMesmoDia()
        {
            await CriarAula(DATA_25_07_INICIO_BIMESTRE_3, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_LOGIN_2222222,
                    TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(DATA_25_07_INICIO_BIMESTRE_3, RecorrenciaAula.AulaUnica, TipoAula.Reposicao, USUARIO_PROFESSOR_LOGIN_2222222,
                    TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), TIPO_CALENDARIO_1);
        }
    }
}
