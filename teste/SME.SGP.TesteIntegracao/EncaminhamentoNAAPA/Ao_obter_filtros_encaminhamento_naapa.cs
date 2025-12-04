using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class Ao_obter_filtros_encaminhamento_naapa : EncaminhamentoNAAPATesteBase
    {
        public Ao_obter_filtros_encaminhamento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
}

        [Fact(DisplayName = "Encaminhamento NAAPA - Obter filtro de prioridades do encaminhamento naapa")]
        public async Task Deve_retornar_registros_de_prioridades_apenas_do_encaminhamento_naapa()
        {
            await AjustarDadosNaBase();

            var mediator = ServiceProvider.GetService<IMediator>();

            var prioridades = await mediator.Send(new ObterPrioridadeAtendimentoNAAPAQuery());

            prioridades.ShouldNotBeNull();
            prioridades.ShouldNotBeEmpty();
            prioridades.Count().ShouldBe(2);
        }

        private async Task AjustarDadosNaBase()
        {
            await CriarQuestionario();
            await CriarQuestoes();
            await CriarRespostas();
            await CriarSecaoEncaminhamentoNAAPAQuestionario();
        }

        private async Task CriarQuestionario()
        {
            //1
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Encaminhamento NAAPA Etapa 1 Seção 1",
                Tipo = TipoQuestionario.EncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //2
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Encaminhamento NAAPA Etapa 1 Seção 2 - Infantil",
                Tipo = TipoQuestionario.EncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //3
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Encaminhamento NAAPA Etapa 1 Seção 3 - Itinerância",
                Tipo = TipoQuestionario.EncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //4
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Relatório Dinâmico Encaminhamento NAAPA",
                Tipo = TipoQuestionario.RelatorioDinamicoEncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //5
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Relatório Dinâmico Encaminhamento NAAPA - Infantil",
                Tipo = TipoQuestionario.RelatorioDinamicoEncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //6
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Relatório Dinâmico Encaminhamento NAAPA - Fundamental, Médio, EJA, CIEJA, MOVA, CMCT, ETEC",
                Tipo = TipoQuestionario.RelatorioDinamicoEncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //7
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Relatório Dinâmico Encaminhamento NAAPA - Atendimento",
                Tipo = TipoQuestionario.RelatorioDinamicoEncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        private async Task CriarQuestoes()
        {
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Prioridade",
                Obrigatorio = true,
                Tipo = TipoQuestao.Combo,
                NomeComponente = "PRIORIDADE",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Questao()
            {
                QuestionarioId = 4,
                Ordem = 1,
                Nome = "Prioridade",
                Obrigatorio = true,
                Tipo = TipoQuestao.Combo,
                NomeComponente = "PRIORIDADE",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        private async Task CriarRespostas()
        {
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 1,
                Ordem = 1,
                Nome = "Normal",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //id 2
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 1,
                Ordem = 2,
                Nome = "Prioritária",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //id 3
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 2,
                Ordem = 1,
                Nome = "Normal",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //id 4
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 2,
                Ordem = 2,
                Nome = "Prioritária",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        private async Task CriarSecaoEncaminhamentoNAAPAQuestionario()
        {
            //Id 1
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = 1,
                Nome = "Informações do Estudante",
                NomeComponente = NOME_SECAO_ENCAMINHAMENTO_NAAPA_INFORMACOES_ESTUDANTE,
                Etapa = 1,
                Ordem = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //Id 2
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = 2,
                Nome = "Questões apresentadas",
                NomeComponente = NOME_SECAO_ENCAMINHAMENTO_NAAPA_QUESTOES_APRESENTADAS_INFANTIL,
                Etapa = 1,
                Ordem = 2,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //Id 3
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = ID_QUESTIONARIO_NAAPA_ITINERANCIA,
                Nome = "Apoio e Acompanhamento",
                NomeComponente = EncaminhamentoNAAPAConstants.SECAO_ITINERANCIA,
                Etapa = 1,
                Ordem = 3,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //Id 4
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_4,
                Nome = "Informações do Estudante",
                NomeComponente = NOME_SECAO_ENCAMINHAMENTO_NAAPA_INFORMACOES_ESTUDANTE,
                Etapa = 1,
                Ordem = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //Id 5
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_INFANTIL_5,
                Nome = "Questões apresentadas - Somente infantil",
                NomeComponente = NOME_SECAO_ENCAMINHAMENTO_NAAPA_QUESTOES_APRESENTADAS_INFANTIL,
                Etapa = 1,
                Ordem = 2,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //Id 6
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_EF_EJA_CIEJA_MOVA_CMCT_ETEC_6,
                Nome = "Questões apresentadas - Todos exceto infantil",
                NomeComponente = NOME_SECAO_ENCAMINHAMENTO_NAAPA_QUESTOES_APRESENTADAS_FUNDAMENTAL,
                Etapa = 1,
                Ordem = 2,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //Id 7
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_ATENDIMENTO_7,
                Nome = "Apoio e acompanhamento",
                NomeComponente = EncaminhamentoNAAPAConstants.SECAO_ITINERANCIA,
                Etapa = 1,
                Ordem = 3,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }
    }
}
