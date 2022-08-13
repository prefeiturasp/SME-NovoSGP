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
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

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
                typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(
                typeof(IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterMatriculasAlunoNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarSituacaoConselho), ServiceLifetime.Scoped));            
        }

        [Fact]
        public async Task Deve_apresentar_situacao_nao_iniciado()
        {
            await CriarDados(COMPONENTE_LINGUA_PORTUGUESA_ID_138, ANO_8, Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio, false, false);

            var consulta = ServiceProvider.GetService<IConsultasConselhoClasseRecomendacao>();
            consulta.ShouldNotBeNull();

            var conselhosClasses = ObterTodos<ConselhoClasse>();
            var conselhoClasseId = conselhosClasses.Select(c => c.Id).FirstOrDefault();

            var fechamentosTurmas = ObterTodos<FechamentoTurma>();
            var fechamentoTurmaId = fechamentosTurmas.Select(c => c.Id).FirstOrDefault();
            
            var retorno = await consulta.ObterRecomendacoesAlunoFamilia(conselhoClasseId, fechamentoTurmaId, ALUNO_CODIGO_1,
                TURMA_CODIGO_1, BIMESTRE_1);
            
            (retorno.SituacaoConselho == SituacaoConselhoClasse.NaoIniciado.GetAttribute<DisplayAttribute>().Name).ShouldBeTrue();
        }
        
        [Fact]
        public async Task Deve_apresentar_situacao_em_andamento()
        {
            await CriarDados(COMPONENTE_LINGUA_PORTUGUESA_ID_138, ANO_8, Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio, true, false);

            await CriarConselhoClasseTodosBimestres();

            var consulta = ServiceProvider.GetService<IConsultasConselhoClasseRecomendacao>();
            consulta.ShouldNotBeNull();

            var conselhosClasses = ObterTodos<ConselhoClasse>();
            var conselhoClasseId = conselhosClasses.Select(c => c.Id).FirstOrDefault();

            var fechamentosTurmas = ObterTodos<FechamentoTurma>();
            var fechamentoTurmaId = fechamentosTurmas.Select(c => c.Id).FirstOrDefault();
            
            var retorno = await consulta.ObterRecomendacoesAlunoFamilia(conselhoClasseId, fechamentoTurmaId, ALUNO_CODIGO_1,
                TURMA_CODIGO_1, BIMESTRE_1);
            
            (retorno.SituacaoConselho == SituacaoConselhoClasse.EmAndamento.GetAttribute<DisplayAttribute>().Name).ShouldBeTrue();
        }        
        
        [Fact]
        public async Task Deve_apresentar_situacao_concluido()
        {
            await CriarDados(COMPONENTE_LINGUA_PORTUGUESA_ID_138, ANO_8, Modalidade.Medio,
                ModalidadeTipoCalendario.FundamentalMedio, true, false);
            
            var useCase = ServiceProvider.GetService<ISalvarConselhoClasseAlunoNotaUseCase>();
            useCase.ShouldNotBeNull();

            string[] alunosCodigos = { ALUNO_CODIGO_1, ALUNO_CODIGO_2, ALUNO_CODIGO_3, ALUNO_CODIGO_4 };


            foreach (var alunoCodigo in alunosCodigos)
            {
                await ExecutarTesteSemValidacao(ObterSalvarConselhoClasseAlunoNotaDto(alunoCodigo, COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    TipoNota.Nota, FECHAMENTO_TURMA_ID_1, BIMESTRE_1));
            }
            
            var consulta = ServiceProvider.GetService<IConsultasConselhoClasseRecomendacao>();
            consulta.ShouldNotBeNull();

            var conselhosClasses = ObterTodos<ConselhoClasse>();
            var conselhoClasseId = conselhosClasses.Select(c => c.Id).FirstOrDefault();

            var fechamentosTurmas = ObterTodos<FechamentoTurma>();
            var fechamentoTurmaId = fechamentosTurmas.Select(c => c.Id).FirstOrDefault();
            
            var retorno = await consulta.ObterRecomendacoesAlunoFamilia(conselhoClasseId, fechamentoTurmaId, ALUNO_CODIGO_1,
                TURMA_CODIGO_1, BIMESTRE_1);
            
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
            var filtroNota = new FiltroNotasDto
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