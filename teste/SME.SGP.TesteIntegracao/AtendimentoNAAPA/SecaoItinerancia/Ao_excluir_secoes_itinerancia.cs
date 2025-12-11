//using Microsoft.Extensions.DependencyInjection;
//using Shouldly;
//using SME.SGP.Dominio;
//using SME.SGP.Dominio.Enumerados;
//using SME.SGP.TesteIntegracao.AtendimentoNAAPA;
//using SME.SGP.TesteIntegracao.Setup;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;

//namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.SecaoItinerancia
//{
//    public class Ao_excluir_secoes_itinerancia : AtendimentoNAAPATesteBase
//    {
//        public Ao_excluir_secoes_itinerancia(CollectionFixture collectionFixture) : base(collectionFixture)
//        {
//        }

//        protected override void RegistrarFakes(IServiceCollection services)
//        {
//            base.RegistrarFakes(services);
//        }

//        [Fact(DisplayName = "Encaminhamento NAAPA - Marcar como excluido (lógico) seção itinerância e questões/respostas vinculadas a ela")]
//        public async Task Deve_Marcar_Secao_itinerancia_Questoes_Respostas_Como_Excluido()
//        {
//            var filtroNAAPA = new FiltroNAAPADto()
//            {
//                Perfil = ObterPerfilCP(),
//                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
//                Modalidade = Modalidade.Fundamental,
//                AnoTurma = "8",
//                DreId = 1,
//                CodigoUe = "1",
//                TurmaId = TURMA_ID_1,
//                Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
//                Prioridade = NORMAL
//            };

//            await CriarDadosBase(filtroNAAPA);


//            var excluirSecaoItineranciaEncaminhamentoNAAPA = ObterServicoExcluirSecaoItineranciaEncaminhamentoNAAPA();
//            await GerarDadosEncaminhamentoNAAPA(DateTimeExtension.HorarioBrasilia().Date);


//            var encaminhamentoNAAPASecao = ObterTodos<EncaminhamentoNAAPASecao>();
//            encaminhamentoNAAPASecao.ShouldNotBeNull();
//            encaminhamentoNAAPASecao.Where(encaminhamentoSecao => !encaminhamentoSecao.Excluido).Count().ShouldBe(2);
//            encaminhamentoNAAPASecao.FirstOrDefault().SecaoEncaminhamentoNAAPAId.ShouldBe(ID_SECAO_ENCAMINHAMENTO_NAAPA_INFORMACOES_ESTUDANTE);
//            encaminhamentoNAAPASecao.LastOrDefault().SecaoEncaminhamentoNAAPAId.ShouldBe(ID_SECAO_ENCAMINHAMENTO_NAAPA_ITINERANCIA);

//            var questaoEncaminhamentoNAAPA = ObterTodos<QuestaoEncaminhamentoNAAPA>();
//            questaoEncaminhamentoNAAPA.ShouldNotBeNull();
//            questaoEncaminhamentoNAAPA.Where(encaminhamentoQuestao => !encaminhamentoQuestao.Excluido).Count().ShouldBe(7);

//            var respostaEncaminhamentoNAAPA = ObterTodos<RespostaEncaminhamentoNAAPA>();
//            respostaEncaminhamentoNAAPA.ShouldNotBeNull();
//            respostaEncaminhamentoNAAPA.Where(encaminhamentoResposta => !encaminhamentoResposta.Excluido).Count().ShouldBe(7);

//            var retorno = await excluirSecaoItineranciaEncaminhamentoNAAPA.Executar(1, 2);
//            retorno.ShouldBeTrue();

//            var encaminhamentoNAAPASecoesExcluidas = ObterTodos<EncaminhamentoNAAPASecao>();
//            encaminhamentoNAAPASecoesExcluidas.ShouldNotBeNull();
//            var encaminhamentoNAAPASeccaoExcluida = encaminhamentoNAAPASecoesExcluidas.Where(encaminhamentoSecao => encaminhamentoSecao.Excluido);
//            encaminhamentoNAAPASeccaoExcluida.Count().ShouldBe(1);
//            encaminhamentoNAAPASeccaoExcluida.FirstOrDefault().SecaoEncaminhamentoNAAPAId.ShouldBe(ID_SECAO_ENCAMINHAMENTO_NAAPA_ITINERANCIA);
//            encaminhamentoNAAPASecoesExcluidas.Where(encaminhamentoSecao => !encaminhamentoSecao.Excluido).Count().ShouldBe(1);

//            var questaoEncaminhamentoNAAPAExcluida = ObterTodos<QuestaoEncaminhamentoNAAPA>();
//            questaoEncaminhamentoNAAPAExcluida.ShouldNotBeNull();
//            questaoEncaminhamentoNAAPAExcluida.Where(encaminhamentoQuestao => encaminhamentoQuestao.Excluido).Count().ShouldBe(5);
//            questaoEncaminhamentoNAAPAExcluida.Where(encaminhamentoQuestao => !encaminhamentoQuestao.Excluido).Count().ShouldBe(2);

//            var respostaEncaminhamentoNAAPAExcluida = ObterTodos<RespostaEncaminhamentoNAAPA>();
//            respostaEncaminhamentoNAAPAExcluida.ShouldNotBeNull();
//            respostaEncaminhamentoNAAPAExcluida.Where(encaminhamentoResposta => encaminhamentoResposta.Excluido).Count().ShouldBe(5);
//            respostaEncaminhamentoNAAPAExcluida.Where(encaminhamentoResposta => !encaminhamentoResposta.Excluido).Count().ShouldBe(2);
//        }



//        private async Task GerarDadosEncaminhamentoNAAPA(DateTime dataQueixa)
//        {
//            await CriarEncaminhamentoNAAPA();
//            await CriarEncaminhamentoNAAPASecao();
//            await CriarQuestoesEncaminhamentoNAAPA();
//            await CriarRespostasEncaminhamentoNAAPA(dataQueixa);
//        }

//        private async Task CriarRespostasEncaminhamentoNAAPA(DateTime dataQueixa)
//        {
//            //Informações estudante
//            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
//            {
//                QuestaoEncaminhamentoId = 1,
//                Texto = dataQueixa.ToString("dd/MM/yyyy"),
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });

//            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
//            {
//                QuestaoEncaminhamentoId = 2,
//                Texto = "1",
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });

//            //itinerancia 1
//            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
//            {
//                QuestaoEncaminhamentoId = 3,
//                Texto = DateTimeExtension.HorarioBrasilia().Date.ToString("yyyy/MM/dd"),
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });

//            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
//            {
//                QuestaoEncaminhamentoId = 4,
//                Texto = "",
//                RespostaId = ID_OPCAO_RESPOSTA_ATENDIMENTO_NAO_PRESENCIAL,
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });

//            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
//            {
//                QuestaoEncaminhamentoId = 5,
//                Texto = "",
//                RespostaId = ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });

//            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
//            {
//                QuestaoEncaminhamentoId = 6,
//                Texto = "Teste Descricao Atendimento",
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });

//            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
//            {
//                QuestaoEncaminhamentoId = 7,
//                Texto = "Teste Descricao Procedimento de Trabalho",
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });
//        }

//        private async Task CriarQuestoesEncaminhamentoNAAPA()
//        {
//            //informações estudante
//            //Id 1
//            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
//            {
//                EncaminhamentoNAAPASecaoId = 1,
//                QuestaoId = 1,
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });

//            //Id 2
//            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
//            {
//                EncaminhamentoNAAPASecaoId = 1,
//                QuestaoId = 2,
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });

//            //itinerancia 1
//            //Id 3
//            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
//            {
//                EncaminhamentoNAAPASecaoId = 2,
//                QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });

//            //Id 4
//            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
//            {
//                EncaminhamentoNAAPASecaoId = 2,
//                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });

//            //Id 5
//            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
//            {
//                EncaminhamentoNAAPASecaoId = 2,
//                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });

//            //Id 6
//            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
//            {
//                EncaminhamentoNAAPASecaoId = 2,
//                QuestaoId = ID_QUESTAO_DESCRICAO_ATENDIMENTO,
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });

//            //Id 7
//            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
//            {
//                EncaminhamentoNAAPASecaoId = 2,
//                QuestaoId = ID_QUESTAO_DESCRICAO_PROCEDIMENTO_TRABALHO,
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });
//        }

//        private async Task CriarEncaminhamentoNAAPASecao()
//        {
//            //Id 1
//            await InserirNaBase(new EncaminhamentoNAAPASecao()
//            {
//                EncaminhamentoNAAPAId = 1,
//                SecaoEncaminhamentoNAAPAId = ID_SECAO_ENCAMINHAMENTO_NAAPA_INFORMACOES_ESTUDANTE,
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });

//            //Id 2
//            await InserirNaBase(new EncaminhamentoNAAPASecao()
//            {
//                EncaminhamentoNAAPAId = 1,
//                SecaoEncaminhamentoNAAPAId = ID_SECAO_ENCAMINHAMENTO_NAAPA_ITINERANCIA,
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });
//        }

//        private async Task CriarEncaminhamentoNAAPA()
//        {
//            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
//            {
//                TurmaId = TURMA_ID_1,
//                AlunoCodigo = ALUNO_CODIGO_1,
//                Situacao = SituacaoNAAPA.AguardandoAtendimento,
//                AlunoNome = "Nome do aluno 1",
//                CriadoEm = DateTimeExtension.HorarioBrasilia(),
//                CriadoPor = SISTEMA_NOME,
//                CriadoRF = SISTEMA_CODIGO_RF
//            });
//        }
//    }
//}