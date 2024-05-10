using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq.Protected;
using Moq;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.Commands;
using SME.SGP.TesteIntegracao.Constantes;
using SME.SGP.TesteIntegracao.MapeamentoEstudantes.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Text;
using Newtonsoft.Json;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes
{

    public class Ao_obter_informacoes_atualizadas_mapeamento_estudante : MapeamentoBase
    {
        private const long RECUPERACAO_PARALELA_AUTORAL_HISTORIA = 1765;
        private const long PAP_RECUPERACAO_DE_APRENDIZAGENS = 1322;
        private const long PAP_PROJETO_COLABORATIVO = 1770;
        private const long SRM = 1030;
        private const long PAEE_COLABORATIVO = 1310;
        private const long ACOMPANHAMENTO_PEDAGOGICO_MATEMATICA = 1255;

        private const string JsonComboMultiplaEscolhaDinamico_FortalecimentoAprendizagens = "[{\"index\":\"1255\",\"value\":\"1255\"}]";
        private const string JsonComboMultiplaEscolhaDinamico_PAP = "[{\"index\":\"1765\",\"value\":\"1765\"},{\"index\":\"1322\",\"value\":\"Contraturno\"},{\"index\":\"1770\",\"value\":\"Colaborativo\"}]";
        private const string JsonComboMultiplaEscolhaDinamico_SRMCEFAI = "[{\"index\":\"1030\",\"value\":\"1030\"},{\"index\":\"1310\",\"value\":\"1310\"}]";

        public Ao_obter_informacoes_atualizadas_mapeamento_estudante(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter informações para atualização de mapeamento estudante")]
        public async Task Ao_obter_informacoes_atualizadas()
        {
            await CriarDadosBase();
            await CriarDadosAtualizacaoMapeamentoEstudante(4, DateTimeExtension.HorarioBrasilia().Year);
            var mediator = ServiceProvider.GetService<IMediator>();
            var retorno = await mediator.Send(new ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQuery(ALUNO_CODIGO_1, 
                                                                                                     DateTimeExtension.HorarioBrasilia().Year,
                                                                                                     4));
            retorno.ShouldNotBeNull();
            retorno.TurmaAnoAnterior.ShouldBe("EF-Turma Nome 4");
            retorno.IdParecerConclusivoAnoAnterior.ShouldBe(2);
            retorno.DescricaoParecerConclusivoAnoAnterior.ShouldBe("Promovido");
            retorno.AnotacoesPedagogicasBimestreAnterior.ShouldBe("ANOTAÇÕES PEDAGÓGICAS DO BIMESTRE ANTERIOR");
            retorno.QdadeEncaminhamentosNAAPAAtivos.ShouldBe(2);
            retorno.QdadePlanosAEEAtivos.ShouldBe(3);
            retorno.QdadeBuscasAtivasBimestre.ShouldBe(3);
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter informações de turmas de programa para atualização de mapeamento estudante")]
        public async Task Ao_obter_informacoes_atualizadas_turmas_programa()
        {
            await CriarDadosBase();
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            var componenteTurmaAlunoDto = new List<ComponenteTurmaAlunoDto>()
            {
                new(ALUNO_CODIGO_1, long.Parse(TURMA_CODIGO_1), RECUPERACAO_PARALELA_AUTORAL_HISTORIA, RECUPERACAO_PARALELA_AUTORAL_HISTORIA.ToString()),
                new(ALUNO_CODIGO_1, long.Parse(TURMA_CODIGO_2), PAP_RECUPERACAO_DE_APRENDIZAGENS, PAP_RECUPERACAO_DE_APRENDIZAGENS.ToString()),
                new(ALUNO_CODIGO_1, long.Parse(TURMA_CODIGO_2), PAP_PROJETO_COLABORATIVO, PAP_PROJETO_COLABORATIVO.ToString()),
                new(ALUNO_CODIGO_1, long.Parse(TURMA_CODIGO_3), SRM, SRM.ToString()),
                new(ALUNO_CODIGO_1, long.Parse(TURMA_CODIGO_3), PAEE_COLABORATIVO, PAEE_COLABORATIVO.ToString()),
                new(ALUNO_CODIGO_1, long.Parse(TURMA_CODIGO_4), ACOMPANHAMENTO_PEDAGOGICO_MATEMATICA, ACOMPANHAMENTO_PEDAGOGICO_MATEMATICA.ToString()),
            };
            var json = new StringContent(JsonConvert.SerializeObject(componenteTurmaAlunoDto), Encoding.UTF8, "application/json");

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = json
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
                                    {BaseAddress = new Uri("https://api.eol.com.br/somefakeurl")}; 
            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var _obterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQueryHandler = new ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQueryHandler(mockHttpClientFactory.Object);

            var retorno = await _obterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQueryHandler.Handle(new ObterInformacoesTurmasProgramaAlunoMapeamentoEstudanteQuery(ALUNO_CODIGO_1,
                                                                                                     DateTimeExtension.HorarioBrasilia().Year), new CancellationToken());

            retorno.ShouldNotBeNull();
            retorno.ComponentesFortalecimentoAprendizagens.SerializarJsonTipoQuestaoComboMultiplaEscolhaDinamico().ShouldBe(JsonComboMultiplaEscolhaDinamico_FortalecimentoAprendizagens);
            retorno.ComponentesPAP.SerializarJsonTipoQuestaoComboMultiplaEscolhaDinamico().ShouldBe(JsonComboMultiplaEscolhaDinamico_PAP);
            retorno.ComponentesSRMCEFAI.SerializarJsonTipoQuestaoComboMultiplaEscolhaDinamico().ShouldBe(JsonComboMultiplaEscolhaDinamico_SRMCEFAI);
        }

        private async Task CriarDadosAtualizacaoMapeamentoEstudante(int bimestre, int anoLetivo)
        {
            await InserirNaBaseAsync(new ConselhoClasseParecerConclusivo { Nome = "Retido por frequencia", Aprovado = false, Nota = false, CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF });
            var idParecerPromovido = await InserirNaBaseAsync(new ConselhoClasseParecerConclusivo { Nome = "Promovido", Aprovado = true, Nota = true, CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF });
            await InserirNaBase(new ConselhoClasseConsolidadoTurmaAluno
            {
                AlunoCodigo = ALUNO_CODIGO_1,  
                TurmaId = await InserirNaBaseAsync(new Dominio.Turma
                            {
                                UeId = 1,
                                Ano = "1",
                                CodigoTurma = TURMA_CODIGO_4,
                                ModalidadeCodigo = Modalidade.Fundamental,
                                AnoLetivo = anoLetivo - 1,
                                Semestre = 0,
                                Nome = TURMA_NOME_4,
                                TipoTurma = TipoTurma.Regular,
                                TipoTurno = (int)TipoTurnoEOL.Tarde
                }),
                Status = SituacaoConselhoClasse.Concluido,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                ParecerConclusivoId = idParecerPromovido
            });

            for (int i = 1; i <= 3; i++)
                await InserirNaBase(new PlanoAEE()
                {
                    TurmaId = TURMA_ID_1,
                    AlunoCodigo = ALUNO_CODIGO_1,
                    Situacao = SituacaoPlanoAEE.Validado,
                    AlunoNome = ALUNO_NOME_1,
                    AlunoNumero = 1,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

            for (int i = 1; i <= 2; i++)
                await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
                {
                    TurmaId = TURMA_ID_1,
                    AlunoCodigo = ALUNO_CODIGO_1,
                    Situacao = SituacaoNAAPA.EmAtendimento,
                    AlunoNome = ALUNO_NOME_1,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

            var idFechamento = await InserirNaBaseAsync(new FechamentoTurma()
            {
                PeriodoEscolarId = ObterTodos<PeriodoEscolar>().FirstOrDefault(p => p.Bimestre == bimestre-1).Id,
                TurmaId = ObterTodos<Dominio.Turma>().FirstOrDefault(t => t.CodigoTurma == TURMA_CODIGO_1).Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });

            var idConselho = await InserirNaBaseAsync(new ConselhoClasse()
            {
                FechamentoTurmaId = idFechamento,
                Situacao = SituacaoConselhoClasse.Concluido,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });

            await InserirNaBase(new ConselhoClasseAluno()
            {
                ConselhoClasseId = idConselho,
                AnotacoesPedagogicas = "ANOTAÇÕES PEDAGÓGICAS DO BIMESTRE ANTERIOR",
                AlunoCodigo = CODIGO_ALUNO_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });

            await Executor.ExecutarComando(new PublicarQuestionarioBuscaAtivaComando(this));
            await CriarRegistrosAcaoEstudante(ObterTodos<PeriodoEscolar>().FirstOrDefault(p => p.Bimestre == bimestre).PeriodoInicio);
        }

        private async Task CriarRegistrosAcaoEstudante(DateTime dataRegistroAcao)
        {
            for (int i = 1; i <= 3; i++)
            {
                var idRegistroAcao = await InserirNaBaseAsync(new Dominio.RegistroAcaoBuscaAtiva()
                {
                    TurmaId = TURMA_ID_1,
                    AlunoCodigo = CODIGO_ALUNO_1,
                    AlunoNome = "Nome do aluno 1",
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

                var idSecao = await InserirNaBaseAsync(new RegistroAcaoBuscaAtivaSecao()
                {
                    RegistroAcaoBuscaAtivaId = idRegistroAcao,
                    SecaoRegistroAcaoBuscaAtivaId = ConstantesQuestionarioBuscaAtiva.SECAO_REGISTRO_ACAO_ID_1,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    Concluido = false
                });

                var idQuestao = await InserirNaBaseAsync(new QuestaoRegistroAcaoBuscaAtiva()
                {
                    RegistroAcaoBuscaAtivaSecaoId = idSecao,
                    QuestaoId = ObterTodos<Questao>().FirstOrDefault(q => q.NomeComponente == ConstantesQuestionarioBuscaAtiva.QUESTAO_1_NOME_COMPONENTE_DATA_REGISTRO_ACAO).Id,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

                await InserirNaBase(new RespostaRegistroAcaoBuscaAtiva()
                {
                    QuestaoRegistroAcaoBuscaAtivaId = idQuestao,
                    Texto = dataRegistroAcao.ToString("yyyy-MM-dd"),
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
                dataRegistroAcao = dataRegistroAcao.AddDays(2);
            }
        }
    }
}
