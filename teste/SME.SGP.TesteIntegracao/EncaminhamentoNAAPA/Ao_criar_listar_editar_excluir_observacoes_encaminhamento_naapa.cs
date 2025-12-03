using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Infra;
using Xunit;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.ServicosFakes;
using Shouldly;
using SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.ServicosFake;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_criar_listar_editar_excluir_observacoes_encaminhamento_naapa : EncaminhamentoNAAPATesteBase
    {
        public Ao_criar_listar_editar_excluir_observacoes_encaminhamento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigoPorIdQuery, string>), typeof(ObterTurmaCodigoPorIdQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorCodigoEolQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery, EncaminhamentoNAAPAHistoricoAlteracoes>), typeof(ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Criar ,Listar ,Atualizar e Excluir Observações do Encaminhamento NAAPA")]
        public async Task Ao_criar_listar_editar_observacoes_encaminhamento_naaapa()
        {
            var encaminhamentoExistente = await CriarEncaminhamentoNAAPA();
            var useCaseListar = ObterObservacoesDeEncaminhamentoNAAPA();
            var useCaseSalvar = SalvarObservacoesDeEncaminhamentoNAAPA();
            var useCaseExcluir = ExcluirObservacoesDeEncaminhamentoNAAPA();

            var primeiraObservacao = new AtendimentoNAAPAObservacaoSalvarDto
            {
                EncaminhamentoNAAPAId = encaminhamentoExistente.Id,
                Observacao = "Primeira Observacao"
            };
            var segundaObservacao = new AtendimentoNAAPAObservacaoSalvarDto
            {
                EncaminhamentoNAAPAId = encaminhamentoExistente.Id,
                Observacao = "Segunda Observacao"
            };
            await useCaseSalvar.Executar(primeiraObservacao);
            await useCaseSalvar.Executar(segundaObservacao);

            var listaObservacoes = await useCaseListar.Executar(encaminhamentoExistente.Id);

            listaObservacoes.Items.Count().ShouldBeEquivalentTo(2);
            listaObservacoes.Items.Count(x => x.Observacao == primeiraObservacao.Observacao).ShouldBeEquivalentTo(1);
            listaObservacoes.Items.Count(x => x.Observacao == segundaObservacao.Observacao).ShouldBeEquivalentTo(1);

            var obterObservacaoParaAtualizar = listaObservacoes.Items.FirstOrDefault(x => x.Observacao == primeiraObservacao.Observacao);

            var observacaoAtualizar = new AtendimentoNAAPAObservacaoSalvarDto
            {
                Observacao = "Primeira Observacao Atualizada",
                Id = obterObservacaoParaAtualizar.Id,
                EncaminhamentoNAAPAId = encaminhamentoExistente.Id,
            };

            await useCaseSalvar.Executar(observacaoAtualizar);


            var listaObservacoesAtualizada = await useCaseListar.Executar(encaminhamentoExistente.Id);

            listaObservacoesAtualizada.Items.Count().ShouldBeEquivalentTo(2);
            listaObservacoesAtualizada.Items.Count(x => x.Observacao == observacaoAtualizar.Observacao).ShouldBeEquivalentTo(1);
            listaObservacoesAtualizada.Items.Count(x => x.Observacao == segundaObservacao.Observacao).ShouldBeEquivalentTo(1);


            var observacaoParaExclusao = listaObservacoesAtualizada.Items.FirstOrDefault(x => x.Observacao == observacaoAtualizar.Observacao);
            await useCaseExcluir.Executar(observacaoParaExclusao.Id);
            var listaObservacoesAposExclusao = await useCaseListar.Executar(encaminhamentoExistente.Id);
            listaObservacoesAposExclusao.Items.Count().ShouldBeEquivalentTo(1);

        }

        private async Task<ResultadoAtendimentoNAAPADto> CriarEncaminhamentoNAAPA()
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

            var encaminhamentosNaapaDto = new AtendimentoNAAPADto()
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
            return retorno;
        }
    }
}
