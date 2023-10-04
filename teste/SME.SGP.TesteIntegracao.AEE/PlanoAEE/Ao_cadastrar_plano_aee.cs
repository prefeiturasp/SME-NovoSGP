using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_cadastrar_plano_aee : PlanoAEETesteBase
    {
        public Ao_cadastrar_plano_aee(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaRegularESrmPorAlunoQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterTurmaRegularESrmPorAlunoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Plano AEE - Deve Criar Um plano AEE Novo")]
        public async Task Ao_cadastrar_plano()
        {
            var salvarPlanoAEEUseCase = ObterServicoSalvarPlanoAEEUseCase();

            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });

            var planoAEEPersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Questoes = ObterQuestoes(),
                TurmaCodigo = TURMA_CODIGO_1,
                ResponsavelRF = USUARIO_PROFESSOR_LOGIN_1111111,
            };
            
            var retorno = await salvarPlanoAEEUseCase.Executar(planoAEEPersistenciaDto);
            retorno.ShouldNotBeNull();
            retorno.PlanoId.ShouldBe(1);
            retorno.PlanoVersaoId.ShouldBe(1);

            var planoAeeVersaos = ObterTodos<PlanoAEEVersao>();
            planoAeeVersaos.ShouldNotBeNull();
            planoAeeVersaos.Count.ShouldBe(1);

            var questoesPlanoAeeQuestaos = ObterTodos<PlanoAEEQuestao>();
            questoesPlanoAeeQuestaos.ShouldNotBeNull();
            questoesPlanoAeeQuestaos.Count.ShouldBeGreaterThan(1);
            
            var questoesPlanoAeeRespostas = ObterTodos<PlanoAEEResposta>();
            questoesPlanoAeeRespostas.ShouldNotBeNull();
            questoesPlanoAeeRespostas.Count.ShouldBeGreaterThan(1);
        }

        private List<PlanoAEEQuestaoDto> ObterQuestoes()
        {
            return new List<PlanoAEEQuestaoDto>()
            {
                new PlanoAEEQuestaoDto()
                { 
                    QuestaoId   = 1,
                    Resposta = "1",
                    TipoQuestao = TipoQuestao.PeriodoEscolar
                },
                new PlanoAEEQuestaoDto()
                { 
                    QuestaoId   = 2,
                    Resposta = "4",
                    TipoQuestao = TipoQuestao.Radio
                },
                new PlanoAEEQuestaoDto()
                { 
                    QuestaoId   = 3,
                    Resposta = "[{\"diaSemana\":\"Segunda\",\"horarioInicio\":\"2022-09-14T08:00:38\",\"horarioTermino\":\"2022-09-14T17:30:38\",\"id\":2},{\"diaSemana\":\"Terça\",\"horarioInicio\":\"2022-09-14T09:00:00\",\"horarioTermino\":\"2022-09-14T18:30:00\",\"id\":2},{\"diaSemana\":\"Quarta\",\"horarioInicio\":\"2022-09-14T12:00:00\",\"horarioTermino\":\"2022-09-14T18:30:00\",\"id\":3},{\"diaSemana\":\"Quinta\",\"horarioInicio\":\"2022-09-14T07:35:00\",\"horarioTermino\":\"2022-09-14T16:45:00\",\"id\":4},{\"diaSemana\":\"Sexta\",\"horarioInicio\":\"2022-09-14T08:45:00\",\"horarioTermino\":\"2022-09-14T16:00:00\",\"id\":5}]",
                    TipoQuestao = TipoQuestao.FrequenciaEstudanteAEE
                },
                new PlanoAEEQuestaoDto()
                { 
                    QuestaoId   = 4,
                    Resposta = "3",
                    TipoQuestao = TipoQuestao.Radio
                },
                new PlanoAEEQuestaoDto()
                { 
                    QuestaoId   = 5,
                    Resposta = "4 - Forma de atendimento educacional especializado do estudante",
                    TipoQuestao = TipoQuestao.Texto
                },
                new PlanoAEEQuestaoDto()
                { 
                    QuestaoId   = 6,
                    Resposta = "5 - Objetivos do AEE",
                    TipoQuestao = TipoQuestao.Texto
                },
                new PlanoAEEQuestaoDto()
                { 
                    QuestaoId   = 7,
                    Resposta = "6 - Orientações e ações para o desenvolvimento/atividades do AEE",
                    TipoQuestao = TipoQuestao.Texto
                },
                new PlanoAEEQuestaoDto()
                { 
                    QuestaoId   = 8,
                    Resposta = "14",
                    TipoQuestao = TipoQuestao.Radio
                },
                new PlanoAEEQuestaoDto()
                { 
                    QuestaoId   = 9,
                    Resposta = "7 - Tem necessidade de recursos de Acessibilidade/Materiais para eliminação de barreiras para a sala regular (Seleção de materiais, equipamentos e mobiliário)",
                    TipoQuestao = TipoQuestao.Texto
                },
                new PlanoAEEQuestaoDto()
                { 
                    QuestaoId   = 10,
                    Resposta = "1",
                    TipoQuestao = TipoQuestao.Radio
                },
                new PlanoAEEQuestaoDto()
                { 
                    QuestaoId   = 11,
                    Resposta = "8 - Tem necessidade de recursos de Acessibilidade/Materiais para eliminação de barreiras para a sala de recursos multifuncionais (Seleção de materiais, equipamentos e mobiliário)",
                    TipoQuestao = TipoQuestao.Texto
                },
                new PlanoAEEQuestaoDto()
                { 
                    QuestaoId   = 12,
                    Resposta = "2",
                    TipoQuestao = TipoQuestao.Radio
                },
                new PlanoAEEQuestaoDto()
                { 
                    QuestaoId   = 13,
                    Resposta = "9 - Mobilização dos Recursos Humanos da U.E. ou parcerias na unidade educacional",
                    TipoQuestao = TipoQuestao.Texto
                },
                new PlanoAEEQuestaoDto()
                { 
                    QuestaoId   = 14,
                    Resposta = "6",
                    TipoQuestao = TipoQuestao.Radio
                },
                new PlanoAEEQuestaoDto()
                { 
                    QuestaoId   = 15,
                    Resposta = "10 - Mobilização dos Recursos Humanos com profissionais externos à unidade educacional",
                    TipoQuestao = TipoQuestao.Texto
                },
            };

        }
    }
}