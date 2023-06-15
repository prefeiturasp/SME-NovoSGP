using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_realizar_devolucao : PlanoAEETesteBase
    {
        public Ao_realizar_devolucao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEAnoPlanoAeeQuery, AlunoReduzidoDto>), typeof(ObterAlunoPorCodigoEAnoPlanoAeeQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>), typeof(ObterParametroSistemaPorTipoEAnoQueryHanlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioLogadoQuery, Usuario>), typeof(ObterUsuarioLogadoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaRegularESrmPorAlunoQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterTurmaRegularESrmPorAlunoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Plano AEE - Com o CP editar um plano que está na situação Aguardando parecer da Coordenação e clicar em Devolver, preenchendo o campo de motivo")]
        public async Task Editar_plano_que_esta_na_situacao_aguardando_parecer_da_coordenacao()
        {
            //Criando Plano
            var salvarPlanoAeeUseCase = ObterServicoSalvarPlanoAEEUseCase();

            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });

            var planoAeePersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = 0,
                Questoes = ObterQuestoes(),
                TurmaCodigo = TURMA_CODIGO_1,
                ResponsavelRF = USUARIO_CP_LOGIN_3333333,
            };
            
            await salvarPlanoAeeUseCase.Executar(planoAeePersistenciaDto);
            
            //Obter todos os planos
            var obterTodosPlanoAee = ObterTodos<Dominio.PlanoAEE>();
            obterTodosPlanoAee.ShouldNotBeNull();
            
            //Buscando Plano Criado
            var filtroObter = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(obterTodosPlanoAee.FirstOrDefault()?.Id,TURMA_CODIGO_1,1);
            
            var obterPlanoAeeUseCase =  ObterServicoObterPlanoAEEPorIdUseCase();
            var retornoObter = await obterPlanoAeeUseCase.Executar(filtroObter);
            retornoObter.ShouldNotBeNull();
            retornoObter.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.ParecerCP);

            // Devolvendo o Plano Com a Justificativa
            var filtroDevolver = new DevolucaoPlanoAEEDto(){PlanoAEEId = retornoObter.Id,Motivo = "Motivo Teste"};
            var devolverUseCase = ObterServicoDevolverPlanoAEEUseCase();
            var retornoDevolver = await devolverUseCase.Executar(filtroDevolver);
            retornoDevolver.ShouldBeTrue();
            
            //Obter Todas Pendencias
            var obterTodosPendenciasAee = ObterTodos<Dominio.Pendencia>();
            obterTodosPendenciasAee.ShouldNotBeNull();
            obterTodosPendenciasAee.FirstOrDefault()!.Tipo.ShouldBeEquivalentTo(TipoPendencia.AEE);
            obterTodosPendenciasAee.FirstOrDefault()!.Situacao.ShouldBeEquivalentTo(SituacaoPendencia.Resolvida);
            
            obterTodosPendenciasAee.LastOrDefault()!.Tipo.ShouldBeEquivalentTo(TipoPendencia.AEE);
            obterTodosPendenciasAee.LastOrDefault()!.Situacao.ShouldBeEquivalentTo(SituacaoPendencia.Pendente);

        }

        [Fact(DisplayName = "Plano AEE - Com o Coordenador do CEFAI editar um plano que está na situação Aguardando atribuição de PAAI e clicar em Devolver, preenchendo o campo de motivo.")]
        public async Task Editar_um_plano_que_esta_na_situacao_aguardando_atribuicao_paai()
        {
            //Criando Plano como CP
            var salvarPlanoAeeUseCase = ObterServicoSalvarPlanoAEEUseCase();

            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCoordenadorCefai(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });

            var planoAeePersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Questoes = ObterQuestoes(),
                TurmaCodigo = TURMA_CODIGO_1,
                ResponsavelRF = USUARIO_CEFAI_LOGIN_3333333,
            };
            await salvarPlanoAeeUseCase.Executar(planoAeePersistenciaDto);
            
            //Verificando se criou o plano no banco
            var obterTodosPlanoAee = ObterTodos<Dominio.PlanoAEE>();
            obterTodosPlanoAee.ShouldNotBeNull();
            
            //Buscando Plano Criado com Situacao Parecer do CP
            var filtroObter = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(obterTodosPlanoAee.FirstOrDefault()?.Id,TURMA_CODIGO_1,1);
            
            var obterPlanoAeeUseCase =  ObterServicoObterPlanoAEEPorIdUseCase();
            var retornoObter = await obterPlanoAeeUseCase.Executar(filtroObter);
            retornoObter.ShouldNotBeNull();
            retornoObter.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.ParecerCP);
            
            //Gerar Parece do CP
            var cpPlanoAeeUseCase = ObterServicoCadastrarParecerCPPlanoAEEUseCase();
            var retornoParecerCp = await cpPlanoAeeUseCase.Executar(retornoObter.Id, new PlanoAEECadastroParecerDto() {Parecer = "Parecer do CP"});
            retornoParecerCp.ShouldBeTrue();
            
            //Buscando Plano Criado com Atribuição PAAI
            var filtroObterPaai = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(retornoObter.Id,TURMA_CODIGO_1,1);
            var obterPlanoAeeUseCasePaai =  ObterServicoObterPlanoAEEPorIdUseCase();
            var retornoObterPaai = await obterPlanoAeeUseCasePaai.Executar(filtroObterPaai);
            retornoObterPaai.ShouldNotBeNull();
            retornoObterPaai.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.AtribuicaoPAAI);
            
            
            // Devolvendo o Plano Com a Justificativa do CEFAI
            var filtroDevolver = new DevolucaoPlanoAEEDto(){PlanoAEEId = retornoObter.Id,Motivo = "Motivo Teste"};
            var devolverUseCase = ObterServicoDevolverPlanoAEEUseCase();
            var retornoDevolver = await devolverUseCase.Executar(filtroDevolver);
            retornoDevolver.ShouldBeTrue();
            
            //Obter Todas Pendencias
            var obterTodosPendenciasAee = ObterTodos<Dominio.Pendencia>();
            obterTodosPendenciasAee.ShouldNotBeNull();
            obterTodosPendenciasAee.Count(x => x.Tipo == TipoPendencia.AEE).ShouldBeEquivalentTo(3);
            
            obterTodosPendenciasAee.Count(x => x.Situacao == SituacaoPendencia.Resolvida).ShouldBeEquivalentTo(1);
            obterTodosPendenciasAee.Count(x => x.Situacao == SituacaoPendencia.Pendente).ShouldBeEquivalentTo(2);
        }
        [Fact(DisplayName = "Plano AEE - Com o PAAI editar um plano que está na situação Aguardando parecer do CEFAI e clicar em Devolver, preenchendo o campo de motivo")]
        public async Task Editar_um_plano_que_esta_na_situacao_aguardando_parecer_do_cefai()
        {
            //Criando Plano
            var salvarPlanoAeeUseCase = ObterServicoSalvarPlanoAEEUseCase();

            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilPaai(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });

            var planoAeePersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = 0,
                Questoes = ObterQuestoes(),
                TurmaCodigo = TURMA_CODIGO_1,
                ResponsavelRF = USUARIO_CP_LOGIN_3333333,
            };
            
            await salvarPlanoAeeUseCase.Executar(planoAeePersistenciaDto);
            
            //Verificando se salvou no banco
            var obterTodosPlanoAee = ObterTodos<Dominio.PlanoAEE>();
            obterTodosPlanoAee.ShouldNotBeNull();
            
            //Buscando Plano Criado
            var filtroObter = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(obterTodosPlanoAee.FirstOrDefault()!.Id,TURMA_CODIGO_1,1);
            
            var obterPlanoAeeUseCase =  ObterServicoObterPlanoAEEPorIdUseCase();
            var retornoObter = await obterPlanoAeeUseCase.Executar(filtroObter);
            retornoObter.ShouldNotBeNull();
            retornoObter.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.ParecerCP);
            
            //Gerar Parece do CP
            var cpPlanoAeeUseCase = ObterServicoCadastrarParecerCPPlanoAEEUseCase();
            var retornoParecerCp = await cpPlanoAeeUseCase.Executar(retornoObter.Id, new PlanoAEECadastroParecerDto() {Parecer = "Parecer do CP"});
            retornoParecerCp.ShouldBeTrue();
            
            //Buscando Plano Criado com Atribuição PAAI
            var filtroObterPaai = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(retornoObter.Id,TURMA_CODIGO_1,1);
            var obterPlanoAeeUseCasePaai =  ObterServicoObterPlanoAEEPorIdUseCase();
            var retornoObterPaai = await obterPlanoAeeUseCasePaai.Executar(filtroObterPaai);
            retornoObterPaai.ShouldNotBeNull();
            retornoObterPaai.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.AtribuicaoPAAI);

            
            //Realizar Atribuição para um usuario PAAI pelo Coordenador do CEFAI
            var servicoAtribuicaoResponsavel = ObterServicoAtribuirResponsavelPlanoAEEUseCase();
            var retornoAtribuicaoUsuarioPaai = await servicoAtribuicaoResponsavel.Executar(retornoObter.Id,USUARIO_PAAI_LOGIN_3333333);
            retornoAtribuicaoUsuarioPaai.ShouldBeTrue();
            
            // Devolvendo o Plano Com a Justificativa do PAAI
            var filtroDevolver = new DevolucaoPlanoAEEDto(){PlanoAEEId = retornoObter.Id,Motivo = "Motivo Teste PAAI"};
            var devolverUseCase = ObterServicoDevolverPlanoAEEUseCase();
            var retornoDevolver = await devolverUseCase.Executar(filtroDevolver);
            retornoDevolver.ShouldBeTrue();
            
            //Obter Todas Pendencias
            var obterTodosPendenciasAee = ObterTodos<Dominio.Pendencia>();
            obterTodosPendenciasAee.ShouldNotBeNull();
            obterTodosPendenciasAee.Count(x => x.Tipo == TipoPendencia.AEE).ShouldBeEquivalentTo(4);
            
            obterTodosPendenciasAee.Count(x => x.Situacao == SituacaoPendencia.Resolvida).ShouldBeEquivalentTo(1);
            obterTodosPendenciasAee.Count(x => x.Situacao == SituacaoPendencia.Pendente).ShouldBeEquivalentTo(3);
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