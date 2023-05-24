using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
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
    public class Ao_alterar_nota_conceito_pos_conselho_bimestre_final : ConselhoDeClasseTesteBase
    {
        public Ao_alterar_nota_conceito_pos_conselho_bimestre_final(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularDto>>), typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesRegenciaPorAnoETurnoQuery, IEnumerable<DisciplinaDto>>), typeof(ObterComponentesCurricularesRegenciaPorAnoETurnoQueryFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesRegenciaPorAnoEolQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesRegenciaPorAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Theory]
        [InlineData(false, BIMESTRE_2)]
        [InlineData(false, BIMESTRE_FINAL)]
        //[InlineData(true, BIMESTRE_2)]
        //[InlineData(true, BIMESTRE_FINAL)]
        public async Task Ao_alterar_nota_conceito_pos_conselho_bimestre_e_final_fundamental(bool anoAnterior, int bimestre)
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Conceito,
                ANO_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior,
                bimestre,
                SituacaoConselhoClasse.EmAndamento,
                true);
           
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_PORTUGUES_ID_138,TipoNota.Conceito);
            
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Conceito, FECHAMENTO_TURMA_ID_5, BIMESTRE_FINAL);
            
            await ExecutarTesteSemValidacao(salvarConselhoClasseAlunoNotaDto);
            
            salvarConselhoClasseAlunoNotaDto.ConselhoClasseId = 5;
            salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.Conceito = new Random().Next(1, 3);
            
            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, anoAnterior,TipoNota.Conceito);
        }
        
        [Theory]
        [InlineData(false, BIMESTRE_2)]
        [InlineData(false, BIMESTRE_FINAL)]
        //[InlineData(true, BIMESTRE_2)]
        //[InlineData(true, BIMESTRE_FINAL)]
        public async Task Ao_alterar_nota_conceito_pos_conselho_bimestre_e_final_eja(bool anoAnterior, int bimestre)
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Conceito,
                ANO_1,
                Modalidade.EJA,
                ModalidadeTipoCalendario.EJA,
                anoAnterior,
                bimestre,
                SituacaoConselhoClasse.EmAndamento,
                true);
        
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Conceito);
            
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Conceito, FECHAMENTO_TURMA_ID_5, BIMESTRE_FINAL);
            
            await ExecutarTesteSemValidacao(salvarConselhoClasseAlunoNotaDto);
            
            salvarConselhoClasseAlunoNotaDto.ConselhoClasseId = 5;
            salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.Conceito = new Random().Next(1, 3);
            
            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, anoAnterior,TipoNota.Conceito);
        }
        
        [Theory]
        [InlineData(false, BIMESTRE_2)]
        [InlineData(false, BIMESTRE_FINAL)]
        //[InlineData(true, BIMESTRE_2)]
        //[InlineData(true, BIMESTRE_FINAL)]
        public async Task Ao_alterar_nota_conceito_pos_conselho_bimestre_e_final_regencia_classe(bool anoAnterior, int bimestre)
        {
            await CriarDados(ObterPerfilProfessor(),
                COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105,
                TipoNota.Conceito,
                ANO_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior,
                bimestre,
                SituacaoConselhoClasse.EmAndamento,
                true);
        
            await CriarConselhoClasseTodosBimestres(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105,TipoNota.Conceito);
            
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, TipoNota.Conceito, FECHAMENTO_TURMA_ID_5, BIMESTRE_FINAL);
            
            await ExecutarTesteSemValidacao(salvarConselhoClasseAlunoNotaDto);
            
            salvarConselhoClasseAlunoNotaDto.ConselhoClasseId = 5;
            salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.Conceito = new Random().Next(1, 3);
            
            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, anoAnterior,TipoNota.Conceito, componentesRegencia: 2);
        }

        private async Task CriarDados(string perfil, long componente, TipoNota tipo, string anoTurma, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, bool anoAnterior, int bimestre, SituacaoConselhoClasse situacaoConselhoClasse = SituacaoConselhoClasse.NaoIniciado, bool criarFechamentoDisciplinaAlunoNota = false, bool criarPeriodoReabertura = true)
        {
            var dataAula = anoAnterior ? DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1) : DATA_02_05_INICIO_BIMESTRE_2;

            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = bimestre,
                ComponenteCurricular = componente.ToString(),
                TipoNota = tipo,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = anoAnterior,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = criarFechamentoDisciplinaAlunoNota,
                SituacaoConselhoClasse = situacaoConselhoClasse,
                CriarPeriodoReabertura = criarPeriodoReabertura
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, true, ATIVIDADE_AVALIATIVA_1);
        }
    }
}
