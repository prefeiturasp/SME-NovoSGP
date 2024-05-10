using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_encerrar_automaticamente : PlanoAEETesteBase
    {
        public Ao_encerrar_automaticamente(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterMatriculasAlunoPorCodigoEAnoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterMatriculasAlunoPorCodigoEAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaRegularESrmPorAlunoQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterTurmaRegularESrmPorAlunoQueryHandlerFake), ServiceLifetime.Scoped));

        }

        [Fact(DisplayName = "Plano AEE - Deve Encerrar automáticamente os planos de alunos que ficaram inativos ou foram transferidos")]
        public async Task Deve_Encerrar_automaticamente_aluno_inativo_ou_transferido()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });

            await CriarPlanosAeeParaEncerrarAutomaticamente();
            var servicoEncerrarTratar = EncerrarPlanosAEEEstudantesInativosTratarUseCase();
            var planosAeeMensagem = ObterTodos<Dominio.PlanoAEE>();

            foreach(var plano in planosAeeMensagem)
            {
                var mensagemJson = JsonConvert.SerializeObject(plano);
                var retornoUseCase = await servicoEncerrarTratar.Executar(new MensagemRabbit() { Mensagem = mensagemJson });
                retornoUseCase.ShouldBeTrue();
            }

            var planosAee = ObterTodos<Dominio.PlanoAEE>();
            planosAee.ShouldNotBeNull();
            
            planosAee.Count(x => x.Situacao == SituacaoPlanoAEE.ParecerCP).ShouldBeEquivalentTo(3);
            planosAee.Count(x => x.Situacao == SituacaoPlanoAEE.EncerradoAutomaticamente).ShouldBeEquivalentTo(1);
        }

        [Fact(DisplayName = "Plano AEE - Não Deve Encerrar automáticamente os planos de Alunos Concluidos, com matricula no Ano seguinte na mesma UE")]
        public async Task Nao_deve_encerrar_aluno_concluido_com_matricula_no_ano_seguinte_mesma_ue()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });
            await CriarDreUe("1","1");
            await CriarTurma(Modalidade.Fundamental, "1", "2", TipoTurma.Regular, 1, 2021,false);
            await CriarPlanosAeeAlunoConcluido();
            
            var servicoEncerrar = EncerrarPlanosAEEEstudantesInativosUseCase();
            
            var retornoUseCase = await servicoEncerrar.Executar(new MensagemRabbit());
            retornoUseCase.ShouldBeTrue();
            var planosAee = ObterTodos<Dominio.PlanoAEE>();
            planosAee.ShouldNotBeNull();
            planosAee.Count(x => x.Situacao == SituacaoPlanoAEE.ParecerCP).ShouldBeEquivalentTo(1);
            planosAee.Count(x => x.Situacao == SituacaoPlanoAEE.EncerradoAutomaticamente).ShouldBeEquivalentTo(0);
        }
        
        [Fact(DisplayName = "Plano AEE - Deve Encerrar automáticamente os planos de Alunos Concluidos, com matricula no Ano seguinte em outra UE")]
        public async Task Deve_encerrar_aluno_concluido_com_matricula_no_ano_seguinte_em_outra_ue()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });
            await CriarDreUe("2","2");
            await CriarTurma(Modalidade.EducacaoInfantil, "2", "2", TipoTurma.Regular, 2, 2021,false);
            await CriarPlanosAeeAlunoConcluido();
            
            var servicoEncerrarTratar = EncerrarPlanosAEEEstudantesInativosTratarUseCase();
            var planosAeeMensagem = ObterTodos<Dominio.PlanoAEE>();
            var mensagemJson = JsonConvert.SerializeObject(planosAeeMensagem.FirstOrDefault());

            var retornoUseCase = await servicoEncerrarTratar.Executar(new MensagemRabbit() { Mensagem = mensagemJson });
            retornoUseCase.ShouldBeTrue();

            var planosAee = ObterTodos<Dominio.PlanoAEE>();
            planosAee.ShouldNotBeNull();
            Assert.Equal(0, planosAee.Count(x => x.Situacao == SituacaoPlanoAEE.ParecerCP));
            Assert.Equal(1, planosAee.Count(x => x.Situacao == SituacaoPlanoAEE.EncerradoAutomaticamente));
        }


        private new async Task CriarDreUe(string codigoDre,string codigoUe)
        {
            await InserirNaBase(new Dre
            {
                CodigoDre = codigoDre,
                Abreviacao = DRE_NOME_1,
                Nome = DRE_NOME_1
            });

            await InserirNaBase(new Ue
            {
                CodigoUe = codigoUe,
                DreId = 2,
                Nome = codigoUe,
            });
        }

        private async Task CriarPlanosAeeAlunoConcluido()
        {
            var salvarPlanoAeeUseCase = ObterServicoSalvarPlanoAEEUseCase();
            await salvarPlanoAeeUseCase.Executar(new PlanoAEEPersistenciaDto()
            {
                TurmaId = 2,
                AlunoCodigo = "5",
                Situacao = 0,
                Questoes = ObterQuestoes(),
                TurmaCodigo = "2",
                ResponsavelRF = USUARIO_CP_LOGIN_3333333,
            });
        }
        private async Task CriarPlanosAeeParaEncerrarAutomaticamente()
        {
            var salvarPlanoAeeUseCase = ObterServicoSalvarPlanoAEEUseCase();
            await salvarPlanoAeeUseCase.Executar(new PlanoAEEPersistenciaDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = 0,
                Questoes = ObterQuestoes(),
                TurmaCodigo = TURMA_CODIGO_1,
                ResponsavelRF = USUARIO_CP_LOGIN_3333333,
            });
            await salvarPlanoAeeUseCase.Executar(new PlanoAEEPersistenciaDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_2,
                Situacao = 0,
                Questoes = ObterQuestoes(),
                TurmaCodigo = TURMA_CODIGO_1,
                ResponsavelRF = USUARIO_CP_LOGIN_3333333,
            });
            await salvarPlanoAeeUseCase.Executar(new PlanoAEEPersistenciaDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_3,
                Situacao = 0,
                Questoes = ObterQuestoes(),
                TurmaCodigo = TURMA_CODIGO_1,
                ResponsavelRF = USUARIO_CP_LOGIN_3333333,
            });
            await salvarPlanoAeeUseCase.Executar(new PlanoAEEPersistenciaDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_4,
                Situacao = 0,
                Questoes = ObterQuestoes(),
                TurmaCodigo = TURMA_CODIGO_1,
                ResponsavelRF = USUARIO_CP_LOGIN_3333333,
            });
        }
        private List<PlanoAEEQuestaoDto> ObterQuestoes()
        {
            return new List<PlanoAEEQuestaoDto>()
            {
                new PlanoAEEQuestaoDto()
                {
                    QuestaoId = 1,
                    Resposta = "1",
                    TipoQuestao = TipoQuestao.PeriodoEscolar
                },
                new PlanoAEEQuestaoDto()
                {
                    QuestaoId = 2,
                    Resposta = "4",
                    TipoQuestao = TipoQuestao.Radio
                },
                new PlanoAEEQuestaoDto()
                {
                    QuestaoId = 3,
                    Resposta = "[{\"diaSemana\":\"Segunda\",\"horarioInicio\":\"2022-09-14T08:00:38\",\"horarioTermino\":\"2022-09-14T17:30:38\",\"id\":2},{\"diaSemana\":\"Terça\",\"horarioInicio\":\"2022-09-14T09:00:00\",\"horarioTermino\":\"2022-09-14T18:30:00\",\"id\":2},{\"diaSemana\":\"Quarta\",\"horarioInicio\":\"2022-09-14T12:00:00\",\"horarioTermino\":\"2022-09-14T18:30:00\",\"id\":3},{\"diaSemana\":\"Quinta\",\"horarioInicio\":\"2022-09-14T07:35:00\",\"horarioTermino\":\"2022-09-14T16:45:00\",\"id\":4},{\"diaSemana\":\"Sexta\",\"horarioInicio\":\"2022-09-14T08:45:00\",\"horarioTermino\":\"2022-09-14T16:00:00\",\"id\":5}]",
                    TipoQuestao = TipoQuestao.FrequenciaEstudanteAEE
                },
                new PlanoAEEQuestaoDto()
                {
                    QuestaoId = 4,
                    Resposta = "3",
                    TipoQuestao = TipoQuestao.Radio
                },
                new PlanoAEEQuestaoDto()
                {
                    QuestaoId = 5,
                    Resposta = "4 - Forma de atendimento educacional especializado do estudante",
                    TipoQuestao = TipoQuestao.Texto
                },
                new PlanoAEEQuestaoDto()
                {
                    QuestaoId = 6,
                    Resposta = "5 - Objetivos do AEE",
                    TipoQuestao = TipoQuestao.Texto
                },
                new PlanoAEEQuestaoDto()
                {
                    QuestaoId = 7,
                    Resposta = "6 - Orientações e ações para o desenvolvimento/atividades do AEE",
                    TipoQuestao = TipoQuestao.Texto
                },
                new PlanoAEEQuestaoDto()
                {
                    QuestaoId = 8,
                    Resposta = "14",
                    TipoQuestao = TipoQuestao.Radio
                },
                new PlanoAEEQuestaoDto()
                {
                    QuestaoId = 9,
                    Resposta = "7 - Tem necessidade de recursos de Acessibilidade/Materiais para eliminação de barreiras para a sala regular (Seleção de materiais, equipamentos e mobiliário)",
                    TipoQuestao = TipoQuestao.Texto
                },
                new PlanoAEEQuestaoDto()
                {
                    QuestaoId = 10,
                    Resposta = "1",
                    TipoQuestao = TipoQuestao.Radio
                },
                new PlanoAEEQuestaoDto()
                {
                    QuestaoId = 11,
                    Resposta = "8 - Tem necessidade de recursos de Acessibilidade/Materiais para eliminação de barreiras para a sala de recursos multifuncionais (Seleção de materiais, equipamentos e mobiliário)",
                    TipoQuestao = TipoQuestao.Texto
                },
                new PlanoAEEQuestaoDto()
                {
                    QuestaoId = 12,
                    Resposta = "2",
                    TipoQuestao = TipoQuestao.Radio
                },
                new PlanoAEEQuestaoDto()
                {
                    QuestaoId = 13,
                    Resposta = "9 - Mobilização dos Recursos Humanos da U.E. ou parcerias na unidade educacional",
                    TipoQuestao = TipoQuestao.Texto
                },
                new PlanoAEEQuestaoDto()
                {
                    QuestaoId = 14,
                    Resposta = "6",
                    TipoQuestao = TipoQuestao.Radio
                },
                new PlanoAEEQuestaoDto()
                {
                    QuestaoId = 15,
                    Resposta = "10 - Mobilização dos Recursos Humanos com profissionais externos à unidade educacional",
                    TipoQuestao = TipoQuestao.Texto
                },
            };
        }
    }
}