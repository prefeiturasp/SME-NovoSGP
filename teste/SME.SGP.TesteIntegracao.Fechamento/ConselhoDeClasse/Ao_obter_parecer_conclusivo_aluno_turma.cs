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
using ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake = SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes.ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake;
using ObterTurmaItinerarioEnsinoMedioQueryHandlerFake = SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes.ObterTurmaItinerarioEnsinoMedioQueryHandlerFake;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_obter_parecer_conclusivo_aluno_turma : ConselhoDeClasseTesteBase
    {
        public Ao_obter_parecer_conclusivo_aluno_turma(CollectionFixture collectionFixture) : base(collectionFixture)
        { }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake138), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorTurmasCodigoQuery, IEnumerable<DisciplinaDto>>), typeof(ObterComponentesCurricularesPorTurmasCodigoLPQueryFake), ServiceLifetime.Scoped));
            
        }

        [Fact(DisplayName = "Conselho Classe - Deve retornar as informações do parecer conclusivo do aluno")]
        public async Task Ao_obter_parecer_conclusivo_deve_retornar_as_informacoes_do_parecer_do_aluno()
        {
            var dataAula = DateTimeExtension.HorarioBrasilia().AddDays(-1);

            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = null,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_4,
                DataAula = dataAula,
                CriarFechamentoDisciplinaAlunoNota = false,
                SituacaoConselhoClasse = SituacaoConselhoClasse.EmAndamento
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, dataAula, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(dataAula, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, false, ATIVIDADE_AVALIATIVA_1);
            
            await InserirNaBase(new ConselhoClasse()
            {
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Situacao = SituacaoConselhoClasse.EmAndamento
            });
            
            await InserirNaBase(new ConselhoClasseAluno()
            {
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                ParecerAlteradoManual = true,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
            
            await InserirNaBase(new ConselhoClasseNota()
            {
                ConselhoClasseAlunoId = CONSELHO_CLASSE_ALUNO_ID_1,
                ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Nota = NOTA_8,
                Justificativa = "Justificativa",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            var obterPareceresConclusivosUseCase = ServiceProvider.GetService<IObterParecerConclusivoAlunoTurmaUseCase>();
            
            var retorno = await obterPareceresConclusivosUseCase.Executar(TURMA_CODIGO_1,ALUNO_CODIGO_1);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBeGreaterThan(0);
            retorno.Nome.ShouldNotBeEmpty();

            var parecerConclusivo = ObterTodos<ConselhoClasseAluno>();
            parecerConclusivo.First().ParecerAlteradoManual.ShouldBeFalse();
        }
    }
}