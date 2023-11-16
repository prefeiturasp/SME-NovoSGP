using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva
{
    public class Ao_listar_registros_acao_busca_ativa : RegistroAcaoBuscaAtivaTesteBase
    {
        
   
        public Ao_listar_registros_acao_busca_ativa(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            /*services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaCodigoPorIdQuery, string>), typeof(ObterTurmaCodigoPorIdQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorCodigoEolQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));*/
        }

        [Fact(DisplayName = "Ao listar as seções de registro de ação - sem Id registro")]
        public async Task Ao_listar_secoes_registro_acao()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtro);
            var useCase = ObterUseCaseListagemSecoes();
            var retorno = await useCase.Executar(new FiltroSecoesDeRegistroAcao());
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(1);
            retorno.All(r => !r.Concluido).ShouldBeTrue();
            retorno.All(r => r.Auditoria.EhNulo()).ShouldBeTrue();
        }

        [Fact(DisplayName = "Ao listar questões por questionário e respostas de registro de ação")]
        public async Task Ao_listar_questoes_questionario()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtro);
            await GerarDadosRegistroAcao(DateTimeExtension.HorarioBrasilia().Date);
            var useCase = ObterUseCaseListagemQuestionario();
            var retorno = await useCase.Executar(QUESTIONARIO_REGISTRO_ACAO_ID_1, 1);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(4);
            retorno.Where(q => q.Id == QUESTAO_1_ID_DATA_REGISTRO_ACAO).FirstOrDefault()
                            .Resposta.FirstOrDefault()
                            .Texto.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.ToString("dd/MM/yyyy"));

            var opcoesResposta = ObterTodos<OpcaoResposta>();
            var opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP && q.Nome == "Sim").FirstOrDefault();
            retorno.Where(q => q.Id == QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP).FirstOrDefault()
                            .Resposta.FirstOrDefault()
                            .OpcaoRespostaId.ShouldBe(opcaoRespostaBase.Id);
            opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_3_ID_PROCEDIMENTO_REALIZADO && q.Nome == "Visita Domiciliar").FirstOrDefault();
            retorno.Where(q => q.Id == QUESTAO_3_ID_PROCEDIMENTO_REALIZADO).FirstOrDefault()
                            .Resposta.FirstOrDefault()
                            .OpcaoRespostaId.ShouldBe(opcaoRespostaBase.Id);
        }


        private async Task GerarDadosRegistroAcao(DateTime dataRegistro)
        {
            await CriarRegistroAcao();
            await CriarRegistroAcaoSecao();
            await CriarQuestoesRegistroAcao();
            await CriarRespostasRegistroAcao(dataRegistro);
        }

        private async Task CriarRespostasRegistroAcao(DateTime dataRegistro)
        {
            await InserirNaBase(new Dominio.RespostaRegistroAcaoBuscaAtiva()
            {
                QuestaoRegistroAcaoBuscaAtivaId = QUESTAO_1_ID_DATA_REGISTRO_ACAO,
                Texto = dataRegistro.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var opcoesResposta = ObterTodos<OpcaoResposta>();
            var opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP && q.Nome == "Sim").FirstOrDefault();
            await InserirNaBase(new Dominio.RespostaRegistroAcaoBuscaAtiva()
            {
                QuestaoRegistroAcaoBuscaAtivaId = QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP,
                RespostaId = opcaoRespostaBase.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == QUESTAO_3_ID_PROCEDIMENTO_REALIZADO && q.Nome == "Visita Domiciliar").FirstOrDefault();
            await InserirNaBase(new Dominio.RespostaRegistroAcaoBuscaAtiva()
            {
                QuestaoRegistroAcaoBuscaAtivaId = QUESTAO_3_ID_PROCEDIMENTO_REALIZADO,
                RespostaId = opcaoRespostaBase.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarQuestoesRegistroAcao()
        {
            await InserirNaBase(new Dominio.QuestaoRegistroAcaoBuscaAtiva()
            {
                RegistroAcaoBuscaAtivaSecaoId = 1,
                QuestaoId = QUESTAO_1_ID_DATA_REGISTRO_ACAO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoRegistroAcaoBuscaAtiva()
            {
                RegistroAcaoBuscaAtivaSecaoId = 1,
                QuestaoId = QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoRegistroAcaoBuscaAtiva()
            {
                RegistroAcaoBuscaAtivaSecaoId = 1,
                QuestaoId = QUESTAO_3_ID_PROCEDIMENTO_REALIZADO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarRegistroAcaoSecao()
        {
            await InserirNaBase(new Dominio.RegistroAcaoBuscaAtivaSecao()
            {
                RegistroAcaoBuscaAtivaId = 1,
                SecaoRegistroAcaoBuscaAtivaId = SECAO_REGISTRO_ACAO_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarRegistroAcao()
        {
            await InserirNaBase(new Dominio.RegistroAcaoBuscaAtiva()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}

