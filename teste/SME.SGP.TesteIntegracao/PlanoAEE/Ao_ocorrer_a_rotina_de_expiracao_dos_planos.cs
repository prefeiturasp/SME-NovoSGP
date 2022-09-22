using System;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_ocorrer_a_rotina_de_expiracao_dos_planos : PlanoAEETesteBase
    {
        public Ao_ocorrer_a_rotina_de_expiracao_dos_planos(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>), typeof(ObterParametroSistemaPorTipoEAnoQueryHanlerFake), ServiceLifetime.Scoped));
        }
        
        [Fact(DisplayName = "Plano AEE -  Todos os planos que estiverem Validado deverão ser alterados para Expirado e deverá ser gerada pendência para os responsáveis pelo plano.")]
        public async Task Deve_alterar_status_validado_para_expirado()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });

            await CriarPlanoAeePorSituacao();
            await CriarPlanoAeePorSituacao();
            await CriarPlanoAeePorSituacao(SituacaoPlanoAEE.Validado, true,true);
            await CriarPlanoAeePorSituacao(SituacaoPlanoAEE.Devolvido, true);
            await CriarPlanoAeePorSituacao(SituacaoPlanoAEE.AtribuicaoPAAI, true);
            
            var pendencias = ObterTodos<Pendencia>();
            pendencias.Count().ShouldBeEquivalentTo(5);
            
            var pendenciasPlanoAee = ObterTodos<PendenciaPlanoAEE>();
            pendenciasPlanoAee.Count().ShouldBeEquivalentTo(5);
            
            var obterPlanosVersoes = ObterTodos<PlanoAEEVersao>();
            obterPlanosVersoes.Count().ShouldBeEquivalentTo(5);
            
            var servicoExpirarPlano = ObterServicoGerarPendenciaValidadePlanoAEEUseCase();
            var retornoExpirarPlanos = await servicoExpirarPlano.Executar(new MensagemRabbit());
            retornoExpirarPlanos.ShouldBeTrue();
            
            var obterTodosOsPlanos = ObterTodos<Dominio.PlanoAEE>();
            obterTodosOsPlanos.ShouldNotBeNull();

            obterTodosOsPlanos.Count().ShouldBeEquivalentTo(5);
            obterTodosOsPlanos.Count(x => x.Situacao == SituacaoPlanoAEE.Validado).ShouldBeEquivalentTo(0);
            obterTodosOsPlanos.Count(x => x.Situacao == SituacaoPlanoAEE.ParecerCP).ShouldBeEquivalentTo(2);
            obterTodosOsPlanos.Count(x => x.Situacao == SituacaoPlanoAEE.Expirado).ShouldBeEquivalentTo(1);
            obterTodosOsPlanos.Count(x => x.Situacao == SituacaoPlanoAEE.Devolvido).ShouldBeEquivalentTo(1);
            obterTodosOsPlanos.Count(x => x.Situacao == SituacaoPlanoAEE.AtribuicaoPAAI).ShouldBeEquivalentTo(1);

            var obterTodasPendenciasPlanoAee = ObterTodos<PendenciaPlanoAEE>();
            obterTodasPendenciasPlanoAee.Count().ShouldBeEquivalentTo(6);
            
            var obterTodasPendencias = ObterTodos<Pendencia>();
            obterTodasPendencias.Count().ShouldBeEquivalentTo(6);
            obterTodasPendencias.Count(x => x.Situacao == SituacaoPendencia.Resolvida).ShouldBeEquivalentTo(1);
            obterTodasPendencias.Count(x => x.Situacao == SituacaoPendencia.Pendente).ShouldBeEquivalentTo(5);
        }

        private async Task CriarPlanoAeePorSituacao(SituacaoPlanoAEE situacaoPlanoAee = SituacaoPlanoAEE.ParecerCP, bool alterarSitacao = false,bool resolverPendencia = false)
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
            if (alterarSitacao)
            {
                var usuarios = ObterTodos<Usuario>();
                var usuarioId = usuarios.FirstOrDefault(x => x.CodigoRf == USUARIO_CP_LOGIN_3333333)!.Id;
                //Atualizando
                var plano = new Dominio.PlanoAEE()
                {
                    Id = salvar.PlanoId,
                    TurmaId = planoAeePersistenciaDto.TurmaId,
                    Situacao = situacaoPlanoAee,
                    AlunoCodigo = planoAeePersistenciaDto.AlunoCodigo,
                    AlunoNumero = 1,
                    AlunoNome = TURMA_CODIGO_1,
                    ResponsavelId = usuarioId,
                    CriadoPor = "Sistema",
                    CriadoEm = DateTime.Now,
                    CriadoRF = "1"
                };

                await AtualizarNaBase(plano);
            }

            if (resolverPendencia)
            {
                var pendenciaPlanoAee = ObterTodos<PendenciaPlanoAEE>();
                var pendenciaId = pendenciaPlanoAee.FirstOrDefault(x => x.PlanoAEEId == salvar.PlanoId)!.PendenciaId;
                var pendencias = ObterTodos<Pendencia>();
                var pendeciaParaAtualizar = pendencias.FirstOrDefault(x => x.Id == pendenciaId);
                pendeciaParaAtualizar.Situacao = SituacaoPendencia.Resolvida;
                await AtualizarNaBase(pendeciaParaAtualizar);
            }
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