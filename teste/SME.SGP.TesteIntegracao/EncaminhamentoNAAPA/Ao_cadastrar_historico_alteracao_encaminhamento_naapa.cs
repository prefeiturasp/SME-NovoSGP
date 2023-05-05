using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_cadastrar_historico_alteracao_encaminhamento_naapa : EncaminhamentoNAAPATesteBase
    {
        public Ao_cadastrar_historico_alteracao_encaminhamento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Encaminhamento NAAPA Histórico de alteração - cadastra o histórico para seção")]
        public async Task Ao_cadastrar_historico_alteracao_secao()
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

            var dataQueixa = DateTimeExtension.HorarioBrasilia().Date;
            
            await GerarDadosEncaminhamentoNAAPA(dataQueixa);

            var secaoDto = new EncaminhamentoNAAPASecaoDto()
            {
                SecaoId = 1,
                Concluido = false,
                Questoes = new List<EncaminhamentoNAAPASecaoQuestaoDto>()
                {
                    new EncaminhamentoNAAPASecaoQuestaoDto()
                    {
                        QuestaoId= 1,
                        RespostaEncaminhamentoId=1,
                        Resposta = dataQueixa.AddDays(10).ToString("dd/MM/yyyy"),
                        TipoQuestao = TipoQuestao.Data
                    },
                    new EncaminhamentoNAAPASecaoQuestaoDto()
                    {
                        QuestaoId = 2,
                        RespostaEncaminhamentoId= 2,
                        Resposta = "1",
                        TipoQuestao = TipoQuestao.Texto
                    }
                }
            };

            var encaminhamento = ObterTodos<Dominio.EncaminhamentoNAAPA>().FirstOrDefault();
            encaminhamento.Secoes = ObterTodos<EncaminhamentoNAAPASecao>();
            encaminhamento.Secoes.FirstOrDefault().SecaoEncaminhamentoNAAPA = ObterTodos<SecaoEncaminhamentoNAAPA>().FirstOrDefault(secao => secao.Id == 1);
            encaminhamento.Secoes.FirstOrDefault().Questoes = ObterTodos<QuestaoEncaminhamentoNAAPA>();
            var repostas = ObterTodos<RespostaEncaminhamentoNAAPA>();
            var questoes = ObterTodos<Questao>();

            foreach (var questao in encaminhamento.Secoes.FirstOrDefault().Questoes)
            {
                questao.Questao = questoes.Find(s => s.Id == questao.QuestaoId);
                questao.Respostas = repostas.FindAll(resposta => resposta.QuestaoEncaminhamentoId == questao.Id);
            }

            var mediator = ServiceProvider.GetService<IMediator>();

            await mediator.Send(new RegistrarHistoricoDeAlteracaoEncaminhamentoNAAPACommand(secaoDto, encaminhamento));

            var historico = ObterTodos<EncaminhamentoNAAPAHistoricoAlteracoes>()?.FirstOrDefault();

            historico.ShouldNotBeNull();
            historico.CamposAlterados.ShouldBe("Data de entrada da queixa");
        }

        private async Task GerarDadosEncaminhamentoNAAPA(DateTime dataQueixa)
        {
            await CriarEncaminhamentoNAAPA();
            await CriarEncaminhamentoNAAPASecao();
            await CriarQuestoesEncaminhamentoNAAPA();
            await CriarRespostasEncaminhamentoNAAPA(dataQueixa);
        }

        private async Task CriarRespostasEncaminhamentoNAAPA(DateTime dataQueixa)
        {
            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarQuestoesEncaminhamentoNAAPA()
        {
            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPASecao()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPA()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.Rascunho,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                SituacaoMatriculaAluno = SituacaoMatriculaAluno.Concluido
            });
        }
    }
}
