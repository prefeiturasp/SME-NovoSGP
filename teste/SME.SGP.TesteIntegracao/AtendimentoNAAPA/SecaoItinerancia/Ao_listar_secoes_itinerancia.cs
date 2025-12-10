using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.SecaoItinerancia
{
    //public class Ao_listar_secoes_itinerancia : AtendimentoNAAPATesteBase
    //{
    //    public Ao_listar_secoes_itinerancia(CollectionFixture collectionFixture) : base(collectionFixture)
    //    {
    //    }

    //    protected override void RegistrarFakes(IServiceCollection services)
    //    {
    //        base.RegistrarFakes(services);
    //    }

    //    [Fact(DisplayName = "Encaminhamento NAAPA - Retornar seções de itinerância de encaminhamento NAAPA pré definido")]
    //    public async Task Deve_retornar_registros_secoes_itinerancia()
    //    {
    //        var filtroNAAPA = new FiltroNAAPADto()
    //        {
    //            Perfil = ObterPerfilCP(),
    //            TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
    //            Modalidade = Modalidade.Fundamental,
    //            AnoTurma = "8",
    //            DreId = 1,
    //            CodigoUe = "1",
    //            TurmaId = TURMA_ID_1,
    //            Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
    //            Prioridade = NORMAL
    //        };

    //        await CriarDadosBase(filtroNAAPA);


    //        var obterSecoesItineranciaEncaminhamentoNaapaUseCase = ObterServicoListagemSecoesItineranciaEncaminhamentoNaapa();
    //        await GerarDadosEncaminhamentoNAAPA(DateTimeExtension.HorarioBrasilia().Date);

    //        var retorno = await obterSecoesItineranciaEncaminhamentoNaapaUseCase.Executar(1);

    //        retorno.ShouldNotBeNull("Nenhuma seção de itinerância registrada para o Encaminhamento NAAPA");
    //        retorno.Items.Count().ShouldBe(2, "Qdade registros seções itinerância Encaminhamento NAAPA inválida");

    //        var anexos = new string[] { "Arquivo 1 Itinerância NAAPA 1", "Arquivo 2 Itinerância NAAPA 1" };
    //        var primeiraSecaoItinerancia = retorno.Items.OrderBy(secao => secao.Auditoria.Id).FirstOrDefault();
    //        primeiraSecaoItinerancia.ShouldNotBeNull("1º Seção de itinerância não registrada para o Encaminhamento NAAPA");
    //        primeiraSecaoItinerancia.DataAtendimento.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
    //        primeiraSecaoItinerancia.TipoAtendimento.ShouldBe("Itinerância");
    //        primeiraSecaoItinerancia.Auditoria.CriadoPor.ShouldNotBeNull("Usuário criador não preenchido em auditoria 1º Seção de itinerância");
    //        primeiraSecaoItinerancia.Arquivos.Count().ShouldBe(2);
    //        primeiraSecaoItinerancia.Arquivos.All(arq => anexos.Contains(arq.Nome)).ShouldBe(true);

    //        anexos = new string[] { "Arquivo 1 Itinerância NAAPA 2", "Arquivo 2 Itinerância NAAPA 2" };
    //        var segundaSecaoItinerancia = retorno.Items.OrderBy(secao => secao.Auditoria.Id).LastOrDefault();
    //        segundaSecaoItinerancia.ShouldNotBeNull("2º Seção de itinerância não registrada para o Encaminhamento NAAPA");
    //        segundaSecaoItinerancia.DataAtendimento.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.AddDays(4));
    //        segundaSecaoItinerancia.TipoAtendimento.ShouldBe("Grupo de Trabalho NAAPA");
    //        segundaSecaoItinerancia.Auditoria.CriadoPor.ShouldNotBeNull("Usuário criador não preenchido em auditoria 2º Seção de itinerância");
    //        segundaSecaoItinerancia.Arquivos.Count().ShouldBe(2);
    //        segundaSecaoItinerancia.Arquivos.All(arq => anexos.Contains(arq.Nome)).ShouldBe(true);
    //    }



    //    private async Task GerarDadosEncaminhamentoNAAPA(DateTime dataQueixa)
    //    {
    //        await CriarEncaminhamentoNAAPA();
    //        await CriarEncaminhamentoNAAPASecao();
    //        await CriarQuestoesEncaminhamentoNAAPA();
    //        await CriarQuestoesAnexoItineranciaEncaminhamentoNAAPA();
    //        await CriarRespostasEncaminhamentoNAAPA(dataQueixa);
    //        await CriarRespostasAnexoItineranciaEncaminhamentoNAAPA();
    //    }

    //    private async Task CriarRespostasAnexoItineranciaEncaminhamentoNAAPA()
    //    {
    //        //Anexos - Itinerancia 1
    //        await InserirNaBase(new Arquivo()
    //        {
    //            Codigo = Guid.NewGuid(),
    //            Nome = $"Arquivo 1 Itinerância NAAPA 1",
    //            CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF,
    //            TipoConteudo = "application/pdf",
    //            Tipo = TipoArquivo.ItineranciaAtendimentoNAAPA
    //        });
    //        await InserirNaBase(new Arquivo()
    //        {
    //            Codigo = Guid.NewGuid(),
    //            Nome = $"Arquivo 2 Itinerância NAAPA 1",
    //            CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF,
    //            TipoConteudo = "application/pdf",
    //            Tipo = TipoArquivo.ItineranciaAtendimentoNAAPA
    //        });

    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 13,
    //            Texto = "",
    //            ArquivoId = 1,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 13,
    //            Texto = "",
    //            ArquivoId = 2,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //Anexos - Itinerancia 2
    //        await InserirNaBase(new Arquivo()
    //        {
    //            Codigo = Guid.NewGuid(),
    //            Nome = $"Arquivo 1 Itinerância NAAPA 2",
    //            CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF,
    //            TipoConteudo = "application/pdf",
    //            Tipo = TipoArquivo.ItineranciaAtendimentoNAAPA
    //        });
    //        await InserirNaBase(new Arquivo()
    //        {
    //            Codigo = Guid.NewGuid(),
    //            Nome = $"Arquivo 2 Itinerância NAAPA 2",
    //            CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF,
    //            TipoConteudo = "application/pdf",
    //            Tipo = TipoArquivo.ItineranciaAtendimentoNAAPA
    //        });

    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 14,
    //            Texto = "",
    //            ArquivoId = 3,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 14,
    //            Texto = "",
    //            ArquivoId = 4,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //    }

    //    private async Task CriarRespostasEncaminhamentoNAAPA(DateTime dataQueixa)
    //    {
    //        //Informações estudante
    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 1,
    //            Texto = dataQueixa.ToString("dd/MM/yyyy"),
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 2,
    //            Texto = "1",
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //itinerancia 1
    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 3,
    //            Texto = DateTimeExtension.HorarioBrasilia().Date.ToString("yyyy/MM/dd"),
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 4,
    //            Texto = "",
    //            RespostaId = ID_OPCAO_RESPOSTA_ATENDIMENTO_NAO_PRESENCIAL,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 5,
    //            Texto = "",
    //            RespostaId = ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 6,
    //            Texto = "Teste Descricao Atendimento",
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 7,
    //            Texto = "Teste Descricao Procedimento de Trabalho",
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //itinerancia 2
    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 8,
    //            Texto = DateTimeExtension.HorarioBrasilia().Date.AddDays(4).ToString("yyyy/MM/dd"),
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 9,
    //            Texto = "",
    //            RespostaId = ID_OPCAO_RESPOSTA_GRUPO_TRAB_NAAPA,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 10,
    //            Texto = "",
    //            RespostaId = ID_OPCAO_RESPOSTA_OUTRO_PROCEDIMENTO,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 11,
    //            Texto = "Teste Descricao Atendimento 02",
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        await InserirNaBase(new RespostaEncaminhamentoNAAPA()
    //        {
    //            QuestaoEncaminhamentoId = 12,
    //            Texto = "Teste Descricao Procedimento de Trabalho 02",
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //    }

    //    private async Task CriarQuestoesAnexoItineranciaEncaminhamentoNAAPA()
    //    {
    //        //Id 13 - Itinerancia 1
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = 2,
    //            QuestaoId = ID_QUESTAO_ANEXOS_ITINERANCIA,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //        //Id 14 - Itinerancia 2
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = 3,
    //            QuestaoId = ID_QUESTAO_ANEXOS_ITINERANCIA,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //    }
    //    private async Task CriarQuestoesEncaminhamentoNAAPA()
    //    {
    //        //informações estudante
    //        //Id 1
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = 1,
    //            QuestaoId = 1,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //Id 2
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = 1,
    //            QuestaoId = 2,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //itinerancia 1
    //        //Id 3
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = 2,
    //            QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //Id 4
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = 2,
    //            QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //Id 5
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = 2,
    //            QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //Id 6
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = 2,
    //            QuestaoId = ID_QUESTAO_DESCRICAO_ATENDIMENTO,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //Id 7
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = 2,
    //            QuestaoId = ID_QUESTAO_DESCRICAO_PROCEDIMENTO_TRABALHO,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //itinerancia 2
    //        //Id 8
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = 3,
    //            QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //Id 9
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = 3,
    //            QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //Id 10
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = 3,
    //            QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //Id 11
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = 3,
    //            QuestaoId = ID_QUESTAO_DESCRICAO_ATENDIMENTO,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //Id 12
    //        await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
    //        {
    //            EncaminhamentoNAAPASecaoId = 3,
    //            QuestaoId = ID_QUESTAO_DESCRICAO_PROCEDIMENTO_TRABALHO,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //    }

    //    private async Task CriarEncaminhamentoNAAPASecao()
    //    {
    //        //Id 1
    //        await InserirNaBase(new EncaminhamentoNAAPASecao()
    //        {
    //            EncaminhamentoNAAPAId = 1,
    //            SecaoEncaminhamentoNAAPAId = ID_SECAO_ENCAMINHAMENTO_NAAPA_INFORMACOES_ESTUDANTE,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //Id 2
    //        await InserirNaBase(new EncaminhamentoNAAPASecao()
    //        {
    //            EncaminhamentoNAAPAId = 1,
    //            SecaoEncaminhamentoNAAPAId = ID_SECAO_ENCAMINHAMENTO_NAAPA_ITINERANCIA,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });

    //        //Id 3
    //        await InserirNaBase(new EncaminhamentoNAAPASecao()
    //        {
    //            EncaminhamentoNAAPAId = 1,
    //            SecaoEncaminhamentoNAAPAId = ID_SECAO_ENCAMINHAMENTO_NAAPA_ITINERANCIA,
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //    }

    //    private async Task CriarEncaminhamentoNAAPA()
    //    {
    //        await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
    //        {
    //            TurmaId = TURMA_ID_1,
    //            AlunoCodigo = ALUNO_CODIGO_1,
    //            Situacao = SituacaoNAAPA.AguardandoAtendimento,
    //            AlunoNome = "Nome do aluno 1",
    //            CriadoEm = DateTimeExtension.HorarioBrasilia(),
    //            CriadoPor = SISTEMA_NOME,
    //            CriadoRF = SISTEMA_CODIGO_RF
    //        });
    //    }
    //}
}