using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.ServicosFakes;
using SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva
{
    public class Ao_cadastrar_editar_registro_acao_busca_ativa : RegistroAcaoBuscaAtivaTesteBase
    {
        
   
        public Ao_cadastrar_editar_registro_acao_busca_ativa(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            /*services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigoPorIdQuery, string>), typeof(ObterTurmaCodigoPorIdQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorCodigoEolQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));*/
        }

        [Fact(DisplayName = "Registro de Ação - Cadastrar")]
        public async Task Ao_cadastrar_registro_acao()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtro);

            /*var registrarEncaminhamentoNaapaUseCase = ObterServicoRegistrarEncaminhamento();

            var dataQueixa = DateTimeExtension.HorarioBrasilia().Date;
            
            var encaminhamentosNaapaDto = new EncaminhamentoNAAPADto()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno do naapa",
                Secoes = new List<EncaminhamentoNAAPASecaoDto>()
                {
                    new ()
                    {
                        SecaoId = 1,
                        Questoes = new List<EncaminhamentoNAAPASecaoQuestaoDto>()
                        {
                            new ()
                            {
                                QuestaoId = 1,
                                Resposta = dataQueixa.ToString("dd/MM/yyyy"),
                                TipoQuestao = TipoQuestao.Data
                            },
                            new ()
                            {
                                QuestaoId = 2,
                                Resposta = "1",
                                TipoQuestao = TipoQuestao.Combo
                            }
                        }
                    }
                }
            };

            var retorno = await registrarEncaminhamentoNaapaUseCase.Executar(encaminhamentosNaapaDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.AlteradoEm.HasValue.ShouldBeFalse();
            (retorno.Auditoria.CriadoEm.Year == dataQueixa.Year).ShouldBeTrue();
            
            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            encaminhamentoNAAPA.FirstOrDefault()?.Situacao.Equals(SituacaoNAAPA.Rascunho).ShouldBeTrue();
            encaminhamentoNAAPA.Count.ShouldBe(1);
            encaminhamentoNAAPA.FirstOrDefault()?.SituacaoMatriculaAluno.Equals(SITUACAO_MATRICULA_ALUNO_TURMA_ID_1).ShouldBeTrue();

            var encaminhamentoNAAPASecao = ObterTodos<EncaminhamentoNAAPASecao>();
            encaminhamentoNAAPASecao.ShouldNotBeNull();
            encaminhamentoNAAPASecao.FirstOrDefault()?.SecaoEncaminhamentoNAAPAId.ShouldBe(1);
            encaminhamentoNAAPASecao.FirstOrDefault()?.Concluido.ShouldBeTrue();
            
            var questaoEncaminhamentoNAAPA = ObterTodos<QuestaoEncaminhamentoNAAPA>();
            questaoEncaminhamentoNAAPA.ShouldNotBeNull();
            questaoEncaminhamentoNAAPA.Count.ShouldBe(2);
            questaoEncaminhamentoNAAPA.Any(a => a.QuestaoId == 1).ShouldBeTrue();
            questaoEncaminhamentoNAAPA.Any(a => a.QuestaoId == 2).ShouldBeTrue();

            var respostaEncaminhamentoNAAPA = ObterTodos<RespostaEncaminhamentoNAAPA>();
            respostaEncaminhamentoNAAPA.ShouldNotBeNull();
            respostaEncaminhamentoNAAPA.Count.ShouldBe(2);
            respostaEncaminhamentoNAAPA.Any(a => a.RespostaId == 1).ShouldBeTrue();
            respostaEncaminhamentoNAAPA.Any(a => a.Texto.Equals(dataQueixa.ToString("dd/MM/yyyy"))).ShouldBeTrue();*/
        }


        /*[Fact(DisplayName = "Registro de Ação - Editar")]
        public async Task Ao_editar_registro_acao()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var registrarEncaminhamentoNaapaUseCase = ObterServicoRegistrarEncaminhamento();

            var dataQueixa = DateTimeExtension.HorarioBrasilia().Date;
            dataQueixa.AddDays(-10);
            
            await GerarDadosEncaminhamentoNAAPA(dataQueixa);

            dataQueixa.AddDays(4);
            
            var encaminhamentosNaapaDto = new EncaminhamentoNAAPADto()
            {
                Id = 1,
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno do naapa",
                Secoes = new List<EncaminhamentoNAAPASecaoDto>()
                {
                    new ()
                    {
                        SecaoId = 1,
                        Questoes = new List<EncaminhamentoNAAPASecaoQuestaoDto>()
                        {
                            new ()
                            {
                                RespostaEncaminhamentoId = 1,
                                QuestaoId = 1,
                                Resposta = dataQueixa.ToString("dd/MM/yyyy"),
                                TipoQuestao = TipoQuestao.Data,
                                
                            },
                            new ()
                            {
                                RespostaEncaminhamentoId = 2,
                                QuestaoId = 2,
                                Resposta = "2",
                                TipoQuestao = TipoQuestao.Combo,
                                
                            }
                        }
                    }
                }
            };

            var encaminhamento = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            
            var retorno = await registrarEncaminhamentoNaapaUseCase.Executar(encaminhamentosNaapaDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            retorno.Auditoria.ShouldNotBeNull();
            retorno.Auditoria.AlteradoEm.HasValue.ShouldBeTrue();
            
            var encaminhamentoNAAPA = ObterTodos<Dominio.EncaminhamentoNAAPA>();
            encaminhamentoNAAPA.FirstOrDefault()?.Situacao.Equals(SituacaoNAAPA.Rascunho).ShouldBeTrue();
            encaminhamentoNAAPA.Count.ShouldBe(1);
            encaminhamentoNAAPA.FirstOrDefault()?.SituacaoMatriculaAluno.Equals(SITUACAO_MATRICULA_ALUNO_FIXA).ShouldBeTrue();

            var encaminhamentoNAAPASecao = ObterTodos<EncaminhamentoNAAPASecao>();
            encaminhamentoNAAPASecao.ShouldNotBeNull();
            encaminhamentoNAAPASecao.FirstOrDefault().SecaoEncaminhamentoNAAPAId.ShouldBe(1);
            
            var questaoEncaminhamentoNAAPA = ObterTodos<QuestaoEncaminhamentoNAAPA>();
            questaoEncaminhamentoNAAPA.ShouldNotBeNull();
            questaoEncaminhamentoNAAPA.Count.ShouldBe(2);
            questaoEncaminhamentoNAAPA.Any(a=> a.QuestaoId == 1).ShouldBeTrue();
            questaoEncaminhamentoNAAPA.Any(a=> a.QuestaoId == 2).ShouldBeTrue();
            
            var respostaEncaminhamentoNAAPA = ObterTodos<RespostaEncaminhamentoNAAPA>();
            respostaEncaminhamentoNAAPA.ShouldNotBeNull();
            respostaEncaminhamentoNAAPA.Count.ShouldBe(2);
            respostaEncaminhamentoNAAPA.Any(a=> a.RespostaId == 2).ShouldBeTrue();
            respostaEncaminhamentoNAAPA.Any(a=> a.Texto.Equals(dataQueixa.ToString("dd/MM/yyyy"))).ShouldBeTrue();
        }*/

    }
}

