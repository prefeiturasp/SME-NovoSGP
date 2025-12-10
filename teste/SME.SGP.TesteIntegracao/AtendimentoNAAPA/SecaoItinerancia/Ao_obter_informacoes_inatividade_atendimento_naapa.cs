using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.SecaoItinerancia
{
    //public class Ao_obter_informacoes_inatividade_atendimento_naapa : AtendimentoNAAPATesteBase
    //{
    //    private const long ENCAMINHAMENTO_ID_1 = 1;
    //    private const long ENCAMINHAMENTO_ID_2 = 2;
    //    private const long ENCAMINHAMENTO_SECAO_ID_1 = 1;
    //    private const long ENCAMINHAMENTO_SECAO_ID_2 = 2;
    //    private const long ENCAMINHAMENTO_SECAO_ID_3 = 3;
    //    private const long ENCAMINHAMENTO_SECAO_ID_4 = 4;
    //    private const long RESPOSTA_ITINERANCIA_ID_1 = 1;
    //    private const long RESPOSTA_ITINERANCIA_ID_3 = 3;
    //    private const long RESPOSTA_ITINERANCIA_ID_7 = 7;
    //    private const long RESPOSTA_ITINERANCIA_ID_9 = 9;
    //    public Ao_obter_informacoes_inatividade_atendimento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
    //    {
    //    }

    //    [Fact(DisplayName = "Encaminhamento NAAPA - Obter informações de inatividade sem atendimento e sem notificação")]
    //    public async Task Ao_obter_informacoes_inatividade_sem_atendimento_sem_notificacao()
    //    {
    //        var filtroNAAPA = new FiltroNAAPADto()
    //        {
    //            Perfil = ObterPerfilCP(),
    //            TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
    //            Modalidade = Modalidade.Fundamental,
    //            AnoTurma = ANO_8,
    //            DreId = DRE_ID_1,
    //            CodigoUe = UE_CODIGO_1,
    //            TurmaId = TURMA_ID_1,
    //            Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
    //            Prioridade = NORMAL
    //        };
    //        await CriarDadosBase(filtroNAAPA);

    //        var mediator = ServiceProvider.GetService<IMediator>();
    //        var dataCriacao = DateTimeExtension.HorarioBrasilia().Date;

    //        await GerarDadosEncaminhamentoNAAPA(dataCriacao, ENCAMINHAMENTO_ID_1);

    //        dataCriacao = dataCriacao.AddDays(-40);
    //        await GerarDadosEncaminhamentoNAAPA(
    //                            dataCriacao,
    //                            ENCAMINHAMENTO_ID_2,
    //                            ENCAMINHAMENTO_SECAO_ID_2,
    //                            RESPOSTA_ITINERANCIA_ID_3);

    //        var informacoes = await mediator.Send(new ObterAtendimentosNAAPAComInatividadeDeAtendimentoQuery(UE_ID_1));

    //        informacoes.ShouldNotBeNull();
    //        informacoes.Count().ShouldBe(1);
    //        var informacao = informacoes.FirstOrDefault();
    //        informacao.ShouldNotBeNull();
    //        informacao.EncaminhamentoId.ShouldBe(ENCAMINHAMENTO_ID_2);
    //        informacao.TurmaId.ShouldBe(TURMA_ID_1);
    //        informacao.AlunoCodigo.ShouldBe(ALUNO_CODIGO_1);
    //    }

    //    [Fact(DisplayName = "Encaminhamento NAAPA - Obter informações de inatividade sem atendimento e com notificação")]
    //    public async Task Ao_obter_informacoes_inatividade_sem_atendimento_com_notificacao()
    //    {
    //        var filtroNAAPA = new FiltroNAAPADto()
    //        {
    //            Perfil = ObterPerfilCP(),
    //            TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
    //            Modalidade = Modalidade.Fundamental,
    //            AnoTurma = ANO_8,
    //            DreId = DRE_ID_1,
    //            CodigoUe = UE_CODIGO_1,
    //            TurmaId = TURMA_ID_1,
    //            Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
    //            Prioridade = NORMAL
    //        };
    //        await CriarDadosBase(filtroNAAPA);

    //        var mediator = ServiceProvider.GetService<IMediator>();
    //        var dataCriacao = DateTimeExtension.HorarioBrasilia().Date;

    //        await GerarDadosEncaminhamentoNAAPA(
    //                        dataCriacao,
    //                        ENCAMINHAMENTO_ID_1,
    //                        ENCAMINHAMENTO_SECAO_ID_1,
    //                        RESPOSTA_ITINERANCIA_ID_1,
    //                        dataCriacao.AddDays(-40));

    //        await GerarDadosEncaminhamentoNAAPA(
    //                        dataCriacao,
    //                        ENCAMINHAMENTO_ID_2,
    //                        ENCAMINHAMENTO_SECAO_ID_2,
    //                        RESPOSTA_ITINERANCIA_ID_3);

    //        var informacoes = await mediator.Send(new ObterAtendimentosNAAPAComInatividadeDeAtendimentoQuery(UE_ID_1));

    //        informacoes.ShouldNotBeNull();
    //        informacoes.Count().ShouldBe(1);
    //        var informacao = informacoes.FirstOrDefault();
    //        informacao.ShouldNotBeNull();
    //        informacao.EncaminhamentoId.ShouldBe(ENCAMINHAMENTO_ID_1);
    //        informacao.TurmaId.ShouldBe(TURMA_ID_1);
    //        informacao.AlunoCodigo.ShouldBe(ALUNO_CODIGO_1);
    //    }

    //    [Fact(DisplayName = "Encaminhamento NAAPA - Obter informações de inatividade com atendimento e sem notificação")]
    //    public async Task Ao_obter_informacoes_inatividade_com_atendimento_sem_notificacao()
    //    {
    //        var filtroNAAPA = new FiltroNAAPADto()
    //        {
    //            Perfil = ObterPerfilCP(),
    //            TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
    //            Modalidade = Modalidade.Fundamental,
    //            AnoTurma = ANO_8,
    //            DreId = DRE_ID_1,
    //            CodigoUe = UE_CODIGO_1,
    //            TurmaId = TURMA_ID_1,
    //            Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
    //            Prioridade = NORMAL
    //        };
    //        await CriarDadosBase(filtroNAAPA);

    //        var mediator = ServiceProvider.GetService<IMediator>();
    //        var dataCriacao = DateTimeExtension.HorarioBrasilia().Date;

    //        await GerarDadosEncaminhamentoNAAPA(dataCriacao, ENCAMINHAMENTO_ID_1);
    //        await CriarEncaminhamentoNAAPASecao(ENCAMINHAMENTO_ID_1, ID_QUESTIONARIO_NAAPA_ITINERANCIA);
    //        await CriarQuestoesEncaminhamentoNAAPAItinerario(ENCAMINHAMENTO_SECAO_ID_2);
    //        await CriarRespostasEncaminhamentoNAAPAItinerario(dataCriacao, RESPOSTA_ITINERANCIA_ID_3);

    //        await GerarDadosEncaminhamentoNAAPA(dataCriacao, ENCAMINHAMENTO_ID_2, ENCAMINHAMENTO_SECAO_ID_3, RESPOSTA_ITINERANCIA_ID_7);
    //        await CriarEncaminhamentoNAAPASecao(ENCAMINHAMENTO_ID_2, ID_QUESTIONARIO_NAAPA_ITINERANCIA);
    //        await CriarQuestoesEncaminhamentoNAAPAItinerario(ENCAMINHAMENTO_SECAO_ID_4);
    //        await CriarRespostasEncaminhamentoNAAPAItinerario(dataCriacao.AddDays(-40), RESPOSTA_ITINERANCIA_ID_9);

    //        var informacoes = await mediator.Send(new ObterAtendimentosNAAPAComInatividadeDeAtendimentoQuery(UE_ID_1));

    //        informacoes.ShouldNotBeNull();
    //        informacoes.Count().ShouldBe(1);
    //        var informacao = informacoes.FirstOrDefault();
    //        informacao.ShouldNotBeNull();
    //        informacao.EncaminhamentoId.ShouldBe(ENCAMINHAMENTO_ID_2);
    //        informacao.TurmaId.ShouldBe(TURMA_ID_1);
    //        informacao.AlunoCodigo.ShouldBe(ALUNO_CODIGO_1);
    //    }


    //    [Fact(DisplayName = "Encaminhamento NAAPA - Obter informações de inatividade com atendimento e com notificação")]
    //    public async Task Ao_obter_informacoes_inatividade_com_atendimento_com_notificacao()
    //    {
    //        var filtroNAAPA = new FiltroNAAPADto()
    //        {
    //            Perfil = ObterPerfilCP(),
    //            TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
    //            Modalidade = Modalidade.Fundamental,
    //            AnoTurma = ANO_8,
    //            DreId = DRE_ID_1,
    //            CodigoUe = UE_CODIGO_1,
    //            TurmaId = TURMA_ID_1,
    //            Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
    //            Prioridade = NORMAL
    //        };
    //        await CriarDadosBase(filtroNAAPA);

    //        var mediator = ServiceProvider.GetService<IMediator>();
    //        var dataCriacao = DateTimeExtension.HorarioBrasilia().Date;

    //        await GerarDadosEncaminhamentoNAAPA(
    //                        dataCriacao,
    //                        ENCAMINHAMENTO_ID_1,
    //                        ENCAMINHAMENTO_SECAO_ID_1,
    //                        RESPOSTA_ITINERANCIA_ID_1,
    //                        dataCriacao.AddDays(-40));
    //        await CriarEncaminhamentoNAAPASecao(ENCAMINHAMENTO_ID_1, ID_QUESTIONARIO_NAAPA_ITINERANCIA);
    //        await CriarQuestoesEncaminhamentoNAAPAItinerario(ENCAMINHAMENTO_SECAO_ID_2);
    //        await CriarRespostasEncaminhamentoNAAPAItinerario(dataCriacao, RESPOSTA_ITINERANCIA_ID_3);

    //        await GerarDadosEncaminhamentoNAAPA(
    //                        dataCriacao,
    //                        ENCAMINHAMENTO_ID_2,
    //                        ENCAMINHAMENTO_SECAO_ID_3,
    //                        RESPOSTA_ITINERANCIA_ID_7);
    //        await CriarEncaminhamentoNAAPASecao(ENCAMINHAMENTO_ID_2, ID_QUESTIONARIO_NAAPA_ITINERANCIA);
    //        await CriarQuestoesEncaminhamentoNAAPAItinerario(ENCAMINHAMENTO_SECAO_ID_4);
    //        await CriarRespostasEncaminhamentoNAAPAItinerario(dataCriacao, RESPOSTA_ITINERANCIA_ID_9);

    //        var informacoes = await mediator.Send(new ObterAtendimentosNAAPAComInatividadeDeAtendimentoQuery(UE_ID_1));

    //        informacoes.ShouldNotBeNull();
    //        informacoes.Count().ShouldBe(1);
    //        var informacao = informacoes.FirstOrDefault();
    //        informacao.ShouldNotBeNull();
    //        informacao.EncaminhamentoId.ShouldBe(ENCAMINHAMENTO_ID_1);
    //        informacao.TurmaId.ShouldBe(TURMA_ID_1);
    //        informacao.AlunoCodigo.ShouldBe(ALUNO_CODIGO_1);
    //    }

    //    [Theory(DisplayName = "Encaminhamento NAAPA - Obter informações de inatividade por situação indevida")]
    //    [InlineData(SituacaoNAAPA.Rascunho)]
    //    [InlineData(SituacaoNAAPA.Encerrado)]
    //    public async Task Ao_obter_informacoes_inatividade_de_atendimento_encaminhamento_por_situacao_indevida(SituacaoNAAPA situacao)
    //    {
    //        var filtroNAAPA = new FiltroNAAPADto()
    //        {
    //            Perfil = ObterPerfilCP(),
    //            TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
    //            Modalidade = Modalidade.Fundamental,
    //            AnoTurma = ANO_8,
    //            DreId = DRE_ID_1,
    //            CodigoUe = UE_CODIGO_1,
    //            TurmaId = TURMA_ID_1,
    //            Situacao = (int)situacao,
    //            Prioridade = NORMAL
    //        };
    //        await CriarDadosBase(filtroNAAPA);

    //        var mediator = ServiceProvider.GetService<IMediator>();
    //        var dataCriacao = DateTimeExtension.HorarioBrasilia().Date;

    //        await GerarDadosEncaminhamentoNAAPA(
    //                            dataCriacao.AddDays(-40),
    //                            ENCAMINHAMENTO_ID_1,
    //                            ENCAMINHAMENTO_SECAO_ID_1,
    //                            RESPOSTA_ITINERANCIA_ID_1,
    //                            null,
    //                            situacao);

    //        var informacoes = await mediator.Send(new ObterAtendimentosNAAPAComInatividadeDeAtendimentoQuery(UE_ID_1));
    //        informacoes.ShouldNotBeNull();
    //        informacoes.Count().ShouldBe(0);
    //    }

    //    private async Task GerarDadosEncaminhamentoNAAPA(
    //                            DateTime dataCriacao,
    //                            long encaminhamentoId,
    //                            long encaminhamentoSecaoId = 1,
    //                            long encaminhamentoResposta = 1,
    //                            DateTime? dataUltimaNotificacao = null,
    //                            SituacaoNAAPA situacao = SituacaoNAAPA.EmAtendimento)
    //    {
    //        await CriarEncaminhamentoNAAPA(dataCriacao, dataUltimaNotificacao, situacao);
    //        await CriarEncaminhamentoNAAPASecao(encaminhamentoId, encaminhamentoSecaoId);
    //        await CriarQuestoesEncaminhamentoNAAPA(encaminhamentoSecaoId);
    //        await CriarRespostasEncaminhamentoNAAPA(dataCriacao, encaminhamentoResposta);
    //    }

    //    private async Task CriarQuestoesEncaminhamentoNAAPA(long encaminhamentoSecaoId)
    //    {
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = encaminhamentoSecaoId,
    //            QuestaoId = ID_QUESTAO_DATA_ENTRADA_QUEIXA,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = encaminhamentoSecaoId,
    //            QuestaoId = ID_QUESTAO_PRIORIDADE,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //    }

    //    private async Task CriarQuestoesEncaminhamentoNAAPAItinerario(long encaminhamentoSecaoId)
    //    {
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = encaminhamentoSecaoId,
    //            QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = encaminhamentoSecaoId,
    //            QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = encaminhamentoSecaoId,
    //            QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = encaminhamentoSecaoId,
    //            QuestaoId = ID_QUESTAO_DESCRICAO_ATENDIMENTO,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //    }

    //    private async Task CriarRespostasEncaminhamentoNAAPAItinerario(DateTime dataAtendimento, long questaoId)
    //    {
    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = questaoId,
    //            Texto = dataAtendimento.ToString("yyyy/MM/dd"),
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //        questaoId += 1;
    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = questaoId,
    //            RespostaId = ID_ATENDIMENTO_NAO_PRESENCIAL,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //        questaoId += 1;
    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = questaoId,
    //            RespostaId = ID_ACOES_LUDICAS,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //        questaoId += 1;
    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = questaoId,
    //            Texto = "Descrição do atendimento",
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //    }

    //    private async Task CriarEncaminhamentoNAAPASecao(long encaminhamentoId, long secaoEncaminhamentoId)
    //    {
    //        await InserirNaBase(new EncaminhamentoNAAPASecao()
    //        {
    //            EncaminhamentoNAAPAId = encaminhamentoId,
    //            SecaoEncaminhamentoNAAPAId = secaoEncaminhamentoId,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //    }

    //    private async Task CriarEncaminhamentoNAAPA(DateTime dataCriacao, DateTime? dataUltimaNotificacao, SituacaoNAAPA situacao)
    //    {
    //        await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
    //        {
    //            TurmaId = TURMA_ID_1,
    //            AlunoCodigo = ALUNO_CODIGO_1,
    //            Situacao = situacao,
    //            AlunoNome = "Nome do aluno 1",
    //            DataUltimaNotificacaoSemAtendimento = dataUltimaNotificacao,
    //            CriadoEm = dataCriacao,
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //    }

    //    private async Task CriarRespostasEncaminhamentoNAAPA(DateTime dataQueixa, long questaoEncaminhamentoId)
    //    {
    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = questaoEncaminhamentoId,
    //            Texto = dataQueixa.ToString("yyyy/MM/dd"),
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = questaoEncaminhamentoId + 1,
    //            Texto = "1",
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //    }
    //}
}
