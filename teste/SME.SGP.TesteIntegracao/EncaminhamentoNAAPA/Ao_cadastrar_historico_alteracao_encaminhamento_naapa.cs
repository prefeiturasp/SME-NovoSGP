using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Commands;
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

            var secaoDto = new AtendimentoNAAPASecaoDto()
            {
                SecaoId = 1,
                Concluido = false,
                Questoes = new List<AtendimentoNAAPASecaoQuestaoDto>()
                {
                    new AtendimentoNAAPASecaoQuestaoDto()
                    {
                        QuestaoId= 1,
                        RespostaEncaminhamentoId=1,
                        Resposta = dataQueixa.AddDays(10).ToString("dd/MM/yyyy"),
                        TipoQuestao = TipoQuestao.Data
                    },
                    new AtendimentoNAAPASecaoQuestaoDto()
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

            await mediator.Send(new RegistrarHistoricoDeAlteracaoEncaminhamentoNAAPACommand(secaoDto, encaminhamento.Secoes.FirstOrDefault(), TipoHistoricoAlteracoesEncaminhamentoNAAPA.Alteracao));

            var historico = ObterTodos<EncaminhamentoNAAPAHistoricoAlteracoes>()?.FirstOrDefault();

            historico.ShouldNotBeNull();
            historico.CamposAlterados.ShouldBe("Data de entrada da queixa");
        }

        [Fact(DisplayName = "Encaminhamento NAAPA Histórico de alteração - cadastra o histórico ao alterar situação")]
        public async Task Ao_cadastrar_historico_alteracao_ao_alterar_situacao_do_encaminhamento_naapa()
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

            var encaminhamento = ObterTodos<Dominio.EncaminhamentoNAAPA>().FirstOrDefault();

            var mediator = ServiceProvider.GetService<IMediator>();

            await mediator.Send(new RegistrarHistoricoDeAlteracaoDaSituacaoDoEncaminhamentoNAAPACommand(encaminhamento, SituacaoNAAPA.EmAtendimento));

            var historico = ObterTodos<EncaminhamentoNAAPAHistoricoAlteracoes>()?.FirstOrDefault();

            historico.ShouldNotBeNull();

            historico.CamposAlterados.ShouldBe("Situação");
        }

        [Fact(DisplayName = "Encaminhamento NAAPA Histórico de alteração - cadastra o histórico ao excluir atendimento")]
        public async Task Ao_cadastrar_historico_alteracao_ao_excluir_atendimento_do_encaminhamento_naapa()
        {
            const long ENCAMINHAMENTO_NAAPA_SECAO_ID = 2;

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
            await GerarDadosEncaminhamentoNAAPAItinerario(dataQueixa);

            var mediator = ServiceProvider.GetService<IMediator>();

            await mediator.Send(new RegistrarHistoricoDeAlteracaoExclusaoAtendimentoEncaminhamentoNAAPACommad(ENCAMINHAMENTO_NAAPA_SECAO_ID));

            var historico = ObterTodos<EncaminhamentoNAAPAHistoricoAlteracoes>()?.FirstOrDefault();

            historico.ShouldNotBeNull();

            historico.DataAtendimento.ShouldBe(dataQueixa.ToString("dd/MM/yyyy"));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA Histórico de alteração - cadastra o histórico ao imprimir o encaminhamento")]
        public async Task Ao_cadastrar_historico_alteracao_ao_imprimir_o_encaminhamento_naapa()
        {
            const long ENCAMINHAMENTO_NAAPA_ID = 1;
            const long USUARIO_LOGADO_ID = 1;

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

            await CriarEncaminhamentoNAAPA();

            var mediator = ServiceProvider.GetService<IMediator>();

            var ids = new long[] { ENCAMINHAMENTO_NAAPA_ID };

            await mediator.Send(new RegistrarHistoricoDeAlteracaoDeImpressaoDoEncaminhamentoNAAPACommand(ids, USUARIO_LOGADO_ID));

            var historico = ObterTodos<EncaminhamentoNAAPAHistoricoAlteracoes>()?.FirstOrDefault();

            historico.ShouldNotBeNull();

            historico.EncaminhamentoNAAPAId.ShouldBe(ENCAMINHAMENTO_NAAPA_ID);
            historico.TipoHistorico.ShouldBe(TipoHistoricoAlteracoesEncaminhamentoNAAPA.Impressao);
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

        private async Task GerarDadosEncaminhamentoNAAPAItinerario(DateTime dataQueixa)
        {
            await CriarEncaminhamentoNAAPASecaoItinerario();
            await CriarQuestoesEncaminhamentoNAAPAItinerario();
            await CriarRespostasEncaminhamentoNAAPAItinerario(dataQueixa);
        }

        private async Task CriarEncaminhamentoNAAPASecaoItinerario()
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarQuestoesEncaminhamentoNAAPAItinerario()
        {
            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_DESCRICAO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarRespostasEncaminhamentoNAAPAItinerario(DateTime dataQueixa)
        {
            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 3,
                Texto = dataQueixa.ToString("yyyy-MM-ddT00:00:00"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 4,
                RespostaId = ID_ATENDIMENTO_NAO_PRESENCIAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 5,
                RespostaId = ID_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 6,
                Texto = "Descrição do atendimento",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
