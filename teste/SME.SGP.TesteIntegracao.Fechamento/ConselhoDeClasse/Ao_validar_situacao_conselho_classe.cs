using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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
using Xunit;
using ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake = SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes.ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_validar_situacao_conselho_classe : ConselhoDeClasseTesteBase
    {
        public Ao_validar_situacao_conselho_classe(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularDto>>),
                typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFakeValidarSituacaoConselho), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(
                typeof(IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterMatriculasAlunoNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarSituacaoConselho), ServiceLifetime.Scoped));     
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_apresentar_conselho_classe_situacao_nao_iniciado()
        {
            await CriarDados(COMPONENTE_LINGUA_PORTUGUESA_ID_138, ANO_8, Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio, true, false);
            
            var consulta = ServiceProvider.GetService<IConsultaConselhoClasseRecomendacaoUseCase>();
            consulta.ShouldNotBeNull();
            
            var conselhosClasses = ObterTodos<ConselhoClasse>();
            var situacaoConselhoClasse = conselhosClasses.Select(c => c.Situacao).FirstOrDefault();
            
            (situacaoConselhoClasse == SituacaoConselhoClasse.NaoIniciado).ShouldBeTrue();
        }
        
        [Fact]
        public async Task Deve_apresentar_conselho_classe_situacao_em_andamento()
        {
            await CriarDados(COMPONENTE_LINGUA_PORTUGUESA_ID_138, ANO_8, Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio, true, false);
            
            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoNotaUseCase>();
            useCase.ShouldNotBeNull();

            //-> Inserir conselho do primeiro aluno
            await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(0, ALUNO_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota, FECHAMENTO_TURMA_ID_1, BIMESTRE_1));

            var consulta = ServiceProvider.GetService<IConsultaConselhoClasseRecomendacaoUseCase>();
            consulta.ShouldNotBeNull();
            
            var conselhosClasses = ObterTodos<ConselhoClasse>();
            var situacaoConselhoClasse = conselhosClasses.Select(c => c.Situacao).FirstOrDefault();
            
            (situacaoConselhoClasse == SituacaoConselhoClasse.EmAndamento).ShouldBeTrue();
        }        
        
        [Fact]
        public async Task Deve_apresentar_conselho_classe_situacao_concluido()
        {
            await CriarDados(COMPONENTE_LINGUA_PORTUGUESA_ID_138, ANO_8, Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio, true, false);
            
            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoNotaUseCase>();
            useCase.ShouldNotBeNull();

            //-> Inserir conselho do primeiro aluno
            await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(0, ALUNO_CODIGO_1, 
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota, FECHAMENTO_TURMA_ID_1, BIMESTRE_1));

            var conselhoClasseId = ObterTodos<ConselhoClasse>().Select(c => c.Id).FirstOrDefault();
            
            //-> Inserir conselho para os demais alunos
            string[] alunosCodigos = { ALUNO_CODIGO_2, ALUNO_CODIGO_3, ALUNO_CODIGO_4 };            

            foreach (var alunoCodigo in alunosCodigos)
            {
                await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(conselhoClasseId, alunoCodigo, COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    TipoNota.Nota, FECHAMENTO_TURMA_ID_1, BIMESTRE_1));
            }
            
            var consulta = ServiceProvider.GetService<IConsultaConselhoClasseRecomendacaoUseCase>();
            consulta.ShouldNotBeNull();

            var conselhosClasses = ObterTodos<ConselhoClasse>();
            var situacaoConselhoClasse = conselhosClasses.Select(c => c.Situacao).FirstOrDefault();
            
            (situacaoConselhoClasse == SituacaoConselhoClasse.Concluido).ShouldBeTrue();
        }        
        
        [Fact]
        public async Task Deve_apresentar_conselho_classe_aluno_situacao_concluido()
        {
            await CriarDados(COMPONENTE_LINGUA_PORTUGUESA_ID_138, ANO_8, Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio, true, false);
            
            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoNotaUseCase>();
            useCase.ShouldNotBeNull();

            //-> Inserir aluno aluno
            await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(0, ALUNO_CODIGO_1,
                COMPONENTE_CURRICULAR_ARTES_ID_139,
                TipoNota.Nota, FECHAMENTO_TURMA_ID_1, BIMESTRE_1));
            
            var conselhoClasseId = ObterTodos<ConselhoClasse>().Select(c => c.Id).FirstOrDefault();
            
            await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(conselhoClasseId, ALUNO_CODIGO_1,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TipoNota.Nota, FECHAMENTO_TURMA_ID_1, BIMESTRE_1));

            var consulta = ServiceProvider.GetService<IConsultaConselhoClasseRecomendacaoUseCase>();
            consulta.ShouldNotBeNull();

            var fechamentosTurmas = ObterTodos<FechamentoTurma>();
            var fechamentoTurmaId = fechamentosTurmas.Select(c => c.Id).FirstOrDefault();
            
            var retorno = await consulta.Executar(new ConselhoClasseRecomendacaoDto()
            {
                ConselhoClasseId = conselhoClasseId,
                FechamentoTurmaId = fechamentoTurmaId,
                AlunoCodigo = ALUNO_CODIGO_1,
                CodigoTurma = TURMA_CODIGO_1,
                Bimestre = BIMESTRE_1}
            );
            
            (retorno.SituacaoConselho == SituacaoConselhoClasse.Concluido.GetAttribute<DisplayAttribute>().Name).ShouldBeTrue();
        }        
        
        private async Task CriarDados(
            string componenteCurricular,
            string anoTurma, 
            Modalidade modalidade,
            ModalidadeTipoCalendario modalidadeTipoCalendario,
            bool criarFechamentoDisciplinaAlunoNota,
            bool criarConselhoClasseFinal
        )
        {            
            var filtroNota = new FiltroConselhoClasseDto
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                ComponenteCurricular = componenteCurricular,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = false,
                DataAula = DATA_03_01_INICIO_BIMESTRE_1,
                TipoNota = TipoNota.Nota,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento,
                CriarFechamentoDisciplinaAlunoNota = criarFechamentoDisciplinaAlunoNota,
                CriarConselhoClasseFinal = criarConselhoClasseFinal,
                Bimestre = BIMESTRE_1
            };
            
            await CriarDadosBase(filtroNota);
        }        
    }
}