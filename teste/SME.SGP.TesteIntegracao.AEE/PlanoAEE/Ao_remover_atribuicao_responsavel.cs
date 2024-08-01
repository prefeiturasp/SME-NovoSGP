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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_remover_atribuicao_responsavel : PlanoAEETesteBase
    {
        public Ao_remover_atribuicao_responsavel(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEAnoPlanoAeeQuery, AlunoReduzidoDto>), typeof(ObterAlunoPorCodigoEAnoPlanoAeeQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>), typeof(ObterParametroSistemaPorTipoEAnoQueryHanlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaRegularESrmPorAlunoQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterTurmaRegularESrmPorAlunoQueryHandlerFake), ServiceLifetime.Scoped));
        }
        
        [Fact(DisplayName = "Plano AEE - Com o Coordenador do CEFAI remover atribuição do PAAI.")]
        public async Task Remover_atribuicao_paai_com_usuario_cefai()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCoordenadorCefai(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });

            var idPlano = await CriarPlanoAeePorSituacao(SituacaoPlanoAEE.ParecerPAAI);

            var filtroObter = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(idPlano, TURMA_CODIGO_1,1);

            var obterPlanoAeeUseCase = ObterServicoObterPlanoAEEPorIdUseCase();
            var retornoObter = await obterPlanoAeeUseCase.Executar(filtroObter);
            retornoObter.ShouldNotBeNull();
            retornoObter.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.ParecerPAAI);

            var servicoRemocaoAtribuicaoResponsavel = ObterServicoRemocaoAtribuirResponsavelPlanoAEEUseCase();
            var retornoAtribuicaoUsuarioPaai = await servicoRemocaoAtribuicaoResponsavel.Executar(retornoObter.Id);
            retornoAtribuicaoUsuarioPaai.ShouldBeTrue();

            var retornoObterPlanoAlterado = ObterTodos<Dominio.PlanoAEE>();
            retornoObterPlanoAlterado.ShouldNotBeNull();
            (retornoObterPlanoAlterado.FirstOrDefault().Situacao == SituacaoPlanoAEE.AtribuicaoPAAI).ShouldBeTrue();
            retornoObterPlanoAlterado.FirstOrDefault().ResponsavelPaaiId.ShouldBeNull();

            var pendencias = ObterTodos<Pendencia>();
            pendencias.ShouldNotBeNull();
            pendencias.Count(x => x.Tipo == TipoPendencia.AEE).ShouldBeEquivalentTo(2);

            pendencias.Count(x => x.Situacao == SituacaoPendencia.Pendente && !x.Excluido).ShouldBeEquivalentTo(1);
            pendencias.Count(x => x.Situacao == SituacaoPendencia.Pendente && x.Excluido).ShouldBeEquivalentTo(1);
        }

        [Fact(DisplayName = "Plano AEE - Com o Coordenador do CEFAI remover atribuição do PAAI quando expirado/validado.")]
        public async Task Remover_atribuicao_paai_com_usuario_cefai_situacao_expirado_validado()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCoordenadorCefai(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });

            var idPlano1 = await CriarPlanoAeePorSituacao(SituacaoPlanoAEE.Expirado);
            var idPlano2 = await CriarPlanoAeePorSituacao(SituacaoPlanoAEE.Validado);
            const int qdadePendenciasOriginadasInclusao = 2;
            

            var servicoRemocaoAtribuicaoResponsavel = ObterServicoRemocaoAtribuirResponsavelPlanoAEEUseCase();
            var retornoAtribuicaoUsuarioPaai = await servicoRemocaoAtribuicaoResponsavel.Executar(idPlano1);
            retornoAtribuicaoUsuarioPaai.ShouldBeTrue();

            retornoAtribuicaoUsuarioPaai = await servicoRemocaoAtribuicaoResponsavel.Executar(idPlano2);
            retornoAtribuicaoUsuarioPaai.ShouldBeTrue();

            var retornoObterPlanoAlterado = ObterTodos<Dominio.PlanoAEE>();
            retornoObterPlanoAlterado.ShouldNotBeNull();
            (retornoObterPlanoAlterado.FirstOrDefault().Situacao == SituacaoPlanoAEE.Expirado).ShouldBeTrue();
            retornoObterPlanoAlterado.FirstOrDefault().ResponsavelPaaiId.ShouldBeNull();
            (retornoObterPlanoAlterado.LastOrDefault().Situacao == SituacaoPlanoAEE.Validado).ShouldBeTrue();
            retornoObterPlanoAlterado.LastOrDefault().ResponsavelPaaiId.ShouldBeNull();

            var pendencias = ObterTodos<Pendencia>();
            pendencias.Count().ShouldBeEquivalentTo(qdadePendenciasOriginadasInclusao);
        }

        private async Task<long> CriarPlanoAeePorSituacao(SituacaoPlanoAEE situacaoPlanoAee)
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
            
            var salvar = await salvarPlanoAeeUseCase.Executar(planoAeePersistenciaDto);
            
            var plano = new Dominio.PlanoAEE()
            {
                Id = salvar.PlanoId,
                TurmaId = planoAeePersistenciaDto.TurmaId,
                Situacao = situacaoPlanoAee,
                AlunoCodigo = planoAeePersistenciaDto.AlunoCodigo,
                AlunoNumero = 1,
                AlunoNome = TURMA_CODIGO_1,
                ResponsavelId = long.Parse(USUARIO_CP_LOGIN_3333333),
                CriadoPor = SISTEMA_NOME, CriadoEm = DateTime.Now, CriadoRF = SISTEMA_CODIGO_RF
            };
            await AtualizarNaBase(plano);

            return salvar.PlanoId;
        }
        
        private List<PlanoAEEQuestaoDto> ObterQuestoes()
        {
            return new List<PlanoAEEQuestaoDto>()
            {
                new ()
                {
                    QuestaoId   = 1, Resposta = "1", TipoQuestao = TipoQuestao.PeriodoEscolar
                },
                new ()
                {
                    QuestaoId   = 2, Resposta = "4", TipoQuestao = TipoQuestao.Radio
                },
                new ()
                {
                    QuestaoId   = 3, 
                    Resposta = "[{\"diaSemana\":\"Segunda\",\"horarioInicio\":\"2022-09-14T08:00:38\",\"horarioTermino\":\"2022-09-14T17:30:38\",\"id\":2},{\"diaSemana\":\"Terça\",\"horarioInicio\":\"2022-09-14T09:00:00\",\"horarioTermino\":\"2022-09-14T18:30:00\",\"id\":2},{\"diaSemana\":\"Quarta\",\"horarioInicio\":\"2022-09-14T12:00:00\",\"horarioTermino\":\"2022-09-14T18:30:00\",\"id\":3},{\"diaSemana\":\"Quinta\",\"horarioInicio\":\"2022-09-14T07:35:00\",\"horarioTermino\":\"2022-09-14T16:45:00\",\"id\":4},{\"diaSemana\":\"Sexta\",\"horarioInicio\":\"2022-09-14T08:45:00\",\"horarioTermino\":\"2022-09-14T16:00:00\",\"id\":5}]", 
                    TipoQuestao = TipoQuestao.FrequenciaEstudanteAEE
                },
                new ()
                { 
                    QuestaoId   = 4,
                    Resposta = "3",
                    TipoQuestao = TipoQuestao.Radio
                },
                new ()
                { 
                    QuestaoId   = 5,
                    Resposta = "4 - Forma de atendimento educacional especializado do estudante",
                    TipoQuestao = TipoQuestao.Texto
                },
                new ()
                { 
                    QuestaoId   = 6,
                    Resposta = "5 - Objetivos do AEE",
                    TipoQuestao = TipoQuestao.Texto
                },
                new ()
                { 
                    QuestaoId   = 7,
                    Resposta = "6 - Orientações e ações para o desenvolvimento/atividades do AEE",
                    TipoQuestao = TipoQuestao.Texto
                },
                new ()
                { 
                    QuestaoId   = 8,
                    Resposta = "14",
                    TipoQuestao = TipoQuestao.Radio
                },
                new ()
                { 
                    QuestaoId   = 9,
                    Resposta = "7 - Tem necessidade de recursos de Acessibilidade/Materiais para eliminação de barreiras para a sala regular (Seleção de materiais, equipamentos e mobiliário)",
                    TipoQuestao = TipoQuestao.Texto
                },
                new ()
                { 
                    QuestaoId   = 10,
                    Resposta = "1",
                    TipoQuestao = TipoQuestao.Radio
                },
                new ()
                { 
                    QuestaoId   = 11,
                    Resposta = "8 - Tem necessidade de recursos de Acessibilidade/Materiais para eliminação de barreiras para a sala de recursos multifuncionais (Seleção de materiais, equipamentos e mobiliário)",
                    TipoQuestao = TipoQuestao.Texto
                },
                new ()
                { 
                    QuestaoId   = 12,
                    Resposta = "2",
                    TipoQuestao = TipoQuestao.Radio
                },
                new ()
                { 
                    QuestaoId   = 13,
                    Resposta = "9 - Mobilização dos Recursos Humanos da U.E. ou parcerias na unidade educacional",
                    TipoQuestao = TipoQuestao.Texto
                },
                new ()
                { 
                    QuestaoId   = 14,
                    Resposta = "6",
                    TipoQuestao = TipoQuestao.Radio
                },
                new ()
                { 
                    QuestaoId   = 15,
                    Resposta = "10 - Mobilização dos Recursos Humanos com profissionais externos à unidade educacional",
                    TipoQuestao = TipoQuestao.Texto
                },
            };

        }
    }
}