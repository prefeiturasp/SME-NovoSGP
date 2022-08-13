using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_validar_situacao_do_parecer_conclusivo : ConselhoDeClasseTesteBase
    {
        public Ao_validar_situacao_do_parecer_conclusivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorTurmasCodigoQuery, IEnumerable<DisciplinaDto>>), typeof(ObterComponentesCurricularesPorTurmasCodigoQueryFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>),typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }
        
        [Fact]
        public async Task Ao_reprocessar_parecer_conclusivo_aluno()
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota,
                ANO_7,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false, 
                SituacaoConselhoClasse.EmAndamento,
                true);
            
            await CriarConselhoClasseTodosBimestres(TipoNota.Nota,true);
            
            // await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, false, TipoNota.Nota);
            
            // var filtroConselhoClasse = new FiltroConselhoClasseDto()
            // {
            //     Perfil = ObterPerfilProfessor(),
            //     Modalidade = Modalidade.Fundamental,
            //     TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            //     Bimestre = BIMESTRE_1,
            //     SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
            //     InserirConselhoClassePadrao = true,
            //     InserirFechamentoAlunoPadrao = true,
            //     CriarPeriodoEscolar = true,
            //     TipoNota = TipoNota.Nota,
            //     AnoTurma = "6"
            // };
            //
            // await CriarDadosBase(filtroConselhoClasse);
            //
            // var conselhoClasseFechamentoAluno = new ConselhoClasseFechamentoAlunoDto()
            // {
            //     AlunoCodigo = ALUNO_CODIGO_1,
            //     ConselhoClasseId = 1,
            //     FechamentoTurmaId = 1
            // };
            //
            // var reprocessarParecerConclusivoAlunoUseCase = RetornarReprocessarParecerConclusivoAlunoUseCase();
            //
            // var retorno = await reprocessarParecerConclusivoAlunoUseCase.Executar(new MensagemRabbit(JsonConvert.SerializeObject(conselhoClasseFechamentoAluno)));
            //
            // retorno.ShouldBeTrue();
            //
            // var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            // parecerConclusivo.Any(f=> f.ConselhoClasseParecerId > 0).ShouldBeTrue();
        }    
        
        private async Task CriarDados(string perfil, long componente, TipoNota tipo, string anoTurma, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, bool anoAnterior, SituacaoConselhoClasse situacaoConselhoClasse = SituacaoConselhoClasse.NaoIniciado, bool criarFechamentoDisciplinaAlunoNota = false)
        {
            var dataAula = anoAnterior ? DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1) : DATA_02_05_INICIO_BIMESTRE_2;

            var filtroNota = new FiltroNotasDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = componente.ToString(),
                TipoNota = tipo,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = anoAnterior,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = criarFechamentoDisciplinaAlunoNota,
                SituacaoConselhoClasse = situacaoConselhoClasse
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, true, ATIVIDADE_AVALIATIVA_1);
        }
    }
}