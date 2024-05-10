using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ConsolidacaoConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;
using ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake = SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes.ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake;
using ObterTurmaItinerarioEnsinoMedioQueryHandlerFake = SME.SGP.TesteIntegracao.ServicosFakes.ObterTurmaItinerarioEnsinoMedioQueryHandlerFake;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_inserir_alterar_nota_conceito_pos_conselho_bimestre_final: ConselhoDeClasseTesteBase
    {
        public Ao_inserir_alterar_nota_conceito_pos_conselho_bimestre_final(CollectionFixture collectionFixture) : base(collectionFixture)
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
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterInfoComponentesCurricularesESPorTurmasCodigoQuery, IEnumerable<InfoComponenteCurricular>>), typeof(ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }
        
        [Fact]
        public async Task Deve_inserir_nota_conceito_pos_conselho_bimestre_2()
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Conceito);
            
            await CriarDados(ObterPerfilProfessor(), 
                salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular, 
                TipoNota.Conceito, 
                ANO_3, 
                Modalidade.Fundamental, 
                ModalidadeTipoCalendario.FundamentalMedio, 
                false, 
                SituacaoConselhoClasse.EmAndamento, 
                true);
            
            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, false,TipoNota.Conceito);
            
        }

        [Fact]
        public async Task Deve_inserir_nota_conceito_pos_conselho_bimestre_final()
        {
            await CriarDados(ObterPerfilProfessor(),
                            COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                            TipoNota.Conceito,
                            ANO_1,
                            Modalidade.Fundamental,
                            ModalidadeTipoCalendario.FundamentalMedio,
                            false, 
                            SituacaoConselhoClasse.EmAndamento,
                            true);
            
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_PORTUGUES_ID_138,TipoNota.Conceito);
            var conselhoClasseNotas = ObterTodos<ConselhoClasseNota>();
            
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Conceito, FECHAMENTO_TURMA_ID_5, BIMESTRE_FINAL);
            
            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, false, TipoNota.Conceito);
        }
        
        [Fact]
        public async Task Deve_alterar_nota_conceito_pos_conselho_bimestre_2()
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Conceito);
            
            await CriarDados(ObterPerfilProfessor(), 
                salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.CodigoComponenteCurricular, 
                TipoNota.Conceito, 
                ANO_3, 
                Modalidade.Fundamental, 
                ModalidadeTipoCalendario.FundamentalMedio, 
                false, 
                SituacaoConselhoClasse.EmAndamento, 
                true);
            
            await ExecutarTesteSemValidacao(salvarConselhoClasseAlunoNotaDto);

            salvarConselhoClasseAlunoNotaDto.ConselhoClasseId = 1;
            salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.Conceito = new Random().Next(1, 3);
            
            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, false,TipoNota.Conceito);
        }
        
        [Fact]
        public async Task Deve_alterar_nota_conceito_pos_conselho_bimestre_final()
        {
            var salvarConselhoClasseAlunoNotaDto = ObterSalvarConselhoClasseAlunoNotaDto(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, TipoNota.Conceito, FECHAMENTO_TURMA_ID_5, BIMESTRE_FINAL);
           
            await CriarDados(ObterPerfilProfessor(),
            COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
            TipoNota.Conceito,
            ANO_1,
            Modalidade.Fundamental,
            ModalidadeTipoCalendario.FundamentalMedio,
            false, 
            SituacaoConselhoClasse.EmAndamento,
            true);
            
            await CriarConselhoClasseTodosBimestres(COMPONENTE_CURRICULAR_PORTUGUES_ID_138,TipoNota.Conceito);
            
            await ExecutarTesteSemValidacao(salvarConselhoClasseAlunoNotaDto);
            
            salvarConselhoClasseAlunoNotaDto.ConselhoClasseId = 5;
            salvarConselhoClasseAlunoNotaDto.ConselhoClasseNotaDto.Conceito = new Random().Next(1, 3);
            
            await ExecutarTeste(salvarConselhoClasseAlunoNotaDto, false,TipoNota.Conceito);
        }
        

        private async Task CriarDados(string perfil, long componente, TipoNota tipo, string anoTurma, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, bool anoAnterior, SituacaoConselhoClasse situacaoConselhoClasse = SituacaoConselhoClasse.NaoIniciado, bool criarFechamentoDisciplinaAlunoNota = false)
        {
            var dataAula = anoAnterior ? DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1) : DATA_02_05_INICIO_BIMESTRE_2;

            var filtroNota = new FiltroConselhoClasseDto()
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