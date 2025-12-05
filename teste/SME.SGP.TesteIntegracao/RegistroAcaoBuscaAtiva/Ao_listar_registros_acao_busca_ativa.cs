using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFakes;
using SME.SGP.TesteIntegracao.Constantes;
using SME.SGP.TesteIntegracao.Setup;
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
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFakeNAAPA), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterConsultaFrequenciaGeralAlunoQuery, string>), typeof(ObterConsultaFrequenciaGeralAlunoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Registro de Ação - Listar as seções")]
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

        [Fact(DisplayName = "Registro de Ação - Listar questões/respostas por questionário/registro de ação")]
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
            await GerarDadosRegistroAcao_2PrimeirasQuestoes(DateTimeExtension.HorarioBrasilia().Date);
            var useCase = ObterUseCaseListagemQuestionario();
            var retorno = await useCase.Executar(ConstantesQuestionarioBuscaAtiva.QUESTIONARIO_REGISTRO_ACAO_ID_1, 1);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(3);
            retorno.Where(q => q.Id == ConstantesQuestionarioBuscaAtiva.QUESTAO_1_ID_DATA_REGISTRO_ACAO).FirstOrDefault()
                            .Resposta.FirstOrDefault()
                            .Texto.ShouldBe(DateTimeExtension.HorarioBrasilia().Date.ToString("yyyy-MM-dd"));

            var opcoesResposta = ObterTodos<OpcaoResposta>();
            var opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == ConstantesQuestionarioBuscaAtiva.QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP 
                                                         && q.Nome == ConstantesQuestionarioBuscaAtiva.QUESTAO_OPCAO_RESPOSTA_SIM).FirstOrDefault();
            retorno.Where(q => q.Id == ConstantesQuestionarioBuscaAtiva.QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP).FirstOrDefault()
                            .Resposta.FirstOrDefault()
                            .OpcaoRespostaId.ShouldBe(opcaoRespostaBase.Id);
        }

        [Fact(DisplayName = "Registro de Ação - Obter registro de ação por id")]
        public async Task Ao_obter_registro_acao_por_id()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtro);
            await GerarDadosRegistroAcao_2PrimeirasQuestoes(DateTimeExtension.HorarioBrasilia().Date);
            var useCase = ObterUseCaseObtencaoRegistroAcao();
            var retorno = await useCase.Executar(1);
            retorno.ShouldNotBeNull();
            retorno.Aluno.ShouldNotBeNull();

            var registroAcao = ObterTodos<Dominio.RegistroAcaoBuscaAtiva>();
            registroAcao.Count.ShouldBe(1);

            retorno.DreId.ShouldBe(1);
            retorno.DreCodigo.ShouldBe(DRE_CODIGO_1);
            retorno.DreNome.ShouldBe(DRE_NOME_1);

            retorno.UeId.ShouldBe(1);
            retorno.UeCodigo.ShouldBe(UE_CODIGO_1);
            retorno.UeNome.Contains(UE_NOME_1).ShouldBeTrue();

            retorno.TurmaId.ShouldBe(1);
            retorno.TurmaCodigo.ShouldBe(TURMA_CODIGO_1);
            retorno.TurmaNome.ShouldBe(TURMA_NOME_1);

            retorno.AnoLetivo.ShouldBe(DateTimeExtension.HorarioBrasilia().Year);
            retorno.Modalidade.ShouldBe((int)Modalidade.Fundamental);
        }

        [Fact(DisplayName = "Registro de Ação - Listar registros de ação aluno e turma")]
        public async Task Ao_listar_registros_acao_estudantes_ausentes()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };

            await CriarDadosBase(filtro);
            var dataRegistro = DateTimeExtension.HorarioBrasilia().Date;
            await GerarDadosRegistroAcao_2PrimeirasQuestoes(dataRegistro.AddMonths(-1));
            await GerarDadosRegistroAcao_2PrimeirasQuestoes(dataRegistro);
            await GerarDadosRegistroAcao_2PrimeirasQuestoes(dataRegistro.AddMonths(1));
            var useCase = ObterUseCaseListagemRegistrosAcao_EstudantesAusentes();
            var retorno = await useCase.Executar(new FiltroRegistrosAcaoCriancasEstudantesAusentesDto()
            {
                CodigoAluno = ALUNO_CODIGO_1,
                TurmaId = TURMA_ID_1
            });
            retorno.ShouldNotBeNull();
            retorno.TotalRegistros.ShouldBe(3);
            retorno.Items.Count().ShouldBe(3);
            retorno.Items.Any(ra => ra.DataRegistro == dataRegistro.AddMonths(-1)).ShouldBeTrue();
            retorno.Items.Any(ra => ra.DataRegistro == dataRegistro).ShouldBeTrue();
            retorno.Items.Any(ra => ra.DataRegistro == dataRegistro.AddMonths(1)).ShouldBeTrue();
        }
    }
}

