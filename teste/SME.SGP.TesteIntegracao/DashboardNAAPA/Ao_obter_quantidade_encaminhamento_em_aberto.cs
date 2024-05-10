using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.DashboardNAAPA
{
    public class Ao_obter_quantidade_encaminhamento_em_aberto : TesteBaseComuns
    {
        public Ao_obter_quantidade_encaminhamento_em_aberto(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_obter_quantidade_encaminhamento_por_dre()
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilProfessor());

            await CriarUsuarios();

            await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                Quantidade = 4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Situacao = SituacaoNAAPA.EmAtendimento,
                Quantidade = 4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Situacao = SituacaoNAAPA.Rascunho,
                Quantidade = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterQuantidadeEncaminhamentoNAAPAEmAbertoPorDreUseCase>();
            var dto = new FiltroQuantidadeEncaminhamentoNAAPAEmAbertoDto() { AnoLetivo = DateTimeExtension.HorarioBrasilia().Year, DreId = DRE_ID_1 };
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();
            retorno.Graficos.FirstOrDefault().Quantidade.ShouldBe(8);
        }

        [Fact]
        public async Task Ao_obter_quantidade_encaminhamento_todas_dre()
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilProfessor());

            await CriarUsuarios();

            await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                Quantidade = 4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new ConsolidadoEncaminhamentoNAAPA()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_2,
                Situacao = SituacaoNAAPA.EmAtendimento,
                Quantidade = 4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterQuantidadeEncaminhamentoNAAPAEmAbertoPorDreUseCase>();
            var dto = new FiltroQuantidadeEncaminhamentoNAAPAEmAbertoDto() { AnoLetivo = DateTimeExtension.HorarioBrasilia().Year, DreId = null };
            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();
            var graficos = retorno.Graficos.ToList();
            graficos.Count().ShouldBe(2);
            graficos.Exists(consolidado => consolidado.Descricao == DRE_NOME_1).ShouldBeTrue();
            graficos.Exists(consolidado => consolidado.Descricao == DRE_NOME_2).ShouldBeTrue();
        }
    }
}
