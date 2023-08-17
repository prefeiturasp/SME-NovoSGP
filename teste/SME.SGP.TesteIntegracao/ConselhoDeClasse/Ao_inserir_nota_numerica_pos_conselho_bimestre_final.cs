using System.Collections.Generic;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using Xunit;
using SME.SGP.TesteIntegracao.ServicosFakes;
using ObterTurmaItinerarioEnsinoMedioQueryHandlerFake = SME.SGP.TesteIntegracao.ServicosFakes.ObterTurmaItinerarioEnsinoMedioQueryHandlerFake;
using ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake = SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes.ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_inserir_nota_numerica_pos_conselho_bimestre_final : ConselhoDeClasseTesteBase
    {
        public Ao_inserir_nota_numerica_pos_conselho_bimestre_final(CollectionFixture collectionFixture) : base(collectionFixture)
        {
             
        }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTipoNotaPorTurmaQuery, TipoNota>), typeof(ObterTipoNotaPorTurmaQueryHandlerFakeNota), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterNotaTipoPorAnoModalidadeDataReferenciaQuery, NotaTipoValor>), typeof(ObterNotaTipoPorAnoModalidadeDataReferenciaQueryHandlerFakeNota), ServiceLifetime.Scoped));
        }

        [Theory]
        [InlineData(false)]
        //[InlineData(true)]
        public async Task Ao_lancar_nota_numerica_pos_conselho_bimestre_final_fundamental(bool anoAnterior)
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior, 
                SituacaoConselhoClasse.EmAndamento,
                true);
            
            await CriarConselhoClasseTodosBimestres();
            
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Nota, FECHAMENTO_TURMA_ID_5, BIMESTRE_FINAL);
            
            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, anoAnterior, TipoNota.Nota);
        }

        [Theory]
        [InlineData(false)]
        //[InlineData(true)]
        public async Task Ao_lancar_nota_numerica_pos_conselho_bimestre_final_fundamental_cp(bool anoAnterior)
        {
            await CriarDados(ObterPerfilCP(),
                            COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                            ANO_5,
                            Modalidade.Fundamental,
                            ModalidadeTipoCalendario.FundamentalMedio,
                            anoAnterior, 
                            SituacaoConselhoClasse.EmAndamento,
                            true);
                
            await CriarConselhoClasseTodosBimestres();

            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Nota, FECHAMENTO_TURMA_ID_5, BIMESTRE_FINAL);
            
            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, anoAnterior, TipoNota.Nota,SituacaoConselhoClasse.EmAndamento,true);
        }

        [Theory]
        [InlineData(false)]
        //[InlineData(true)]
        public async Task Ao_lancar_nota_pos_conselho_bimestre_numerica_medio(bool anoAnterior)
        {
            await CriarDados(ObterPerfilProfessor(),
                            COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                            ANO_7,
                            Modalidade.Fundamental,
                            ModalidadeTipoCalendario.FundamentalMedio,
                            anoAnterior, 
                            SituacaoConselhoClasse.EmAndamento,
                            true);
            
            await CriarConselhoClasseTodosBimestres();
            
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Nota, FECHAMENTO_TURMA_ID_5, BIMESTRE_FINAL);
            
            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, anoAnterior, TipoNota.Nota);
        }

        [Theory]
        [InlineData(false)]
        //[InlineData(true)]
        public async Task Ao_lancar_nota_numerica_pos_conselho_bimestre_final_medio_diretor(bool anoAnterior)
        {
            await CriarDados(ObterPerfilDiretor(),
                            COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                            ANO_5,
                            Modalidade.Medio,
                            ModalidadeTipoCalendario.FundamentalMedio,
                            anoAnterior, 
                            SituacaoConselhoClasse.EmAndamento,
                            true);
            
            await CriarConselhoClasseTodosBimestres();
            
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Nota, FECHAMENTO_TURMA_ID_5, BIMESTRE_FINAL);
            
            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, anoAnterior, TipoNota.Nota,SituacaoConselhoClasse.EmAndamento,true);
        }

        [Theory]
        [InlineData(false)]
        //[InlineData(true)]
        public async Task Ao_lancar_nota_numerica_pos_conselho_bimestre_final_eja(bool anoAnterior)
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                ANO_9,
                Modalidade.EJA,
                ModalidadeTipoCalendario.EJA,
                anoAnterior, 
                SituacaoConselhoClasse.EmAndamento,
                true);
            
            await CriarConselhoClasseTodosBimestres(ehEja: true);
            
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Nota, FECHAMENTO_TURMA_ID_5, BIMESTRE_FINAL);
            
            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, anoAnterior, TipoNota.Nota, ehEja: true);
                
        }

        [Theory]
        [InlineData(false)]
        //[InlineData(true)]
        public async Task Ao_lancar_nota_pos_conselho_bimestre_numerica_regencia_classe(bool anoAnterior)
        {
            await CriarDados(ObterPerfilProfessor(),
            COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
            ANO_6,
            Modalidade.Fundamental,
            ModalidadeTipoCalendario.FundamentalMedio,
            anoAnterior, 
            SituacaoConselhoClasse.EmAndamento,
            true);
            
            await CriarConselhoClasseTodosBimestres();

            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Nota, FECHAMENTO_TURMA_ID_5, BIMESTRE_FINAL);
            
            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, anoAnterior, TipoNota.Nota);
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
                        bool criarConselhosTodosBimestres = false)
        {
            var dataAula = anoAnterior ? DATA_03_10_INICIO_BIMESTRE_4.AddYears(-1) : DATA_03_10_INICIO_BIMESTRE_4;

            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_FINAL,
                ComponenteCurricular = componente.ToString(),
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = anoAnterior,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = criarFechamentoDisciplinaAlunoNota,
                SituacaoConselhoClasse = situacaoConselhoClasse
            };

            await CriarDadosBase(filtroNota);
        }
    }
}
