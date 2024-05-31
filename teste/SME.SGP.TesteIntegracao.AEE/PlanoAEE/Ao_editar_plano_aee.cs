using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elastic.Apm.Api;
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
    public class Ao_editar_plano_aee : PlanoAEETesteBase
    {
        public Ao_editar_plano_aee(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEAnoPlanoAeeQuery, AlunoReduzidoDto>), typeof(ObterAlunoPorCodigoEAnoPlanoAeeQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasAlunoPorFiltroQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTurmasAlunoPorFiltroPlanoAEEQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>), typeof(ObterParametroSistemaPorTipoEAnoQueryHanlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterVersoesPlanoAEEQuery, IEnumerable<PlanoAEEVersaoDto>>), typeof(ObterVersoesPlanoAEEQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterVersaoPlanoAEEPorIdQuery, PlanoAEEVersaoDto>), typeof(ObterVersaoPlanoAEEPorIdQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaRegularESrmPorAlunoQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterTurmaRegularESrmPorAlunoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<EhGestorDaEscolaQuery, bool>), typeof(EhGestorDaEscolaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Plano AEE - Alterar o responsável pelo plano")]
        public async Task Alterar_responsável_pelo_plano()
        {
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
            
            var planoAeeDto = await salvarPlanoAeeUseCase.Executar(planoAeePersistenciaDto);
            
            var filtroObter = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(planoAeeDto.PlanoId,TURMA_CODIGO_1,1);
            
            var obterPlanoAeeUseCase =  ObterServicoObterPlanoAEEPorIdUseCase();
            var retornoObter = await obterPlanoAeeUseCase.Executar(filtroObter);
            retornoObter.ShouldNotBeNull();
            retornoObter.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.ParecerCP);

            var servicoAlterarResponsavelPlano = ObterServicoAtribuirResponsavelGeralDoPlanoUseCase();
            var alterarResponsavel = await servicoAlterarResponsavelPlano.Executar(planoAeeDto.PlanoId,USUARIO_PROFESSOR_LOGIN_2222222,USUARIO_PROFESSOR_LOGIN_2222222);
            alterarResponsavel.ShouldBeTrue();
            
            var retornoObterPlanoAlterado = await obterPlanoAeeUseCase.Executar(filtroObter);
            retornoObterPlanoAlterado.ShouldNotBeNull();
            
            retornoObterPlanoAlterado.Responsavel.ResponsavelNome.ShouldContain(USUARIO_PROFESSOR_LOGIN_2222222);
            retornoObterPlanoAlterado.Responsavel.ResponsavelRF.ShouldContain(USUARIO_PROFESSOR_LOGIN_2222222);

        }

        [Fact(DisplayName = "Plano AEE - Quando o plano estiver na situação Expirado, realizar qualquer edição e o plano deverá ficar na situação Validado e deverá ser criada uma nova versão")]
        public async Task Alterar_plano_expirado()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });
            await CriarPlanoAeePorSituacao(SituacaoPlanoAEE.Expirado);
            var obterTodos = ObterTodos<Dominio.PlanoAEE>();
            obterTodos.ShouldNotBeNull();
            obterTodos.FirstOrDefault()!.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.Expirado);
            
            var filtroObter = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(1,TURMA_CODIGO_1,1);
            var obterPlanoAeeUseCase =  ObterServicoObterPlanoAEEPorIdUseCase();
            var retornoObter = await obterPlanoAeeUseCase.Executar(filtroObter);
            retornoObter.ShouldNotBeNull();
            retornoObter.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.Expirado);

            
            var planoAeeEditado = new PlanoAEEPersistenciaDto()
            {
                Questoes = ObterQuestoes(),
                Id = retornoObter.Id,
                TurmaCodigo = retornoObter.Turma.Codigo,
                AlunoCodigo = retornoObter.Aluno.CodigoAluno,
                Situacao = SituacaoPlanoAEE.Expirado,
                ResponsavelRF = USUARIO_CP_LOGIN_3333333
            };
            var salvarPlanoAeeUseCase = ObterServicoSalvarPlanoAEEUseCase();
            var salvarEditar = await salvarPlanoAeeUseCase.Executar(planoAeeEditado);

            var planos = ObterTodos<Dominio.PlanoAEE>();
            planos.FirstOrDefault()!.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.ParecerCP);

            var obterTodosPendenciasAee = ObterTodos<Dominio.Pendencia>();
            obterTodosPendenciasAee.ShouldNotBeNull();
            obterTodosPendenciasAee.Count(x => x.Tipo == TipoPendencia.AEE).ShouldBeEquivalentTo(2);
            
            obterTodosPendenciasAee.Count(x => x.Situacao == SituacaoPendencia.Resolvida).ShouldBeEquivalentTo(1);
            obterTodosPendenciasAee.Count(x => x.Situacao == SituacaoPendencia.Pendente).ShouldBeEquivalentTo(1);
        }

        [Fact(DisplayName = "Plano AEE - Quando o plano estiver na situação Devolvido, ao editar ele deve ir para a situação de Aguardando parecer da coordenação ")]
        public async Task Alterar_plano_devolvido()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });
            await CriarPlanoAeePorSituacao(SituacaoPlanoAEE.Devolvido);
            var obterTodos = ObterTodos<Dominio.PlanoAEE>();
            obterTodos.ShouldNotBeNull();
            obterTodos.FirstOrDefault()!.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.Devolvido);
            
            var filtroObter = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(1,TURMA_CODIGO_1,1);
            var obterPlanoAeeUseCase =  ObterServicoObterPlanoAEEPorIdUseCase();
            var retornoObter = await obterPlanoAeeUseCase.Executar(filtroObter);
            retornoObter.ShouldNotBeNull();
            retornoObter.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.Devolvido);
            
            var planoAeeEditado = new PlanoAEEPersistenciaDto()
            {
                Questoes = ObterQuestoes(),
                Id = retornoObter.Id,
                TurmaCodigo = retornoObter.Turma.Codigo,
                AlunoCodigo = retornoObter.Aluno.CodigoAluno,
                Situacao = SituacaoPlanoAEE.Expirado,
                ResponsavelRF = USUARIO_CP_LOGIN_3333333
            };
            var salvarPlanoAeeUseCase = ObterServicoSalvarPlanoAEEUseCase();
            var salvarEditar = await salvarPlanoAeeUseCase.Executar(planoAeeEditado);

            var planos = ObterTodos<Dominio.PlanoAEE>();
            planos.FirstOrDefault()!.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.ParecerCP);

            var obterTodosPendenciasAee = ObterTodos<Dominio.Pendencia>();
            obterTodosPendenciasAee.ShouldNotBeNull();
            obterTodosPendenciasAee.Count(x => x.Tipo == TipoPendencia.AEE).ShouldBeEquivalentTo(2);
            
            obterTodosPendenciasAee.Count(x => x.Situacao == SituacaoPendencia.Resolvida).ShouldBeEquivalentTo(1);
            obterTodosPendenciasAee.Count(x => x.Situacao == SituacaoPendencia.Pendente).ShouldBeEquivalentTo(1);
        }

        [Fact(DisplayName = "Plano AEE - Ao atualizar um plano, o periodo escolar tem que ser o atual")]
        public async Task Ao_atualizar_um_plano_o_periodo_escolar_tem_que_ser_atual()
        {
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
                ResponsavelRF = USUARIO_CP_LOGIN_3333333
            };

            var planoAeeDto = await salvarPlanoAeeUseCase.Executar(planoAeePersistenciaDto);

            var filtroObter = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(planoAeeDto.PlanoId, TURMA_CODIGO_1, 1);

            var obterPlanoAeeUseCase = ObterServicoObterPlanoAEEPorIdUseCase();
            var retornoObter = await obterPlanoAeeUseCase.Executar(filtroObter);
            retornoObter.ShouldNotBeNull();
            retornoObter.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.ParecerCP);

            var questao = retornoObter.Questoes.FirstOrDefault(x => x.TipoQuestao == TipoQuestao.PeriodoEscolar);

            questao.ShouldNotBeNull();

            var respostaPeriodoEscolar = questao.Resposta.FirstOrDefault(x => x.Texto == "4");

            respostaPeriodoEscolar.ShouldNotBeNull();
        }

        [Fact(DisplayName = "Plano AEE - Com usuário do CP alterar qualquer campo do plano e submeter a alteração.")]
        public async Task Com_usuario_do_cp_alterar_qualquer_campo()
        {
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
            
            var planoAeeDto = await salvarPlanoAeeUseCase.Executar(planoAeePersistenciaDto);
            
            var filtroObter = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(planoAeeDto.PlanoId,TURMA_CODIGO_1,1);
            
            var obterPlanoAeeUseCase =  ObterServicoObterPlanoAEEPorIdUseCase();
            var retornoObter = await obterPlanoAeeUseCase.Executar(filtroObter);
            retornoObter.ShouldNotBeNull();
            retornoObter.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.ParecerCP);
            
            var planoAeeEditado = new PlanoAEEPersistenciaDto()
            {
                Questoes = ObterQuestoes(),
                Id = retornoObter.Id,
                TurmaCodigo = retornoObter.Turma.Codigo,
                AlunoCodigo = retornoObter.Aluno.CodigoAluno,
                ResponsavelRF = USUARIO_LOGIN_CP
            };
            var salvarEditar = await salvarPlanoAeeUseCase.Executar(planoAeeEditado);

            var planos = ObterTodos<Dominio.PlanoAEE>();
            planos.FirstOrDefault()!.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.ParecerCP);
            
            var obterTodosPendenciasAee = ObterTodos<Dominio.Pendencia>();
            obterTodosPendenciasAee.ShouldNotBeNull();
            obterTodosPendenciasAee.Count(x => x.Tipo == TipoPendencia.AEE).ShouldBeEquivalentTo(2);
            
            obterTodosPendenciasAee.Count(x => x.Situacao == SituacaoPendencia.Resolvida).ShouldBeEquivalentTo(1);
            obterTodosPendenciasAee.Count(x => x.Situacao == SituacaoPendencia.Pendente).ShouldBeEquivalentTo(1);
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
        private async Task CriarPlanoAeePorSituacao(SituacaoPlanoAEE situacaoPlanoAee)
        {
            var salvarPlanoAeeUseCase = ObterServicoSalvarPlanoAEEUseCase();

            var planoAeePersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = 0,
                Questoes = ObterQuestoes(),
                TurmaCodigo = TURMA_CODIGO_1,
                ResponsavelRF = USUARIO_CP_LOGIN_3333333,
            };
            
            //Inserindo
            var salvar = await salvarPlanoAeeUseCase.Executar(planoAeePersistenciaDto);
            //Atualizando
            var plano = new Dominio.PlanoAEE()
            {
                Id = salvar.PlanoId,
                TurmaId = planoAeePersistenciaDto.TurmaId,
                Situacao = situacaoPlanoAee,
                AlunoCodigo = planoAeePersistenciaDto.AlunoCodigo,
                AlunoNumero = 1,
                AlunoNome = TURMA_CODIGO_1,
                ResponsavelId = long.Parse(USUARIO_CP_LOGIN_3333333),
                CriadoPor = "Sistema",
                CriadoEm = DateTime.Now,
                CriadoRF = "1"
            };
            await AtualizarNaBase(plano);;
        }
        
    }
}