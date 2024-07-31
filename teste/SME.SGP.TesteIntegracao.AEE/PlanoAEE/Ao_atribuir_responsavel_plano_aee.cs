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
    public class Ao_atribuir_responsavel_plano_aee : PlanoAEETesteBase
    {
        public Ao_atribuir_responsavel_plano_aee(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEAnoPlanoAeeQuery, AlunoReduzidoDto>), typeof(ObterAlunoPorCodigoEAnoPlanoAeeQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>), typeof(ObterParametroSistemaPorTipoEAnoQueryHanlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaRegularESrmPorAlunoQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterTurmaRegularESrmPorAlunoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Plano AEE - Com o Coordenador do CEFAI realizar atribuição do PAAI.")]
        public async Task Realizar_atribuicao_paai_com_usuario_cefai()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCoordenadorCefai(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });
            
            var idPlano = await CriarPlanoAeePorSituacao(SituacaoPlanoAEE.ParecerPAAI);
            
            var filtroObter = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(idPlano,TURMA_CODIGO_1,1);
            
            var obterPlanoAeeUseCase =  ObterServicoObterPlanoAEEPorIdUseCase();
            var retornoObter = await obterPlanoAeeUseCase.Executar(filtroObter);
            retornoObter.ShouldNotBeNull();
            retornoObter.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.ParecerPAAI);

            var servicoAtribuicaoResponsavel = ObterServicoAtribuirResponsavelPlanoAEEUseCase();
            var retornoAtribuicaoUsuarioPaai = await servicoAtribuicaoResponsavel.Executar(retornoObter.Id,USUARIO_LOGIN_PAAI);
            retornoAtribuicaoUsuarioPaai.ShouldBeTrue();

            var retornoObterPlanoAlterado = ObterTodos<Dominio.PlanoAEE>();
            var usuarios = ObterTodos<Usuario>();
            retornoObterPlanoAlterado.ShouldNotBeNull();
            
            var paaiAtribuido = usuarios.FirstOrDefault(x => x.CodigoRf == USUARIO_LOGIN_PAAI);
            retornoObterPlanoAlterado.FirstOrDefault()!.ResponsavelPaaiId.ShouldBeEquivalentTo(paaiAtribuido!.Id);

            var pendencias = ObterTodos<Pendencia>();
            pendencias.ShouldNotBeNull();
            pendencias.Count(x => x.Tipo == TipoPendencia.AEE).ShouldBeEquivalentTo(2);
            
            pendencias.Count(x => x.Situacao == SituacaoPendencia.Pendente && !x.Excluido).ShouldBeEquivalentTo(1);
            pendencias.Count(x => x.Situacao == SituacaoPendencia.Pendente && x.Excluido).ShouldBeEquivalentTo(1);
            
        }

        [Fact(DisplayName = "Plano AEE - Com o Coordenador do CEFAI realizar atribuição do PAAI quando expirado/validado.")]
        public async Task Realizar_atribuicao_paai_com_usuario_cefai_expirado_validado()
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

            var servicoAtribuicaoResponsavel = ObterServicoAtribuirResponsavelPlanoAEEUseCase();
            var retornoAtribuicaoUsuarioPaai = await servicoAtribuicaoResponsavel.Executar(idPlano1, USUARIO_LOGIN_PAAI);
            retornoAtribuicaoUsuarioPaai.ShouldBeTrue();
            retornoAtribuicaoUsuarioPaai = await servicoAtribuicaoResponsavel.Executar(idPlano2, USUARIO_LOGIN_PAAI);
            retornoAtribuicaoUsuarioPaai.ShouldBeTrue();

            var retornoObterPlanoAlterado = ObterTodos<Dominio.PlanoAEE>();
            var usuarios = ObterTodos<Usuario>();
            retornoObterPlanoAlterado.ShouldNotBeNull();

            var paaiAtribuido = usuarios.FirstOrDefault(x => x.CodigoRf == USUARIO_LOGIN_PAAI);
            retornoObterPlanoAlterado.FirstOrDefault()!.ResponsavelPaaiId.ShouldBeEquivalentTo(paaiAtribuido!.Id);
            retornoObterPlanoAlterado.FirstOrDefault()!.Situacao.Equals(SituacaoPlanoAEE.Expirado).ShouldBeTrue();
            retornoObterPlanoAlterado.LastOrDefault()!.ResponsavelPaaiId.ShouldBeEquivalentTo(paaiAtribuido!.Id);
            retornoObterPlanoAlterado.LastOrDefault()!.Situacao.Equals(SituacaoPlanoAEE.Validado).ShouldBeTrue();

            var pendencias = ObterTodos<Pendencia>();
            pendencias.ShouldNotBeNull();
            pendencias.Count().ShouldBeEquivalentTo(qdadePendenciasOriginadasInclusao);

        }

        [Fact(DisplayName = "Plano AEE - Alterar o PAAI atribuído - A pendência deverá ser transferida para o novo PAAI")]
        public async Task Alterar_o_paai_atribuído()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCoordenadorCefai(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });
            
            var idPlano = await CriarPlanoAeePorSituacao(SituacaoPlanoAEE.ParecerPAAI);
            
            var filtroObter = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(idPlano,TURMA_CODIGO_1,1);
            
            var obterPlanoAeeUseCase =  ObterServicoObterPlanoAEEPorIdUseCase();
            var retornoObter = await obterPlanoAeeUseCase.Executar(filtroObter);
            retornoObter.ShouldNotBeNull();
            retornoObter.Situacao.ShouldBeEquivalentTo(SituacaoPlanoAEE.ParecerPAAI);

            var servicoAtribuicaoResponsavel = ObterServicoAtribuirResponsavelPlanoAEEUseCase();
            var retornoAtribuicaoUsuarioPaai = await servicoAtribuicaoResponsavel.Executar(retornoObter.Id,USUARIO_PAAI_LOGIN_3333333);
            retornoAtribuicaoUsuarioPaai.ShouldBeTrue();

            //Aterar o PAAI
            var retornoAtribuicaoUsuarioPaaiAlterar = await servicoAtribuicaoResponsavel.Executar(retornoObter.Id,USUARIO_LOGIN_PAAI);
            retornoAtribuicaoUsuarioPaaiAlterar.ShouldBeTrue();
            

            var pendencias = ObterTodos<Pendencia>();
            pendencias.ShouldNotBeNull();
            pendencias.Count(x => x.Tipo == TipoPendencia.AEE).ShouldBeEquivalentTo(3);
            
            pendencias.Count(x => x.Situacao == SituacaoPendencia.Pendente && !x.Excluido).ShouldBeEquivalentTo(1);
            pendencias.Count(x => x.Situacao == SituacaoPendencia.Pendente && x.Excluido).ShouldBeEquivalentTo(2);
            
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
            await AtualizarNaBase(plano);

            return salvar.PlanoId;
        }
    }
}