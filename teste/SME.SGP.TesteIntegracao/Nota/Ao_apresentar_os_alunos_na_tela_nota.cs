using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Nota
{
    public class Ao_apresentar_os_alunos_na_tela_nota : NotaTesteBase
    {
        private const string ALUNO_CODIGO_12 = "12";

        public Ao_apresentar_os_alunos_na_tela_nota(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(SME.SGP.TesteIntegracao.Nota.ServicosFakes.ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        //[Fact]
        public async Task Alunos_novos_devem_aparecer_com_tooltip_durante_15_dias()
        {
            var listaDeAlunos = await ObterListaDeAlunos();

            listaDeAlunos.ShouldNotBeNull();
            var listaDeMarcador = listaDeAlunos.FindAll(alunos => alunos.Marcador.NaoEhNulo());
            listaDeMarcador.ShouldNotBeNull();
            listaDeMarcador.Exists(marcador => marcador.Marcador.Tipo == TipoMarcadorFrequencia.Novo).ShouldBeTrue();
        }

        //[Fact]
        public async Task Alunos_inativos_devem_aparecer_com_tooltip_ate_data_de_inativacao()
        {
            var listaDeAlunos = await ObterListaDeAlunos();

            listaDeAlunos.ShouldNotBeNull();
            var listaDeMarcador = listaDeAlunos.FindAll(alunos => alunos.Marcador.NaoEhNulo());
            listaDeMarcador.ShouldNotBeNull();
            listaDeMarcador.Exists(marcador => marcador.Marcador.Tipo == TipoMarcadorFrequencia.Inativo).ShouldBeTrue();
        }

        //[Fact]
        public async Task Alunos_inativos_antes_do_inicio_do_ano_letivo_nao_devem_aparecer_na_tela()
        {
            var listaDeAlunos = await ObterListaDeAlunos();

            listaDeAlunos.ShouldNotBeNull();
            listaDeAlunos.Exists(alunos => alunos.Id == ALUNO_CODIGO_12).ShouldBeFalse();
        }

        private async Task<List<NotasConceitosAlunoRetornoDto>> ObterListaDeAlunos()
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_5
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_03_01_INICIO_BIMESTRE_1, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_03_01_INICIO_BIMESTRE_1, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_2222222, false, ATIVIDADE_AVALIATIVA_1);

            var useCase = ServiceProvider.GetService<IObterNotasParaAvaliacoesUseCase>();
            var retorno = await useCase.Executar(ObterNotaConceitoListar());
            retorno.ShouldNotBeNull();

            return retorno.Bimestres.FirstOrDefault()?.Alunos;
        }

        private ListaNotasConceitosDto ObterNotaConceitoListar()
        {
            return new ListaNotasConceitosDto()
            {
                TurmaId = long.Parse(TURMA_CODIGO_1),
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Bimestre = BIMESTRE_1,
                DisciplinaCodigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Modalidade = Modalidade.Fundamental,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                Semestre = SEMESTRE_1,
                TurmaCodigo = TURMA_CODIGO_1,
                TurmaHistorico = false,
                PeriodoInicioTicks = DATA_03_01_INICIO_BIMESTRE_1.Ticks,
                PeriodoFimTicks = DATA_01_05_FIM_BIMESTRE_1.Ticks,
            };
        }
    }
}
