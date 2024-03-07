using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva
{
    public class Ao_excluir_registro_acao_busca_ativa : RegistroAcaoBuscaAtivaTesteBase
    {
        
        public Ao_excluir_registro_acao_busca_ativa(CollectionFixture collectionFixture) : base(collectionFixture)
        { }


        [Fact(DisplayName = "Registro de Ação - Excluir")]
        public async Task Ao_excluir_registro_acao()
        {
            var filtro = new FiltroRegistroAcaoDto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8"
            };
            var data = DateTimeExtension.HorarioBrasilia().Date;

            await CriarDadosBase(filtro);
            await GerarDadosRegistroAcao_2PrimeirasQuestoes(data);

            var useCase = ObterUseCaseExclusaoRegistroAcao();
            var retorno = await useCase.Executar(1);
            retorno.ShouldBeTrue();

            var registroAcaoExcluido = ObterTodos<Dominio.RegistroAcaoBuscaAtiva>();
            registroAcaoExcluido.ShouldNotBeNull();
            registroAcaoExcluido.Where(registroAcao => registroAcao.Excluido).Count().ShouldBe(1);
            registroAcaoExcluido.Any(registroAcao => !registroAcao.Excluido).ShouldBeFalse();

            var registroAcaoSecaoExcluida = ObterTodos<RegistroAcaoBuscaAtivaSecao>();
            registroAcaoSecaoExcluida.ShouldNotBeNull();
            registroAcaoSecaoExcluida.FirstOrDefault().SecaoRegistroAcaoBuscaAtivaId.ShouldBe(1);
            registroAcaoSecaoExcluida.Where(encaminhamentoSecao => encaminhamentoSecao.Excluido).Count().ShouldBe(1);
            registroAcaoSecaoExcluida.Any(encaminhamentoSecao => !encaminhamentoSecao.Excluido).ShouldBeFalse();

            var questaoRegistroAcaoExcluida = ObterTodos<QuestaoRegistroAcaoBuscaAtiva>();
            questaoRegistroAcaoExcluida.ShouldNotBeNull();
            questaoRegistroAcaoExcluida.Where(registroAcaoQuestao => registroAcaoQuestao.Excluido).Count().ShouldBe(2);
            questaoRegistroAcaoExcluida.Any(registroAcaoQuestao => !registroAcaoQuestao.Excluido).ShouldBeFalse();

            var respostaRegistroAcaoExcluida = ObterTodos<RespostaRegistroAcaoBuscaAtiva>();
            respostaRegistroAcaoExcluida.ShouldNotBeNull();
            respostaRegistroAcaoExcluida.Where(registroAcaoResposta => registroAcaoResposta.Excluido).Count().ShouldBe(2);
            respostaRegistroAcaoExcluida.Any(registroAcaoResposta => !registroAcaoResposta.Excluido).ShouldBeFalse();
        }

    }
}

