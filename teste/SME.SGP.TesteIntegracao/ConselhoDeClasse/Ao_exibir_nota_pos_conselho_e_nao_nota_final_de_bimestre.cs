using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake = SME.SGP.TesteIntegracao.ServicosFakes.ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake;
using ObterTurmaItinerarioEnsinoMedioQueryHandlerFake = SME.SGP.TesteIntegracao.ServicosFakes.ObterTurmaItinerarioEnsinoMedioQueryHandlerFake;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_exibir_nota_pos_conselho_e_nao_nota_final_de_bimestre : ConselhoDeClasseTesteBase
    {
        public Ao_exibir_nota_pos_conselho_e_nao_nota_final_de_bimestre(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularDto>>), typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterMatriculasAlunoNaTurmaQueryHandlerFakeAlunoCodigo1), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesRegenciaPorAnoEolQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesRegenciaPorAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTipoNotaPorTurmaQuery, TipoNota>), typeof(ObterTipoNotaPorTurmaQueryHandlerFakeNota), ServiceLifetime.Scoped));

            

        }

        [Fact]
        public async Task Deve_exibir_as_notas_de_fechamento_na_tela_de_conselho_de_classe()
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false,
                SituacaoConselhoClasse.EmAndamento,
                true);

            var consultasConselhoClasseAluno = ServiceProvider.GetService<IObterNotasFrequenciaUseCase>();

            var retorno = await consultasConselhoClasseAluno.Executar(new ConselhoClasseNotasFrequenciaDto(CONSELHO_CLASSE_ID_1, FECHAMENTO_TURMA_ID_3, ALUNO_CODIGO_1, TURMA_CODIGO_1, BIMESTRE_3, false));

            retorno.ShouldNotBeNull();
            var notas = retorno.NotasConceitos.FirstOrDefault().ComponentesCurriculares.FirstOrDefault();
            notas.NotaPosConselho.Nota.ShouldNotBeNull();
            notas.NotasFechamentos.FirstOrDefault().NotaConceito.ShouldNotBeNull();
        }

        [Fact]
        public async Task Deve_exibir_as_notas_de_conselho_na_tela_de_conselho_de_classe()
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Nota);

            await CriarDados(ObterPerfilProfessor(),
                salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular,
                ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false,
                SituacaoConselhoClasse.EmAndamento,
                true);

            await ExecutarTesteSemValidacao(salvarConselhoClasseAlunoNotaDto);

            var consultasConselhoClasseAluno = ServiceProvider.GetService<IObterNotasFrequenciaUseCase>();

            var ret = ObterTodos<ConselhoClasseNota>();

            var retorno = await consultasConselhoClasseAluno.Executar(new ConselhoClasseNotasFrequenciaDto(CONSELHO_CLASSE_ID_1, FECHAMENTO_TURMA_ID_3, ALUNO_CODIGO_1, TURMA_CODIGO_1, BIMESTRE_3, false));

            retorno.ShouldNotBeNull();
            var notas = retorno.NotasConceitos.FirstOrDefault().ComponentesCurriculares.FirstOrDefault();
            notas.NotaPosConselho.Nota.ShouldNotBeNull();
            notas.NotasFechamentos.FirstOrDefault().NotaConceito.ShouldNotBeNull();
        }

        private async Task CriarDados(
            string perfil,
            long componente,
            string anoTurma,
            Modalidade modalidade,
            ModalidadeTipoCalendario modalidadeTipoCalendario,
            bool anoAnterior,
            SituacaoConselhoClasse situacaoConselhoClasse = SituacaoConselhoClasse.NaoIniciado,
            bool criarFechamentoDisciplinaAlunoNota = false,
            TipoNota tipoNota = TipoNota.Nota)
        {
            var dataAula = anoAnterior ? DATA_25_07_INICIO_BIMESTRE_3.AddYears(-1) : DATA_25_07_INICIO_BIMESTRE_3;

            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_3,
                ComponenteCurricular = componente.ToString(),
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = anoAnterior,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = criarFechamentoDisciplinaAlunoNota,
                SituacaoConselhoClasse = situacaoConselhoClasse,
                TipoNota = tipoNota
            };

            await CriarDadosBase(filtroNota);
        }
    }
}